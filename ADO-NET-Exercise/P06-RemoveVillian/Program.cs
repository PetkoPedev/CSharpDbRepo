using System;
using System.Data.SqlClient;

namespace P06_RemoveVillian
{
    class Program
    {
        private static string connectionString = "Server=DESKTOP-E5PBLPH\\SQLEXPRESS;" +
            "Database=MinionsDB;" +
            "Integrated Security=true";

        private static SqlConnection connection = new SqlConnection(connectionString);

        private static SqlTransaction transaction;
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            connection.Open();

            using (connection)
            {
                transaction = connection.BeginTransaction();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = "SELECT Name FROM Villains WHERE Id = @villainId";
                command.Parameters.AddWithValue("@villainId", id);

                object value = command.ExecuteScalar();

                if (value == null)
                {
                    Console.WriteLine("No such villain was found.");
                }
            }
        }
    }
}
