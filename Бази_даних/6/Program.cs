using System;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=lab6.db";

        // Підключення до бази даних
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Створення таблиць та початкове наповнення даними
            CreateAndPopulateDatabase(connection);
            OutputAllData(connection);

            Console.WriteLine("Базу даних успішно створено та наповнено початковими даними.");
        }
    }

    static void CreateAndPopulateDatabase(SqliteConnection connection)
    {
        // Створення таблиць із відповідними полями
        string createUserTableQuery = "CREATE TABLE Users (UserId INTEGER PRIMARY KEY, Username TEXT, Password TEXT)";
        string createRoleTableQuery = "CREATE TABLE Roles (RoleId INTEGER PRIMARY KEY, RoleName TEXT)";
        string createPermissionTableQuery = "CREATE TABLE Permissions (PermissionId INTEGER PRIMARY KEY, PermissionName TEXT)";
        string createObjectTableQuery = "CREATE TABLE Objects (ObjectId INTEGER PRIMARY KEY, ObjectName TEXT)";
        string createUserRoleTableQuery = "CREATE TABLE UserRoles (UserId INTEGER, RoleId INTEGER, FOREIGN KEY(UserId) REFERENCES Users(UserId), FOREIGN KEY(RoleId) REFERENCES Roles(RoleId))";
        string createRolePermissionTableQuery = "CREATE TABLE RolePermissions (RoleId INTEGER, PermissionId INTEGER, FOREIGN KEY(RoleId) REFERENCES Roles(RoleId), FOREIGN KEY(PermissionId) REFERENCES Permissions(PermissionId))";
        string createUserObjectPermissionTableQuery = "CREATE TABLE UserObjectPermissions (UserId INTEGER, ObjectId INTEGER, PermissionId INTEGER, FOREIGN KEY(UserId) REFERENCES Users(UserId), FOREIGN KEY(ObjectId) REFERENCES Objects(ObjectId), FOREIGN KEY(PermissionId) REFERENCES Permissions(PermissionId))";

        ExecuteQuery(connection, createUserTableQuery);
        ExecuteQuery(connection, createRoleTableQuery);
        ExecuteQuery(connection, createPermissionTableQuery);
        ExecuteQuery(connection, createObjectTableQuery);
        ExecuteQuery(connection, createUserRoleTableQuery);
        ExecuteQuery(connection, createRolePermissionTableQuery);
        ExecuteQuery(connection, createUserObjectPermissionTableQuery);

        // Наповнення таблиць даними
        // Наповнення таблиці Users
        ExecuteQuery(connection, "INSERT INTO Users (UserId, Username, Password) VALUES (1, 'user1', 'password1')");
        ExecuteQuery(connection, "INSERT INTO Users (UserId, Username, Password) VALUES (2, 'user2', 'password2')");

        // Наповнення таблиці Roles
        ExecuteQuery(connection, "INSERT INTO Roles (RoleId, RoleName) VALUES (1, 'admin')");
        ExecuteQuery(connection, "INSERT INTO Roles (RoleId, RoleName) VALUES (2, 'user')");

        // Наповнення таблиці Permissions
        ExecuteQuery(connection, "INSERT INTO Permissions (PermissionId, PermissionName) VALUES (1, 'read')");
        ExecuteQuery(connection, "INSERT INTO Permissions (PermissionId, PermissionName) VALUES (2, 'write')");

        // Наповнення таблиці Objects
        ExecuteQuery(connection, "INSERT INTO Objects (ObjectId, ObjectName) VALUES (1, 'object1')");
        ExecuteQuery(connection, "INSERT INTO Objects (ObjectId, ObjectName) VALUES (2, 'object2')");

        // Наповнення таблиці UserRoles
        ExecuteQuery(connection, "INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1)");
        ExecuteQuery(connection, "INSERT INTO UserRoles (UserId, RoleId) VALUES (2, 2)");

        // Наповнення таблиці RolePermissions
        ExecuteQuery(connection, "INSERT INTO RolePermissions (RoleId, PermissionId) VALUES (1, 1)");
        ExecuteQuery(connection, "INSERT INTO RolePermissions (RoleId, PermissionId) VALUES (2, 2)");

        // Наповнення таблиці UserObjectPermissions
        ExecuteQuery(connection, "INSERT INTO UserObjectPermissions (UserId, ObjectId, PermissionId) VALUES (1, 1, 1)");
        ExecuteQuery(connection, "INSERT INTO UserObjectPermissions (UserId, ObjectId, PermissionId) VALUES (2, 2, 2)");
    }

    static void ExecuteQuery(SqliteConnection connection, string query)
    {
        using (SqliteCommand command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }
    
    static void OutputAllData(SqliteConnection connection)
    {
        // Виведення даних з таблиці Users
        OutputTableData(connection, "Users");

        // Виведення даних з таблиці Roles
        OutputTableData(connection, "Roles");

        // Виведення даних з таблиці Permissions
        OutputTableData(connection, "Permissions");

        // Виведення даних з таблиці Objects
        OutputTableData(connection, "Objects");

        // Виведення даних з таблиці UserRoles
        OutputTableData(connection, "UserRoles");

        // Виведення даних з таблиці RolePermissions
        OutputTableData(connection, "RolePermissions");

        // Виведення даних з таблиці UserObjectPermissions
        OutputTableData(connection, "UserObjectPermissions");
    }

    static void OutputTableData(SqliteConnection connection, string tableName)
    {
        Console.WriteLine($"Дані з таблиці \"{tableName}\":");

        // Retrieve column names
        var columnNames = new List<string>();
        using (var command = new SqliteCommand($"PRAGMA table_info({tableName})", connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnNames.Add(reader.GetString(1)); // Column name is at index 1
                }
            }
        }

        // Output column names
        foreach (var columnName in columnNames)
        {
            Console.Write($"{columnName,-20}"); // Adjust spacing as needed
        }
        Console.WriteLine();

        // Retrieve and output data
        using (SqliteCommand command = new SqliteCommand($"SELECT * FROM {tableName}", connection))
        {
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader[i],-20}"); // Adjust spacing as needed
                    }
                    Console.WriteLine();
                }
            }
        }
        Console.WriteLine();
    }
}

