using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Commands;
//using Login2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Login2.ViewModels
{
    public class ProfileViewModel:MyBaseViewModel
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Main_ViewModel class.
        /// </summary>
        /// 
        public ProfileViewModel()
        {

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
            MessageBox.Show("abc");
        }

        private bool CanExecute_UpdateProfile(object obj)
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
