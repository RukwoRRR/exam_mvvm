using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace exam_preparing
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private double _a;
        private double _b;
        private double _result;
        private bool _isBusy;

        public MainViewModel(string connectionString)
        {
            _databaseService = new DatabaseService(connectionString);
            CalculateCommand = new RelayCommand(async () => await CalculateAsync(), () => !IsBusy);
            Results = []; // ObservableCollection

            LoadResultsAsync();
        }

        public double A
        {
            get => _a;
            set
            {
                _a = value;
                OnPropertyChanged();
            }
        }

        public double B
        {
            get => _b;
            set
            {
                _b = value;
                OnPropertyChanged();
            }
        }

        public double Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((RelayCommand)CalculateCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CalculateCommand { get; }

        public ObservableCollection<IntegrationResult> Results { get; }

        private async Task CalculateAsync()
        {
            IsBusy = true;

            try
            {
                Result = await Task.Run(() => IntegrationCalculator.Integrate(A, B, 10000));
                var result = new IntegrationResult { A = A, B = B, Result = Result };
                await _databaseService.SaveResultAsync(result);
                Results.Add(result);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadResultsAsync()
        {
            var results = await _databaseService.GetResultsAsync();
            foreach (var result in results)
            {
                Results.Add(result);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}