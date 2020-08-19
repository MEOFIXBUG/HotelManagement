using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Login2.ViewModels.HumanResources
{
    public class InsertStaffViewModel:MyBaseViewModel
    {
        private Dictionary<int, string> _allRole;

        public Dictionary<int, string> AllRole
        {
            get { return _allRole; }
            set { _allRole = value; RaisePropertyChanged(); }
        }

        private staff _staff;

        public staff Staff
        {
            get { return _staff; }
            set { _staff = value; RaisePropertyChanged(); }
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                RaisePropertyChanged();
            }
        }
        public InsertStaffViewModel()
        {
            _visibility = Visibility.Hidden;
            _allRole = new Dictionary<int, string>();
            _allRole = Enum.GetValues(typeof(Roles))
                               .Cast<Roles>()
                               .ToDictionary(t => (int)t, t => t.GetDescription());
            _staff = new staff();
        }
        private ICommand _addStaffCommand;
        public ICommand AddStaffCommand
        {
            get
            {
                return _addStaffCommand ??
                     (_addStaffCommand = new RelayCommand<object>(Execute_AddStaff, CanExecute_AddStaff));
            }
        }
        private void Execute_AddStaff(object obj)
        {
            var p = (staff)obj;
            //insert account

            //insert staff

            Visibility = Visibility.Visible;
            MessageBox.Show("Tài Khoản Đã Được Tạo Tự Động");
        }
        private bool CanExecute_AddStaff(object obj)
        {
            return true;
        }
    }
}
