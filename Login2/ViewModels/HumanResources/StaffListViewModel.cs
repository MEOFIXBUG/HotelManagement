using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Commands;
using Login2.Models;
using Login2.Views.HumanResources;
using Microsoft.Practices.ServiceLocation;
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

namespace Login2.ViewModels.HumanResources
{
    public class StaffListViewModel : MyBaseViewModel
    {
        private staff _selectedItem;

        public staff SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<staff> _staffList;
        public ObservableCollection<staff> StaffList
        {
            get { return _staffList; }
            set { _staffList = value; }
        }
        private IRepository<staff> staffRepository = null;
        private IRepository<account> accountRepository = null;
        public string KeyWord
        {
            get { return _keyVal; }
            set
            {
                _keyVal = value;
                RaisePropertyChanged();
            }
        }
        private string _keyVal;
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

        public StaffListViewModel()
        {
            var UID = (int)LoginViewModel.session.getAccountID();
            staffRepository = new BaseRepository<staff>();
            accountRepository = new BaseRepository<account>();
            var res = staffRepository.Get(s => s.Account_id != UID);
            _staffList = new ObservableCollection<staff>();
            res.Distinct().ToList().ForEach(i => _staffList.Add(i));
            //AllRole = ConvertEnumToList.GetListOfDescription<Roles>();
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
            var UID = (int)LoginViewModel.session.getAccountID();
            var res = staffRepository.Get(s => s.Account_id != UID);
            StaffList.Clear();
            res.Distinct().ToList().ForEach(i => StaffList.Add(i));
        }
        private ICommand _updateRoleViewCommand;
        public ICommand UpdateRoleViewCommand
        {
            get
            {
                return _updateRoleViewCommand ??
                     (_updateRoleViewCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateRoleView, Execute_UpdateRoleView));
            }
        }

        private bool CanExecute_UpdateRoleView(object arg)
        {
            var p = (staff)arg;
            if (p == null) return false;
            return true;
        }
        [AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_UpdateRoleView(object obj)
        {
            var p = (staff)obj;
            var index = _staffList.IndexOf(p);
            var popUpRole = new PopUpRole();
            ParameterSetter.SetParameter(p.Account_id);
            popUpRole.ShowDialog();
        }
        private ICommand _updateInfoStaffCommand;
        public ICommand UpdateInfoStaffCommand
        {
            get
            {
                return _updateInfoStaffCommand ??
                     (_updateInfoStaffCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateInfoStaff, Execute_UpdateInfoStaff));
            }
        }

        private bool CanExecute_UpdateInfoStaff(object arg)
        {
            var p = (staff)arg;
            if (p == null) return false;
            return true;
        }
        [AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_UpdateInfoStaff(object obj)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var p = (staff)obj;
                staffRepository.Update(p);
                staffRepository.Save();
                var selectedAcc = accountRepository.Get(acc => acc.ID == p.Account_id).FirstOrDefault();
                selectedAcc.UserName = ExtraFunction.generateUserName(p);
                accountRepository.Update(selectedAcc);
                accountRepository.Save();
                System.Windows.Forms.MessageBox.Show("Successfully updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
    }

}
