namespace FitCR
{
    /// <summary>Column of a grid: its header and how to get its value from each item.</summary>
    internal class GridColumn<T>
    {
        public GridColumn(string header, Func<T, object> value)
        {
            this.Header = header;
            this.Value = value;
        }

        public string Header { get; }

        public Func<T, object> Value { get; }
    }
}
