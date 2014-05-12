using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;

namespace ForgetME
{
    public partial class PhotoPassword : PhoneApplicationPage
    {
        private int tapCount = 0;
        private Point[] pnts;


        public PhotoPassword()
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
            switch (tapCount)
            {
                case 1:
                    imgDot1.Margin = new Thickness(pnt.X - 20, pnt.Y - 20, 0, 0);
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

        private void loadPoints(Point[] pnts)
        {
            int loopCount = 0;
            foreach (var pnt in pnts)
            {


                switch (loopCount++)
                {
                    case 0:
                        imgDot1.Margin = new Thickness(pnt.X - 20, pnt.Y - 20, 0, 0);
                        imgDot1.Visibility = System.Windows.Visibility.Visible;
                        imgDot1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        imgDot1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        break;
                    case 1:
                        imgDot2.Margin = new Thickness(pnt.X - 20, pnt.Y - 20, 0, 0);
                        imgDot2.Visibility = System.Windows.Visibility.Visible;
                        imgDot2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        imgDot2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        break;
                    case 2:
                        imgDot3.Margin = new Thickness(pnt.X - 20, pnt.Y - 20, 0, 0);
                        imgDot3.Visibility = System.Windows.Visibility.Visible;
                        imgDot3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        imgDot3.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        break;
                    default:
                        break;
                }

            }

        }

        private void hideDot()
        {
            Thread.Sleep(3000);
            
        }

        private void btnCancel_click(object sender, System.EventArgs e)
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


        private void btnDone_click(object sender, System.EventArgs e)
        {
            GlobalSetting.originalTapPoints = pnts;

            NavigationService.GoBack();
        }
        private BitmapImage loadImageFromStorage()
        {
            var bimg = new BitmapImage();
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = iso.OpenFile(GlobalSetting.passFilePath, FileMode.Open, FileAccess.Read))
                {
                    bimg.SetSource(stream);
                }
            }
            return bimg;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // load image in the drawing suface.
            ImageBrush imgbe = new ImageBrush();
            imgbe.ImageSource = loadImageFromStorage();
            drawingSurface1.Background = imgbe;
            //load points
            if (GlobalSetting.settingsExt.OriginalTapPoints != null)
            {
                loadPoints(GlobalSetting.settingsExt.OriginalTapPoints);
            }
            base.OnNavigatedTo(e);
        }

    }
}