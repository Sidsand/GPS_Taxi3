using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using System.Device.Location;

namespace OOP_lab2_Maps
{
    
        abstract class MapObject
        {
            public string title;
            public DateTime date;

            public MapObject(string title)
            {
                this.title = title;
                date = DateTime.Now;
            }

            public string GetTitle()
            {
                return title;
            }

             public DateTime GetCreationData()
            {
                return date;
            }


            abstract public double GetDistance(PointLatLng point); 
           
            abstract public PointLatLng GetFocus();

            abstract public GMapMarker GetMarker();

        }
    
}
