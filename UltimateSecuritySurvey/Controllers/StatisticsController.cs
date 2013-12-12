using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


namespace UltimateSecuritySurvey.Controllers
{
    public class StatisticsController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();
        //
        // GET: /Statistics/

        public ActionResult Index()
        {

            //var xvals = db.CustomerSurveys.Select(e => e.startDate).ToList();

            //var yvals = db.CustomerSurveys.GroupBy(a => a.startDate.Month);

            //var yvals = from item in db.CustomerSurveys
            //            group item by item.startDate.Month into StartDates
            //            select new 
            //            {
            //                dateCount = StartDates
            //            }

            //var yvals = new List<int>() { 1, 3 };



            //var chart = new Chart();
            //chart.Width = 800;
            //chart.Height = 300;

            //var chartArea = new ChartArea();
            //chartArea.AxisX.LabelStyle.Format = "MMMM/yy";
            //chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            //chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            //chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            //chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            //chart.ChartAreas.Add(chartArea);

            //var series = new Series();
            //series.Name = "Series1";
            //series.ChartType = SeriesChartType.FastLine;
            //series.XValueType = ChartValueType.DateTime;
            //chart.Series.Add(series);

            // bind the datapoints
            //chart.Series["Series1"].Points.DataBindY(xvals);

            // copy the series and manipulate the copy
            //chart.DataManipulator.CopySeriesValues("Series1", "Series2");
            //chart.DataManipulator.FinancialFormula(
            //    FinancialFormula.WeightedMovingAverage,
            //    "Series2"
            //);
            //chart.Series["Series2"].ChartType = SeriesChartType.FastLine;

            // draw!


            // write out a file
            //chart.SaveImage("chart.png", ChartImageFormat.Png);

            //ViewBag.chart = chart;
            //var test = db.CustomerSurveys.GroupBy(s => s.startDate.Month, x => x.startDate.Year);
            //ViewBag.Count = test;
            
            
            Dictionary<string, int> stats = new Dictionary<string, int>();
            stats.Add("Users", db.UserAccounts.Count());
            stats.Add("Customers", db.Customers.Count());
            stats.Add("Surveys", db.CustomerSurveys.Count());
            stats.Add("Base Surveys", db.GenericSurveys.Count());
            stats.Add("Questions", db.Questions.Count());

            ViewBag.Stats = stats;
            //ViewBag.Users = db.UserAccounts.
            return View();
        }

    }
}
