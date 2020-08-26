using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Commands;
using Login2.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Login2.ViewModels.Sales
{
    class InsertRoomViewModel : MyBaseViewModel
    {
        private room _room;

        public room Room
        {
            get { return _room; }
            set { _room = value; RaisePropertyChanged(); }
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
        private IRepository<room> roomRepository = null;
        private IRepository<room_status> roomStatusRepository = null;
        private IRepository<room_type> roomTypeRepository = null;
        public InsertRoomViewModel()
        {
            roomRepository = new BaseRepository<room>();
            roomStatusRepository = new BaseRepository<room_status>();
            roomTypeRepository = new BaseRepository<room_type>();
            _roomStatusList = roomStatusRepository.GetAll();
            _roomTypeList = roomTypeRepository.GetAll();
            _room = new room();
        }
        private ICommand _addRoomCommand;
        public ICommand AddRoomCommand
        {
            get
            {
                return _addRoomCommand ??
                     (_addRoomCommand = new RoleBasedSecurityCommand<object>(CanExecute_AddRoom, Execute_AddRoom));
            }
        }

        private bool CanExecute_AddRoom(object arg)
        {
            var p = (room)arg;
            if (p == null || p.HasErrors) return false;
            return true;
        }

        private void Execute_AddRoom(object obj)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var p = (room)obj;
                p.Status = SelectedStatus;
                p.Type = SelectedType;
                roomRepository.Insert(p);
                roomRepository.Save();
                System.Windows.Forms.MessageBox.Show("Successfully Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private ICommand _addRoomExcelCommand;
        public ICommand AddRoomExcelCommand
        {
            get
            {
                return _addRoomExcelCommand ??
                     (_addRoomExcelCommand = new RoleBasedSecurityCommand<object>(CanExecute_AddRoomExcel, Execute_AddRoomExcel));
            }
        }

        private bool CanExecute_AddRoomExcel(object arg)
        {
            return true;
        }

        private void Execute_AddRoomExcel(object obj)
        {
            
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {

                if (dlg.FileName != null)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    //doc excel.
                    using (ExcelPackage package = new ExcelPackage(new FileInfo($"{dlg.FileName}")))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets.FirstOrDefault();
                        // get infoTeam from excel 
                        var newcollection = worksheet.ConvertSheetToObjects<room>();
                        int length = newcollection.Count();
                        var count = 0;
                        foreach (var item in newcollection)
                        {
                            if (!item.HasErrors)
                            {
                                roomRepository.Insert(item);
                                roomRepository.Save();
                                count++;
                            }
                        }
                        string report = $" Tổng cộng: {length}\n Thành công: {count} \n Thất bại: {length - count}";
                        System.Windows.Forms.MessageBox.Show(report, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }

                
                //ghi data


            }
        }

        //MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
        //if (messageBoxResult == MessageBoxResult.Yes)
        //{
        //    var p = (room)obj;
        //    p.Status = SelectedStatus;
        //    p.Type = SelectedType;
        //    roomRepository.Insert(p);
        //    roomRepository.Save();
        //    System.Windows.Forms.MessageBox.Show("Successfully Saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}


    }
}
