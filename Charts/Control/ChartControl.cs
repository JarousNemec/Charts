using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Charts.Models;

namespace Charts.Control
{
    public partial class ChartControl : UserControl
    {
        private IEnumerable<ChartPoint> _points;

        public ChartControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private double _xMax = 0;
        private double _yMax = 0;
        public void SetPoints(IEnumerable<ChartPoint> points)
        {
            _points = points;
            if (_points == null) return;
            if (_points.Count() < 2)
                return;
            _xMax = _points.Max(a => a.X) / Width;
            _yMax = _points.Max(a => a.Y) / Height;
            Invalidate();
        }

        private void ChartControl_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            DrawAxes(g);

            if (_points == null) return;
            if (_points.Count() < 2)
                return;

            var points = new List<PointF>();

            DrawRakes(g);

            foreach (var point in _points)
            {
                points.Add(new PointF() { X = (float)(point.X / _xMax), Y = (float)(Height - point.Y / _yMax) });
            }

            g.DrawLines(Pens.Red, points.ToArray());

            if (_lastLoc != null)
            {
                DrawValueUnderCursor(g);
            }
        }

        private void DrawValueUnderCursor(Graphics g)
        {
            g.DrawLine(Pens.Blue, _lastLoc.X, 0, _lastLoc.X, Width);
            g.DrawLine(Pens.Blue, 0, _lastLoc.Y, Width, _lastLoc.Y);
            Font drawFont = new Font("Arial", 16);
            g.DrawString(
                $"X: {Math.Round(_lastLoc.X * _xMax, 1)}, Y: {Math.Round(_points.Max(a => a.Y) - _lastLoc.Y * _yMax, 1)}",
                drawFont, Brushes.Bisque, _lastLoc);
        }

        private void DrawRakes(Graphics g)
        {
            float x = 10;
            while (x < Width)
            {
                g.DrawLine(Pens.Black, x, Height - 12, x, Height - 8);
                x += (float)(1 / _xMax);
            }

            float y = Height - 10;
            while (y > 0)
            {
                g.DrawLine(Pens.Black, 8, y, 12, y);
                y -= (float)(1 / _yMax);
            }
        }

        private void DrawAxes(Graphics g)
        {
            g.DrawLine(Pens.Black, 0, Height - 10, Width, Height - 10);
            g.DrawLine(Pens.Black, 10, 0, 10, Height);
        }

        private Point _lastLoc;

        private void ChartControl_MouseMove(object sender, MouseEventArgs e)
        {
            _lastLoc = e.Location;
            Invalidate();
        }
    }
}