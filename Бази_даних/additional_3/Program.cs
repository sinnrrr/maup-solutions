using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace ImageDatabaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbConnectionString = "Data Source=image_database.db";

            try
            {
                using (SqliteConnection connection = new SqliteConnection(dbConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("З'єднання з базою даних SQLite успішно встановлено.");

                    // Створення таблиці, якщо вона ще не існує
                    CreateImagesTable(connection);

                    while (true)
                    {
                        Console.WriteLine("Оберіть дію:");
                        Console.WriteLine("1. Завантажити зображення");
                        Console.WriteLine("2. Отримати список зображень");
                        Console.WriteLine("3. Видалити зображення");
                        Console.WriteLine("0. Вийти");

                        int choice = Convert.ToInt32(Console.ReadLine());

                        switch (choice)
                        {
                            case 1:
                                UploadImage(connection);
                                break;
                            case 2:
                                GetImageList(connection);
                                break;
                            case 3:
                                DeleteImage(connection);
                                break;
                            case 0:
                                return;
                            default:
                                Console.WriteLine("Невірний вибір. Будь ласка, спробуйте ще раз.");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }

        static void CreateImagesTable(SqliteConnection connection)
        {
            string createImagesTable = @"
                CREATE TABLE IF NOT EXISTS Images (
                  id INTEGER PRIMARY KEY AUTOINCREMENT,
                  name TEXT NOT NULL,
                  data BLOB NOT NULL
                )";

            ExecuteNonQuery(connection, createImagesTable);
            Console.WriteLine("Таблицю 'Images' успішно створено або вона вже існує.");
        }

        static void ExecuteNonQuery(SqliteConnection connection, string query)
        {
            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        static void UploadImage(SqliteConnection connection)
        {
            Console.WriteLine("Введіть повний шлях до зображення:");
            string imagePath = Console.ReadLine();

            byte[] imageBytes = File.ReadAllBytes(imagePath);

            Console.WriteLine("Введіть назву зображення:");
            string imageName = Console.ReadLine();

            string query = "INSERT INTO Images (name, data) VALUES (@name, @data)";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", imageName);
                command.Parameters.AddWithValue("@data", imageBytes);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} рядків додано.");
            }
        }

        static void GetImageList(SqliteConnection connection)
        {
            string query = "SELECT id, name FROM Images";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Список зображень:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader.GetInt32(0)}, Назва: {reader.GetString(1)}");
                    }
                }
            }
        }

        static void DeleteImage(SqliteConnection connection)
        {
            Console.WriteLine("Введіть ID зображення, яке потрібно видалити:");
            int imageId = Convert.ToInt32(Console.ReadLine());

            string query = "DELETE FROM Images WHERE id = @id";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", imageId);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} рядків видалено.");
            }
        }
    }
}

