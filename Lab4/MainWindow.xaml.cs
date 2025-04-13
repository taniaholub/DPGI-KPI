using System.Windows;
using System.Data;

namespace Lab4
{
    public partial class MainWindow : Window
    {
        private AdoAssistant _adoAssistant;

        public MainWindow()
        {
            InitializeComponent();
            _adoAssistant = new AdoAssistant();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        // Метод для оновлення даних у списку
        public void RefreshData()
        {
            int selectedIndex = list.SelectedIndex;
            list.DataContext = _adoAssistant.TableLoad();

            // Зберігаємо вибраний елемент, якщо можливо
            if (list.Items.Count > 0)
            {
                list.SelectedIndex = selectedIndex >= 0 && selectedIndex < list.Items.Count
                    ? selectedIndex
                    : 0;
                list.Focus();
            }
        }

        // Обробник для додавання запису
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateClientWindow();
            createWindow.ShowDialog();
            // Оновлюємо список після закриття вікна
            RefreshData();
        }

        // Обробник для оновлення запису
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = list.SelectedItem as DataRowView;
            if (selectedClient != null)
            {
                var updateWindow = new UpdateClientWindow(selectedClient.Row);
                updateWindow.ShowDialog();
                // Оновлюємо список після закриття вікна
                RefreshData();
            }
        }

        // Обробник для видалення запису
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = list.SelectedItem as DataRowView;
            if (selectedClient != null)
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цього клієнта?",
                    "Підтвердження видалення", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _adoAssistant.DeleteClient((int)selectedClient["ClientId"]);
                        RefreshData();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Помилка при видаленні клієнта: " + ex.Message);
                    }
                }
            }
        }
    }
}
