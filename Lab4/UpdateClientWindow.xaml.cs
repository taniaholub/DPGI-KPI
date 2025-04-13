using System.Data;
using System.Windows;

namespace Lab4
{
    public partial class UpdateClientWindow : Window
    {
        private AdoAssistant _adoAssistant;
        private DataRow _clientRow;
        private MainWindow _mainWindow;

        public UpdateClientWindow(DataRow clientRow)
        {
            InitializeComponent();
            _adoAssistant = new AdoAssistant();
            _clientRow = clientRow;
            _mainWindow = Application.Current.MainWindow as MainWindow;

            ClientNameTextBox.Text = _clientRow["ClientName"].ToString();
            PhoneNumberTextBox.Text = _clientRow["PhoneNumber"].ToString();
            AddressTextBox.Text = _clientRow["Address"].ToString();
            OrderAmountTextBox.Text = _clientRow["OrderAmount"].ToString();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int clientId = (int)_clientRow["ClientId"];
                string clientName = ClientNameTextBox.Text;
                string phoneNumber = PhoneNumberTextBox.Text;
                string address = AddressTextBox.Text;
                decimal orderAmount = decimal.Parse(OrderAmountTextBox.Text);

                _adoAssistant.UpdateClient(clientId, clientName, phoneNumber, address, orderAmount);

                // Оновлюємо список в головному вікні
                if (_mainWindow != null)
                {
                    _mainWindow.RefreshData();
                }

                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Помилка при оновленні клієнта: " + ex.Message);
            }
        }
    }
}

