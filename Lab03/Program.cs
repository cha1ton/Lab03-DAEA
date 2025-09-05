using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03
{
    // Clase para representar un estudiante
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    internal class Program
    {
        static string connectionString = "Data Source=LAB1502-023\\SQLEXPRESS;" +
                                          "Initial Catalog=Tecsup2025; User Id=userChalton; Pwd=123456;" +
                                          "TrustServerCertificate=true";

        static void Main(string[] args)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    Console.WriteLine("Conexion Exitosa GAAA!!!");

                    // Llamar función conectada (DataTable)
                    DataTable dtStudents = GetStudentsUsingDataTable(sqlConnection);
                    Console.WriteLine("Lista de estudiantes usando DataTable:");
                    foreach (DataRow row in dtStudents.Rows)
                    {
                        Console.WriteLine($"{row["StudentId"]} - {row["FirstName"]} {row["LastName"]}");
                    }

                    // Llamar función desconectada (Lista de objetos)
                    List<Student> studentsList = GetStudentsUsingList();
                    Console.WriteLine("\nLista de estudiantes usando Lista de objetos:");
                    foreach (var student in studentsList)
                    {
                        Console.WriteLine($"{student.StudentId} - {student.FirstName} {student.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Función conectada que usa DataTable para retornar estudiantes
        static DataTable GetStudentsUsingDataTable(SqlConnection connection)
        {
            string query = "SELECT StudentId, FirstName, LastName FROM Students";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Función desconectada que retorna lista de objetos Student
        static List<Student> GetStudentsUsingList()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT StudentId, FirstName, LastName FROM Students";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    });
                }

                reader.Close();
            }

            return students;
        }
    }
}
