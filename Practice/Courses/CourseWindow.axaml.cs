using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice;

public partial class CourseWindow : UserControl
{
    private Database _database = new Database();
    private List<Course> _courses;

    private string _fullTable =
        "select course_id, course_name, course_description, language_name from course " +
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
                LanguageStudy = reader.GetString("language_name")
            };
            _courses.Add(currentCourse);
        }
        _database.closeConnection();
        CourseDataGrid.ItemsSource = _courses;
    }
}