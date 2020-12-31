using Login2.Auxiliary.DomainObjects;
using Login2.Auxiliary.Scanner;
using Login2.Auxiliary.WebAPIRequest;
using Login2.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Login2
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public App()
        {
            CaptureScreen a = new CaptureScreen();
            a.Show();
            //FPTApiRequest fPTApiRequest = new FPTApiRequest();

            //var a = fPTApiRequest.Post("idr/vnm", "D:\\yyy.jpg");

            //var strdata = JsonConvert.DeserializeObject<InfoCMT>(a);
            //object o = new { image = File.OpenRead("C:\\Users\\doans\\Downloads\\UseCaseDiagram1.jpg") };

            //using (var ctx = new hotelEntities())
            //{
            //    var DataBaseScript = ((IObjectContextAdapter)ctx).ObjectContext.CreateDatabaseScript();
            //    SaveFileDialog savefile = new SaveFileDialog();
            //    // set a default file name 
            //    savefile.FileName = "unknown.txt";
            //    // set filters - this can be done in properties as well 
            //    savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            //    if (savefile.ShowDialog().Value)
            //    {
            //        using (StreamWriter sw = new StreamWriter(savefile.FileName))
            //            sw.WriteLine(DataBaseScript);
            //    }
            //}
            //Register Syncfusion license
            SyncfusionLicenseProvider.RegisterLicense("Mjk2ODUxQDMxMzgyZTMyMmUzMEFOYzB1NnJieDc3QUNkRUQrQ3pUZHVqR0w3UjB5S3BBNW5qdmRsS1RQZ2s9");
        }

    }
}
