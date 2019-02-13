using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TestTask.Models;

namespace TestTask.Instruments
{
    /// <summary>
    /// Сравнивает временные ряды
    /// </summary>
    static public class Comparator
    {
        #region Public methods

        /// <summary>
        /// Найти точки пересечения двух временных рядов. сравнивает первый ряд со вторым
        /// </summary>
        /// <param name="timeSeries1"></param>
        /// <param name="timeSeries2"></param>
        /// <returns></returns>
        static public List<Point> CompareTimeSeries(TimeSeriesModel timeSeries1, TimeSeriesModel timeSeries2)
        {
            List<Segment> ts1 = new List<Segment>();
            List<Segment> ts2 = new List<Segment>();
            List<Point> result = new List<Point>();

            //Добавить в список первую точку 
            if (timeSeries1.Points[0].Y > timeSeries2.Points[0].Y && timeSeries1.Points[0].X <= timeSeries2.Points[0].X)
            {
                result.Add(timeSeries1.Points[0]);
            }

            ts1 = CreateSegments(timeSeries1.Points);
            ts2 = CreateSegments(timeSeries2.Points);
            //Поиск пересечений отрезков
            for (int i = 0; i < ts1.Count; i++)
            {
                for (int j = 0; j < ts2.Count; j++)
                {
                    if (IntersectionOfSegments(ts1[i], ts2[j]).intersection)
                    {
                        result.Add(IntersectionOfSegments(ts1[i], ts2[j]).point);
                    }
                }
            }
            //Добавить в список последнюю точку
            if (timeSeries1.Points.Last().Y > timeSeries2.Points.Last().Y)
            {
                result.Add(timeSeries1.Points.Last());
            }

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Поиск точки пересечения сегментов
        /// </summary>
        /// <param name="s1">Сегмент 1</param>
        /// <param name="s2">Сегмент 2</param>
        /// <returns> Если есть пересечение то intersection = true</returns>
        static private (bool intersection, Point point) IntersectionOfSegments(Segment s1, Segment s2)
        {
            var result = (intersection: false, point: new Point(0, 0));
            double x = 0;
            double y = 0;
            x = (s2.B - s1.B) / (s1.K - s2.K);
            y = s2.K * x + s2.B;
            if (x > s1.FirstPoint.X && x < s1.LastPoint.X && x > s2.FirstPoint.X && x < s2.LastPoint.X)
            {
                result.intersection = true;
                result.point = new Point(x, y);
                return result;
            }

            return result;
        }

        /// <summary>
        /// Собрать список сегментов из точек
        /// </summary>
        /// <param name="points">Точки</param>
        /// <returns></returns>
        static private List<Segment> CreateSegments(List<Point> points)
        {
            var result = new List<Segment>();
            for (int i = 1; i < points.Count; i++)
            {
                Segment segment1 = new Segment(points[i - 1], points[i]);
                result.Add(segment1);
            }

            return result;
        }

        #endregion
    }
}