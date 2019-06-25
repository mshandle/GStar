using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test {

    public class MouseEvent
    {
        public int touchX;
        public int touchY;
    }

    public class Graphic
    {
        public void DrawLine(Point Star, Point End)
        {

        }

        public void DrawRectangel(Point point,float width, float height)
        {

        }

    }


    public class MainForm : MonoBehaviour
    {
        private List<Line> lines = new List<Line>();

        private List<Rect> rects = new List<Rect>();
        //change
        private bool isCheckLine = false;
        private bool isCheckRect = false;

        private Point LastTouch = new Point();
        public void OnMouseDown(MouseEvent e)
        {
            LastTouch.x = e.touchX;
            LastTouch.y = e.touchX;
        }

        public void OnMouseUp(MouseEvent e)
        {
            Point curTouch = new Point();
            curTouch.x = e.touchX;
            curTouch.y = e.touchY;
            if (isCheckLine)
            {
                lines.Add(new Line(LastTouch, curTouch));
            }

            if (isCheckRect)
            {
                float width = Mathf.Abs(curTouch.x - LastTouch.x);
                float height = Mathf.Abs(curTouch.y - LastTouch.y);
                rects.Add(new Rect(LastTouch, width, height));
            }
        }

        public void OnDraw(Graphic graphic)
        {
            //...
            foreach (var e in lines)
            {
                graphic.DrawLine(e.Star, e.end);
            }

            foreach(var e in rects)
            {
                graphic.DrawRectangel(e.point, e.x, e.y);
            }

            //...
        }
    }

}

namespace Test2
{

    public class MouseEvent
    {
        public int touchX;
        public int touchY;
    }

    public class Graphic
    {
        public void DrawLine(Point Star, Point End)
        {

        }

        public void DrawRectangel(Point point, float width, float height)
        {

        }

    }


    public class MainForm : MonoBehaviour
    {
        private List<Sharp> sharps = new List<Sharp>();

        private bool isCheckLine = false;
        private bool isCheckRect = false;

        private Point LastTouch = new Point();
        public void OnMouseDown(MouseEvent e)
        {
            LastTouch.x = e.touchX;
            LastTouch.y = e.touchX;
        }

        public void OnMouseUp(MouseEvent e)
        {
            Point curTouch = new Point();
            curTouch.x = e.touchX;
            curTouch.y = e.touchY;
            if (isCheckLine)
            {
                sharps.Add(new Line(LastTouch, curTouch));
            }

            if (isCheckRect)
            {
                float width = Mathf.Abs(curTouch.x - LastTouch.x);
                float height = Mathf.Abs(curTouch.y - LastTouch.y);
                sharps.Add(new Rect(LastTouch, width, height));
            }
        }

        public void OnDraw(Graphic graphic)
        {
            //...
            foreach (var e in sharps)
            {
                e.Draw(graphic);
            }
            //...
        }
    }

}