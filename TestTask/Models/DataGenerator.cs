using System;
using System.Collections.Generic;

namespace TestTask.Models
{
    /// <summary>
    /// Генератор данных
    /// </summary>
    public class DataGenerator
    {
        #region Fields

        private Random _random = new Random();

        #endregion

        #region Public metods

        /// <summary>
        /// Получить массив случайных чисел
        /// </summary>
        /// <param name="count">Количество чисел</param>
        /// <param name="min">минимальное значение</param>
        /// <param name="max">максимальное значение</param>
        /// <returns>Отсортированный массив случайных чисел</returns>
        public List<double> GetRandomDoubles(int count, int min, int max)
        {
            var result = new List<double>();
            for (int i = 0; i < count; i++)
            {
                result.Add(Generator(min, max));
            }
            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Генератор числа
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="max">Максимальное значение</param>
        /// <returns>Случайное число</returns>
        private double Generator(int min, int max)
        {
            int beforeComma;
            double afterComma;
            afterComma = _random.NextDouble();
            beforeComma = _random.Next(min, max);
            return (afterComma + beforeComma);
        }

        #endregion
    }
}