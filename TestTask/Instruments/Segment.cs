using System.Windows;

namespace TestTask.Instruments
{
    /// <summary>
    /// Отрезок y = k*x + b
    /// k - коэф. наклона прямой
    /// b - смещение прямой по Y
    /// </summary>
    public class Segment
    {
        #region Fields

        /// <summary>
        /// Начальная точка отрезка
        /// </summary>
        public Point FirstPoint { get; }

        /// <summary>
        /// Конечная точка отрезка
        /// </summary>
        public Point LastPoint { get; }

        /// <summary>
        /// Коэф. наклона прямой
        /// </summary>
        public double K { get; private set; }

        /// <summary>
        /// Смещение прямой по Y
        /// </summary>
        public double B { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Создать отрезок по двум точкам
        /// </summary>
        /// <param name="firstPoint">первая точка</param>
        /// <param name="lastPoint">конечная точка</param>
        public Segment(Point firstPoint, Point lastPoint)
        {
            FirstPoint = firstPoint;
            LastPoint = lastPoint;
            CalculateKandB();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Рассчитать к и b
        /// </summary>
        private void CalculateKandB()
        {
            K = (FirstPoint.Y - LastPoint.Y) / (FirstPoint.X - LastPoint.X);
            B = FirstPoint.Y - K * FirstPoint.X;
        }

        #endregion
    }
}