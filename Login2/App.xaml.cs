using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Login2
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license
            SyncfusionLicenseProvider.RegisterLicense("Mjk2ODUxQDMxMzgyZTMyMmUzMEFOYzB1NnJieDc3QUNkRUQrQ3pUZHVqR0w3UjB5S3BBNW5qdmRsS1RQZ2s9");
        }
      
    }
}
