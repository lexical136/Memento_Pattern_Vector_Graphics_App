﻿using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Assignment_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Start");
            Console.WriteLine("Commands:");
            Console.WriteLine("        H            Help - displays this message");
            Console.WriteLine("        A            Add <shape> to canvas");
            Console.WriteLine("        U            Update a shape");
            Console.WriteLine("        Un           Undo last operation");
            Console.WriteLine("        Re           Redo last operation");
            Console.WriteLine("        C            Clear canvas");
            Console.WriteLine("        De           Delete shape");
            Console.WriteLine("        S            Save");
            Console.WriteLine("        D            Display");
            Console.WriteLine("        Q            Quit application");
            ArrayList svgList = new ArrayList(); //makes a list to store the shapes
            Stack svgListUndo = new Stack();  //Stack that stores old arraylists to revert back to
            Stack svgListRedo = new Stack();  //Stores undo actions to redo
            
            int z=0;
            bool loop = true;
            while(loop==true){ //a loop to continualy take commands
            //Console.WriteLine("Commands: add, delete, update, display, export"); //all commands that can be used

            String baseRequest = (Console.ReadLine()).ToLower(); //input command
            String request="";
            switch(baseRequest){ //selects the enterd command
                case "a":
                svgListUndo.Push(new ArrayList(svgList));
                Console.WriteLine("Shape to add? (rectangle, circle, ellipse, line, polyline, polygon)");
                request = Console.ReadLine();
                svgListRedo.Clear();
                svgList = factory(request, svgList); //calling a method to make the shape and add to the list
                break;

                case "u":
                svgListUndo.Push(new ArrayList(svgList));
                Console.WriteLine("Change shape to?");
                request = Console.ReadLine();
                svgListRedo.Clear();
                requestInterpU(request, svgList); //calling a method to make changes to a exisiting shape
                break;

                case "de":
                svgListUndo.Push(new ArrayList(svgList));
                Console.WriteLine("Layer to delete?");
                int layer = Convert.ToInt32(Console.ReadLine());
                svgListRedo.Clear();
                shapeDelete(layer, svgList); //calling a method to delete a exisiting shape
                break;

                case "s":
                Console.WriteLine("File name");
                string fileName=Console.ReadLine()+".svg";
                export(fileName, svgList); //calling a method to construct the svg file using the list
                Console.WriteLine("File saved");
                break;

                case "d":
                consoleDisplay(svgList); //method for displying the svg in console
                break;

                case "c": //clears the canvas
                svgListUndo.Push(new ArrayList(svgList));
                svgList.Clear();
                Console.WriteLine("Canvas cleared!");
                break;

                case "h": //displays the list items
                Console.WriteLine("Commands:");
                Console.WriteLine("        H            Help - displays this message");
                Console.WriteLine("        A            Add <shape> to canvas");
                Console.WriteLine("        U            Update a shape");
                Console.WriteLine("        Un           Undo last operation");
                Console.WriteLine("        Re           Redo last operation");
                Console.WriteLine("        C            Clear canvas");
                Console.WriteLine("        De           Delete shape");
                Console.WriteLine("        S            Save");
                Console.WriteLine("        D            Display");
                Console.WriteLine("        Q            Quit application");
                break;

                case "un": //undoes actions
                if(svgListUndo.Count>0){
                    svgListRedo.Push(svgList);
                    svgList=(ArrayList)svgListUndo.Pop();
                    Console.WriteLine("Undo completed");
                }
                else{
                    Console.WriteLine("No command to undo");
                }
                break;

                case "re": //redoes actions
                if(svgListRedo.Count>0){
                    svgListUndo.Push(svgList);
                    svgList=(ArrayList)svgListRedo.Pop();
                    Console.WriteLine("Redo completed");
                }
                else{
                    Console.WriteLine("No command to redo");
                }
                break;

                case "q": //ends the program
                loop=false;
                Console.WriteLine("Goodbye!");
                break;

                default:
                Console.WriteLine("Invalid command!");
                break;
            }}
            z++;
        }

        public static void export(string fileName, ArrayList svgList){ //constructs the SVG file then writes to it
            if(!File.Exists(fileName)){
                using(StreamWriter sw = File.CreateText(fileName)){
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<html>");
                sw.WriteLine("<body>");
                sw.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"1000\" height=\"1000\">");
                
                for(int i=0; i<svgList.Count; i++){
                    if(svgList[i] is Rectangle){
                        sw.WriteLine("<"+((Rectangle)svgList[i]).N+" width=\""+((Rectangle)svgList[i]).H+"\" height=\""+((Rectangle)svgList[i]).W+"\" x=\""+((Rectangle)svgList[i]).X+"\" y=\""+((Rectangle)svgList[i]).Y+"\" fill=\""+((Rectangle)svgList[i]).FIL+"\" stroke=\""+((Rectangle)svgList[i]).LIN+"\" />");
                    }
                    if(svgList[i] is Circle){
                        sw.WriteLine("<"+((Circle)svgList[i]).N+" r=\""+((Circle)svgList[i]).R+"\" cx=\""+((Circle)svgList[i]).CX+"\" cy=\""+((Circle)svgList[i]).CY+"\" fill=\""+((Circle)svgList[i]).FIL+"\" stroke=\""+((Circle)svgList[i]).LIN+"\" />");
                    }
                    if(svgList[i] is Ellipse){
                        sw.WriteLine("<"+((Ellipse)svgList[i]).N+" rx=\""+((Ellipse)svgList[i]).RX+"\" ry=\""+((Ellipse)svgList[i]).RY+"\" cx=\""+((Ellipse)svgList[i]).CX+"\" cy=\""+((Ellipse)svgList[i]).CY+"\" fill=\""+((Ellipse)svgList[i]).FIL+"\" stroke=\""+((Ellipse)svgList[i]).LIN+"\" />");
                    }
                    if(svgList[i] is Line){
                        sw.WriteLine("<"+((Line)svgList[i]).N+" x1=\""+((Line)svgList[i]).X1+"\" y1=\""+((Line)svgList[i]).Y1+"\" x2=\""+((Line)svgList[i]).X2+"\" y2=\""+((Line)svgList[i]).Y2+"\" fill=\""+((Line)svgList[i]).FIL+"\" stroke=\""+((Line)svgList[i]).LIN+"\" />");
                    }
                    if(svgList[i] is Polyline){
                        sw.Write("<polyline points=\"");
                        for(int k=0;k<(((Polyline)svgList[i]).XY).Count-1;k++){
                            sw.Write(((Polyline)svgList[i]).XY[k]+","+((Polyline)svgList[i]).XY[k+1]+" ");
                            k++;
                        }
                        sw.Write("\""+/*" fill=\""+((Polyline)svgList[i]).FIL+"\*/" style=\"fill:none; stroke:"+((Polyline)svgList[i]).LIN+";\"/>");
                    }
                    if(svgList[i] is Polygon){
                        sw.Write("<polygon points=\"");
                        for(int k=0;k<(((Polygon)svgList[i]).XY).Count-1;k++){
                            sw.Write(((Polygon)svgList[i]).XY[k]+","+((Polygon)svgList[i]).XY[k+1]+" ");
                            k++;
                        }
                        sw.Write("\""+/*" fill=\""+((Polyline)svgList[i]).FIL+"\*/" style=\"fill:"+((Polygon)svgList[i]).FIL+"; stroke:"+((Polygon)svgList[i]).LIN+";\"/>");
                    }
                }
                sw.WriteLine("</svg>");
                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
                }
            }
            else{
                Console.WriteLine("File alredy exists");
            }
            
        }

        public static void consoleDisplay(ArrayList svgList){ //writes the svg to console
            
        Console.WriteLine("<!DOCTYPE html>");
        Console.WriteLine("<html>");
        Console.WriteLine("<body>");
        Console.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"1000\" height=\"1000\">");
        
        for(int i=0; i<svgList.Count; i++){
            if(svgList[i] is Rectangle){
                Console.WriteLine("<"+((Rectangle)svgList[i]).N+" width=\""+((Rectangle)svgList[i]).H+"\" height=\""+((Rectangle)svgList[i]).W+"\" x=\""+((Rectangle)svgList[i]).X+"\" y=\""+((Rectangle)svgList[i]).Y+"\" fill=\""+((Rectangle)svgList[i]).FIL+"\" stroke=\""+((Rectangle)svgList[i]).LIN+"\" />");
            }
            if(svgList[i] is Circle){
                Console.WriteLine("<"+((Circle)svgList[i]).N+" r=\""+((Circle)svgList[i]).R+"\" cx=\""+((Circle)svgList[i]).CX+"\" cy=\""+((Circle)svgList[i]).CY+"\" fill=\""+((Circle)svgList[i]).FIL+"\" stroke=\""+((Circle)svgList[i]).LIN+"\" />");
            }
            if(svgList[i] is Ellipse){
                Console.WriteLine("<"+((Ellipse)svgList[i]).N+" rx=\""+((Ellipse)svgList[i]).RX+"\" ry=\""+((Ellipse)svgList[i]).RY+"\" cx=\""+((Ellipse)svgList[i]).CX+"\" cy=\""+((Ellipse)svgList[i]).CY+"\" fill=\""+((Ellipse)svgList[i]).FIL+"\" stroke=\""+((Ellipse)svgList[i]).LIN+"\" />");
            }
            if(svgList[i] is Line){
                Console.WriteLine("<"+((Line)svgList[i]).N+" x1=\""+((Line)svgList[i]).X1+"\" y1=\""+((Line)svgList[i]).Y1+"\" x2=\""+((Line)svgList[i]).X2+"\" y2=\""+((Line)svgList[i]).Y2+"\" fill=\""+((Line)svgList[i]).FIL+"\" stroke=\""+((Line)svgList[i]).LIN+"\" />");
            }
            if(svgList[i] is Polyline){
                Console.Write("<polyline points=\"");
                for(int k=0;k<(((Polyline)svgList[i]).XY).Count-1;k++){
                    Console.Write(((Polyline)svgList[i]).XY[k]+","+((Polyline)svgList[i]).XY[k+1]+" ");
                    k++;
                }
                Console.Write("\""+/*" fill=\""+((Polyline)svgList[i]).FIL+"\*/" style=\"fill:none; stroke:"+((Polyline)svgList[i]).LIN+";\"/>");
            }
            if(svgList[i] is Polygon){
                Console.Write("<polygon points=\"");
                for(int k=0;k<(((Polygon)svgList[i]).XY).Count-1;k++){
                    Console.Write(((Polygon)svgList[i]).XY[k]+","+((Polygon)svgList[i]).XY[k+1]+" ");
                    k++;
                }
                Console.Write("\""+/*" fill=\""+((Polyline)svgList[i]).FIL+"\*/" style=\"fill:"+((Polygon)svgList[i]).FIL+"; stroke:"+((Polygon)svgList[i]).LIN+";\"/>");
            }
        }
        Console.WriteLine("</svg>");
        Console.WriteLine("</body>");
        Console.WriteLine("</html>");
        }

        public static ArrayList factory(String request, ArrayList svgList){ //Constructs the shape using random numbers
            string n = "";
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            string fil = "blue";
            string lin = "black";
            ArrayList xy = new ArrayList();
            Random ran = new Random();
            
            request.ToLower();
            switch(request){
                case "rectangle":
                //rectangleC(x, y, width, height)
                n = "rect";
                Console.WriteLine("Rectangle X Coordinate");
                x = Convert.ToInt32(Console.ReadLine());
                //x = ran.Next(1, 500);
                Console.WriteLine("Rectangle Y Coordinate");
                y = Convert.ToInt32(Console.ReadLine());
                //y = ran.Next(1, 500);
                Console.WriteLine("Rectangle Height");
                h = Convert.ToInt32(Console.ReadLine());
                //h = ran.Next(1, 500);
                Console.WriteLine("Rectangle Width");
                //w = ran.Next(1, 500);
                w = Convert.ToInt32(Console.ReadLine());
                Rectangle re = new Rectangle(n,x,y,h,w,lin,fil);
                svgList.Add(re);
                Console.WriteLine("Rectangle added");
                return(svgList);

                case "circle":
                //circleC(r, cx, cy)
                n = "circle";
                Console.WriteLine("Circle X Coordinate");
                //h = ran.Next(1, 500);
                h = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Circle Y Coordinate");
                //y = ran.Next(1, 500);
                y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Circle radius");
                //x = ran.Next(1, 500);
                x = Convert.ToInt32(Console.ReadLine());
                Circle re2 = new Circle(n,x,y,h,lin,fil);
                svgList.Add(re2);
                Console.WriteLine("Circle added");
                return(svgList);

                case "ellipse":
                //Ellipse(rx, ry, cx, cy, z)
                n = "ellipse";
                Console.WriteLine("Ellipse X Coordinate");
                //x = ran.Next(1, 500);
                x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Ellipse Y Coordinate");
                //y = ran.Next(1, 500);
                y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Ellipse RX");
                //h = ran.Next(1, 500);
                h = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Ellipse RY");
                //w = ran.Next(1, 500);
                w = Convert.ToInt32(Console.ReadLine());
                Ellipse re3 = new Ellipse(n,x,y,h,w,lin,fil);
                svgList.Add(re3);
                Console.WriteLine("Ellipse added");
                return(svgList);

                case "line":
                //Line(x1, y1, x2, y2, z)
                n = "line";
                Console.WriteLine("Line X1 Coordinate");
                //x = ran.Next(1, 500);
                x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Line Y1 Coordinate");
                //y = ran.Next(1, 500);
                y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Line X2 Coordinate");
                //h = ran.Next(1, 500);
                h = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Line Y2 Coordinate");
                //w = ran.Next(1, 500);
                w = Convert.ToInt32(Console.ReadLine());
                Line re4 = new Line(n,x,y,h,w,lin,fil);
                svgList.Add(re4);
                Console.WriteLine("Line added");
                return(svgList);

                case "polyline":
                //polylineC(...)
                string temp="";
                bool loop2=true;
                n = "polyline";
                Console.WriteLine("Intial point X1 Coordinate");
                //xy.Add(ran.Next(1, 500));
                xy.Add(Convert.ToInt32(Console.ReadLine()));
                Console.WriteLine("Intial point Y1 Coordinate");
                //xy.Add(ran.Next(1, 500));
                xy.Add(Convert.ToInt32(Console.ReadLine()));
                while(loop2==true){
                    Console.WriteLine("Enter the next point's X Coordinate or type done");
                    temp=Console.ReadLine();
                    if(temp=="done"){
                        loop2=false;
                    }
                    else{
                        //xy.Add(ran.Next(1, 500));
                        xy.Add(Convert.ToInt32(temp));
                        Console.WriteLine("Enter the point's Y Coordinate");
                        //xy.Add(ran.Next(1, 500));
                        xy.Add(Convert.ToInt32(Console.ReadLine()));
                    }
                }
                Polyline re5 = new Polyline(n,xy,lin,fil);
                svgList.Add(re5);
                return(svgList);

                case "polygon":
                //polygonC(...)
                temp="";
                loop2=true;
                n = "polygon";
                Console.WriteLine("Intial point X1 Coordinate");
                //xy.Add(ran.Next(1, 500));
                xy.Add(Convert.ToInt32(Console.ReadLine()));
                Console.WriteLine("Intial point Y1 Coordinate");
                //xy.Add(ran.Next(1, 500));
                xy.Add(Convert.ToInt32(Console.ReadLine()));
                while(loop2==true){
                    Console.WriteLine("Enter the next point's X Coordinate or type done");
                    temp=Console.ReadLine();
                    if(temp=="done"){
                        loop2=false;
                    }
                    else{
                        //xy.Add(ran.Next(1, 500));
                        xy.Add(Convert.ToInt32(temp));
                        Console.WriteLine("Enter the point's Y Coordinate");
                        //xy.Add(ran.Next(1, 500));
                        xy.Add(Convert.ToInt32(Console.ReadLine()));
                    }
                }
                Polygon re6 = new Polygon(n,xy,lin,fil);
                svgList.Add(re6);
                return(svgList);
            }
            Console.WriteLine("Invalid shape!");
            return(svgList);
        }//End of factory

        public static ArrayList requestInterpU(String request, ArrayList svgList){ //updates using random numbers
            string n = "";
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            string fil = "blue";
            string lin = "black";
            ArrayList xy = new ArrayList();
            Random ran = new Random();
            Console.WriteLine("Layer to update?");
            int z = Convert.ToInt32(Console.ReadLine());

            if(z !< svgList.Count-1){
                request="x";
            }
            request.ToLower();
            switch(request){
                case "rectangle":
                //rectangleC(x, y, width, height)
                //Console.WriteLine("Rectangle at Z"+z);
                n = "rect";
                Console.WriteLine("Rectangle X");
                x = ran.Next(1, 500);
                //x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Rectangle Y");
                y = ran.Next(1, 500);
                //y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Rectangle Height");
                h = ran.Next(1, 500);
                //h = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Rectangle Width");
                w = ran.Next(1, 500);
                //w = Convert.ToInt32(Console.ReadLine());
                Rectangle re = new Rectangle(n,x,y,h,w,lin,fil);
                svgList[z]=re;
                Console.WriteLine("Shape updated to rectangle");
                return(svgList);

                case "circle":
                //circleC(r, cx, cy)
                n = "circle";
                Console.WriteLine("circle CX");
                x = ran.Next(1, 500);
                //x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("circle CY");
                y = ran.Next(1, 500);
                //y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("circle radius");
                h = ran.Next(1, 500);
                //h = Convert.ToInt32(Console.ReadLine());
                Circle re2 = new Circle(n,x,y,h,lin,fil);
                svgList[z]=re2;
                Console.WriteLine("Shape updated to circle");
                return(svgList);

                case "ellipse":
                //Ellipse(rx, ry, cx, cy, z)
                n = "ellipse";
                Console.WriteLine("ellipse CX");
                x = ran.Next(1, 500);
                //x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("ellipse CY");
                y = ran.Next(1, 500);
                //y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("ellipse RX");
                h = ran.Next(1, 500);
                //h = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("ellipse RY");
                w = ran.Next(1, 500);
                //w = Convert.ToInt32(Console.ReadLine());
                Ellipse re3 = new Ellipse(n,x,y,h,w,lin,fil);
                svgList[z]=re3;
                Console.WriteLine("Shape updated to ellipse");
                return(svgList);

                case "line":
                //Line(x1, y1, x2, y2, z)
                n = "line";
                Console.WriteLine("line X1");
                x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("line Y1");
                y = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("line X2");
                h = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("line Y2");
                w = Convert.ToInt32(Console.ReadLine());
                Line re4 = new Line(n,x,y,h,w,lin,fil);
                svgList[z]=re4;
                Console.WriteLine("Shape updated to line");
                return(svgList);

                case "polyline":
                //polylineC(...)
                string temp="";
                bool loop2=true;
                n = "polyline";
                Console.WriteLine("Intial point X1");
                xy.Add(ran.Next(1, 500));
                //xy.Add(Convert.ToInt32(Console.ReadLine()));
                Console.WriteLine("Intial point Y1");
                xy.Add(ran.Next(1, 500));
                //xy.Add(Convert.ToInt32(Console.ReadLine()));
                while(loop2==true){
                    Console.WriteLine("Next point X or type done");
                    temp=Console.ReadLine();
                    if(temp=="done"){
                        loop2=false;
                    }
                    else{
                        xy.Add(ran.Next(1, 500));
                        //xy.Add(Convert.ToInt32(temp));
                        Console.WriteLine("Next point Y");
                        xy.Add(ran.Next(1, 500));
                        //xy.Add(Convert.ToInt32(Console.ReadLine()));
                    }
                }
                Polyline re5 = new Polyline(n,xy,lin,fil);
                svgList[z]=re5;
                return(svgList);

                case "polygon":
                //polygonC(...)
                temp="";
                loop2=true;
                n = "polygon";
                Console.WriteLine("Intial point X1");
                xy.Add(ran.Next(1, 500));
                //xy.Add(Convert.ToInt32(Console.ReadLine()));
                Console.WriteLine("Intial point Y1");
                xy.Add(ran.Next(1, 500));
                //xy.Add(Convert.ToInt32(Console.ReadLine()));
                while(loop2==true){
                    Console.WriteLine("Next point X or type done");
                    temp=Console.ReadLine();
                    if(temp=="done"){
                        loop2=false;
                    }
                    else{
                        xy.Add(ran.Next(1, 500));
                        //xy.Add(Convert.ToInt32(temp));
                        Console.WriteLine("Next point Y");
                        xy.Add(ran.Next(1, 500));
                        //xy.Add(Convert.ToInt32(Console.ReadLine()));
                    }
                }
                Polyline re6 = new Polyline(n,xy,lin,fil);
                svgList[z]=re6;
                return(svgList);
            }
            Console.WriteLine("Invalid update!");
            return(svgList);
        }//End of requestInterpU

        public static ArrayList shapeDelete(int z, ArrayList svgList){
            //Console.WriteLine(svgList[z].N);
            if(z < svgList.Count){
                Console.WriteLine("The shape on layer "+z+" was deleted");
                svgList.RemoveAt(z);
            } 
            else{
                Console.WriteLine("No shape on layer "+z);
            }
            
            return(svgList);
        }


        public class Shape{
            public Shape(string n, int x, int y, int w, int h, int z, string lin, string fil){
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
            public int Y { get; set;}
            public int W { get; set;}
            public int H { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }

        public class Rectangle{
            public Rectangle(string n, int x, int y, int w, int h, string lin, string fil){
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
            public int Y { get; set;}
            public int W { get; set;}
            public int H { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }

        //Circle(r, cx, cy, z)
        public class Circle{
            public Circle(string n, int r, int cx, int cy, string lin, string fil){
                N = n;
                R = r;
                CX = cx;
                CY = cy;
                LIN = lin;
                FIL = fil;
            
            }
            public string N { get; set; }
            public int R { get; set;}
            public int CX { get; set; }
            public int CY { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }

        //Ellipse(rx, ry, cx, cy, z)
        public class Ellipse{
            public Ellipse(string n, int rx, int ry, int cx, int cy, string lin, string fil){
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
            public int RY { get; set;}
            public int CX { get; set;}
            public int CY { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }

        //Line(x1, y1, x2, y2, z)
        public class Line{
            public Line(string n, int x1, int y1, int x2, int y2, string lin, string fil){
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
            public int Y1 { get; set;}
            public int X2 { get; set;}
            public int Y2 { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }

        //polyline(x1, y1, x2, y2..., z)
        public class Polyline{
            public Polyline(string n, ArrayList xy, string lin, string fil){
                N = n;
                XY = xy;
                LIN = lin;
                FIL = fil;
            }
            public string N { get; set;}
            public ArrayList XY { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }

        //polygon(x1, y1, x2, y2... connects, z)
        public class Polygon{
            public Polygon(string n, ArrayList xy, string lin, string fil){
                N = n;
                XY = xy;
                LIN = lin;
                FIL = fil;
            }
            public string N { get; set;}
            public ArrayList XY { get; set;}
            public string LIN { get; set;}
            public string FIL { get; set;}
        }
    }
}
