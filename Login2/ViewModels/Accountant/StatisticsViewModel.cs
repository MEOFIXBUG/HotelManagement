using GalaSoft.MvvmLight.Command;
using Login2.Auxiliary.Repository;
using Login2.Models;
using Microsoft.Win32;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Login2.ViewModels.Accountant
{
    public class Data
    {
        public string Name { get; set; }

        public double Total { get; set; }

        public double RoomPrice { get; set; }


    }
    public class StatisticsViewModel : MyBaseViewModel
    {
        CultureInfo americanCulture = new CultureInfo("en-US");
        public ObservableCollection<Data> _data { get; set; }
        public ObservableCollection<Data> Data
        {
            get { return _data; }
            set { _data = value; RaisePropertyChanged(); }
        }
        public List<string> Months { get; set; }
        public List<int> Years { get; set; }

        public int _selectedMonthIndex;
        public int SelectedMonthIndex
        {
            get { return _selectedMonthIndex; }
            set { _selectedMonthIndex = value; RaisePropertyChanged(); }
        }
        public int _selectedYearItem;
        public int SelectedYearItem
        {
            get { return _selectedYearItem; }
            set { _selectedYearItem = value; RaisePropertyChanged(); }
        }
        public string _what;
        public string What
        {
            get { return _what; }
            set { _what = value; RaisePropertyChanged(); }
        }
        private IRepository<booking> bookingRepository = null;
        public StatisticsViewModel()
        {
            bookingRepository = new BaseRepository<booking>();
            _selectedMonthIndex = DateTime.Now.Month;
            _selectedYearItem = DateTime.Now.Year;
            Months = americanCulture.DateTimeFormat.MonthNames.Take(12).ToList();
            Months.Insert(0, "All");
            Years= Enumerable.Range(_selectedYearItem - 2, 3).ToList();
            _data = new ObservableCollection<Data>();
            Execute_DrawChart(null);
        }
        private ICommand _drawChartCommand;
        public ICommand DrawChartCommand
        {
            get
            {
                return _drawChartCommand ??
                     (_drawChartCommand = new RelayCommand<object>(Execute_DrawChart, CanExecute_DrawChart));
            }
        }
        private void Execute_DrawChart(object obj)
        {
            Data.Clear();
            if (SelectedMonthIndex == 0)
            {
                What = "Tháng";
                for (int i = 1; i <= 12; i++)
                {
                    var a = bookingRepository.Get(k => k.PaidTime.Value.Year == SelectedYearItem && k.PaidTime.Value.Month == i, null, "booking_details");

                    double b = 0;
                    foreach (var item in a)
                    {
                        b += item.booking_details.Select(x => x.Amount).Sum();
                    }
                    Data.Add(new Data
                    {
                        Name = Months[i],
                        Total = a.Select(x => x.Total).Sum(),
                        RoomPrice = b
                    }); ;
                }
            }
            else
            {
                What = "Ngày";
                int days = DateTime.DaysInMonth(SelectedYearItem, SelectedMonthIndex);
                for (int i = 1; i < days+1; i++)
                {
                    var a = bookingRepository.Get(k => k.PaidTime.Value.Year == SelectedYearItem && k.PaidTime.Value.Month == SelectedMonthIndex && k.PaidTime.Value.Day == i, null, "booking_details");
                    double b = 0;
                    foreach (var item in a)
                    {
                        b += item.booking_details.Select(x => x.Amount).Sum();
                    }
                    Data.Add(new Data
                    {
                        Name = i.ToString(),
                        Total = a.Select(x => x.Total).Sum(),
                        RoomPrice = b
                    }); ;
                }
            }
           
            
        }
        private bool CanExecute_DrawChart(object obj)
        {
            return true;
        }
        private ICommand _printChartCommand;
        public ICommand PrintChartCommand
        {
            get
            {
                return _printChartCommand ??
                     (_printChartCommand = new RelayCommand<object>(Execute_PrintChart, CanExecute_PrintChart));
            }
        }
        private void Execute_PrintChart(object obj)
        {
            var p = (SfChart)obj;
            p.Print();
        }
        private bool CanExecute_PrintChart(object obj)
        {
            return true;
        }

        private ICommand _exportChartCommand;
        public ICommand ExportChartCommand
        {
            get
            {
                return _exportChartCommand ??
                     (_exportChartCommand = new RelayCommand<object>(Execute_ExportChart, CanExecute_ExportChart));
            }
        }
        private void Execute_ExportChart(object obj)
        {
            var p = (SfChart)obj;
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg,*.jpeg)|*.jpg;*.jpeg|Gif (*.gif)|*.gif|PNG(*.png)|*.png|TIFF(*.tif,*.tiff)|*.tif|All files (*.*)|*.*";

            if (sfd.ShowDialog() == true)
            {

                using (Stream fs = sfd.OpenFile())
                {

                    p.Save(fs, new PngBitmapEncoder());

                }

            }
        }
        private bool CanExecute_ExportChart(object obj)
        {
            return true;
        }
    }
}
