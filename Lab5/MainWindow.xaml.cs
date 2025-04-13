
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lab5
{
    public partial class MainWindow : Window
    {
        private DPGI2Entities db = new DPGI2Entities();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // Завантаження даних для всіх вкладок
        private void LoadData()
        {
            // Завантажуємо клієнтів
            ClientsDataGrid.ItemsSource = db.Clients.ToList();
            // Завантажуємо компанії
            CompaniesDataGrid.ItemsSource = db.Companies.ToList();

            // Мінімальні витрати по компаніях
            var minExpensesByCompany = db.Clients
                .Select(c => new {
                    c.CompanyCode,
                    c.Expenses,
                    CompanyName = db.Companies.FirstOrDefault(comp => comp.CompanyCode == c.CompanyCode).CompanyName
                })
                .GroupBy(c => new { c.CompanyCode, c.CompanyName })
                .Select(g => new
                {
                    CompanyName = g.Key.CompanyName,
                    MinExpenses = g.Min(c => c.Expenses)
                })
                .ToList();

            MinExpensesDataGrid.ItemsSource = minExpensesByCompany;

            // Загальні витрати по компаніях
            var revenueData = db.Clients
                .Select(c => new {
                    c.CompanyCode,
                    c.Expenses,
                    CompanyName = db.Companies.FirstOrDefault(comp => comp.CompanyCode == c.CompanyCode).CompanyName
                })
                .ToList()
                .GroupBy(c => new { c.CompanyCode, c.CompanyName })
                .Select(g => new
                {
                    CompanyName = g.Key.CompanyName,
                    TotalExpenses = g.Sum(c => c.Expenses)
                })
                .ToList();

            TotalRevenueDataGrid.ItemsSource = revenueData;

            // Клієнти з компаніями (JOIN)
            var clientsWithCompanies = db.Clients
                .Join(db.Companies,
                    client => client.CompanyCode,
                    company => company.CompanyCode,
                    (client, company) => new
                    {
                        ClientName = client.Name,
                        Phone = client.Phone,
                        CompanyName = company.CompanyName,
                        Expenses = client.Expenses
                    })
                .ToList();

            ClientsWithCompaniesDataGrid.ItemsSource = clientsWithCompanies;
        }

        // Пошук клієнта по назві
        private void SearchClientButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = ClientNameTextBox.Text.Trim();
            FilteredClientsDataGrid.ItemsSource = db.Clients
                .Where(c => c.Name.Contains(searchText))
                .ToList();
        }
    }
}
