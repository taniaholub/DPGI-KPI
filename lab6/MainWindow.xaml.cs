using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

namespace WpfTimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int remainingSeconds;
        private readonly AdoAssistant adoAssistant;
        private ObservableCollection<TimerSetting> savedSettings;

        public ObservableCollection<TimerSetting> SavedSettings
        {
            get => savedSettings;
            set
            {
                savedSettings = value;
                OnPropertyChanged(nameof(SavedSettings));
            }
        }

        private TimerSetting selectedTimerSetting;
        public TimerSetting SelectedTimerSetting
        {
            get => selectedTimerSetting;
            set
            {
                selectedTimerSetting = value;
                OnPropertyChanged(nameof(SelectedTimerSetting));
            }
        }

        public ICommand StartTimerCommand { get; }
        public ICommand ResetTimerCommand { get; }
        public ICommand LoadTimerCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString_ADO"].ConnectionString;
            adoAssistant = new AdoAssistant(connectionString);
            SavedSettings = new ObservableCollection<TimerSetting>();
            StartTimerCommand = new RelayCommand(StartTimer, CanStartTimer);
            ResetTimerCommand = new RelayCommand(ResetTimer, CanResetTimer);
            LoadTimerCommand = new RelayCommand(LoadTimer, CanLoadTimer);
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            DataContext = this;
            LoadSavedSettings();
        }

        private void LoadSavedSettings()
        {
            try
            {
                var settings = adoAssistant.LoadAllTimerSettings();
                SavedSettings.Clear();
                foreach (var setting in settings)
                {
                    SavedSettings.Add(setting);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartTimer(object parameter)
        {
            if (int.TryParse(MinutesInput.Text, out int minutes) && minutes >= 0 &&
                int.TryParse(SecondsInput.Text, out int seconds) && seconds >= 0)
            {
                remainingSeconds = (minutes * 60) + seconds;
                if (remainingSeconds > 0)
                {
                    try
                    {
                        adoAssistant.SaveTimerSettings(minutes, seconds);
                        LoadSavedSettings(); // Refresh the ComboBox after saving
                        timer.Start();
                        StartButton.IsEnabled = false;
                        ResetButton.IsEnabled = true;
                        UpdateTimerDisplay();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

        private void ResetTimer(object parameter)
        {
            timer.Stop();
            remainingSeconds = 0;
            MinutesInput.Text = "0";
            SecondsInput.Text = "0";
            TimerLabel.Text = "Залишилось: 0:00";
            StartButton.IsEnabled = true;
            ResetButton.IsEnabled = false;
        }

        private void LoadTimer(object parameter)
        {
            if (SelectedTimerSetting == null)
            {
                MessageBox.Show("Виберіть налаштування для завантаження.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var settings = adoAssistant.LoadTimerSettingsById(SelectedTimerSetting.Id);
                if (settings.HasValue)
                {
                    int minutes = settings.Value.Minutes;
                    int seconds = settings.Value.Seconds;
                    MinutesInput.Text = minutes.ToString();
                    SecondsInput.Text = seconds.ToString();
                    remainingSeconds = (minutes * 60) + seconds;
                    TimerLabel.Text = $"Залишилось: {minutes}:{seconds:D2}";
                }
                else
                {
                    MessageBox.Show("Не вдалося завантажити вибрані налаштування.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanStartTimer(object parameter) => !timer.IsEnabled;
        private bool CanResetTimer(object parameter) => true;
        private bool CanLoadTimer(object parameter) => !timer.IsEnabled;

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateTimerDisplay();

            if (remainingSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Час вийшов!", "Таймер", MessageBoxButton.OK, MessageBoxImage.Information);
                StartButton.IsEnabled = true;
                ResetButton.IsEnabled = false;
            }
        }

        private void UpdateTimerDisplay()
        {
            TimerLabel.Text = $"Залишилось: {remainingSeconds / 60}:{remainingSeconds % 60:D2}";
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

 
}