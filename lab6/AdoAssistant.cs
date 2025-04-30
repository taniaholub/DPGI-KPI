using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WpfTimerApp
{
    public class TimerSetting
    {
        public int Id { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{CreatedAt:yyyy-MM-dd HH:mm:ss}: {Minutes} хв {Seconds} сек";
        }
    }

    public class AdoAssistant
    {
        private readonly string connectionString;

        public AdoAssistant(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void SaveTimerSettings(int minutes, int seconds)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO TimerSettings (Minutes, Seconds, CreatedAt) VALUES (@Minutes, @Seconds, @CreatedAt)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Minutes", minutes);
                        command.Parameters.AddWithValue("@Seconds", seconds);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при збереженні налаштувань таймера: {ex.Message}", ex);
            }
        }

        public List<TimerSetting> LoadAllTimerSettings()
        {
            var settingsList = new List<TimerSetting>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Minutes, Seconds, CreatedAt FROM TimerSettings ORDER BY CreatedAt DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                settingsList.Add(new TimerSetting
                                {
                                    Id = reader.GetInt32(0),
                                    Minutes = reader.GetInt32(1),
                                    Seconds = reader.GetInt32(2),
                                    CreatedAt = reader.GetDateTime(3)
                                });
                            }
                        }
                    }
                }
                return settingsList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні налаштувань таймера: {ex.Message}", ex);
            }
        }

        public (int Minutes, int Seconds)? LoadTimerSettingsById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Minutes, Seconds FROM TimerSettings WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (reader.GetInt32(0), reader.GetInt32(1));
                            }
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при завантаженні налаштувань таймера за Id: {ex.Message}", ex);
            }
        }
    }
}