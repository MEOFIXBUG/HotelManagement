using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Commands;
using Login2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Login2.ViewModels.HumanResources
{
    public class PopUpRoleViewModel : MyBaseViewModel
    {
        private Dictionary<int, string> _allRole;

        public Dictionary<int, string> AllRole
        {
            get { return _allRole; }
            set { _allRole = value; RaisePropertyChanged(); }
        }
        private account currentAcc;
        private int _roleId;
        public int RoleID
        {
            get { return _roleId; }
            set { _roleId = value; RaisePropertyChanged(); }
        }
        private IRepository<account> accountRepository = null;
        public PopUpRoleViewModel()
        {
            accountRepository = new BaseRepository<account>();
            currentAcc = new account();
            Messenger.Default.Register<Parameter>(this,  res =>  Function(res.param));
            _allRole = new Dictionary<int, string>();
            _allRole = Enum.GetValues(typeof(Roles))
                               .Cast<Roles>()
                               .ToDictionary(t => (int)t, t => t.GetDescription());
        }
        private  void Function(object obj)
        {
            currentAcc = accountRepository.GetByID((int)obj);
            RoleID = currentAcc.Role-1;
        }
        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ??
                     (_closeWindowCommand = new RelayCommand<object>(Execute_CloseWindow, CanExecute_CloseWindow));
            }
        }
        private void Execute_CloseWindow(object obj)
        {

            var p = (Window)obj;
            p.Close();
        }
        private bool CanExecute_CloseWindow(object obj)
        {
            return true;
        }
        private ICommand _updateRoleCommand;
        public ICommand UpdateRoleCommand
        {
            get
            {
                return _updateRoleCommand ??
                     (_updateRoleCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateRole, Execute_UpdateRole));
            }
        }

        private bool CanExecute_UpdateRole(object arg)
        {
            return true;
        }
        [AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_UpdateRole(object obj)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var p = (int)obj + 1;
                currentAcc.Role = p;
                accountRepository.Update(currentAcc);
                accountRepository.Save();
                System.Windows.Forms.MessageBox.Show("Successfully updated", "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
        }
    }
}
