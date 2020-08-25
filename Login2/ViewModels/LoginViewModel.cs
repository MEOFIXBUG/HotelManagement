using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Models;
using Login2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Login2.ViewModels
{
    public class LoginViewModel : MyBaseViewModel
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        /// 
        Session session;
        private IRepository<account> accountRepository = null;
        public LoginViewModel()
        {
            session = Session.GetCurrentSingleton();
            accountRepository = new BaseRepository<account>();
            /*
			 * Not much point in this case, but for the record, you can have 
			 * different data depending on if you're in design or runtime like this:
			 */


            // This will register our method with the Messenger class for incoming 
            // messages of type RefreshPeople.
            //Messenger.Default.Register<RefreshPeople>(this, (msg) => Execute_RefreshPeople(msg.PeopleToFetch));
        }
        #endregion

        #region Properties
        public string UserName
        {
            get { return _userVal; }
            set
            {
                _userVal = value;
                RaisePropertyChanged();
            }
        }
        private string _userVal;

        #endregion

        #region Login command 

        /*
		 * First way of using the relay commands and ICommand. 
		 * The Command is created when we first try to get it, saving us the need to
		 * initialize it in the constructor.
		 */

        /// <summary>
        /// You'll bind to this from a button on the GUI.
        /// Note the Execute    -> this will be run when the button is clicked
        /// and the CanExecute  -> This will enable / disable the button to begin with
        /// 
        /// Also, we're passing an int through the binding in the XAML:
        /// ----> CommandParameter="{Binding ElementName=Cmb_NamesAmount, Path=SelectedItem}" 
        /// which is an item from the list of ints we defined. so the RelayCommand takes an "int",
        /// hence the syntax with the type of paramater we'll accept
        /// ----> "_refreshNames_command = new RelayCommand..." 
        /// </summary>
        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                     (_loginCommand = new RelayCommand<object>(Execute_Login, CanExecute_Login));
            }
        }



        /// <summary>
        /// If you select 1 in the drop down menu, the button will become disable. 
        /// Rather simple, but it's a place holder for whatever logic you might want.
        /// </summary>
        /// <param name="arg">We're passing an int from the xaml.</param>
        /// <returns></returns>
        private bool CanExecute_Login(object pass)
        {
            return true;
            //if (pass != null)
            //{
            //    var p = (System.Windows.Controls.PasswordBox)pass;
            //    if (UserName == null || p.Password == null) return false;
            //    return true;
            //}
            //return false;
        }

        /// <summary>
        /// This happens when you click the button.
        /// </summary>
        /// <param name="arg"></param>
        private void Execute_Login(object pass)
        {
            var p = (System.Windows.Controls.PasswordBox)pass;
            //Xac thuc dang nhap (phan quyen )
            //var Account=get..
            var Account = accountRepository.Get(u => u.UserName.Equals(UserName) && u.Password.Equals(p.Password)).FirstOrDefault();
            if (Account != null)
            {
                var accountID = Account.ID;
                //var role = Roles.HumanResources;
                var role = (Roles)Account.Role;
                var Role = role.ToString();
                //Phan Quyen (Role-base....)
                GenericIdentity identity = new GenericIdentity(UserName);
                Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[] { Role });
                session.AccountID = accountID;
                session.Role = role;
                System.Windows.Application.Current.Properties["Session"] = session;
                var homePage = new Home();
                homePage.Show();
                Login login = App.Current.Windows.OfType<Login>().FirstOrDefault();
                if (login != null)
                {
                    login.Close();
                }
            }
            else
            {
                MessageBox.Show("Sai Tai Khoan hoac Mat Khau");
            }
        }
        #endregion
    }
}
