using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.Repository;
using Login2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Login2.ViewModels.Receptionist
{
    public class CustomerViewModel : MyBaseViewModel
    {
        private List<customer> _listCustomer;
        private customer _customerInfo;
        private string _searchString;
        private string _contentButton;
        private Visibility _addButtonVisbility;
        private Visibility _updateButtonVisbility;
        public List<customer> ListCustomer { get => _listCustomer; set { _listCustomer = value; RaisePropertyChanged(); } }

        public customer CustomerInfo { get => _customerInfo; set { _customerInfo = value; RaisePropertyChanged(); } }
        public string SearchString { get => _searchString; set { _searchString = value; RaisePropertyChanged(); refeshListCustomer(); } }
        public string ContentButton { get => _contentButton; set { _contentButton = value; RaisePropertyChanged(); } }
        public Visibility AddButtonVisbility { get => _addButtonVisbility; set { _addButtonVisbility = value; RaisePropertyChanged(); } }
        public Visibility UpdateButtonVisbility { get => _updateButtonVisbility; set { _updateButtonVisbility = value; RaisePropertyChanged(); } }

        private IRepository<customer> customerRepository = null;
        public CustomerViewModel()
        {
            CustomerInfo = new customer();
            AddButtonVisbility = Visibility.Visible;
            UpdateButtonVisbility = Visibility.Hidden;
            ContentButton = "Thêm khách hàng";

            customerRepository = new BaseRepository<customer>();
            ListCustomer = customerRepository.GetAll().ToList();
        }

        private ICommand _addCustomerCommand;
        public ICommand AddCustomerCommand
        {
            get
            {
                return _addCustomerCommand ??
                     (_addCustomerCommand = new RelayCommand<object>(Execute_AddCustomer, (x) => true));
            }
        }

        

        private void Execute_AddCustomer(object obj)
        {
            //var newCustomer = obj as customer;
            customerRepository.Insert(CustomerInfo);
            customerRepository.Save();
            CustomerInfo = new customer();
            System.Windows.Forms.MessageBox.Show("Thêm khách hàng mới thành công", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private ICommand _updateCustomerCommand;
        public ICommand UpdateCustomerCommand
        {
            get
            {
                return _updateCustomerCommand ??
                     (_updateCustomerCommand = new RelayCommand<object>(Execute_UpdateCustomer, (x) => true));
            }
        }



        private void Execute_UpdateCustomer(object obj)
        {
            updateCustomer(CustomerInfo);

            UpdateButtonVisbility = Visibility.Hidden;
            AddButtonVisbility = Visibility.Visible;
            CustomerInfo = new customer();
            System.Windows.Forms.MessageBox.Show("Cập nhật thành công", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ??
                     (_selectionChangedCommand = new RelayCommand<object>(Execute__selectionChanged, (x) => true));
            }
        }


        private void Execute__selectionChanged(object obj)
        {
            if (obj != null)
            {
                CustomerInfo = (customer)obj;
                UpdateButtonVisbility = Visibility.Visible;
                AddButtonVisbility = Visibility.Hidden;
            }
        }


        private List<customer> searchCustomer(string customerName)
        {
            return customerRepository.Get((x) => x.FullName.Contains(customerName)).ToList();
        }

        private void refeshListCustomer()
        {
            ListCustomer = searchCustomer(SearchString);
        }

        private void updateCustomer(customer customer)
        {
            customerRepository.Update(customer);
            customerRepository.Save();
            

        }
    }
}
