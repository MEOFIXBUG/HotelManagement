using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Helpers;
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
    public class CustomerListViewModel : MyBaseViewModel
    {
        private IRepository<customer> cusRepository = null;

        private ObservableCollection<customer> _cusList;
        public ObservableCollection<customer> CusList
        {
            get { return _cusList; }
            set { _cusList = value; }
        }
        public CustomerListViewModel()
        {
            cusRepository = new BaseRepository<customer>();
            var res = cusRepository.GetAll();
            _cusList = new ObservableCollection<customer>();
            res.Distinct().ToList().ForEach(i => _cusList.Add(i));
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
            var res = cusRepository.GetAll();
            CusList.Clear();
            res.Distinct().ToList().ForEach(i => _cusList.Add(i));
        }
        private ICommand _updateInfoCusCommand;
        public ICommand UpdateInfoCusCommand
        {
            get
            {
                return _updateInfoCusCommand ??
                     (_updateInfoCusCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateInfoCus, Execute_UpdateInfoCus));
            }
        }

        private bool CanExecute_UpdateInfoCus(object arg)
        {
            var p = (customer)arg;
            if (p == null) return false;
            return true;
        }

        private void Execute_UpdateInfoCus(object obj)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Confirmation", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var p = (customer)obj;
                cusRepository.Update(p);
                cusRepository.Save();
                var res = cusRepository.GetAll();
                CusList.Clear();
                res.Distinct().ToList().ForEach(i => _cusList.Add(i));
                System.Windows.Forms.MessageBox.Show("Successfully updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
    }
}
