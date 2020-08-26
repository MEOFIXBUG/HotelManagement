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
        private BindingList<service> _listServiceAdded;
        private booking _bookingInfo;
        private customer _customer;

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


        public RentRoomViewModel()
        {
            roomRepository = new BaseRepository<room>();

            Messenger.Default.Register<Parameter>(this, res => Room = GetRoom(((int)res.param)));
            ListService = getFullService();
            BookingInfo = new booking();
            ListServiceAdded = new BindingList<service>();

            RoomCost = 0;
            ServiceCost = 0;
            Discount = 0;
            TotalCost = 0;

            //StartDate = new DateTime();
            //StartTime = new DateTime();
            //EndDate = new DateTime();
            //EndTime = new DateTime();

            //StartDate = DateTime.Now;
            //StartTime = DateTime.Now;
            //EndDate = DateTime.Now;
            //EndTime = DateTime.Now;
        }


        public room Room { get => _room; set { _room = value; RaisePropertyChanged(); } }
        public BindingList<service> ListService { get => _listService; set { _listService = value; RaisePropertyChanged(); } }
        public BindingList<service> ListServiceAdded { get => _listServiceAdded; set { _listServiceAdded = value; RaisePropertyChanged(); } }
        public booking BookingInfo { get => _bookingInfo; set { _bookingInfo = value; RaisePropertyChanged(); } }
        public customer Customer { get => _customer; set { _customer = value; RaisePropertyChanged(); } }
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
            using (var db = new hotelEntities()) {
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
                ListServiceAdded.Add(s);
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
                var s = p as service;
                ListServiceAdded.Remove(s);
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
            //Room.Status = 1;
            //roomRepository.Update(Room);
            //roomRepository.Save();

            System.Windows.Application.Current.Properties["Commit"] = true;
            closeDialog();
        }



        private int calcRoomCost(DateTime start,DateTime end)
        {
            TimeSpan length = end.Subtract(start);

            return length.Days * (int)Room.Price;
        }

        private int calcServiceCost()
        {
            int total = 0;
            foreach(var item in ListServiceAdded)
            {
                total += (int)item.Price;
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
                dialog.Close();
            }
        }
    }
}
