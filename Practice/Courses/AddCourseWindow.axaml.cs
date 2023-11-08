using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Courses;

public partial class AddCourseWindow : Window
{
    private Database _database = new Database();
    public AddCourseWindow()
    {
        Width = 300;
        Height = 300;
        InitializeComponent();
        LoadDataLanguageCmb();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _database.openConnection();
        string sql =
            "insert into course (course_name, course_description, language_study, price) values (@courseName, @courseDesc, @language, @price);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@courseName", NameTxt.Text);
        command.Parameters.AddWithValue("@courseDesc", DescTxt.Text);
        int selectedLanguageId = GetLanguageStudyId(LanguageCmb.SelectedItem.ToString());
        command.Parameters.AddWithValue("@language", selectedLanguageId);
        command.Parameters.AddWithValue("@price", PriceTxt.Text);
        command.ExecuteNonQuery();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно добавлены!", ButtonEnum.Ok);
        var result = box.ShowAsync();
        _database.closeConnection();
    }

    private void LoadDataLanguageCmb()
    {
        _database.openConnection();
        string sql = "select language_name from language_study;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            LanguageCmb.Items.Add(reader["language_name"].ToString());
        }
        _database.closeConnection();
    }

    private int GetLanguageStudyId(string selectedLanguage)
    {
        _database.openConnection();
        string sql = "select language_study_id from language_study where language_name = @languageName;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@languageName", selectedLanguage);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
}