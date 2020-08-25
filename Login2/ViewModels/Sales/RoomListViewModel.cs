using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Commands;
using Login2.Models;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Login2.ViewModels.Sales
{
    public class RoomListViewModel : MyBaseViewModel
    {
        private room _selectedItem;

        public room SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<room> _roomList;
        public ObservableCollection<room> RoomList
        {
            get { return _roomList; }
            set { _roomList = value; }
        }

        private IEnumerable<room_status> _roomStatusList;
        public IEnumerable<room_status> RoomStatusList
        {
            get { return _roomStatusList; }
            set { _roomStatusList = value; }
        }
        private IEnumerable<room_type> _roomTypeList;
        public IEnumerable<room_type> RoomTypeList
        {
            get { return _roomTypeList; }
            set { _roomTypeList = value; }
        }
        private IRepository<room> roomRepository = null;
        private IRepository<room_status> roomStatusRepository = null;
        private IRepository<room_type> roomTypeRepository = null;
        public RoomListViewModel()
        {
            roomRepository = new BaseRepository<room>();
            roomStatusRepository = new BaseRepository<room_status>();
            roomTypeRepository = new BaseRepository<room_type>();
            _roomStatusList = roomStatusRepository.GetAll();
            _roomTypeList = roomTypeRepository.GetAll();
            var res = roomRepository.Get(null, null, "room_status,room_type");
            _roomList = new ObservableCollection<room>();
            res.Distinct().ToList().ForEach(i => _roomList.Add(i));
        }
        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                     (_searchCommand = new RelayCommand<object>(Execute_Search, CanExecute_Search));
            }
        }

        private bool CanExecute_Search(object arg)
        {
            if (KeyWord != null)
            {
                if (KeyWord.Trim().Length != 0) return true;
            }

            return false;
        }

        private void Execute_Search(object obj)
        {
            var p = (SfDataGrid)obj;
            p.SearchHelper.AllowFiltering = true;
            p.SearchHelper.Search(KeyWord);
        }
        private string _keyVal;
        public string KeyWord
        {
            get { return _keyVal; }
            set
            {
                _keyVal = value;
                RaisePropertyChanged();
            }
        }

        private ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ??
                     (_resetCommand = new RelayCommand<object>(Execute_Reset, CanExecute_Reset));
            }
        }

        private bool CanExecute_Reset(object arg)
        {
            return true;
        }

        private void Execute_Reset(object obj)
        {
            var p = (SfDataGrid)obj;
            p.SearchHelper.ClearSearch();
            var res = roomRepository.Get(null, null, "room_status,room_type");
            RoomList.Clear();
            res.Distinct().ToList().ForEach(i => _roomList.Add(i));
        }
        private int _selectedStatus;
        public int SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; RaisePropertyChanged(); }
        }

        private int _selectedType;
        public int SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = value; RaisePropertyChanged(); }
        }

        private ICommand _updateInfoRoomCommand;
        public ICommand UpdateInfoRoomCommand
        {
            get
            {
                return _updateInfoRoomCommand ??
                     (_updateInfoRoomCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateInfoRoom, Execute_UpdateInfoRoom));
            }
        }

        private bool CanExecute_UpdateInfoRoom(object arg)
        {
            var p = (room)arg;
            if (p == null || p.HasErrors) return false;
            return true;
        }
        
        private void Execute_UpdateInfoRoom(object obj)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var p = (room)obj;
                p.Status = SelectedStatus;
                p.Type = SelectedType;
                roomRepository.Update(p);
                roomRepository.Save();
                var res = roomRepository.Get(null, null, "room_status,room_type");
                RoomList.Clear();
                res.Distinct().ToList().ForEach(i => _roomList.Add(i));
                System.Windows.Forms.MessageBox.Show("Successfully updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private ICommand _delRoomCommand;
        public ICommand DelRoomCommand
        {
            get
            {
                return _delRoomCommand ??
                     (_delRoomCommand = new RoleBasedSecurityCommand<object>(CanExecute_DelRoom, Execute_DelRoom));
            }
        }

        private bool CanExecute_DelRoom(object arg)
        {
            if (SelectedItem == null) return false;
            return true;
        }

        private void Execute_DelRoom(object obj)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var p = (room)obj;
                roomRepository.Delete(p);
                var res = roomRepository.Get(null, null, "room_status,room_type");
                RoomList.Clear();
                res.Distinct().ToList().ForEach(i => _roomList.Add(i));
                System.Windows.Forms.MessageBox.Show("Successfully updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
    }
}
