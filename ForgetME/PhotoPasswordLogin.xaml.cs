using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media;


namespace ForgetME
{
    public partial class PhotoPasswordLogin : PhoneApplicationPage
    {
       


        private int tapCount = 0;
        private Point[] pnts;

        public PhotoPasswordLogin()
        {
            InitializeComponent();
            pnts = new Point[3];
            imgDot1.Visibility = System.Windows.Visibility.Collapsed;
            imgDot2.Visibility = System.Windows.Visibility.Collapsed;
            imgDot3.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void image_onTap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (tapCount > 2)
            {
                MessageBox.Show("Only 3 taps are allowed. Please reset, if you are lost.");
                return;
            }
            pnts[tapCount++] = e.GetPosition(drawingSurface1);
            showTapPoints(pnts[tapCount - 1]);

        }

        private void showTapPoints(Point pnt)
        {
            // show a image in a place.
            if (pnt.X < 20 || pnt.Y < 20)
            {
                // impossible to show points in minus areas.
                return;
            }
            switch (tapCount)
            {
                case 1:
                    imgDot1.Margin = new Thickness(pnt.X-20, pnt.Y-20, 0, 0);
                    imgDot1.Visibility = System.Windows.Visibility.Visible;
                    imgDot1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    imgDot1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    break;
                case 2:
                    imgDot2.Margin = new Thickness(pnt.X - 20, pnt.Y - 20, 0, 0);
                    imgDot2.Visibility = System.Windows.Visibility.Visible;
                    imgDot2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    imgDot2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    break;
                case 3:
                    imgDot3.Margin = new Thickness(pnt.X - 20, pnt.Y - 20, 0, 0);
                    imgDot3.Visibility = System.Windows.Visibility.Visible;
                    imgDot3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    imgDot3.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    break;
                default:
                    break;
            }

            // run a timer to show and hide it automatically.
            //Thread th = new Thread(hideDot);
            //th.Start();
        }

        private void hideDot()
        {
            Thread.Sleep(3000);
            //imgDot.Visibility = System.Windows.Visibility.Collapsed;
        }

        private BitmapImage loadImageFromStorage()
        {
            var bimg = new BitmapImage();
            try
            {
                using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var stream = iso.OpenFile(GlobalSetting.passFilePath, FileMode.Open, FileAccess.Read))
                    {
                        bimg.SetSource(stream);
                    }
                }
            }
            catch (IsolatedStorageException isoex)
            {
                
                throw new Exception(isoex.Message);
            }
          
          
            
            return bimg;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                // load image in the drawing suface.
                ImageBrush imgbe = new ImageBrush();
                imgbe.ImageSource = loadImageFromStorage();
                drawingSurface1.Background = imgbe;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            base.OnNavigatedTo(e);
           
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            PicPassSecurity.PicPass pp = new PicPassSecurity.PicPass(GlobalSetting.settingsExt.OriginalTapPoints, pnts, 50);
            if (pp.CheckPassword())
            {
                NavigationService.Navigate((new Uri("/Assets/NewPivMainList.xaml", UriKind.RelativeOrAbsolute)));
            }
            else// invalid login
            {
                MessageBox.Show("invalid Login, Please try again.");
                resetPass();
            }
        }

        private void btnCancel_tap(object sender, System.EventArgs e)
        {
            resetPass();
        }

        private void resetPass()
        {
            // clear the points saved, instansiate.
            pnts = new Point[3];
            imgDot1.Visibility = System.Windows.Visibility.Collapsed;
            imgDot2.Visibility = System.Windows.Visibility.Collapsed;
            imgDot3.Visibility = System.Windows.Visibility.Collapsed;
            tapCount = 0;
        }






    }
}