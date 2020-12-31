
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using DarrenLee.Media;
using Login2.Auxiliary.DomainObjects;
using Login2.Auxiliary.Helpers;
using Login2.Auxiliary.WebAPIRequest;
using Newtonsoft.Json;

namespace Login2.Auxiliary.Scanner
{
    /// <summary>
    /// Interaction logic for CaptureScreen.xaml
    /// </summary>
    public partial class CaptureScreen : Window
    {
        Camera myCam = new Camera();
        public CaptureScreen()
        {
            InitializeComponent();
            GetInfo();
            myCam.OnFrameArrived += myCam_OnFrameArrived;
        }

        private void myCam_OnFrameArrived(object source, FrameArrivedEventArgs e)
        {
            Image img = e.GetFrame();
            pictureBoxLoading.Image = img;

        }

        private void GetInfo()
        {
            var cameraDevices = myCam.GetCameraSources();
            var cameraResolution = myCam.GetSupportedResolutions();
            foreach (var d in cameraDevices)
            {
                TypeCamera.Items.Add(d);
            }
            foreach (var r in cameraResolution)
            {
                Resolution.Items.Add(r);
            }
            TypeCamera.SelectedIndex = 0;
            Resolution.SelectedIndex = 0;
        }

        private void TypeCamera_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            myCam.ChangeCamera(TypeCamera.SelectedIndex);
        }

        private void Resolution_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            myCam.Start(Resolution.SelectedIndex);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            myCam.Stop();
        }

        private void Capture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                FPTApiRequest fPTApiRequest = new FPTApiRequest();
                var name = $"{Guid.NewGuid()}.jpg";
                pictureBoxLoading.Image.Save(name, ImageFormat.Jpeg);
                var a = fPTApiRequest.Post("idr/vnm", name);
                File.Delete(name);
                var strdata = JsonConvert.DeserializeObject<InfoCMT>(a);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
