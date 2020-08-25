using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login2.ViewModels
{
    public class ViewModelFactory
    {
        private static ViewModelFactory _instance;
        protected ViewModelFactory()
        {
        }
        public static ViewModelFactory Instance()
        {
            // Uses lazy initialization.

            // Note: this is not thread safe.

            if (_instance == null)
            {
                _instance = new ViewModelFactory();
            }

            return _instance;
        }
        public MyBaseViewModel getViewModel(string viewmodelName)
        {
            if (viewmodelName == null)
            {
                return null;
            }

            switch (viewmodelName)
            {
                case "HF_addAccount":
                    {
                        return new HumanResources.InsertStaffViewModel();
                    }
                case "HF_staffList":
                    {
                        return new HumanResources.StaffListViewModel();
                    }
                case "SF_customerList":
                    {
                        return new Sales.CustomerListViewModel();
                    }
                case "SF_roomList":
                    {
                        return new Sales.RoomListViewModel();
                    }
                case "SF_addRoom":
                    {
                        return new Sales.InsertRoomViewModel();
                    }
                case "RF_customerList":
                    {
                        return new Receptionist.CustomerViewModel();
                    }
                case "RF_roomList":
                    {
                        return new Receptionist.RoomViewModel();
                    }
                case "AF_statistical":
                    {
                        return new Accountant.StatisticsViewModel(); ;
                    }

                case "AF_report":
                    {
                        return null;
                    }
                default:
                    // code block
                    break;
            }
            return null;
        }
    }
}
