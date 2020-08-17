using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Commands;
using Login2.Views.HumanResources;
using Microsoft.Practices.ServiceLocation;
using Syncfusion.UI.Xaml.Grid;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Login2.ViewModels.HumanResources
{
    public class OrderInfo
    {
        int orderID;
        string customerId;
        string country;
        string customerName;
        string shippingCity;

        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public string CustomerID
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string ShipCity
        {
            get { return shippingCity; }
            set { shippingCity = value; }
        }

        public OrderInfo(int orderId, string customerName, string country, string customerId, string shipCity)
        {
            this.OrderID = orderId;
            this.CustomerName = customerName;
            this.Country = country;
            this.CustomerID = customerId;
            this.ShipCity = shipCity;
        }
    }
    public class StaffListViewModel : MyBaseViewModel
    {
        //private List<string> _allRole;

        //public List<string> AllRole
        //{
        //    get { return _allRole; }
        //    set { _allRole = value; RaisePropertyChanged(); }
        //}

        private ObservableCollection<OrderInfo> _orders;
        public ObservableCollection<OrderInfo> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
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

        private void GenerateOrders()
        {
            _orders.Add(new OrderInfo(1001, "Maria Anders", "Germany", "ALFKI", "Berlin"));
            _orders.Add(new OrderInfo(1002, "Ana Trujilo", "Mexico", "ANATR", "Mexico D.F."));
            _orders.Add(new OrderInfo(1003, "Antonio Moreno", "Mexico", "ANTON", "Mexico D.F."));
            _orders.Add(new OrderInfo(1004, "Thomas Hardy", "UK", "AROUT", "London"));
            _orders.Add(new OrderInfo(1005, "Christina Berglund", "Sweden", "BERGS", "Lula"));
            _orders.Add(new OrderInfo(1006, "Hanna Moos", "Germany", "BLAUS", "Mannheim"));
            _orders.Add(new OrderInfo(1007, "Frederique Citeaux", "France", "BLONP", "Strasbourg"));
            _orders.Add(new OrderInfo(1008, "Martin Sommer", "Spain", "BOLID", "Madrid"));
            _orders.Add(new OrderInfo(1009, "Laurence Lebihan", "France", "BONAP", "Marseille"));
            _orders.Add(new OrderInfo(1010, "Elizabeth Lincoln", "Canada", "BOTTM", "Tsawassen"));
        }
        public StaffListViewModel()
        {
            _orders = new ObservableCollection<OrderInfo>();
            this.GenerateOrders();
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
            var p = (OrderInfo)arg;
            if (p == null) return false;
            return true;
        }
        [AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_UpdateRoleView(object obj)
        {
            var p = (OrderInfo)obj;
            var index = _orders.IndexOf(p);
            var popUpRole = new PopUpRole();
            ParameterSetter.SetParameter(p.OrderID);
            popUpRole.ShowDialog();
        }
    }

}
