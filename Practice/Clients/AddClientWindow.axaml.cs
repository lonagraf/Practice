using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice;

public partial class AddClientWindow : Window
{
    private Database _database = new Database();
    public AddClientWindow()
    {
        Width = 400;
        Height = 600;
        InitializeComponent();
        LoadDataExperienceCmb();
        LoadDataLevelCmb();
    }

    private void AddClientBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _database.openConnection();
            string sql =
                "INSERT INTO practice.client (surname, firstname, birthday, phone_number, email, language_experience, language_level, Login, Password) " +
                "VALUES (@Surname, @Name, @Birthday, @Phone, @Email, @Experience, @Level, @Login, @Password)";
            using(MySqlCommand command = new MySqlCommand(sql, _database.getConnection()))
            {
                command.Parameters.AddWithValue("@Surname", SurnameTxt.Text);
                command.Parameters.AddWithValue("@Name", NameTxt.Text);
                command.Parameters.AddWithValue("@Birthday", BirthdayPicker.SelectedDate.GetValueOrDefault());
                command.Parameters.AddWithValue("@Phone", PhoneTxt.Text);
                command.Parameters.AddWithValue("@Email", EmailTxt.Text);
                int selectedExperienceId = GetSelectedExperienceId(ExperienceCmb.SelectedItem.ToString());
                int selectedLevelId = GetSelectedLevelId(LevelCmb.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Experience", selectedExperienceId);
                command.Parameters.AddWithValue("@Level", selectedLevelId);
                command.Parameters.AddWithValue("@Login", LoginTxt.Text);
                command.Parameters.AddWithValue("@Password", PassTxt.Text);
                command.ExecuteNonQuery();
                var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Успешно добавлен!", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }
            _database.closeConnection();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
    private void LoadDataExperienceCmb()
    {
        _database.openConnection();
        string sql =
            "select experience_name from practice.language_experience;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                ExperienceCmb.Items.Add(reader["experience_name"].ToString());
            }
        }
        _database.closeConnection();
    }
    private void LoadDataLevelCmb()
    {
        _database.openConnection();
        string sql =
            "select level_name from practice.language_level;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                LevelCmb.Items.Add(reader["level_name"].ToString());
            }
        }
        _database.closeConnection();
    }
    private int GetSelectedExperienceId(string selectedExperience)
    {
        _database.openConnection();
        string sql = "SELECT language_experience_id FROM language_experience WHERE experience_name = @selectedExperience";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedExperience", selectedExperience);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    
    private int GetSelectedLevelId(string selectedLevel)
    {
        _database.openConnection();
        string sql = "SELECT language_level_id FROM language_level WHERE level_name = @selectedLevel";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedLevel", selectedLevel);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
}