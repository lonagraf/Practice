using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Courses;

public partial class EditCourseWindow : Window
{
    private Database _database = new Database();
    private Course _course;
    public EditCourseWindow(Course course)
    {
        Width = 300;
        Height = 300;
        InitializeComponent();
        _course = course;
        NameTxt.Text = _course.CourseName;
        DescTxt.Text = _course.CourseDesc;
        PriceTxt.Text = _course.Price.ToString();
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        int id = _course.CourseID;
        _database.openConnection();
        string sql =
            "update course set course_name = @courseName, course_description = @courseDesc, price = @price where course_id = @id;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@courseName", NameTxt.Text);
        command.Parameters.AddWithValue("@courseDesc", DescTxt.Text);
        command.Parameters.AddWithValue("@price", PriceTxt.Text);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно изменены", ButtonEnum.Ok);
        var result = box.ShowAsync();
    }
}