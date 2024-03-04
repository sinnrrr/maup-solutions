using System;
using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        // Перевірка наявності необхідних аргументів командного рядка
        if (args.Length < 4 || args[0] != "-db" || args[2] != "-t")
        {
            Console.WriteLine("Потрібно вказати шлях до файлу бази даних (-db) та назву таблиці (-t).");
            return;
        }

        string dbFile = args[1];
        string tableName = args[3];
        bool verbose = args.Length >= 5 && args[4] == "-v";

        // Підключення до бази даних SQLite
        using (var connection = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
        {
            connection.Open();

            // Вибірка даних з таблиці
            string selectQuery = $"SELECT * FROM {tableName};";
            using (var command = new SQLiteCommand(selectQuery, connection))
            using (var reader = command.ExecuteReader())
            {
                // Вивід заголовків, якщо вказано параметр -v
                if (verbose)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine($"Поле {reader.GetName(i)}: тип {reader.GetFieldType(i)}");
                    }
                }

                // Вивід даних
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
