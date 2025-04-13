using System.Windows;

namespace Lab4
{
    public partial class CreateClientWindow : Window
    {
        private AdoAssistant _adoAssistant;
        private MainWindow _mainWindow;

        public CreateClientWindow()
        {
            InitializeComponent();
            _adoAssistant = new AdoAssistant();
            _mainWindow = Application.Current.MainWindow as MainWindow;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string clientId = ClientId.Text;
                string clientName = ClientNameTextBox.Text;
                string phoneNumber = PhoneNumberTextBox.Text;
                string address = AddressTextBox.Text;
                decimal orderAmount = decimal.Parse(OrderAmountTextBox.Text);

                _adoAssistant.AddClient(clientId, clientName, phoneNumber, address, orderAmount);

                // Оновлюємо список в головному вікні
                if (_mainWindow != null)
                {
                    _mainWindow.RefreshData();
                }

                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Помилка при створенні клієнта: " + ex.Message);
            }
        }
    }
}
