using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Device.Location;
using System.Windows.Media;
using GMap.NET.MapProviders;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace OOP_lab2_Maps
{
    class Car : MapObject
    {
        public PointLatLng point;
        public Route route;
        private Human pass;
        public event EventHandler Follow;
        Thread newThread;

        // событие прибытия
        public event EventHandler Arrived;
        

        GMapMarker carMarker;
        public Car(string title, PointLatLng point) : base(title)
        {
            this.point = point;
            pass = null;
        }

        public void setPass(Human pass)
        {
            this.pass = pass;
        }

        public override double GetDistance(PointLatLng point)
        {
            GeoCoordinate p1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate p2 = new GeoCoordinate(this.point.Lat, this.point.Lng);

            return p1.GetDistanceTo(p2);
        }

        public override PointLatLng GetFocus()
        {
            return point;
        }

        public void setPosition(PointLatLng point)
        {
            this.point = point;
        }

        public override GMapMarker GetMarker()
        {

            GMapMarker marker= new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 40, // ширина маркера
                    Height = 40, // высота маркера  
                    ToolTip = this.GetTitle(),
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Car.png")), // картинка
                    RenderTransform = new TranslateTransform { X = -20, Y = -20 } // картинка
                }
            };
            carMarker = marker;
           // marker_car.Position = point;
            return marker;

        }

        public GMapMarker moveTo(PointLatLng dest)
        {
            // провайдер навигации
            RoutingProvider routingProvider = GMapProviders.OpenStreetMap;
            // определение маршрута
            MapRoute route = routingProvider.GetRoute(
             point, // начальная точка маршрута
             dest, // конечная точка маршрута
             false, // поиск по шоссе (false - включен)
             false, // режим пешехода (false - выключен)
             (int)15);
            // получение точек маршрута
            List<PointLatLng> routePoints = route.Points;

            this.route = new Route("", routePoints);

            newThread = new Thread(new ThreadStart(MoveByRoute));
            newThread.Start();
          

            return this.route.GetMarker();
        }

        private void MoveByRoute()
        {
            // последовательный перебор точек маршрута
            foreach (var point in route.GetPoints())
            {
                // делегат, возвращающий управление в главный поток
                Application.Current.Dispatcher.Invoke(delegate {
                    // изменение позиции маркера
                    carMarker.Position = point;
                    this.point = point;
                    if (pass != null)
                    {
                        pass.setPosition(point);
                        pass.humanMarker.Position = point;
                        Follow?.Invoke(this, null);
                    }
                });
                // задержка 500 мс
                Thread.Sleep(500);
            }
            if (pass == null)
            {
                // отправка события о прибытии после достижения последней точки маршрута
                Arrived?.Invoke(this, null);
          //      MessageBox.Show("Пункт назначения.");
            }
            else
            {
                MessageBox.Show("Вы прибыли в пункт назначения.");
                newThread.Abort();
                pass = null;
               
            }
          

        }

        
    }
}
