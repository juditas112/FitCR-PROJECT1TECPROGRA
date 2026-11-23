using FitCRController;
using System.Globalization;

namespace FitCR
{
    /// <summary>Helpers shared by the views, so the grids, the forms and the messages are created the same way everywhere.</summary>
    internal static class UiHelper
    {
        public static DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
            };
        }

        /// <summary>Fills a grid with the items. Each row keeps its item, so it can be got back with GetSelected.</summary>
        public static void Fill<T>(DataGridView grid, IEnumerable<T> items, params GridColumn<T>[] columns)
            where T : class
        {
            grid.Columns.Clear();
            grid.Rows.Clear();
            foreach (var header in columns.Select(column => column.Header))
            {
                grid.Columns.Add(header, header);
            }

            foreach (var item in items)
            {
                var index = grid.Rows.Add(columns.Select(column => column.Value(item)).ToArray());
                grid.Rows[index].Tag = item;
            }
        }

        public static T? GetSelected<T>(DataGridView grid)
            where T : class
        {
            return grid.CurrentRow?.Tag as T;
        }

        public static void ConfigureDialog(Form form, string title, int width, int height)
        {
            form.Text = title;
            form.ClientSize = new Size(width, height);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
        }

        public static TableLayoutPanel CreateFormTable()
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(12) };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return table;
        }

        public static void AddRow(TableLayoutPanel table, string caption, Control control)
        {
            var row = table.RowCount;
            table.RowCount = row + 1;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(new Label { Text = caption, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 8, 8, 3) }, 0, row);
            control.Dock = DockStyle.Fill;
            table.Controls.Add(control, 1, row);
        }

        public static Button CreateButton(string text, EventHandler onClick)
        {
            var button = new Button { Text = text, AutoSize = true, Padding = new Padding(8, 2, 8, 2) };
            button.Click += onClick;
            return button;
        }

        public static FlowLayoutPanel CreateButtonPanel(DockStyle dock, params Button[] buttons)
        {
            var panel = new FlowLayoutPanel { Dock = dock, AutoSize = true, Padding = new Padding(8) };
            panel.Controls.AddRange(buttons);
            return panel;
        }

        public static NumericUpDown CreateNumber(decimal minimum, decimal maximum, int decimals, decimal value)
        {
            return new NumericUpDown
            {
                Minimum = minimum,
                Maximum = maximum,
                DecimalPlaces = decimals,
                Value = Math.Clamp(value, minimum, maximum),
            };
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        public static string FormatTime(TimeSpan time)
        {
            return time.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }

        public static void ShowResult(OperationResult result)
        {
            MessageBox.Show(
                result.Message,
                result.Success ? "Fit.CR" : "Error",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
    }
}
