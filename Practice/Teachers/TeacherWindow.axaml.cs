using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Teachers;

public partial class TeacherWindow : UserControl
{
    private Database _database = new Database();
    private List<Teacher> _teachers;
    private string fullTable = "select teacher_id, surname, firstname, birthday, phone_number, email from teacher;";
    public TeacherWindow()
    {
        InitializeComponent();
        ShowTable(fullTable);
    }
    public void ShowTable(string sql)
    {
        _teachers = new List<Teacher>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentClient = new Teacher()
            {
                TeacherID = reader.GetInt32("teacher_id"),
                Surname = reader.GetString("surname"),
                Firstname = reader.GetString("firstname"),
                Birthday = reader.GetDateTime("birthday"),
                Phone = reader.GetString("phone_number"),
                Email = reader.GetString("email"),
            };
            _teachers.Add(currentClient);
        }
        _database.closeConnection();
        TeacherDataGrid.ItemsSource = _teachers;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddTeacherWindow addTeacherWindow = new AddTeacherWindow();
        addTeacherWindow.Show();
    }

    public void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from practice.teacher where teacher_id = @teacherId;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@teacherId", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
    }
    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Teacher selectedTeacher = TeacherDataGrid.SelectedItem as Teacher;

        if (selectedTeacher != null)
        {
            var warning = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить учителя?", ButtonEnum.YesNo);
            var result = await warning.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedTeacher.TeacherID);
                ShowTable(fullTable);
                var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Учитель успешно удален!", ButtonEnum.Ok);
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
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите учителя для удаления!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Teacher selectedTeacher = TeacherDataGrid.SelectedItem as Teacher;
        if (selectedTeacher != null)
        {
            EditTeacherWindow editTeacherWindow = new EditTeacherWindow(selectedTeacher);
            editTeacherWindow.Show();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите учителя для редактирования!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }
    
}