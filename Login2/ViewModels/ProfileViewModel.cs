using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.DomainObjects;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Commands;
using Login2.Models;
using Login2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Login2.ViewModels
{
    public class ProfileViewModel : MyBaseViewModel
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        ///
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }
        private string _roleName;

        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; RaisePropertyChanged(); }
        }
        private RePass _rePass;

        public RePass RePass
        {
            get { return _rePass; }
            set { _rePass = value; RaisePropertyChanged(); }
        }
        int UID = 0;
        private IRepository<account> accountRepository = null;
        private IRepository<staff> staffRepository = null;
        Session session;
        public ProfileViewModel()
        {
            accountRepository = new BaseRepository<account>();
            staffRepository = new BaseRepository<staff>();
            _rePass = new RePass();
            session = Session.GetCurrentSingleton();
            UID = (int)session.AccountID;
            _roleName = session.Role.ToString();
            _myProfile = staffRepository.GetByID(UID);
            _name = _myProfile.Name;
            /*
			 * Not much point in this case, but for the record, you can have 
			 * different data depending on if you're in design or runtime like this:
			 */


            // This will register our method with the Messenger class for incoming 
            // messages of type RefreshPeople.
            //Messenger.Default.Register<RefreshPeople>(this, (msg) => Execute_RefreshPeople(msg.PeopleToFetch));
            //Messenger.Default.Register<Parameter>(this, res => Function(res.param));
        }

        //private void Function(object obj)
        //{
        //    MessageBox.Show(obj.ToString());
        //}
        #endregion

        #region Properties
        private staff _myProfile;

        public staff MyProfile
        {
            get { return _myProfile; }
            set { _myProfile = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Command

        private ICommand _updateProfileCommand;
        public ICommand UpdateProfileCommand
        {
            get
            {
                return _updateProfileCommand ??
                     (_updateProfileCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateProfile, Execute_UpdateProfile));
            }
        }

        [AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_UpdateProfile(object obj)
        {
            //update
            //var a= new staff(obj.ToString());
        }
        private bool CanExecute_UpdateProfile(object obj)
        {
            return true;
        }

        private ICommand _updatePassCommand;
        public ICommand UpdatePassCommand
        {
            get
            {
                return _updatePassCommand ??
                     (_updatePassCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdatePass, Execute_UpdatePass));
            }
        }
        private void Execute_UpdatePass(object obj)
        {
            var p = (RePass)obj;
            if (!p.validate())
            {
                System.Windows.Forms.MessageBox.Show("Empty", "Warning",
    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var curAcc = accountRepository.GetByID(UID);
            var password = curAcc.Password;
            if (password != p.curpass)
            {
                System.Windows.Forms.MessageBox.Show("Wrong Password", "Warning",
    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (p.newpass != p.repass)
                {
                    System.Windows.Forms.MessageBox.Show("New-Password and Re-Password don't match", "Warning",
    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    curAcc.Password = p.newpass;
                    accountRepository.Update(curAcc);
                    accountRepository.Save();
                    System.Windows.Forms.MessageBox.Show("Successfully updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RePass.clear();
                    var ProfilePage = App.Current.Windows.OfType<Profile>().FirstOrDefault();
                    if (ProfilePage != null)
                    {
                        ProfilePage.Close();
                    }
                }
            }

        }
        private bool CanExecute_UpdatePass(object obj)
        {
            return true;
        }

        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ??
                     (_closeWindowCommand = new RoleBasedSecurityCommand<object>(CanExecute_CloseWindow, Execute_CloseWindow));
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
        #endregion

    }
}
