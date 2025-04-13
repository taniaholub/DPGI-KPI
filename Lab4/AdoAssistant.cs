using System.Data;
using System.Data.SqlClient;

namespace Lab4
{
    public class AdoAssistant
    {
        private readonly string connectionString = "Data Source=TANIAHOLUB\\MSSQLSERVER07;Initial Catalog=DPGI;Integrated Security=True";

        // Завантаження таблиці
        public DataTable TableLoad()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandText = "SELECT * FROM Clients";
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        // Додавання нового запису
        public void AddClient(string clientId, string clientName, string phoneNumber, string address, decimal orderAmount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Clients (ClientId, ClientName, PhoneNumber, Address, OrderAmount) VALUES (@ClientId, @ClientName, @PhoneNumber, @Address, @OrderAmount)", connection);
                command.Parameters.AddWithValue("@ClientId", clientId); // Виправлено: використовуємо clientId
                command.Parameters.AddWithValue("@ClientName", clientName);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@OrderAmount", orderAmount);
                command.ExecuteNonQuery();
            }
        }

        // Оновлення запису
        public void UpdateClient(int clientId, string clientName, string phoneNumber, string address, decimal orderAmount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Clients SET ClientName = @ClientName, PhoneNumber = @PhoneNumber, Address = @Address, OrderAmount = @OrderAmount WHERE ClientId = @ClientId", connection);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@ClientName", clientName);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@OrderAmount", orderAmount);
                command.ExecuteNonQuery();
            }
        }

        // Видалення запису
        public void DeleteClient(int clientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Clients WHERE ClientId = @ClientId", connection);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.ExecuteNonQuery();
            }
        }
    }
}