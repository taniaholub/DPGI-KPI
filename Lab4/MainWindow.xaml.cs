using System.Data;
using System.Windows;

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

        public void RefreshData()
        {
            list.ItemsSource = _adoAssistant.TableLoad().DefaultView;
        }

        private void list_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (list.SelectedItem is DataRowView selected)
            {
                ClientIdBox.Text = selected["ClientId"].ToString();
                ClientNameBox.Text = selected["ClientName"].ToString();
                PhoneNumberBox.Text = selected["PhoneNumber"].ToString();
                AddressBox.Text = selected["Address"].ToString();
                OrderAmountBox.Text = selected["OrderAmount"].ToString();
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string clientId = ClientIdBox.Text;
                string clientName = ClientNameBox.Text;
                string phone = PhoneNumberBox.Text;
                string address = AddressBox.Text;
                decimal amount = decimal.Parse(OrderAmountBox.Text);

                _adoAssistant.AddClient(clientId, clientName, phone, address, amount);
                RefreshData();
                MessageBox.Show("Клієнта додано.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedItem is DataRowView selected)
            {
                try
                {
                    int clientId = int.Parse(ClientIdBox.Text);
                    string clientName = ClientNameBox.Text;
                    string phone = PhoneNumberBox.Text;
                    string address = AddressBox.Text;
                    decimal amount = decimal.Parse(OrderAmountBox.Text);

                    _adoAssistant.UpdateClient(clientId, clientName, phone, address, amount);
                    RefreshData();
                    MessageBox.Show("Клієнта оновлено.");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Оберіть клієнта для оновлення.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedItem is DataRowView selected)
            {
                if (MessageBox.Show("Видалити цього клієнта?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _adoAssistant.DeleteClient((int)selected["ClientId"]);
                    RefreshData();
                }
            }
        }
    }
}
