using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Groups;

public partial class ScheduleWindow : UserControl
{
    private Database _database = new Database();
    private List<Schedule> _schedules = new List<Schedule>();

    private string fullTable = "select schedule_id, start, end, group_name from practice.schedule " +
                               "join practice.`group` g on g.group_id = schedule.`group`;";
    public ScheduleWindow()
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
            var currentSchedule = new Schedule()
            {
                ScheduleID = reader.GetInt32("schedule_id"),
                Start = reader.GetDateTime("start"),
                End = reader.GetDateTime("end"),
                Group = reader.GetString("group_name")
            };
            _schedules.Add(currentSchedule);
        }
        _database.closeConnection();
        ScheduleDataGrid.ItemsSource = _schedules;
    }
}