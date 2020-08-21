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
using System.Windows.Forms;
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
        private int _roleId;
        public int RoleID
        {
            get { return _roleId; }
            set { _roleId = value; RaisePropertyChanged(); }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(); }
        }
        private IRepository<staff> staffRepository = null;
        private IRepository<account> accountRepository = null;
        public InsertStaffViewModel()
        {
            staffRepository=new BaseRepository<staff>();
            accountRepository = new BaseRepository<account>();
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
            UserName = ExtraFunction.generateUserName(p);
            var acc = new account
            {
                UserName = UserName,
                Password = p.IdentityCard,
                Role = RoleID + 1,
            };
            accountRepository.Insert(acc);
            accountRepository.Save();
            //insert staff
            p.Account_id = acc.ID;
            staffRepository.Insert(p);
            staffRepository.Save();
            Visibility = Visibility.Visible;
            System.Windows.Forms.MessageBox.Show("Successfully Auto Created", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private bool CanExecute_AddStaff(object obj)
        {
            return true;
        }
    }
}
