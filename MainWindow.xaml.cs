using System;
using System.Collections.ObjectModel;
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
using Color = System.Windows.Media.Color;


namespace PixelPicker
{
   


    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        DispatcherTimer Timer { get; set; }
        public ObservableCollection<Color> PickedColors { get; set; } = new ObservableCollection<Color>();
        public bool PickModeOn { get; set; }


        private Color _pickedColor;
        public Color PickedColor 
        { 
            get { return _pickedColor; }
            set 
            { 
                _pickedColor = value;
                OnPropertyChanged();
            } 
        }


        private double _zoomLevel;
        public double ZoomLevel 
        {
            get { return _zoomLevel; }
            set 
            {
                _zoomLevel = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public MainWindow()
        {
            DataContext = this;
            ZoomLevel = 200;

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

                Bitmap bitmap;
                bitmap = new Bitmap(50, 50, PixelFormat.Format32bppArgb);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen((int)cursorPosition.X - 25, (int)cursorPosition.Y - 25, 0, 0, bitmap.Size);
                }

                handle = bitmap.GetHbitmap();
                imgControl.Source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                var pixel = bitmap.GetPixel(25, 25);
                PickedColor = Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);

                if (MouseHook.GetLeftButtonState() < 0)
                {
                    if (!this.IsMouseOver)
                    {
                        PickedColors.Add(PickedColor);
                    }


                }
            }
            catch (Exception)
            { }
            finally
            {
                DeleteObject(handle);
            }
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr onj);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PickModeOn = !PickModeOn;

            if (PickModeOn)
            {
                Timer.Start();

                Point mousePos = MouseHook.GetCursorPosition();
                myPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.AbsolutePoint;
                myPopup.HorizontalOffset = mousePos.X;
                myPopup.VerticalOffset = mousePos.Y;
                myPopup.IsOpen = true;
            }
            else
            {
                Timer.Stop();
                myPopup.IsOpen = false;
            }
        }
    }
}
