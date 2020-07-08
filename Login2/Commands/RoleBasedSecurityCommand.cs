using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Login2.Commands
{
    public class RoleBasedSecurityCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        private bool _IsAuthorized;

        #region Constructors
       
        public RoleBasedSecurityCommand(Predicate<T> canExecute ,Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;

            _IsAuthorized = IsAuthorized(_execute.Method);
        }

        #endregion

        private bool IsAuthorized(MethodInfo methodInfo)
        {
            bool isAuthorized = true;

            var authorizationAttributes = (AuthorizationAttribute[])methodInfo.GetCustomAttributes(typeof(AuthorizationAttribute), true);

            if (authorizationAttributes.Count() == 0)
            {
                return true;
            }
            foreach (var authorizationAttribute in authorizationAttributes)
            {
                if (authorizationAttribute.AuthorizationType == AuthorizationType.Allow)
                {
                    isAuthorized = Thread.CurrentPrincipal.IsInRole(authorizationAttribute.Role);
                }
                else if (authorizationAttribute.AuthorizationType == AuthorizationType.Deny)
                {
                    isAuthorized = !Thread.CurrentPrincipal.IsInRole(authorizationAttribute.Role);
                }

                if (isAuthorized == false)
                {
                    break;
                }
            }

            return isAuthorized;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (_IsAuthorized)
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }
            else
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }
}
