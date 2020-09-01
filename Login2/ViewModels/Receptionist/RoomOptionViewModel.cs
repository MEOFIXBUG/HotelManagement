using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Commands;
using Login2.Models;
using Login2.Views.Receptionist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Login2.ViewModels.Receptionist
{
    public class RoomOptionViewModel : MyBaseViewModel
    {
        private room _room;
        private int _status;
        public int Status { get => _status; set { _status = value; RaisePropertyChanged(); } }
        public room Room { get => _room; set => _room = value; }

        private IRepository<room> roomRepository = null;

        public RoomOptionViewModel()
        {
            roomRepository = new BaseRepository<room>();
            Messenger.Default.Register<Parameter>(this, res => GetRoom((int)res.param));
        }

        private void GetRoom(int roomID)
        {
            Room = roomRepository.Get(u => u.ID == roomID).FirstOrDefault();
            Status = Room.Status;
        }


        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ??
                     (_closeWindowCommand = new RelayCommand<object>(Execute_CloseWindow, CanExecute_CloseWindow));
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

        private ICommand _rentRoomCommand;
        public ICommand RentRoomCommand
        {
            get
            {
                return _rentRoomCommand ??
                    (_rentRoomCommand = new RoleBasedSecurityCommand<object>(null, Excute_RentRoom));
            }
        }



        private void Excute_RentRoom(object p)
        {
            System.Windows.Application.Current.Properties["CurrentRoomID"] = _room.ID;
            var rentRoomWindow = new RentRoom();
            //ParameterSetter.SetParameter(_room.ID);
            rentRoomWindow.ShowDialog();
            
            

            if (System.Windows.Application.Current.Properties["Commit"] != null)
            {
                bool isCommit = (Boolean)System.Windows.Application.Current.Properties["Commit"];
                if (isCommit)
                {
                    Room.Status = (int)RoomStatus.rented;
                    roomRepository.Update(Room);
                }
                System.Windows.Application.Current.Properties["Commit"] = null;
            }
            
            ViewModelLocator.RenewRentRoom();
            closeDialog();
        }

        private ICommand _checkOutRoomCommand;
        public ICommand CheckOutRoomCommand
        {
            get
            {
                return _checkOutRoomCommand ??
                    (_checkOutRoomCommand = new RoleBasedSecurityCommand<object>(null, Excute_CheckOutRoom));
            }
        }



        private void Excute_CheckOutRoom(object p)
        {
            System.Windows.Application.Current.Properties["CurrentRoomID"] = _room.ID;
            var checkOutWindow = new CheckOut();
            //ParameterSetter.SetParameter(_room.ID);
            checkOutWindow.ShowDialog();
            if (System.Windows.Application.Current.Properties["Commit"] != null)
            {
                bool isCommit = (Boolean)System.Windows.Application.Current.Properties["Commit"];
                if (isCommit)
                {
                    Room.Status = (int)RoomStatus.available;
                    roomRepository.Update(Room);
                }
                System.Windows.Application.Current.Properties["Commit"] = null;
            }

            //Room.Status = (int)RoomStatus.available;
            //roomRepository.Update(Room);

            ViewModelLocator.RenewCheckOut();
            closeDialog();
        }

        private ICommand _cleanRoomCommand;
        public ICommand CleanRoomCommand
        {
            get
            {
                return _cleanRoomCommand ??
                    (_cleanRoomCommand = new RoleBasedSecurityCommand<object>(null, Excute_CleanRoom));
            }
        }



        private void Excute_CleanRoom(object p)
        {
            if (Room.Status == (int)RoomStatus.available)
            {
                Room.Status = (int)RoomStatus.cleaning;
            }
            else if (Room.Status == (int)RoomStatus.cleaning)
            {
                Room.Status = (int)RoomStatus.available;
            }

            roomRepository.Update(Room);
            roomRepository.Save();
            closeDialog();
        }

        private ICommand _fixRoomCommand;
        public ICommand FixRoomCommand
        {
            get
            {
                return _fixRoomCommand ??
                    (_fixRoomCommand = new RoleBasedSecurityCommand<object>(null, Excute_FixRoom));
            }
        }

        private void Excute_FixRoom(object p)
        {
            if (Room.Status == (int)RoomStatus.available)
            {
                Room.Status = (int)RoomStatus.fixing;
            }
            else if (Room.Status == (int)RoomStatus.fixing)
            {
                Room.Status = (int)RoomStatus.available;
            }
            roomRepository.Update(Room);
            roomRepository.Save();
            closeDialog();
        }



        private void closeDialog()
        {
            RoomOption dialog = App.Current.Windows.OfType<RoomOption>().FirstOrDefault();
            if (dialog != null)
            {
                Messenger.Default.Unregister<Parameter>(this, res => GetRoom((int)res.param));
                dialog.Close();
            }
        }
    }
}
