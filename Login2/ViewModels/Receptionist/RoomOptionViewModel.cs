using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class RoomOptionViewModel :MyBaseViewModel 
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
            var rentRoomWindow = new RentRoom();
            ParameterSetter.SetParameter(_room.ID);
            rentRoomWindow.ShowDialog();
            closeDialog();
        }

        private void closeDialog()
        {
            RoomOption dialog = App.Current.Windows.OfType<RoomOption>().FirstOrDefault();
            if (dialog != null)
            {
                dialog.Close();
            }
        }
    }
}
