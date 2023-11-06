using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Groups;

public partial class AddClientGroup : Window
{
    private Database _database = new Database();
    public AddClientGroup()
    {
        InitializeComponent();
        LoadDataClientCmb();
        LoadDataGroupCmb();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _database.openConnection();
            string sql =
                "INSERT INTO practice.clients_in_groups (client, `group`) " +
                "VALUES (@client, @group)";
            using(MySqlCommand command = new MySqlCommand(sql, _database.getConnection()))
            {
                int selectedClientId = GetSelectedClientId(ClientCmb.SelectedItem.ToString());
                int selectedGroupId = GetSelectedGroupId(GroupCmb.SelectedItem.ToString());
                command.Parameters.AddWithValue("@client", selectedClientId);
                command.Parameters.AddWithValue("@group", selectedGroupId);
                command.ExecuteNonQuery();
                var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Успешно добавлен!", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }
            _database.closeConnection();
        }
        catch (Exception exception)
        {
            var ex = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Добавлено максимальное количество студентов в группу!", ButtonEnum.Ok);
            var result = ex.ShowAsync();
        }
    }

    private void LoadDataClientCmb()
    {
        _database.openConnection();
        string sql = "select concat(surname, ' ', firstname) as client from client;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ClientCmb.Items.Add(reader["client"].ToString());
        }
        _database.closeConnection();
    }

    private void LoadDataGroupCmb()
    {
        _database.openConnection();
        string sql = "select group_name from `group`";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            GroupCmb.Items.Add(reader["group_name"].ToString());
        }
        _database.closeConnection();
    }
    private int GetSelectedClientId(string selectedClient)
    {
        string sql = "SELECT client_id FROM client WHERE concat(surname, ' ', firstname) = @selectedClient";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedClient", selectedClient);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    private int GetSelectedGroupId(string selectedGroup)
    {
        string sql = "SELECT group_id FROM `group` WHERE group_name = @selectedGroup";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@selectedGroup", selectedGroup);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
}