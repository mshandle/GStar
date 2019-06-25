
namespace Test
{
    public struct Point
    {
        public int x;
        public int y;
    }

    public class Line
    {
        public Point Star;
        public Point end;

        public Line(Point x, Point y)
        {
            this.Star = x;
            this.end = y;
        }
    }

    public class Rect
    {
        public Point point;
        public float x;
        public float y;

        public Rect(Point point, float x, float y)
        {
            this.point = point;
            this.x = x;
            this.y = y;
        }
    }
}

/// <summary>
/// 
/// </summary>
namespace Test2
{
    public abstract class Sharp
    {
        public abstract void Draw(Graphic drawer);
    }
    public struct Point
    {
        public int x;
        public int y;
    }

    public class Line : Sharp
    {
        public Point Star;
        public Point end;

        public Line(Point x, Point y)
        {
            this.Star = x;
            this.end = y;
        }
        public override void Draw(Graphic drawer) 
        {
            drawer.DrawLine(Star, end);
        }
    }

    public class Rect:Sharp
    {
        public Point point;
        public float x;
        public float y;

        public Rect(Point point, float x, float y)
        {
            this.point = point;
            this.x = x;
            this.y = y;
        }

        public override void Draw(Graphic drawer)
        {
            drawer.DrawRectangel(point,x, y);
        }
    }
}

