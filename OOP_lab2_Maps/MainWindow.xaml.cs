  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;


namespace OOP_lab2_Maps
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MapObject> objs = new List<MapObject>();

        List<PointLatLng> pts = new List<PointLatLng>();
        Route route = null;
        Human human = null;
        Car car = null;
       // public GMapMarker carMarker = null;
        public GMapMarker humanMarker = null;
        GMapMarker dst;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка провайдера карт
            Map.MapProvider = GMapProviders.YandexMap;

            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);

            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
            if (CHm.IsChecked == true)
            {

                pts.Add(point);

                if (objType.SelectedIndex > -1)
                {
                    if (objType.SelectedIndex == 0)
                    {
                        car = new Car(objTitle.Text, pts[0]);
                        objs.Add(car);
                        if (human != null)
                        {
                            car.Arrived += human.CarArrived;
                            human.passSeated += passSeated;

                        }
                    }
                    if (objType.SelectedIndex == 1)
                    {
                        human = new Human(objTitle.Text, pts[0]);
                        objs.Add(human);

                        if (car != null)
                        {
                            car.Arrived += human.CarArrived;
                            human.passSeated += passSeated;
                        }

                    }
                    if (objType.SelectedIndex == 2)
                    {
                        if (human != null)
                        {
                            human.moveTo(point);

                            dst = new GMapMarker(point)
                            {
                                Shape = new Image
                                {
                                    Width = 40, // ширина маркера
                                    Height = 40, // высота маркера  
                                    ToolTip = "dest",
                                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Loc.png")), // картинка
                                    RenderTransform = new TranslateTransform { X = -20, Y = -20 } // картинка
                                }
                            };
                        }
                    }
                    pts = new List<PointLatLng>();
                }

                Map.Markers.Clear();
                objectList.Items.Clear();

                foreach (MapObject cm in objs)
                {
                    Map.Markers.Add(cm.GetMarker());
                    objectList.Items.Add(cm.GetTitle());
                }

                Map.Markers.Add(dst);
            }
        }

      

        private void CHm_Checked(object sender, RoutedEventArgs e)
        {
            if (CHm.IsChecked == true)
            {
                NCHm.IsChecked = false;
                objTitle.IsEnabled = true;
                objType.IsEnabled = true;
                //AddM.IsEnabled = true;
            }
            
        }

        private void NCHm_Checked(object sender, RoutedEventArgs e)
        {
            if (NCHm.IsChecked == true)
            {
                CHm.IsChecked = false;
                objTitle.IsEnabled = false;
                objType.IsEnabled = false;
               // AddM.IsEnabled = false;
            }
        }

        private void ObjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (objectList.SelectedIndex > -1)
            {
                PointLatLng p = objs[objectList.SelectedIndex].GetFocus();
                Map.Position = p;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            objectList.UnselectAll();
            objectList.Items.Clear();

            foreach (MapObject mapObject in objs)
            {
                if (mapObject.GetTitle().Contains(poisk.Text))
                {
                    objectList.Items.Add(mapObject.GetTitle());
                }
            }
        }

        private void RoutMap_Click(object sender, RoutedEventArgs e)
        {
            if (dst != null)
            {
                Map.Markers.Add(car.moveTo(human.GetFocus()));
                car.Follow += Car_Follow;
            }
            else
            {
                MessageBox.Show("Определите пункт назначения.");
            }
        }

        private void Car_Follow(object sender, EventArgs e)
        {
            Car car = (Car)sender;
            car.Arrived += human.CarArrived;
           

            Map.Position = car.GetFocus();

           
        }

        public void passSeated(object sender, EventArgs e)
        {
            var pass = (Human)sender;
            car.setPass(pass);

            Application.Current.Dispatcher.Invoke(delegate {
                Map.Markers.Add (car.moveTo(pass.getDestination()));
            });
        }
    }
}

