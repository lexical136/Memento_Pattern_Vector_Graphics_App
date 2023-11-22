using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector_Graphics_App_v2
{
    internal class ShapeClass
    {
        public class Shape
        {

        }

        public class Rectangle : Shape
        {
            public Rectangle(string n, int x, int y, int w, int h, string lin, string fil)
            {
                N = n;
                X = x;
                Y = y;
                W = w;
                H = h;
                LIN = lin;
                FIL = fil;

            }
            public string N { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public string LIN { get; set; }
            public string FIL { get; set; }
        }

        //Circle(r, cx, cy, z)
        public class Circle : Shape
        {
            public Circle(string n, int r, int cx, int cy, string lin, string fil)
            {
                N = n;
                R = r;
                CX = cx;
                CY = cy;
                LIN = lin;
                FIL = fil;

            }
            public string N { get; set; }
            public int R { get; set; }
            public int CX { get; set; }
            public int CY { get; set; }
            public string LIN { get; set; }
            public string FIL { get; set; }
        }

        //Ellipse(rx, ry, cx, cy, z)
        public class Ellipse : Shape
        {
            public Ellipse(string n, int rx, int ry, int cx, int cy, string lin, string fil)
            {
                N = n;
                RX = rx;
                RY = ry;
                CX = cx;
                CY = cy;
                LIN = lin;
                FIL = fil;

            }
            public string N { get; set; }
            public int RX { get; set; }
            public int RY { get; set; }
            public int CX { get; set; }
            public int CY { get; set; }
            public string LIN { get; set; }
            public string FIL { get; set; }
        }

        //Line(x1, y1, x2, y2, z)
        public class Line : Shape
        {
            public Line(string n, int x1, int y1, int x2, int y2, string lin, string fil)
            {
                N = n;
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
                LIN = lin;
                FIL = fil;

            }
            public string N { get; set; }
            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
            public string LIN { get; set; }
            public string FIL { get; set; }
        }

        //polyline(x1, y1, x2, y2..., z)
        public class Polyline : Shape
        {
            public Polyline(string n, List<int> xy, string lin, string fil)
            {
                N = n;
                XY = xy;
                LIN = lin;
                FIL = fil;
            }
            public string N { get; set; }
            public List<int> XY { get; set; }
            public string LIN { get; set; }
            public string FIL { get; set; }
        }

        //polygon(x1, y1, x2, y2... connects, z)
        public class Polygon : Shape
        {
            public Polygon(string n, List<int> xy, string lin, string fil)
            {
                N = n;
                XY = xy;
                LIN = lin;
                FIL = fil;
            }
            public string N { get; set; }
            public List<int> XY { get; set; }
            public string LIN { get; set; }
            public string FIL { get; set; }
        }
    }
}
