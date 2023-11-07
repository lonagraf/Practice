using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddGroupWindow addGroupWindow = new AddGroupWindow();
        addGroupWindow.Show();
    }

    public void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from practice.`group` where group_id = @groupId;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@groupId", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Group selectedGroup = GroupDataGrid.SelectedItem as Group;

        if (selectedGroup != null)
        {
            var warning = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить группу?", ButtonEnum.YesNo);
            var result = await warning.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedGroup.GroupID);
                ShowTable(_fullTable);
                var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Группа успешно удалена!", ButtonEnum.Ok);
                var successResult = box.ShowAsync();
            }
            else
            {
                var cancelBox = MessageBoxManager.GetMessageBoxStandard("Отмена", "Операция удаления отменена", ButtonEnum.Ok);
                var cancelResult = cancelBox.ShowAsync();
            }
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите группу для удаления!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        EditGroupWindow editGroupWindow = new EditGroupWindow();
        editGroupWindow.Show();
    }
}