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

namespace OOP_lab2_Maps
{
    class Human: MapObject
    {
        public PointLatLng point;
        public PointLatLng destinationPoint;
        public GMapMarker humanMarker;

        // событие прибытия
        public event EventHandler passSeated;

        public Human(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }

        public PointLatLng getDestination()
        {
            return destinationPoint;
        }

        public void setPosition(PointLatLng point)
        {
            this.point = point;
        }

        public void moveTo(PointLatLng dest)
        {
            destinationPoint=dest;
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

        public override GMapMarker GetMarker()
        {

            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 40, // ширина маркера
                    Height = 40, // высота маркера  
                    ToolTip = this.GetTitle(),
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Human.png")), // картинка
                    RenderTransform = new TranslateTransform { X = -20, Y = -20 } // картинка
                }
            };
            humanMarker = marker;
            // marker_car.Position = point;
            return marker;

        }

        // обработчик события прибытия такси
        public void CarArrived(object sender, EventArgs e)
        {
            passSeated?.Invoke(this, EventArgs.Empty);
            // TODO : сесть в машину
            MessageBox.Show("Машина прибыла к пассажиру.");
            
        }

        

    }
}
