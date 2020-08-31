using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Commands;
using Login2.Models;
using Login2.Views.Receptionist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Login2.ViewModels.Receptionist
{
    public class RoomViewModel : MyBaseViewModel
    {
        private List<room> _listRoom;
        private int _totalRoom;
        private int _availableRoom;
        private int _rentedRoom;
        private int _cleaningRoom;
        private int _fixingRoom;


        public RoomViewModel()
        {
            ListRoom = new List<room>();
            resetRoom();

            Excute_LoadAllRoom(null);
        }
        

        public List<room> ListRoom
        {
            get => _listRoom;
            set
            {
                _listRoom = value;
                RaisePropertyChanged();
            }
        }

        public int TotalRoom
        {
            get => _totalRoom;
            set
            {
                _totalRoom = value;
                RaisePropertyChanged();
            }
        }

        public int AvailableRoom { get => _availableRoom; set { _availableRoom = value; RaisePropertyChanged(); } }
        public int RentedRoom { get => _rentedRoom; set { _rentedRoom = value; RaisePropertyChanged(); } }
        public int CleaningRoom { get => _cleaningRoom; set { _cleaningRoom = value; RaisePropertyChanged(); } }
        public int FixingRoom { get => _fixingRoom; set { _fixingRoom = value; RaisePropertyChanged(); } }


        #region load All Room
        private ICommand _LoadAllRoomCommand;
        public ICommand LoadAllRoomCommand
        {
            get
            {
                return _LoadAllRoomCommand ??
                    (_LoadAllRoomCommand = new RoleBasedSecurityCommand<object>(null,Excute_LoadAllRoom));
            }
        }


        private void Excute_LoadAllRoom(object p)
        {
            resetRoom();
            ListRoom = getAllRoom();
            foreach (var item in ListRoom)
            {
                RoomStatus status = (RoomStatus)item.Status;
                switch (status)
                {
                    case RoomStatus.available:
                        AvailableRoom++;
                        break;
                    case RoomStatus.rented:
                        RentedRoom++;
                        break;
                    case RoomStatus.cleaning:
                        CleaningRoom++;
                        break;
                    case RoomStatus.fixing:
                        FixingRoom++;
                        break;
                }
            }
        }
        #endregion

        #region load special Room
        private ICommand _loadRoomCommand;
        public ICommand LoadRoomCommand
        {
            get
            {
                return _loadRoomCommand ??
                    (_loadRoomCommand = new RoleBasedSecurityCommand<object>(null,Excute_LoadRoom));
            }
        }

        private void Excute_LoadRoom(object p)
        {
            RoomStatus status = (RoomStatus)Enum.ToObject(typeof(RoomStatus), Convert.ToInt32(p));
            ListRoom = getListRoom(status, RoomType.all);
        }
        #endregion


        private void resetRoom()
        {
            ListRoom.Clear();
            TotalRoom = 0;
            AvailableRoom = 0;
            RentedRoom = 0;
            CleaningRoom = 0;
            FixingRoom = 0;
        }

        private List<room> getListRoom(RoomStatus status, RoomType type)
        {
            List<room> listRoom = new List<room>();
            using (var db = new hotelEntities())
            {
                if(type == RoomType.all)
                {
                    listRoom = db.rooms.Include("room_type").Include("room_status").Where<room>(r => r.Status == (int)status).ToList<room>();
                }
                else
                {
                    listRoom = db.rooms.Include("room_type").Include("room_status").Where<room>(r => r.Status == (int)status && r.Type == (int)type).ToList<room>();
                }
            }
            return listRoom;
        }

        private List<room> getAllRoom()
        {

            List<room> listRoom = new List<room>();
            using (var db = new hotelEntities())
            {
                listRoom = db.rooms.Include("room_type").Include("room_status").ToList<room>();
            }
            return listRoom;

        }

        private ICommand _roomOptionCommand;
        public ICommand RoomOptionCommand
        {
            get
            {
                return _roomOptionCommand ??
                    (_roomOptionCommand = new RoleBasedSecurityCommand<object>(null, Excute_RoomOption));
            }
        }

        private void Excute_RoomOption(object p)
        {
            var room = p as room;
            //MessageBox.Show(room.ID.ToString());
            var roomOptionDialog = new RoomOption();
            ParameterSetter.SetParameter(room.ID);
            roomOptionDialog.ShowDialog();
            Excute_LoadAllRoom(null);
            ViewModelLocator.RenewRoomOption();
        }

    }

}