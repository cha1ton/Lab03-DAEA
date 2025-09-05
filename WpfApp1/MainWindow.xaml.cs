using Microsoft.Data.SqlClient;  // Asegúrate de usar Microsoft.Data.SqlClient
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=LAB1502-023\\SQLEXPRESS;" +
                                          "Initial Catalog=Tecsup2025; User Id=userChalton; Pwd=123456;" +
                                          "TrustServerCertificate=true";

        public MainWindow()
        {
            InitializeComponent();
            CargarEstudiantes(""); // Carga inicial sin filtro
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = txtSearch.Text.Trim();
            CargarEstudiantes(filtro);
        }

        private void CargarEstudiantes(string nombreFiltro)
        {
            List<Student> estudiantes = new List<Student>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Consulta con parámetro para evitar inyección SQL
                    string query = @"
                        SELECT StudentId, FirstName, LastName
                        FROM Students
                        WHERE FirstName LIKE @nombre OR LastName LIKE @nombre";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", "%" + nombreFiltro + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                estudiantes.Add(new Student
                                {
                                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                });
                            }
                        }
                    }
                }

                dgStudents.ItemsSource = estudiantes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estudiantes: " + ex.Message);
            }
        }
    }

    // Clase para estudiante
    public class Student
    {
        public int StudentId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
