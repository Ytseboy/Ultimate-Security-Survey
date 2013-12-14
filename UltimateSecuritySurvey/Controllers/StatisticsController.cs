using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltimateSecuritySurvey.Models;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Globalization;


namespace UltimateSecuritySurvey.Controllers
{
    public class StatisticsController : Controller
    {
        private SecuritySurveyEntities db = new SecuritySurveyEntities();
        //
        // GET: /Statistics/

        public ActionResult Index()
        {
            #region ChartControl attempt
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
            #endregion

            Dictionary<string, int> basicStats = new Dictionary<string, int>
            {
                {"Users", db.UserAccounts.Count()},
                {"Customers", db.Customers.Count()},
                {"Surveys", db.CustomerSurveys.Count()},
                {"Base Surveys", db.GenericSurveys.Count()},
                {"Questions", db.Questions.Count()}
            };

            ViewBag.BasicStats = basicStats;

            ViewBag.UserActivity = db.CustomerSurveys
                .GroupBy(c => c.supervisorUserId)
                .Join(
                    db.UserAccounts,
                    myObject => myObject.FirstOrDefault().supervisorUserId,
                    name => name.userId,
                    (myObject, name) =>
                        new StatisticsModel
                        {
                            UserKey = myObject.Key,
                            MyCount =  myObject.Count(),
                            Name = name.firstName
                        }
                );

            ViewBag.SurveysPerMonths = surveysPerMonths();

            return View();
        }

        private Dictionary<string, int> surveysPerMonths()
        {
            DateTime firstMonth = db.CustomerSurveys.Min(x => x.startDate);
            DateTime lastMonth = db.CustomerSurveys.Max(x => x.startDate);

            int firstYear = firstMonth.Year;
            int lastYear = lastMonth.Year;
            int yearRange = lastYear - firstYear;

            var surveysPerMonth =
                    //Provides entries Joining with empty months as well
                    (from year in Enumerable.Range(firstYear, yearRange + 1)
                    from month in Enumerable.Range(1, 12)
                    let key = new { Year = year, Month = month }
                    join item in db.CustomerSurveys on key
                              equals new
                              {
                                  item.startDate.Year,
                                  item.startDate.Month
                              } into g
                    select new { Date = key, Count = g.Count() })
                    //Skip first dummy entries
                    .Skip(firstMonth.Month - 1).Reverse()
                    //Skip last dummy entries
                    .Skip(12 - lastMonth.Month)
                    //Take last 12 months
                    .Take(12).Reverse();

            Dictionary<string, int> dataToSend = new Dictionary<string, int>();
            CultureInfo eng = new CultureInfo("en-US");

            foreach (var item in surveysPerMonth)
            {
                dataToSend.Add((new DateTime(item.Date.Year, item.Date.Month, 1)).ToString("MMMM yyyy", eng), item.Count);
            }

            return dataToSend;
        }
    }
}