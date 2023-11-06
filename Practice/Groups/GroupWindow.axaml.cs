using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Groups;

public partial class GroupWindow : UserControl
{
    private Database _database = new Database();
    private List<Group> _groups;

    private string _fullTable =
        "select group_id, group_name, concat(surname,' ',firstname) as 'teacher', max_student_amount, course_name from `group` " +
        "join practice.course c on `group`.course = c.course_id " +
        "join practice.teacher t on t.teacher_id = `group`.teacher;";
    public GroupWindow()
    {
        InitializeComponent();
        ShowTable(_fullTable);
    }

    public void ShowTable(string sql)
    {
        _groups = new List<Group>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentGroup = new Group()
            {
                GroupID = reader.GetInt32("group_id"),
                GroupName = reader.GetString("group_name"),
                Teacher = reader.GetString("teacher"),
                MaxStudentAmount = reader.GetInt32("max_student_amount"),
                Course = reader.GetString("course_name")
            };
            _groups.Add(currentGroup);
        }
        _database.closeConnection();
        GroupDataGrid.ItemsSource = _groups;
    }

    private void ScheduleBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ScheduleWindow scheduleWindow = new ScheduleWindow();
        MainPanel.Children.Add(scheduleWindow);
    }

    private void ClientBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ClientGroupWindow clientGroupWindow = new ClientGroupWindow();
        MainPanel.Children.Add(clientGroupWindow);
    }

    private void AppointmentBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        AttendanceWindow attendanceWindow = new AttendanceWindow();
        MainPanel.Children.Add(attendanceWindow);
    }
}