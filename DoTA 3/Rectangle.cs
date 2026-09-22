using DoTA_3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoTA_3
{
    public class Rectangle
    {
        //Свойства класса
        public Point2D P1 { get; private set; }
        public Point2D P2 { get; private set; }
        public Point2D P3 { get; private set; }
        public Point2D P4 { get; private set; }
        //Конструктор класса
        public Rectangle(Point2D start, int width, int height)
        {
            P1 = start;
            P2 = new Point2D(start.X + width, start.Y);
            P3 = new Point2D(start.X + width, start.Y + height);
            P4 = new Point2D(start.X, start.Y + height);
        }
        public void AddX(int x)
        {
            P1.AddX(x);
            P2.AddX(x);
            P3.AddX(x);
            P4.AddX(x);
        }
        public void AddY(int y)
        {
            P1.AddY(y);
            P2.AddY(y);
            P3.AddY(y);
            P4.AddY(y);
        }
    }
}