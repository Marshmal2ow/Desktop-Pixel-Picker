using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Point = System.Windows.Point;


namespace PixelPicker
{
   


    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Window MagnifierWindow { get; private set; }
        private double _zoomLevel;

        public event PropertyChangedEventHandler PropertyChanged;

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr onj);

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public System.Windows.Media.Color PickedColor { get; set; }

        public bool PickModeOn { get; set; }

        public double ZoomLevel 
        {
            get { return _zoomLevel; }
            set 
            {
                _zoomLevel = value;
                OnPropertyChanged();
            }
        }

        DispatcherTimer Timer { get; set; }
        Point MousePosition { get; set; }
        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();

            Timer = new DispatcherTimer(DispatcherPriority.Send);
            Timer.Interval = TimeSpan.FromMilliseconds(60);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            IntPtr handle = IntPtr.Zero;
            try
            {
                Point cursorPosition = MouseHook.GetCursorPosition();
                myPopup.HorizontalOffset = cursorPosition.X;
                myPopup.VerticalOffset = cursorPosition.Y;

                Window.Top = cursorPosition.Y; 
                Window.Left = cursorPosition.X;

                //Bitmap bitmap;
                //bitmap = new Bitmap(50, 50, PixelFormat.Format32bppArgb);

                //using (Graphics g = Graphics.FromImage(bitmap))
                //{
                //    g.CopyFromScreen((int)cursorPosition.X - 25, (int)cursorPosition.Y - 25, 0, 0, bitmap.Size);
                //}

                //handle = bitmap.GetHbitmap();
                //imgControl.Source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty,
                //    BitmapSizeOptions.FromEmptyOptions());

                //var pixel = bitmap.GetPixel(25, 25);
                //PickedColor = System.Windows.Media.Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                //OnPropertyChanged(nameof(PickedColor));

                //if (MouseHook.GetLeftButtonState() < 0)
                //{
                //    var pixel = bitmap.GetPixel(25, 25);
                //    PickedColor = System.Windows.Media.Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                //    OnPropertyChanged(nameof(PickedColor));
                //}
            }
            catch (Exception)
            { }
            finally
            {
                DeleteObject(handle);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PickModeOn = !PickModeOn;

            if (PickModeOn)
            {
                Timer.Start();
                btnPlay.Content = "Stop pick color";

                Point mousePos = MouseHook.GetCursorPosition();

                Window = new Window();
                Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Window.Top = mousePos.Y;
                Window.Left = mousePos.X;
                Window.Show();

                //myPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.AbsolutePoint;
                //myPopup.HorizontalOffset = mousePos.X;
                //myPopup.VerticalOffset = mousePos.Y;
                //myPopup.IsOpen = true;
            }
            else
            {
                Timer.Stop();
                btnPlay.Content = "Start pick color";
                myPopup.IsOpen = false;
            }
        }
    }
}
