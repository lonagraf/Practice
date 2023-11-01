using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using Practice.Model;

namespace Practice;

public partial class ClientWindow : UserControl
{
    private Database _database = new Database();
    private List<Client> _clients;

    public string fullTable =
        "select client_id, surname, firstname, birthday, phone_number, email, language_need, experience_name, level_name from practice.client " +
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
                ClientID = reader.GetInt32("client_id"),
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

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AddWindow addWindow = new AddWindow();
        addWindow.Show();
    }

    public void Delete(int id)
    {
        try
        {
            _database.openConnection();
            string sql = "delete from practice.client where client_id = @clientId;";
            MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
            command.Parameters.AddWithValue("@clientId", id);
            command.ExecuteNonQuery();
            _database.closeConnection();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при удалении: " + ex.Message);
        }
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            Client selectedClient = ClientDataGrid.SelectedItem as Client;

            if (selectedClient != null)
            {
                var warning = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить клиента?", ButtonEnum.YesNo);
                var result = await warning.ShowAsync();
                if (result == ButtonResult.Yes)
                {
                    Delete(selectedClient.ClientID);
                    ShowTable(fullTable);
                    var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Клиент успешно удален!", ButtonEnum.Ok);
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
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите клиента для удаления!", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при удалении: " + ex.Message);
            throw;
        }
    }
}