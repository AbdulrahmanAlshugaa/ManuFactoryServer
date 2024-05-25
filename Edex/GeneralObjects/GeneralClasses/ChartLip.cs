using DevExpress.XtraCharts;
using Edex.Model;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralClasses
{
   public static  class ChartLip
    {
        public static void Chart(int rowCount, DataTable dt, Control Ctrl)
        {
            ChartControl chart = new ChartControl();

            // Create an empty Bar series and add it to the chart.
            Series series = new Series("Series1", ViewType.Bar);
            chart.Series.Add(series);

            // Generate a data table and bind the series to it.
            series.DataSource = CreateChartData(rowCount, dt);

            // Specify data members to bind the series.
            series.ArgumentScaleType = ScaleType.Auto;
            series.ArgumentDataMember = "Argument";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            // Set some properties to get a nice-looking chart.
            ((SideBySideBarSeriesView)series.View).ColorEach = true;
            ((XYDiagram)chart.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

            ChartTitle chartTitle1 = new ChartTitle();
            ChartTitle chartTitle2 = new ChartTitle();

            chartTitle1.Text = "   <color=blue>أكثر عشرة أصناف مبيعاً</color> ";
            chartTitle2.Text = " عرض بياني لاكثر عشرة أصناف مبيعاً ومبيعات كل صنف ";
            if (UserInfo.Language == iLanguage.English)
            {
                chartTitle1.Text = "   <color=blue>Top ten best-selling items</color> ";
                chartTitle2.Text = "A graphical display of the ten best-selling items and the sales of each item.";
            }
            chartTitle2.WordWrap = true;
            chartTitle2.MaxLineCount = 2;

            // Define the alignment of the titles.
            chartTitle1.Alignment = StringAlignment.Center;
            chartTitle2.Alignment = StringAlignment.Near;

            // Place the titles where it's required.
            chartTitle1.Dock = ChartTitleDockStyle.Top;
            chartTitle2.Dock = ChartTitleDockStyle.Bottom;

            // Customize a title's appearance.
            chartTitle1.Antialiasing = true;
            chartTitle1.Font = new Font("Tahoma", 14, FontStyle.Bold);
            chartTitle1.TextColor = Color.Red;
            chartTitle1.Indent = 10;

            // Add the titles to the chart.
            //chart.Titles.AddRange(new ChartTitle[] {chartTitle1,chartTitle2});
            chart.Titles.AddRange(new ChartTitle[] {chartTitle1});

            // chart.Titles[1].Text = " أكثر عشرة أصناف مبيعا ";
            // Dock the chart into its parent and add it to the current form.
            chart.Dock = DockStyle.Fill;
            Ctrl.Controls.Add(chart);
        }
        private static DataTable CreateChartData(int rowCount, DataTable dt)
        {
            // Create an empty table.
            DataTable table = new DataTable("Table1");

            // Add two columns to the table.
            table.Columns.Add("Argument", typeof(string));
            table.Columns.Add("Value", typeof(double));
              // Add data rows to the table.
            Random rnd = new Random();
            DataRow row = null;
            for (int i = 0; i < rowCount; i++)
            {
                row = table.NewRow();
                if(i< dt.Rows.Count)
                {
                    row["Argument"] = dt.Rows[i]["Argument"].ToString();
                    row["Value"] = double.Parse(dt.Rows[i]["Value"].ToString());
                    table.Rows.Add(row);
                }
               
            }

            return table;
        }

    }
}
