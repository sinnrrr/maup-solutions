using System;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=lab4.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            CreateTableA(connection);
            CreateTableVeryLongNameOfTable(connection);

            // Виконуємо різні SELECT запити з JOIN
            PerformSimpleSelectFromBothTables(connection);
            PerformSelectWithAlias(connection);
            PerformSelectWithCondition(connection, "B");
            PerformCrossJoin(connection);
            PerformJoinWithCondition(connection);
            PerformJoinWithTwoAsAndOneB(connection);
            PerformLeftOuterJoin(connection, true); // A LEFT JOIN B
            PerformLeftOuterJoin(connection, false); // B LEFT JOIN A
        }
    }

    static void CreateTableA(SqliteConnection connection)
    {
        string createTableACommand = "CREATE TABLE IF NOT EXISTS A (id INTEGER);";
        ExecuteNonQuery(connection, createTableACommand);
        string insertDataACommand = "INSERT INTO A (id) VALUES (1), (2), (3), (4), (NULL);";
        ExecuteNonQuery(connection, insertDataACommand);
    }

    static void CreateTableVeryLongNameOfTable(SqliteConnection connection)
    {
        string createTableCommand = "CREATE TABLE IF NOT EXISTS creative_and_long_table_name (id INTEGER);";
        ExecuteNonQuery(connection, createTableCommand);
        string insertDataCommand = "INSERT INTO creative_and_long_table_name (id) VALUES (1), (2), (300), (NULL);";
        ExecuteNonQuery(connection, insertDataCommand);
    }

    static void PerformSimpleSelectFromBothTables(SqliteConnection connection)
    {
        string query = "SELECT * FROM A, creative_and_long_table_name;";
        ExecuteQuery(connection, query);
    }

    static void PerformSelectWithAlias(SqliteConnection connection)
    {
        string query = "SELECT A.id, B.id FROM A, creative_and_long_table_name AS B;";
        ExecuteQuery(connection, query);
    }

    static void PerformSelectWithCondition(SqliteConnection connection, string alias)
    {
        string query = $"SELECT A.id AS A_id, {alias}.id AS {alias}_id FROM A, creative_and_long_table_name AS {alias} WHERE A.id = {alias}.id;";
        ExecuteQuery(connection, query);
    }

    static void PerformCrossJoin(SqliteConnection connection)
    {
        string query = "SELECT A.id, B.id FROM A CROSS JOIN creative_and_long_table_name B;";
        ExecuteQuery(connection, query);
    }

    static void PerformJoinWithCondition(SqliteConnection connection)
    {
        string query = "SELECT A.id, B.id FROM A JOIN creative_and_long_table_name B ON A.id = B.id;";
        ExecuteQuery(connection, query);
    }

    static void PerformJoinWithTwoAsAndOneB(SqliteConnection connection)
    {
        string query = "SELECT A1.id, A2.id, B.id FROM A A1 JOIN creative_and_long_table_name B ON A1.id = B.id JOIN A A2 ON A1.id = A2.id AND A1.id != A2.id;";
        ExecuteQuery(connection, query);
    }

    static void PerformLeftOuterJoin(SqliteConnection connection, bool aLeftJoinB)
    {
        string query;
        if (aLeftJoinB)
        {
            query = "SELECT A.id, B.id FROM A LEFT OUTER JOIN creative_and_long_table_name B ON A.id = B.id;";
        }
        else
        {
            query = "SELECT B.id, A.id FROM creative_and_long_table_name B LEFT OUTER JOIN A ON B.id = A.id;";
        }
        ExecuteQuery(connection, query);
    }

    static void ExecuteNonQuery(SqliteConnection connection, string query)
    {
        using (var command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    static void ExecuteQuery(SqliteConnection connection, string query)
    {
        using (var command = new SqliteCommand(query, connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetValue(i)}\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
