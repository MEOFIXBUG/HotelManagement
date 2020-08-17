﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Login2.Auxiliary.Enums;
using Login2.Auxiliary.Helpers;
using Login2.Commands;
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
    public class PopUpRoleViewModel : MyBaseViewModel
    {
        private Dictionary<int, string> _allRole;

        public Dictionary<int, string> AllRole
        {
            get { return _allRole; }
            set { _allRole = value; RaisePropertyChanged(); }
        }
        private int _acountId;
        public int AccountID
        {
            get { return _acountId; }
            set { _acountId = value; RaisePropertyChanged(); }
        }
        public PopUpRoleViewModel()
        {
            Messenger.Default.Register<Parameter>(this,  res =>  Function(res.param));
            _allRole = new Dictionary<int, string>();
            _allRole = Enum.GetValues(typeof(Roles))
                               .Cast<Roles>()
                               .ToDictionary(t => (int)t, t => t.GetDescription());
        }
        private  void Function(object obj)
        {
            AccountID = (int)obj%4;
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
        private ICommand _updateRoleCommand;
        public ICommand UpdateRoleCommand
        {
            get
            {
                return _updateRoleCommand ??
                     (_updateRoleCommand = new RoleBasedSecurityCommand<object>(CanExecute_UpdateRole, Execute_UpdateRole));
            }
        }

        private bool CanExecute_UpdateRole(object arg)
        {
            return true;
        }
        [AuthorizationAttribute(AuthorizationType.Allow, "HumanResources")]
        private void Execute_UpdateRole(object obj)
        {
            var p = (int)obj;
            System.Windows.MessageBox.Show(p.ToString());
        }
    }
}