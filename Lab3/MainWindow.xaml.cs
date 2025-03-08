using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

namespace WpfTimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int remainingSeconds;

        public ICommand StartTimerCommand { get; }
        public ICommand ResetTimerCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            StartTimerCommand = new RelayCommand(StartTimer, CanStartTimer);
            ResetTimerCommand = new RelayCommand(ResetTimer, CanResetTimer);
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            DataContext = this;
        }

        private void StartTimer(object parameter)
        {
            if (int.TryParse(MinutesInput.Text, out int minutes) && minutes >= 0 &&
                int.TryParse(SecondsInput.Text, out int seconds) && seconds >= 0)
            {
                remainingSeconds = (minutes * 60) + seconds;
                if (remainingSeconds > 0)
                {
                    timer.Start();
                    StartButton.IsEnabled = false;
                    ResetButton.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Введіть час більше 0 секунд.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введіть коректне число хвилин і секунд.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanStartTimer(object parameter) => !timer.IsEnabled;
        private bool CanResetTimer(object parameter) => timer.IsEnabled;

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            TimerLabel.Text = $"Залишилось: {remainingSeconds / 60}:{remainingSeconds % 60:D2}";

            if (remainingSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Час вийшов!", "Таймер", MessageBoxButton.OK, MessageBoxImage.Information);
                StartButton.IsEnabled = true;
                ResetButton.IsEnabled = false;
            }
        }

        private void ResetTimer(object parameter)
        {
            timer.Stop();
            TimerLabel.Text = "Залишилось: 0:00";
            StartButton.IsEnabled = true;
            ResetButton.IsEnabled = false;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => execute(parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}