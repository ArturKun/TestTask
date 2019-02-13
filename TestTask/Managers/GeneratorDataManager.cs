using System.Collections.ObjectModel;
using System.Linq;
using TestTask.Models;

namespace TestTask.Managers
{
    /// <summary>
    /// Менеджер данных
    /// </summary>
    static public class GeneratorDataManager
    {
        #region Const
        /// <summary>
        /// Имя ряда 1
        /// </summary>
        private const string nameTimeSeries1 = "Ряд 1";

        /// <summary>
        /// Имя ряда 2
        /// </summary>
        private const string nameTimeSeries2 = "Ряд 2";

        #endregion

        #region Constructors

        /// <summary>
        /// Получить временные ряды
        /// </summary>
        /// <returns></returns>
        static public ObservableCollection<TimeSeriesModel> GetTimeSeries(int countPoint, int minValue, int maxValue)
        {
            DataGenerator dataGenerator = new DataGenerator();
            var val1 = dataGenerator.GetRandomDoubles(countPoint, minValue, maxValue);
            var val2 = dataGenerator.GetRandomDoubles(countPoint, minValue, maxValue);
            val1[0] = 0;
            val2[0] = 0;
            val1.Sort();
            val2.Sort();
            if (val2.Last() >= val1.Last())
            {
                val1[val1.Count - 1] = val2.Last();
            }
            else
            {
                val2[val2.Count - 1] = val1.Last();
            }
            var ts1 = new TimeSeriesModel(nameTimeSeries1, val1, dataGenerator.GetRandomDoubles(countPoint, minValue, maxValue));

            var ts2 = new TimeSeriesModel(nameTimeSeries2, val2, dataGenerator.GetRandomDoubles(countPoint, minValue, maxValue));

            return new ObservableCollection<TimeSeriesModel>() { ts1, ts2 };
        }

        #endregion
    }
}