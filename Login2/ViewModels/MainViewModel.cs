using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.DomainObjects;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Commands;
using Login2.ViewModels.HumanResources;
using Login2.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Login2.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : MyBaseViewModel
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Roles role = LoginViewModel.session.getRole();
            featuresCollection = ExtraFunction.featureOfRole(role);
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
        private List<string> _featuresCollection;

        public List<string> featuresCollection
        {
            get { return _featuresCollection; }
            set { _featuresCollection = value; RaisePropertyChanged(); }
        }

        #region Test command 

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



        private ICommand _openProfileCommand;
        public ICommand OpenProfileCommand
        {
            get
            {
                return _openProfileCommand ??
                     (_openProfileCommand = new RoleBasedSecurityCommand<object>(CanExecute_OpenProfile, Execute_OpenProfile));
            }
        }

        private MyBaseViewModel _selectedViewModel = new StaffListViewModel();

        public MyBaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// If you select 1 in the drop down menu, the button will become disable. 
        /// Rather simple, but it's a place holder for whatever logic you might want.
        /// </summary>
        /// <param name="arg">We're passing an int from the xaml.</param>
        /// <returns></returns>
        private bool CanExecute_OpenProfile(object o)
        {
            return true;
        }

        /// <summary>
        /// This happens when you click the button.
        /// </summary>
        /// <param name="arg"></param>
        //[AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_OpenProfile(object o)
        {
            var profilePage = new Profile();
            //ParameterSetter.SetParameter(LoginViewModel.session.getAccountID());
            profilePage.ShowDialog();
        }
        #endregion
       
        private ICommand _switchViewCommand;
        public ICommand SwitchViewCommand
        {
            get
            {
                return _switchViewCommand ??
                     (_switchViewCommand = new RoleBasedSecurityCommand<object>(CanExecute_SwitchView, Execute_SwitchView));
            }
        }

        /// <summary>
        /// If you select 1 in the drop down menu, the button will become disable. 
        /// Rather simple, but it's a place holder for whatever logic you might want.
        /// </summary>
        /// <param name="arg">We're passing an int from the xaml.</param>
        /// <returns></returns>
        private bool CanExecute_SwitchView(object o)
        {
            return true;
        }

        /// <summary>
        /// This happens when you click the button.
        /// </summary>
        /// <param name="arg"></param>
        public ICommand updateViewCommand { get; set; }
        //[AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_SwitchView(object param)
        {
            var p = param as ParametersForSwitchView;
            p.transitioningContentSlide.OnApplyTemplate();
            p.gridCursor.Margin = new Thickness(0, (100 + (60 * p.selectedIndex)), 0, 0);
            SelectedViewModel = ExtraFunction.getUserControl(p.selectedIndex, LoginViewModel.session.getRole());
        }
    }
}