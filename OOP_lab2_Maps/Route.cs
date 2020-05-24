using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;

namespace OOP_lab2_Maps
{
    class Route : MapObject
    {

        public List<PointLatLng> points;

        public Route(string title, List<PointLatLng> points) : base(title)
        {
            this.points = new List<PointLatLng>();
            foreach(PointLatLng p in points)
            {
                this.points.Add(p);
            }

        }
        public List<PointLatLng> GetPoints()
        {
            return points;
        }

        public override double GetDistance(PointLatLng point)
        {
            var C = new Class1();
            double distance = C.GetMinDistance(points[0], points[1], point);
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == points.Last())
                {
                    if (C.GetMinDistance(points[i], points[0], point) < distance)
                    {
                        distance = C.GetMinDistance(points[i], points[0], point);
                    }
                }
                else
                {
                    if (C.GetMinDistance(points[i], points[i + 1], point) < distance)
                    {
                        distance = C.GetMinDistance(points[i], points[i + 1], point);
                    }
                }
            }
            return distance;
        }


       

     
        public override PointLatLng GetFocus()
        {
            return points[0];
        }

        public override GMapMarker GetMarker()
        {

            GMapMarker marker = new GMapRoute(points)
            {
                Shape = new Path()
                {
                    Stroke = Brushes.Blue, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    StrokeThickness = 3 // толщина обводки   
                }
            };

            // marker_car.Position = point;
            return marker;

        }
    }
}
