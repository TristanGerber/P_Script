using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading;
using System.Windows;

namespace RAM
{
    /// <summary>
    /// Logique d'interaction pour GraphViewer.xaml
    /// </summary>
    public partial class GraphsViewer : UserControl
    {
        public enum SizeMode
        {
            Zoom = 0,
            Follow = 1,
            Fixed = 2,
        }
        public class PointList<T> : List<T>
        {
            public event EventHandler OnAdd;
            public new void Add(T item)
            {
                base.Add(item);
                if (null != OnAdd)
                    OnAdd(this, null);
            }
        }

        private System.Windows.Media.DrawingContext g;

        public Brush TileColor { get; set; } = Brushes.LightGray;
        public Brush LineColor { get; set; } = Brushes.Black;
        public Brush DotColor { get; set; } = Brushes.Black;
        public bool TileAutoAdjust { get; set; } = true;
        public double TileAdjustUpLimit { get; set; } = 20;
        public double TileAdjustDownLimit { get; set; } = 5;
        public double TileFreqX { get; set; } = 10;
        public double TileFreqY { get; set; } = 10;
        public double MarginY { get; set; } = 0;
        public double MarginX { get; set; } = 0;
        public double LineSize { get; set; } = 2;
        public double DotSize { get; set; } = 3;
        public double MaxY { get; set; } = 100;
        public double MinY { get; private set; } = 0;
        public double GHeight { get; set; } = 100;
        public double MaxX { get; set; } = 100;
        public double MinX { get; private set; } = 0;
        public double GLenght { get; set; } = 100;
        public SizeMode SizeModeY { get; set; } = SizeMode.Zoom;
        public SizeMode SizeModeX { get; set; } = SizeMode.Follow;
        public PointList<Point> Points { get; set; } = new PointList<Point>();

        public GraphsViewer()
        {
            InitializeComponent();
            Points.OnAdd += PointsChanged;
        }

        private void PointsChanged(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                InvalidateVisual();
            });
        }

        private void Update()
        {
            ResizeY();
            ResizeX();
            Draw();
        }

        private void ResizeY()
        {
            if (SizeModeY == SizeMode.Zoom)
            {
                var tempMaxY = GetMaxHeight();

                if (MaxY < tempMaxY)
                    MaxY = tempMaxY;

                GHeight = MaxY - GetMinHeight();
            }
            else if (SizeModeY == SizeMode.Follow)
            {
                var tempMaxY = GetMaxHeight();

                if (MaxY < tempMaxY)
                    MaxY = tempMaxY;
            }
            else if (SizeModeY == SizeMode.Fixed)
            {
            }

            if (GHeight <= 0) GHeight = 1;

            MinY = MaxY - GHeight;
        }

        private void ResizeX()
        {
            if (SizeModeX == SizeMode.Zoom)
            {
                var tempMaxX = GetMaxWidth();

                if (MaxX < tempMaxX)
                    MaxX = tempMaxX;

                GLenght = MaxX - GetMinWidth();
            }
            else if (SizeModeX == SizeMode.Follow)
            {
                var tempMaxX = GetMaxWidth();

                if (MaxX < tempMaxX)
                    MaxX = tempMaxX;
            }
            else if (SizeModeX == SizeMode.Fixed)
            {

            }

            MinX = MaxX - GLenght;
        }
        private double GetMinHeight()
        {
            double min = 0;

            foreach (Point point in Points)
                if (point.Y < min) min = point.Y;

            return min;
        }
        private double GetMaxHeight()
        {
            double max = 0;

            foreach (Point point in Points)
                if (point.Y > max) max = point.Y;

            return max;
        }
        private double GetMinWidth()
        {
            double min = 0;

            foreach (Point point in Points)
                if (point.X < min) min = point.X;

            return min;
        }
        private double GetMaxWidth()
        {
            double max = GLenght;

            foreach (Point point in Points)
                if (point.X > max) max = point.X;

            return max;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            g = drawingContext;
            Update();
        }

        private void Draw()
        {
            if (g != null)
            {
                double tWidth = Width;
                double tHeight = Height;

                double RelativeWidth = tWidth / (GLenght + MarginX);
                double RelativeHeight = tHeight / (GHeight + MarginY);

                int TilesCountY = 0;
                int TilesCountX = 0;

                for (double y = MinY % (TileFreqY * RelativeHeight); y <= tHeight + MinY; y += (TileFreqY * RelativeHeight))
                {
                    g.DrawLine(new Pen(TileColor, LineSize), new Point(0, y), new Point(tWidth, y));
                    TilesCountY++;
                }

                for (double x = MinX % (TileFreqX * RelativeWidth); x <= tWidth + MinX; x += (TileFreqX * RelativeWidth))
                {
                    g.DrawLine(new Pen(TileColor, LineSize), new Point(x, 0), new Point(x, tHeight));
                    TilesCountX++;
                }

                if (TilesCountY > TileAdjustUpLimit)
                    TileFreqY = TileFreqY * (TileAdjustUpLimit / TileAdjustDownLimit);
                else if (TilesCountY < TileAdjustUpLimit / TileAdjustDownLimit)
                    TileFreqY = TileFreqY / (TileAdjustUpLimit / TileAdjustDownLimit);

                if (TilesCountX > TileAdjustUpLimit)
                    TileFreqX = TileFreqX * (TileAdjustUpLimit / TileAdjustDownLimit);
                else if (TilesCountX < TileAdjustUpLimit / TileAdjustDownLimit)
                    TileFreqX = TileFreqX / (TileAdjustUpLimit / TileAdjustDownLimit);

                for (int i = 0; i < Points.Count; i++)
                {

                    Point p1 = new Point((int)((Points[i].X - MinX) * RelativeWidth), (int)((Points[i].Y - MinY) * RelativeHeight));

                    if (p1.X >= 0 && p1.Y >= 0)
                        g.DrawEllipse(DotColor, new Pen(DotColor, DotSize), p1, (int)DotSize, (int)DotSize);

                    if (Points.Count - i != 1)
                    {
                        Point p2 = new Point((int)((Points[i + 1].X - MinX) * RelativeWidth), (int)((Points[i + 1].Y - MinY) * RelativeHeight));

                        g.DrawLine(new Pen(LineColor, LineSize), p1, p2);
                    }
                }
            }
        }
    }
}
