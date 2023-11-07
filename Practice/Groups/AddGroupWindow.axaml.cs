using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Groups;

public partial class AddGroupWindow : Window
{
    private Database _database = new Database();
    public AddGroupWindow()
    {
        InitializeComponent();
        LoadDataTeacherCmb();
        LoadDataCourseCmb();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _database.openConnection();
        string sql = "insert into practice.`group` (group_name, teacher, max_student_amount, course) " +
                     "values (@groupName, @teacher, @maxStudentAmount, @course);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@groupName", GroupNameTxt.Text);
        int selectedTeacherId = GetSelectedTeacherId(TeacherCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@teacher", selectedTeacherId);
        command.Parameters.AddWithValue("@maxStudentAmount", AmountTxt.Text);
        int selectedCourseId = GetSelectedCourseID(CourseCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@course", selectedCourseId);
        command.ExecuteNonQuery();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Успешно добавлен!", ButtonEnum.Ok);
        var result = box.ShowAsync();
        _database.closeConnection();
    }
    

    private void LoadDataTeacherCmb()
    {
        _database.openConnection();
        string sql = "select concat(surname, ' ', firstname) as teacher from practice.teacher;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            TeacherCmb.Items.Add(reader["teacher"].ToString());
        }
        _database.closeConnection();
    }

    private void LoadDataCourseCmb()
    {
        _database.openConnection();
        string sql = "select course_name from practice.course;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            CourseCmb.Items.Add(reader["course_name"].ToString());
        }
        _database.closeConnection();
    }
    private int GetSelectedTeacherId(string selectedTeacher)
    {
        string sql = "SELECT teacher_id FROM practice.teacher WHERE concat(surname, ' ', firstname) = @selectedTeacher;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedTeacher", selectedTeacher);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }

    private int GetSelectedCourseID(string selectedCourse)
    {
        string sql = "select course_id from practice.course where course_name = @selectedCourse;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedCourse", selectedCourse);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
}