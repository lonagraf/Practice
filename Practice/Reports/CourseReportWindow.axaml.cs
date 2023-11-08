using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Reports;

public partial class CourseReportWindow : Window
{
    private Database _database = new Database();
    private List<Report> _courseReports = new List<Report>();

    private string _sql =
        "SELECT c.course_name, COUNT(p.client) AS total_clients, SUM(`sum`) AS total_sum FROM course c " +
        "JOIN payment p ON c.course_id = p.course " +
        "GROUP BY c.course_id " +
        "ORDER BY total_clients DESC;";
    public CourseReportWindow()
    {
        Width = 490;
        Height = 200;
        InitializeComponent();
        ShowTable(_sql);
    }

    private void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentReport = new Report()
            {
                CourseName = reader.GetString("course_name"),
                TotalClients = reader.GetInt32("total_clients"),
                TotalSum = reader.GetDouble("total_sum")
            };
            _courseReports.Add(currentReport);
        }
        _database.closeConnection();
        CourseReportDataGrid.ItemsSource = _courseReports;
    }
}