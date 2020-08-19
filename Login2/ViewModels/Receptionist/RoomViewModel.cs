﻿using GalaSoft.MvvmLight.Command;
using Login2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
        }

        private ICommand _LoadAllRoomCommand;

        public ICommand LoadAllRoomCommand
        {
            get
            {
                return _LoadAllRoomCommand ?? 
                    (_LoadAllRoomCommand = new RelayCommand<object>(Excute_LoadAllRoom, CanExcute_LoadAllRoom));
            }
        }

        public List<room> ListAllRoom { get => _listAllRoom;  }
        public int TotalRoom { get => _totalRoom;
            //set 
            //{ 
            //    TotalRoom = value;
            //    RaisePropertyChanged("TotalRoom");
            //} 
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
                Console.Error.WriteLine("load done");
                foreach (var item in _listAllRoom)
                {
                    Console.Error.WriteLine(item.RoomName);
                }
                
            }
        }
    }
}