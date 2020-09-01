using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Input;

namespace Login2.ViewModels.Receptionist
{
    public class CheckOutViewModel : MyBaseViewModel
    {
        private room _room;

        private BindingList<service> _listService;
        private BindingList<service_details> _listServiceAdded;
        private booking _bookingInfo;
        private booking_details _bookingDetails;
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
        private IRepository<booking> bookingRepository = null;
        private IRepository<booking_details> bookingDetailRepository = null;
        private IRepository<service> servicelRepository = null;
        private IRepository<service_details> serviceDetailRepository = null;
        private IRepository<customer> customerRepository = null;

        public CheckOutViewModel()
        {
            roomRepository = new BaseRepository<room>();
            bookingRepository = new BaseRepository<booking>();
            bookingDetailRepository = new BaseRepository<booking_details>();
            serviceDetailRepository = new BaseRepository<service_details>();
            servicelRepository = new BaseRepository<service>();
            customerRepository = new BaseRepository<customer>();

            Messenger.Default.Register<Parameter>(this, res => getData(((int)res.param)));
            ListService = getFullService();
            
            //ListServiceAdded = new BindingList<service_details>(BookingInfo.booking_details.FirstOrDefault().service_details.ToList());

            RoomCost = 0;
            ServiceCost = 0;
            Discount = 0;
            TotalCost = 0;

            //StartDate = new DateTime(DateTime.Now.Ticks);
            //StartTime = new DateTime(DateTime.Now.Ticks);
            //EndDate = new DateTime(DateTime.Now.Ticks);
            //EndTime = new DateTime(DateTime.Now.Ticks);

            

        }


        public room Room { get => _room; set { _room = value; RaisePropertyChanged(); } }
        public BindingList<service> ListService { get => _listService; set { _listService = value; RaisePropertyChanged(); } }
        public BindingList<service_details> ListServiceAdded { get => _listServiceAdded; set { _listServiceAdded = value; RaisePropertyChanged(); ServiceCost = calcServiceCost(); } }
        public booking BookingInfo { get => _bookingInfo; set { _bookingInfo = value; RaisePropertyChanged(); } }
        public booking_details BookingDetails { get => _bookingDetails; set { _bookingDetails = value; RaisePropertyChanged(); } }
        public customer Customer { get => _customer; set { _customer = value; RaisePropertyChanged(); } }
        public int ServiceCost { get => _serviceCost; set { _serviceCost = value; RaisePropertyChanged(); TotalCost = calcTotal(); } }
        public int RoomCost { get => _roomCost; set { _roomCost = value; RaisePropertyChanged(); TotalCost = calcTotal(); } }
        public int Discount { get => _discount; set { _discount = value; RaisePropertyChanged(); TotalCost = calcTotal(); } }
        public int TotalCost { get => _totalCost; set { _totalCost = value; RaisePropertyChanged(); } }

        public DateTime StartDate { get => _startDate; set { _startDate = value; RaisePropertyChanged(); calcRoomCost(); } }
        public DateTime EndDate { get => _endDate; set { _endDate = value; RaisePropertyChanged(); calcRoomCost(); } }
        public DateTime StartTime { get => _startTime; set { _startTime = value; RaisePropertyChanged(); calcRoomCost(); } }
        public DateTime EndTime { get => _endTime; set { _endTime = value; RaisePropertyChanged(); calcRoomCost(); } }

        private void getData(int roomID)
        {
            room r;
            using (var db = new hotelEntities())
            {
                r = db.rooms.Include("room_status").Include("room_type").Where<room>(x => x.ID == roomID).FirstOrDefault();
            }

            Room = r;
            BookingInfo = getBooking(Room.ID);
            BookingDetails = bookingDetailRepository.Get(x=> x.Booking_id==BookingInfo.ID).FirstOrDefault();
            ListServiceAdded = new BindingList<service_details>(getLitServiceDetails(BookingDetails.ID));
            Customer = customerRepository.Get(x => x.ID == BookingInfo.Customer_id).FirstOrDefault();


            StartDate = (DateTime)BookingDetails.DateOfRentStart;
            StartTime = (DateTime)BookingDetails.DateOfRentStart;
            EndDate = (DateTime)BookingDetails.DateOfRentEnd;
            EndTime = (DateTime)BookingDetails.DateOfRentEnd;
        }

        private BindingList<service> getFullService()
        {
            BindingList<service> list;
            using (var db = new hotelEntities())
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
                ServiceCost = calcServiceCost();
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
            if (System.Windows.Application.Current.Properties["CustomerChoosed"] != null)
            {
                Customer = (customer)System.Windows.Application.Current.Properties["CustomerChoosed"];
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
            //Update Booking
            BookingInfo.IsPaid = true;
            BookingInfo.Total = TotalCost;
            BookingInfo.Discount = Discount;

            bookingRepository.Update(BookingInfo);

            //Update Booking Details
            BookingDetails.DateOfRentEnd = datePlusTime(EndDate, EndTime);
            BookingDetails.Status = 3;

            bookingDetailRepository.Update(BookingDetails);


            //delete service_details
            List<service_details> listServiceOld = serviceDetailRepository.Get(x => x.BookDetailID == BookingDetails.ID).ToList();
            foreach (var item in listServiceOld)
            {
                
                serviceDetailRepository.Delete(item);
            }
           
            //insert service_details
            foreach (var item in ListServiceAdded)
            {
                item.BookDetailID = BookingDetails.ID;
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


        private int calcRoomCost(DateTime start, DateTime end)
        {
            TimeSpan length = end.Subtract(start);
            if (length.TotalDays <= 0)
            {
                return 0;
            }
            return (int)length.TotalDays * (int)Room.Price;
        }

        private int calcServiceCost()
        {
            int total = 0;
            foreach (var item in ListServiceAdded)
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
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        private void closeDialog()
        {
            CheckOut dialog = App.Current.Windows.OfType<CheckOut>().FirstOrDefault();
            if (dialog != null)
            {
                Messenger.Default.Unregister<Parameter>(this, res => getData(((int)res.param)));
                dialog.Close();
            }
        }

        private void addService(service serviceItem)
        {
            foreach (var item in ListServiceAdded)
            {
                if (serviceItem.ID == item.ServiceID)
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

        private booking getBooking(int roomID)
        {
            booking bookingResult = new booking();
            using(var db = new hotelEntities())
            {
                var details = db.booking_details.Include("service_details").Where<booking_details>(x => x.Room_id == roomID && x.Status == 2).FirstOrDefault();
                bookingResult = db.bookings.Where<booking>(x => x.ID == details.Booking_id).FirstOrDefault();
            }
            return bookingResult;
        }

        

        private List<service_details> getLitServiceDetails(int bookingDetailID)
        {
            List<service_details> listResult = new List<service_details>();
            using (var db = new hotelEntities())
            {
                listResult = db.service_details.Include("service").Where<service_details>(x => x.BookDetailID== bookingDetailID).ToList();
            }
            return listResult;
        }
    }
}

