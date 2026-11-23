using FitCRController;
using FitCRModels;

namespace FitCR
{
    /// <summary>Statistics of the user in a range of dates: summary cards, charts and the ranking. They are calculated when the user asks for them.</summary>
    public class StatisticsView : Form
    {
        private readonly string userId;
        private readonly DateTimePicker dtFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(-60) };
        private readonly DateTimePicker dtTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today };
        private readonly Label lblWeight = CreateValueLabel();
        private readonly Label lblWeightChange = CreateValueLabel();
        private readonly Label lblBodyFatChange = CreateValueLabel();
        private readonly Label lblMuscleChange = CreateValueLabel();
        private readonly Label lblVisits = CreateValueLabel();
        private readonly Label lblGoals = CreateValueLabel();
        private readonly ChartPanel chartWeight = new ChartPanel("Peso (kg)", ChartKind.Line);
        private readonly ChartPanel chartBodyFat = new ChartPanel("Grasa corporal (%)", ChartKind.Line);
        private readonly ChartPanel chartVisits = new ChartPanel("Visitas por semana", ChartKind.Bar);
        private readonly DataGridView gridTopVisitors = UiHelper.CreateGrid();
        private readonly DataGridView gridTopAbsent = UiHelper.CreateGrid();

        public StatisticsView(ControllerContainer controllers, string userId)
        {
            this.Controllers = controllers;
            this.userId = userId;
            this.BuildLayout();
            this.Load += this.StatisticsView_Load;
        }

        private ControllerContainer Controllers { get; }

        private static Label CreateValueLabel()
        {
            return new Label { Text = "-", AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold) };
        }

        private static Panel CreateCard(string caption, Label value)
        {
            var card = new Panel { Width = 150, Height = 70, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(6) };
            value.Location = new Point(8, 6);
            card.Controls.Add(value);
            card.Controls.Add(new Label { Text = caption, AutoSize = true, Location = new Point(10, 44), ForeColor = Color.Gray });
            return card;
        }

        private static GroupBox CreateGroup(string title, Control content)
        {
            var group = new GroupBox { Text = title, Dock = DockStyle.Fill, Padding = new Padding(8) };
            group.Controls.Add(content);
            return group;
        }

        private static string FormatChange(double value, string unit)
        {
            return $"{value.ToString("+0.##;-0.##;0")} {unit}";
        }

        private static void FillRanking(DataGridView grid, List<RankingEntry> ranking)
        {
            UiHelper.Fill(
                grid,
                ranking,
                new GridColumn<RankingEntry>("#", entry => ranking.IndexOf(entry) + 1),
                new GridColumn<RankingEntry>("Nombre", entry => $"{entry.User.Name} {entry.User.LastName}"),
                new GridColumn<RankingEntry>("Visitas", entry => entry.Visits));
        }

        private void BuildLayout()
        {
            UiHelper.ConfigureDialog(this, "Fit.CR - Estadísticas", 1000, 640);

            var charts = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
            for (var i = 0; i < 3; i++)
            {
                charts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            }

            charts.Controls.Add(this.chartWeight, 0, 0);
            charts.Controls.Add(this.chartBodyFat, 1, 0);
            charts.Controls.Add(this.chartVisits, 2, 0);

            var cards = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(4) };
            cards.Controls.Add(CreateCard("Peso actual", this.lblWeight));
            cards.Controls.Add(CreateCard("Cambio de peso", this.lblWeightChange));
            cards.Controls.Add(CreateCard("Cambio de grasa", this.lblBodyFatChange));
            cards.Controls.Add(CreateCard("Cambio de musculatura", this.lblMuscleChange));
            cards.Controls.Add(CreateCard("Visitas en el rango", this.lblVisits));
            cards.Controls.Add(CreateCard("Metas cumplidas", this.lblGoals));

            var progressTab = new TabPage("Mi progreso");
            progressTab.Controls.Add(charts);
            progressTab.Controls.Add(cards);

            var ranking = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            ranking.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ranking.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ranking.Controls.Add(CreateGroup("Top 10 más visitantes", this.gridTopVisitors), 0, 0);
            ranking.Controls.Add(CreateGroup("Top 10 más ausentes", this.gridTopAbsent), 1, 0);
            var rankingTab = new TabPage("Ranking del gimnasio");
            rankingTab.Controls.Add(ranking);

            var tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add(progressTab);
            tabs.TabPages.Add(rankingTab);

            var filters = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(10) };
            filters.Controls.Add(new Label { Text = "Desde", AutoSize = true, Margin = new Padding(3, 6, 3, 0) });
            filters.Controls.Add(this.dtFrom);
            filters.Controls.Add(new Label { Text = "Hasta", AutoSize = true, Margin = new Padding(10, 6, 3, 0) });
            filters.Controls.Add(this.dtTo);
            filters.Controls.Add(UiHelper.CreateButton("Consultar", this.BtnQuery_Click));

            this.Controls.Add(tabs);
            this.Controls.Add(filters);
        }

        private void StatisticsView_Load(object? sender, EventArgs e)
        {
            this.QueryData();
        }

        private void BtnQuery_Click(object? sender, EventArgs e)
        {
            this.QueryData();
        }

        private void QueryData()
        {
            var from = this.dtFrom.Value.Date;
            var to = this.dtTo.Value.Date;
            var validation = this.Controllers.Statistics.Validate(from, to);
            if (!validation.Success)
            {
                UiHelper.ShowResult(validation);
                return;
            }

            this.ShowProgress(from, to);
            FillRanking(this.gridTopVisitors, this.Controllers.Statistics.GetTopVisitors(from, to));
            FillRanking(this.gridTopAbsent, this.Controllers.Statistics.GetTopAbsent(from, to));
        }

        private void ShowProgress(DateTime from, DateTime to)
        {
            var statistics = this.Controllers.Statistics;
            var progress = statistics.GetProgress(this.userId, from, to);
            var user = this.Controllers.Users.GetUser(this.userId);

            this.lblWeight.Text = user == null ? "-" : $"{user.Weight} kg";
            this.lblWeightChange.Text = progress.HasEnoughData ? FormatChange(progress.WeightChange, "kg") : "-";
            this.lblBodyFatChange.Text = progress.HasEnoughData ? FormatChange(progress.BodyFatChange, "pts") : "-";
            this.lblMuscleChange.Text = progress.HasEnoughData ? FormatChange(progress.MuscleMassChange, "kg") : "-";
            this.lblVisits.Text = progress.Visits.ToString();
            this.lblGoals.Text = progress.GoalsAchieved.ToString();

            var measurements = statistics.GetMeasurementsInRange(this.userId, from, to);
            this.chartWeight.SetData(measurements.Select(m => new KeyValuePair<string, double>(m.Date.ToString("dd/MM"), m.Weight)));
            this.chartBodyFat.SetData(measurements.Select(m => new KeyValuePair<string, double>(m.Date.ToString("dd/MM"), m.BodyFat)));
            this.chartVisits.SetData(statistics.GetWeeklyVisits(this.userId, from, to).Select(w => new KeyValuePair<string, double>(w.Key.ToString("dd/MM"), w.Value)));
        }
    }
}
