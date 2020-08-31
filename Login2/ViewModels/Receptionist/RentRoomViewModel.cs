using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Commands;
using Login2.Models;
using Login2.Views.Receptionist;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Login2.ViewModels.Receptionist
{
    public class RentRoomViewModel : MyBaseViewModel
    {
        private room _room;

        private BindingList<service> _listService;
        private BindingList<service_details> _listServiceAdded;
        private booking _bookingInfo;
        private customer _customer;
        private int _numOfPeople;
        private int _serviceCost;
        private int _roomCost;
        private int _discount;
        private int _totalCost;


        //time
        DateTime _startDate;
        DateTime _endDate;
        DateTime _startTime;
        DateTime _endTime;

        //private Dictionary<int,>
        private IRepository<room> roomRepository = null;
        private IRepository<booking> bookingRepository = null;
        private IRepository<booking_details> bookingDetailRepository = null;
        private IRepository<service> servicelRepository = null;
        private IRepository<service_details> serviceDetailRepository = null;

        public RentRoomViewModel()
        {
            roomRepository = new BaseRepository<room>();
            bookingRepository = new BaseRepository<booking>();
            bookingDetailRepository = new BaseRepository<booking_details>();
            serviceDetailRepository = new BaseRepository<service_details>();
            servicelRepository = new BaseRepository<service>();
            Messenger.Default.Register<Parameter>(this, res => Room = GetRoom(((int)res.param)));
            ListService = getFullService();
            BookingInfo = new booking();
            ListServiceAdded = new BindingList<service_details>();

            NumOfPeole = 1;

            RoomCost = 0;
            ServiceCost = 0;
            Discount = 0;
            TotalCost = 0;

            //StartDate = new DateTime(DateTime.Now.Ticks);
            //StartTime = new DateTime(DateTime.Now.Ticks);
            //EndDate = new DateTime(DateTime.Now.Ticks);
            //EndTime = new DateTime(DateTime.Now.Ticks);

            StartDate = DateTime.Now;
            StartTime = DateTime.Now;
            EndDate = DateTime.Now;
            EndTime = DateTime.Now;

        }


        public room Room { get => _room; set { _room = value; RaisePropertyChanged(); } }
        public BindingList<service> ListService { get => _listService; set { _listService = value; RaisePropertyChanged(); } }
        public BindingList<service_details> ListServiceAdded { get => _listServiceAdded; set { _listServiceAdded = value; RaisePropertyChanged(); } }
        public booking BookingInfo { get => _bookingInfo; set { _bookingInfo = value; RaisePropertyChanged(); } }
        public customer Customer { get => _customer; set { _customer = value; RaisePropertyChanged(); } }
        public int NumOfPeole { get => _numOfPeople; set { _numOfPeople = value; RaisePropertyChanged(); } }
        public int ServiceCost { get => _serviceCost; set { _serviceCost = value; RaisePropertyChanged(); TotalCost = calcTotal(); } }
        public int RoomCost { get => _roomCost; set { _roomCost = value; RaisePropertyChanged(); TotalCost = calcTotal(); } }
        public int Discount { get => _discount; set { _discount = value;RaisePropertyChanged(); TotalCost = calcTotal(); } }
        public int TotalCost { get => _totalCost; set { _totalCost = value; RaisePropertyChanged(); } }

        public DateTime StartDate { get => _startDate; set { _startDate = value; RaisePropertyChanged(); calcRoomCost(); } }
        public DateTime EndDate { get => _endDate; set { _endDate = value; RaisePropertyChanged(); calcRoomCost(); } }
        public DateTime StartTime { get => _startTime; set { _startTime = value; RaisePropertyChanged(); calcRoomCost(); } }
        public DateTime EndTime { get => _endTime; set { _endTime = value;RaisePropertyChanged(); calcRoomCost(); } }

        private room GetRoom(int roomID)
        {
            room r;
            using (var db = new hotelEntities())
            {
                r = db.rooms.Include("room_status").Include("room_type").Where<room>(x => x.ID == roomID).FirstOrDefault();
            }
            
            return r;
        }

        private BindingList<service> getFullService()
        {
            BindingList<service> list;
            using(var db = new hotelEntities())
            {
                list = new BindingList<service>(db.services.ToList());
            }
            return list;
        }

        private ICommand _addServiceCommand;
        public ICommand AddServiceCommand
        {
            get
            {
                return _addServiceCommand ??
                    (_addServiceCommand = new RoleBasedSecurityCommand<object>(null, Excute_AddService));
            }
        }

        private void Excute_AddService(object p)
        {
            if (p != null)
            {
                var s = p as service;
                addService(s);
                ServiceCost = calcServiceCost();
            }
        }

        private ICommand _removeServiceCommand;
        public ICommand RemoveServiceCommand
        {
            get
            {
                return _removeServiceCommand ??
                    (_removeServiceCommand = new RoleBasedSecurityCommand<object>(null, Excute_RemoveService));
            }
        }

        private void Excute_RemoveService(object p)
        {
            if (p != null)
            {
                var s = p as service_details;
                removeService(s);
                ServiceCost=calcServiceCost();
            }
        }

        private ICommand _selectCustomerCommand;
        public ICommand SelectCustomerCommand
        {
            get
            {
                return _selectCustomerCommand ??
                    (_selectCustomerCommand = new RoleBasedSecurityCommand<object>(null, Excute_SelectCustomer));
            }
        }

       

        private void Excute_SelectCustomer(object p)
        {
            ChooseCustomer dialog = new ChooseCustomer();
            dialog.ShowDialog();
            if ( System.Windows.Application.Current.Properties["CustomerChoosed"] != null)
            {
                Customer = (customer) System.Windows.Application.Current.Properties["CustomerChoosed"];
            }
        }

        private ICommand _commitCommand;
        public ICommand CommitCommand
        {
            get
            {
                return _commitCommand ??
                    (_commitCommand = new RoleBasedSecurityCommand<object>(null, Excute_Commit));
            }
        }


        private void Excute_Commit(object p)
        {
            

            //Luu booking
            booking newbooking = new booking();

            //newbooking.customer = Customer;

            newbooking.Customer_id = Customer.ID;
            newbooking.IsPaid = false;
            newbooking.Total = TotalCost;
            newbooking.Discount = Discount;

            bookingRepository.Insert(newbooking);
            //newbooking.ID = bookingRepository.GetAll().Last<booking>().ID;

            //insert booking_details
            booking_details details = new booking_details();
            details.Booking_id = newbooking.ID;
            details.Status = 2;
            
            details.Room_id = Room.ID;
            details.NumberOfPeople = NumOfPeole;
            details.Amount = Room.Price;
            details.DateOfRentStart = datePlusTime(StartDate, StartTime);
            details.DateOfRentEnd = datePlusTime(EndDate, EndTime);
            details.hasForeigner = Customer.isForeigner;

            
            bookingDetailRepository.Insert(details);

            //insert service_details
            
            foreach (var item in ListServiceAdded)
            {
                item.BookDetailID = details.ID;
                item.service = null;
                serviceDetailRepository.Insert(item);
            }

            
            System.Windows.Application.Current.Properties["Commit"] = true;
            closeDialog();
        }

        private ICommand _QuitCommand;
        public ICommand QuitCommand
        {
            get
            {
                return _QuitCommand ??
                    (_QuitCommand = new RoleBasedSecurityCommand<object>(null, Excute_Quit));
            }
        }



        private void Excute_Quit(object p)
        {
            closeDialog();
        }

        private int calcRoomCost(DateTime start,DateTime end)
        {
            TimeSpan length = end.Subtract(start);
            if (length.Days<= 0)
            {
                return 0;
            }
            return length.Days * (int)Room.Price;
        }

        private int calcServiceCost()
        {
            int total = 0;
            foreach(var item in ListServiceAdded)
            {
                total += (int)item.Price * item.Number;
            }
            return total;
        }

        private int calcTotal()
        {
            return RoomCost + ServiceCost - Discount;
        }

        private void calcRoomCost()
        {
            RoomCost = calcRoomCost(datePlusTime(StartDate, StartTime), datePlusTime(EndDate, EndTime));
        }

        private DateTime datePlusTime(DateTime date, DateTime time)
        {
            return  new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        private void closeDialog()
        {
            RentRoom dialog = App.Current.Windows.OfType<RentRoom>().FirstOrDefault();
            if (dialog != null)
            {
                Messenger.Default.Unregister<Parameter>(this, res => Room = GetRoom(((int)res.param)));
                dialog.Close();
            }
        }

        private void addService(service serviceItem)
        {
            foreach(var item in ListServiceAdded)
            {
                if(serviceItem.ID == item.ServiceID)
                {
                    item.Number++;
                    var newItem = item;
                    ListServiceAdded.Remove(item);
                    ListServiceAdded.Add(newItem);
                    return;
                }
            }

            service_details details = new service_details();
            details.Date = DateTime.Now;
            details.Number = 1;
            details.Price = (int)serviceItem.Price;
            details.ServiceID = serviceItem.ID;
            details.service = serviceItem;

            ListServiceAdded.Add(details);
        }

        private void removeService(service_details serviceItem)
        {
            serviceItem.Number--;
            ListServiceAdded.Remove(serviceItem);

            if (serviceItem.Number > 0)
            {
                ListServiceAdded.Add(serviceItem);
            }
        }
    }
}
