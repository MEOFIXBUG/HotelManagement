using GalaSoft.MvvmLight.Command;
using Login2.Commands;
using Login2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Login2.ViewModels.Receptionist
{
    class RoomViewModel : MyBaseViewModel
    {
        private List<room> _listAllRoom;
        private int _totalRoom;


        public RoomViewModel()
        {
            _listAllRoom = new List<room>();
            _totalRoom = 0;
            Excute_LoadAllRoom(null);
        }

        private ICommand _LoadAllRoomCommand;

        public ICommand LoadAllRoomCommand
        {
            get
            {
                return _LoadAllRoomCommand ?? 
                    (_LoadAllRoomCommand = new RoleBasedSecurityCommand<object>(null, Excute_LoadAllRoom));
            }
        }

        public List<room> ListAllRoom { get => _listAllRoom;
            set
            {
                _listAllRoom = value;
                RaisePropertyChanged("ListAllRoom");
                TotalRoom = _listAllRoom.Count;
            }
        }

        public int TotalRoom { get => _totalRoom;
            set
            {
                TotalRoom = value;
                RaisePropertyChanged("TotalRoom");
            }
        }

        private bool CanExcute_LoadAllRoom(object o)
        {
            return true;
        }

        private void Excute_LoadAllRoom(object p)
        {
            using(var db = new hotelEntities())
            {
                var rooms = from room in db.rooms select room;
                _listAllRoom = rooms.ToList<room>();
                _totalRoom = _listAllRoom.Count;
                RaisePropertyChanged("TotalRoom");
                RaisePropertyChanged("ListAllRoom");
            }
        }
    }
}
