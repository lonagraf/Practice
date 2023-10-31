using System.Collections.Generic;
using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using Practice.Model;

namespace Practice;

public partial class ClientWindow : UserControl
{
    private Database _database = new Database();
    private List<Client> _clients;

    public string fullTable =
        "select surname, firstname, birthday, phone_number, email, language_need, experience_name, level_name from practice.client " +
        "join practice.language_experience on client.language_experience = language_experience.language_experience_id " +
        "join practice.language_level on client.language_level = language_level.language_level_id;";
    public ClientWindow()
    {
        InitializeComponent();
        ShowTable(fullTable);
        
    }

    public void ShowTable(string sql)
    {
        _clients = new List<Client>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentClient = new Client()
            {
                Surname = reader.GetString("surname"),
                Firstname = reader.GetString("firstname"),
                Birthday = reader.GetDateTime("birthday"),
                PhoneNumber = reader.GetString("phone_number"),
                Email = reader.GetString("email"),
                LanguageNeed = reader.GetString("language_need"),
                LanguageExperience = reader.GetString("experience_name"),
                LanguageLevel = reader.GetString("level_name")
            };
            _clients.Add(currentClient);
        }
        _database.closeConnection();
        ClientDataGrid.ItemsSource = _clients;
    }
}