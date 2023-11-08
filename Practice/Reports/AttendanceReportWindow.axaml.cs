using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Reports;

public partial class AttendanceReportWindow : Window
{
    private Database _database = new Database();
    private List<Report> _reports = new List<Report>();

    private string _sql = "SELECT concat(surname, ' ', firstname) as client, " +
                          "SUM(CASE WHEN attendance_status = 'Был' THEN 1 ELSE 0 END) AS total_was, " +
                          "SUM(CASE WHEN attendance_status = 'Не был' THEN 1 ELSE 0 END) AS total_was_not FROM attendance " +
                          "join practice.client c on c.client_id = attendance.client " +
                          "GROUP BY client;";
    public AttendanceReportWindow()
    {
        Width = 455;
        Height = 300;
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
            var currentAttendance = new Report()
            {
                Client = reader.GetString("client"),
                TotalWas = reader.GetInt32("total_was"),
                TotalWasnt = reader.GetInt32("total_was_not")
            };
            _reports.Add(currentAttendance);
        }
        _database.closeConnection();
        AttendanceReportDataGrid.ItemsSource = _reports;
    }
}