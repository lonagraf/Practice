using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using Practice.Courses;

namespace Practice;

public partial class CourseWindow : UserControl
{
    private Database _database = new Database();
    private List<Course> _courses;

    private string _fullTable =
        "select course_id, course_name, course_description, language_name, price from course " +
        "join practice.language_study ls on ls.language_study_id = course.language_study;";
    public CourseWindow()
    {
        InitializeComponent();
        ShowTable(_fullTable);
    }

    public void ShowTable(string sql)
    {
        _courses = new List<Course>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentCourse = new Course()
            {
                CourseID = reader.GetInt32("course_id"),
                CourseName = reader.GetString("course_name"),
                CourseDesc = reader.GetString("course_description"),
                LanguageStudy = reader.GetString("language_name"),
                Price = reader.GetDouble("price")
            };
            _courses.Add(currentCourse);
        }
        _database.closeConnection();
        CourseDataGrid.ItemsSource = _courses;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddCourseWindow addCourseWindow = new AddCourseWindow();
        addCourseWindow.Show();
    }

    private void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from course where course_id = @courseId;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@courseId", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Course selectedCourse = CourseDataGrid.SelectedItem as Course;
        if (selectedCourse != null)
        {
            var warning = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить курс?", ButtonEnum.YesNo);
            var result = await warning.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedCourse.CourseID);
                ShowTable(_fullTable);
                var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Курс успешно удален!", ButtonEnum.Ok);
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
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите строку для удаления!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Course selectedCourse = CourseDataGrid.SelectedItem as Course;
        if (selectedCourse != null)
        {
            EditCourseWindow editCourseWindow = new EditCourseWindow(selectedCourse);
            editCourseWindow.Show();
            ShowTable(_fullTable);
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите строку для редактирования",
                ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
        
    }
}