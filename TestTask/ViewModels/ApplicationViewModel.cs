using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using TestTask.Instruments;
using TestTask.Managers;
using TestTask.Models;
using TestTask.Utils;

namespace TestTask.ViewModel
{
    /// <summary>
    /// Модель представления
    /// </summary>
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// Временные ряды
        /// </summary>
        private ObservableCollection<TimeSeriesModel> _timeSeriesCollection;

        /// <summary>
        /// Коллекция графиков
        /// </summary>
        private SeriesCollection _lineSeriesCollection = new SeriesCollection();

        /// <summary>
        /// Текст с информацией - на каких интервалах доминирует ряд
        /// </summary>
        private List<string> _intersectionOfInformation = new List<string>();

        /// <summary>
        /// Текст с информацией - какой ряд над каким доминирует
        /// </summary>
        private string _intersectionOfInformationLabel;

        /// <summary>
        /// Первый выбранный временной ряд для сравнения
        /// </summary>
        private TimeSeriesModel _currentFirstTimeSeries;

        /// <summary>
        /// Второй выбранный временной ряд для сравнения
        /// </summary>
        private TimeSeriesModel _currentSecondTimeSeries;

        /// <summary>
        /// Количество точек
        /// </summary>
        private int _countPoint = 0;

        /// <summary>
        /// Минимальное значение
        /// </summary>
        private int _minValue = 0;

        /// <summary>
        /// Максимальное значение
        /// </summary>
        private int _maxValue = 0;

        /// <summary>
        /// Сравнить ряды
        /// </summary>
        private RelayCommand _compareCommand;

        /// <summary>
        /// Отобразить временные ряды
        /// </summary>
        private RelayCommand _toDisplayTimeSeriesCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Временные ряды
        /// </summary>
        public ObservableCollection<TimeSeriesModel> TimeSeriesCollection
        {
            get { return _timeSeriesCollection; }
            set
            {

                _timeSeriesCollection = value;
                OnPropertyChanged(nameof(TimeSeriesCollection));
            }
        }

        /// <summary>
        /// Коллекция графиков
        /// </summary>
        public SeriesCollection LineSeriesCollection
        {
            get { return _lineSeriesCollection; }
            private set
            {
                _lineSeriesCollection = value;
                OnPropertyChanged(nameof(LineSeriesCollection));
            }
        }

        /// <summary>
        /// Информация о пересечениях
        /// </summary>
        public List<string> IntersectionOfInformation
        {
            get => _intersectionOfInformation;
            set
            {
                _intersectionOfInformation = value;
                OnPropertyChanged(nameof(IntersectionOfInformation));
            }
        }

        /// <summary>
        /// Информация о сравниваемых рядах
        /// </summary>
        public string IntersectionOfInformationLabel
        {
            get => _intersectionOfInformationLabel;
            set
            {
                _intersectionOfInformationLabel = value;
                OnPropertyChanged(nameof(IntersectionOfInformationLabel));
            }
        }

        /// <summary>
        /// Выбор первого ряда для сравнения
        /// </summary>
        public TimeSeriesModel CurrentFirstTimeSeries
        {
            get { return _currentFirstTimeSeries; }
            set
            {
                _currentFirstTimeSeries = value;
                OnPropertyChanged(nameof(CurrentFirstTimeSeries));
            }
        }

        /// <summary>
        /// Выбор второго ряда для сравнения
        /// </summary>
        public TimeSeriesModel CurrentSecondTimeSeries
        {
            get { return _currentSecondTimeSeries; }
            set
            {
                _currentSecondTimeSeries = value;
                OnPropertyChanged(nameof(CurrentSecondTimeSeries));
            }
        }

