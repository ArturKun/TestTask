using System;
using System.Collections.Generic;
using System.Windows;

namespace TestTask.Models
{
    /// <summary>
    /// Класс временного ряда
    /// </summary>
    public class TimeSeriesModel
    {
        #region Fields


        /// <summary>
        /// Точки временного ряда
        /// </summary>
        public List<Point> Points { get; set; }

        /// <summary>
        /// Имя ряда
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значения
        /// </summary>
        public List<double> Values { get; set; }

        /// <summary>
        /// Временные интервалы
        /// </summary>
        public List<double> TimeIntervals { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Создать временной ряд по временным отсчетам и соответствующим им значениям (количество должно быть равно)
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="timeIntervals">временые отсчеты</param>
        /// <param name="values">значения</param>
        public TimeSeriesModel(string name, List<double> timeIntervals, List<double> values)
        {
            if (timeIntervals.Count != values.Count)
            {
                throw new ArgumentException("Количество временных отсчетов не соответствует количеству значений.");
            }
            Name = name;
            TimeIntervals = timeIntervals;
            Values = values;
            CreatePoints();
        }

        /// <summary>
        /// Создать временной ряд по точкам
        /// </summary>
        /// <param name="points"></param>
        public TimeSeriesModel(List<Point> points)
        {
            TimeIntervals = new List<double>();
            Values = new List<double>();
            Points = points;
            for (int i = 0; i < points.Count; i++)
            {
                TimeIntervals.Add(points[i].X);
                Values.Add(points[i].Y);
            }
        }

        #endregion
        
        #region Private methods

        /// <summary>
        /// Создать точки
        /// </summary>
        private void CreatePoints()
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < TimeIntervals.Count; i++)
            {
                points.Add(new Point(TimeIntervals[i], Values[i]));
            }

            Points = points;
        }

        #endregion
    }
}