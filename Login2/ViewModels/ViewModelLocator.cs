/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Login2"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Login2.ViewModels.HumanResources;
using Microsoft.Practices.ServiceLocation;
using Login2.ViewModels.Receptionist;
namespace Login2.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<PopUpRoleViewModel>();
            SimpleIoc.Default.Register<CustomerViewModel>();
            SimpleIoc.Default.Register<RoomViewModel>();
            SimpleIoc.Default.Register<RentRoomViewModel>();
            SimpleIoc.Default.Register<BookingRoomViewModel>();
            SimpleIoc.Default.Register<RoomOptionViewModel>();
            SimpleIoc.Default.Register<ChooseCustomerViewModel>();
            SimpleIoc.Default.Register<CheckOutViewModel>();

        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public ProfileViewModel Profile
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProfileViewModel>();
            }
        }
        public PopUpRoleViewModel PopUpRole
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PopUpRoleViewModel>();
            }
        }
        public StaffListViewModel StaffList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StaffListViewModel>();
            }
        }
        public InsertStaffViewModel InsertStaff
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InsertStaffViewModel>();
            }
        }

        public RentRoomViewModel RentRoom
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RentRoomViewModel>();
            }
        }
        public CustomerViewModel Customer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CustomerViewModel>();
            }
        } 
        public RoomViewModel Room
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RoomViewModel>();
            }
        }
        public RoomOptionViewModel RoomOption
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RoomOptionViewModel>();
            }
        }

        public ChooseCustomerViewModel ChooseCustomer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChooseCustomerViewModel>();
            }
        }

        public CheckOutViewModel CheckOut
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CheckOutViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
            SimpleIoc.Default.Reset();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<PopUpRoleViewModel>();
            SimpleIoc.Default.Register<CustomerViewModel>();
            SimpleIoc.Default.Register<RoomViewModel>();
            SimpleIoc.Default.Register<RentRoomViewModel>();
            SimpleIoc.Default.Register<BookingRoomViewModel>();

        }

        public static void UnRegisterMainViewModel()
        {
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public static void RenewRentRoom()
        {
            SimpleIoc.Default.Unregister<RentRoomViewModel>();
            SimpleIoc.Default.Register<RentRoomViewModel>();
        }
        public static void RenewRoomOption()
        {
            SimpleIoc.Default.Unregister<RoomOptionViewModel>();
            SimpleIoc.Default.Register<RoomOptionViewModel>();
        }
        public static void RenewCheckOut()
        {
            SimpleIoc.Default.Unregister<CheckOutViewModel>();
            SimpleIoc.Default.Register<CheckOutViewModel>();
        }
    }
}