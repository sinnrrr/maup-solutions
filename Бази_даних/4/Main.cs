using System;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        // Підключення до бази даних SQLite
        using (var connection = new SQLiteConnection("Data Source=suppliers.db;Version=3;"))
        {
            connection.Open();
            
            // Створення таблиці Suppliers з використанням AUTOINCREMENT, NOT NULL і DEFAULT XXX
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierName TEXT NOT NULL,
                    Address TEXT DEFAULT 'Unknown'
                );";
            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            // Вставка даних
            string insertQuery1 = "INSERT INTO Suppliers (SupplierName, Address) VALUES ('Company A', 'Address 1');";
            string insertQuery2 = "INSERT INTO Suppliers (SupplierName) VALUES ('Company B');"; // Відсутнє значення для адреси, за замовчуванням буде використано 'Unknown'
            using (var command = new SQLiteCommand(insertQuery1, connection))
            {
                command.ExecuteNonQuery();
            }
            using (var command = new SQLiteCommand(insertQuery2, connection))
            {
                command.ExecuteNonQuery();
            }

            // Вибірка даних для демонстрації
            string selectQuery = "SELECT * FROM Suppliers;";
            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("SupplierID\tSupplierName\tAddress");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["SupplierID"]}\t\t{reader["SupplierName"]}\t\t{reader["Address"]}");
                    }
                }
            }

            // Вивід повідомлення про успішне завершення
            Console.WriteLine("Базу даних успішно створено та заповнено початковими даними.");
        }
    }
}