        /// <summary>
        /// Количество точек
        /// </summary>
        public int CountPoint
        {
            get
            {
                return _countPoint;
            }
            set
            {
                _countPoint = value;
                OnPropertyChanged(nameof(CountPoint));
            }
        }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                OnPropertyChanged(nameof(MinValue));
            }
        }

        /// <summary>
        /// Максимальное число
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
            }
        }


        #endregion

        #region Commands

        /// <summary>
        /// Отобразить временные ряды
        /// </summary>
        public RelayCommand ToDisplayTimeSeriesCommand
        {
            get
            {
                return _toDisplayTimeSeriesCommand ?? (_toDisplayTimeSeriesCommand = new RelayCommand(obj =>
                {
                    try
                    {
                        TimeSeriesCollection = GeneratorDataManager.GetTimeSeries(CountPoint, MinValue, MaxValue);
                        BuildLineSeries();
                    }
                    catch (ArgumentException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }));
            }
        }

        /// <summary>
        /// Сравнение интервалов
        /// </summary>
        public RelayCommand CompareCommand
        {
            get
            {
                return _compareCommand ?? (_compareCommand = new RelayCommand(obj =>
                {
                    if (CurrentFirstTimeSeries == null || CurrentSecondTimeSeries == null || CurrentFirstTimeSeries == CurrentSecondTimeSeries)
                    {
                        MessageBox.Show("Не выбраны ряды для сравнения.");
                    }
                    else

                    {
                        var points = new List<Point>();
                        points = Comparator.CompareTimeSeries(CurrentFirstTimeSeries, CurrentSecondTimeSeries);
                        IntersectionOfInformation = CreateIntersectionOfInformation(points);
                        IntersectionOfInformationLabel =
                            $"{CurrentFirstTimeSeries.Name}, больше {CurrentSecondTimeSeries.Name} на интервалах:";
                        CreatePointsIntersection(points);
                    }
                }));
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Собрать из TimeSeriesModel LineSeries и отобразить
        /// </summary>
        private void BuildLineSeries()
        {
            LineSeriesCollection.Clear();
            foreach (var timeSeries in TimeSeriesCollection)
            {
                LineSeries lineSeries = new LineSeries();
                //список точек
                ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();
                for (int i = 0; i < timeSeries.TimeIntervals.Count; i++)
                {
                    //создаем новые точки
                    ObservablePoint point = new ObservablePoint(timeSeries.TimeIntervals[i], timeSeries.Values[i]);
                    //добавляем в список
                    points.Add(point);
                }
                lineSeries.LineSmoothness = 0;
                //Подпись графика
                lineSeries.Title = timeSeries.Name;
                //приписываем список точек графику
                lineSeries.Values = points;
                //добавляем график в список графиков
                LineSeriesCollection.Add(lineSeries);
            }
        }

        /// <summary>
        /// Создать точки показывающие интервалы сравнения
        /// </summary>
        /// <param name="points"></param>
        private void CreatePointsIntersection(List<Point> points)
        {
            const string lineSeriesName = "Интервалы";

            foreach (var series in LineSeriesCollection.Where(x => x.Title == lineSeriesName))
            {
                LineSeriesCollection.Remove(series);
            }

            ChartValues<ObservablePoint> chartValues = new ChartValues<ObservablePoint>();

            foreach (var point in points)
            {
                ObservablePoint observablePoint = new ObservablePoint(point.X, point.Y);
                chartValues.Add(observablePoint);
            }
            ScatterSeries sp = new ScatterSeries();
            sp.Title = lineSeriesName;
            sp.Values = chartValues;
            sp.Fill = new SolidColorBrush(Colors.Fuchsia);
            sp.PointGeometry = DefaultGeometries.Triangle;
            LineSeriesCollection.Add(sp);
        }

        /// <summary>
        /// Создать текстовую информацию об интервалах
        /// </summary>
        /// <param name="points">точки интервалов</param>
        private List<string> CreateIntersectionOfInformation(List<Point> points)
        {
            var result = new List<string>();
            for (int i = 0; i < points.Count; i++)
            {
                string str =
                    $"c ({points[i * 2].X:0.00} , {points[i * 2].Y:0.00}) по ({points[i * 2 + 1].X:0.00} , {points[i * 2 + 1].Y:0.00})";
                result.Add(str);
                if (i * 2 + 1 == points.Count - 1)
                {
                    break;
                }
            }
            return result;
        }


        #endregion

        #region Implementaions

        /// <summary>
        /// Изменение свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}