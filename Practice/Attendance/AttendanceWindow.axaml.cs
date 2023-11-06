using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Groups;

public partial class AttendanceWindow : UserControl
{
    private Database _database = new Database();
    private List<Attendance> _attendances = new List<Attendance>();

    private string fullTable =
        "select attendance_id, start, end, concat(surname, ' ', firstname) as 'client' from practice.attendance " +
        "join practice.client c on c.client_id = attendance.client " +
        "join practice.schedule s on s.schedule_id = attendance.schedule;";
    public AttendanceWindow()
    {
        InitializeComponent();
        ShowTable(fullTable);
    }

    public void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentAttendance = new Attendance()
            {
                AttendanceID = reader.GetInt32("attendance_id"),
                Start = reader.GetDateTime("start"),
                End = reader.GetDateTime("end"),
                Client = reader.GetString("client")
            };
            _attendances.Add(currentAttendance);
        }
        _database.closeConnection();
        AttendanceDataGrid.ItemsSource = _attendances;
    }
}