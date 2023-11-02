using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Schedule;

public partial class ScheduleWindow : UserControl
{
    private Database _database;
    private List<Schedule> _schedules;

    private string _fullString = "select schedule_id, weekday, time_start, time_end, group_name from schedule " +
                                 "join pro1_4.`group` g on schedule.`group` = g.group_id;";
    public ScheduleWindow()
    {
        InitializeComponent();
        ShowTable(_fullString);
    }

    public void ShowTable(string sql)
    {
        _database = new Database();
        _schedules = new List<Schedule>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentSchedule = new Schedule()
            {
                ScheduleID = reader.GetInt32("schedule_id"),
                Weekday = reader.GetString("weekday"),
                TimeStart = reader.GetDateTime("time_start"),
                TimeEnd = reader.GetDateTime("time_end"),
                Group = reader.GetString("group_name")
            };
            _schedules.Add(currentSchedule);
        }
        _database.closeConnection();
        ScheduleDataGrid.ItemsSource = _schedules;
    }
}