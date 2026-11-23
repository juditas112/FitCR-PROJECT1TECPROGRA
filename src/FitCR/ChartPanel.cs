using System.Drawing.Drawing2D;

namespace FitCR
{
    internal enum ChartKind
    {
        Line,
        Bar,
    }

    /// <summary>Simple chart drawn with GDI+ for the statistics. It shows a line or bars for a list of labeled values.</summary>
    internal class ChartPanel : Control
    {
        private const int LeftMargin = 46;
        private const int TopMargin = 32;
        private const int RightMargin = 16;
        private const int BottomMargin = 34;
        private const int GridLines = 4;

        private readonly string title;
        private readonly ChartKind kind;
        private List<KeyValuePair<string, double>> points = new List<KeyValuePair<string, double>>();

        public ChartPanel(string title, ChartKind kind)
        {
            this.title = title;
            this.kind = kind;
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
        }

        public void SetData(IEnumerable<KeyValuePair<string, double>> data)
        {
            this.points = data.ToList();
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var titleFont = new Font(this.Font, FontStyle.Bold))
            {
                graphics.DrawString(this.title, titleFont, Brushes.Black, 8, 8);
            }

            var area = new Rectangle(LeftMargin, TopMargin, this.Width - LeftMargin - RightMargin, this.Height - TopMargin - BottomMargin);
            if (area.Width <= 0 || area.Height <= 0)
            {
                return;
            }

            if (this.points.Count == 0)
            {
                graphics.DrawString("Sin datos en este rango", this.Font, Brushes.Gray, area.Left + 10, area.Top + (area.Height / 2));
                return;
            }

            var (min, max) = this.GetRange();
            this.DrawGrid(graphics, area, min, max);
            if (this.kind == ChartKind.Line)
            {
                this.DrawLine(graphics, area, min, max);
            }
            else
            {
                this.DrawBars(graphics, area, max);
            }

            this.DrawLabels(graphics, area);
        }

        private static float ToY(Rectangle area, double value, double min, double max)
        {
            return (float)(area.Bottom - ((value - min) / (max - min) * area.Height));
        }

        private (double Min, double Max) GetRange()
        {
            var max = this.points.Max(point => point.Value);
            if (this.kind == ChartKind.Bar)
            {
                return (0, Math.Max(max, 1));
            }

            var min = this.points.Min(point => point.Value);
            var padding = Math.Max((max - min) * 0.1, 0.5);
            return (min - padding, max + padding);
        }

        private float GetCenterX(Rectangle area, int index)
        {
            if (this.kind == ChartKind.Bar)
            {
                return area.Left + ((float)area.Width / this.points.Count * (index + 0.5f));
            }

            return this.points.Count == 1 ? area.Left + (area.Width / 2f) : area.Left + (index * (float)area.Width / (this.points.Count - 1));
        }

        private void DrawGrid(Graphics graphics, Rectangle area, double min, double max)
        {
            using var gridPen = new Pen(Color.Gainsboro, 1);
            for (var i = 0; i <= GridLines; i++)
            {
                var value = min + ((max - min) * i / GridLines);
                var y = ToY(area, value, min, max);
                graphics.DrawLine(gridPen, area.Left, y, area.Right, y);
                graphics.DrawString(value.ToString("0.#"), this.Font, Brushes.Gray, 4, y - 8);
            }
        }

        private void DrawLine(Graphics graphics, Rectangle area, double min, double max)
        {
            var coordinates = this.points
                .Select((point, index) => new PointF(this.GetCenterX(area, index), ToY(area, point.Value, min, max)))
                .ToArray();

            if (coordinates.Length > 1)
            {
                using var linePen = new Pen(Color.SteelBlue, 2);
                graphics.DrawLines(linePen, coordinates);
            }

            foreach (var coordinate in coordinates)
            {
                graphics.FillEllipse(Brushes.SteelBlue, coordinate.X - 3, coordinate.Y - 3, 6, 6);
            }
        }

        private void DrawBars(Graphics graphics, Rectangle area, double max)
        {
            var width = (float)area.Width / this.points.Count * 0.6f;
            for (var i = 0; i < this.points.Count; i++)
            {
                var top = ToY(area, this.points[i].Value, 0, max);
                var x = this.GetCenterX(area, i) - (width / 2);
                graphics.FillRectangle(Brushes.SteelBlue, x, top, width, area.Bottom - top);
                if (this.points.Count <= 16)
                {
                    graphics.DrawString(this.points[i].Value.ToString("0"), this.Font, Brushes.Black, x, top - 16);
                }
            }
        }

        private void DrawLabels(Graphics graphics, Rectangle area)
        {
            var step = Math.Max(1, (int)Math.Ceiling(this.points.Count / 8.0));
            for (var i = 0; i < this.points.Count; i += step)
            {
                graphics.DrawString(this.points[i].Key, this.Font, Brushes.Gray, this.GetCenterX(area, i) - 18, area.Bottom + 6);
            }
        }
    }
}
