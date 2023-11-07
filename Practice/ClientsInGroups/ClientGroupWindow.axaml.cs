using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Groups;

public partial class ClientGroupWindow : UserControl
{
    private Database _database = new Database();
    private List<ClientGroup> _clientGroups = new List<ClientGroup>();

    private string fullTable =
        "select clients_in_groups_id, concat(surname, ' ', firstname) as 'client', group_name from practice.clients_in_groups " +
        "join practice.`group` g on g.group_id = clients_in_groups.`group` " +
        "join practice.client c on c.client_id = clients_in_groups.client;";
    public ClientGroupWindow()
    {
        InitializeComponent();
        ShowTable(fullTable);
    }

    public void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentClientGroup = new ClientGroup()
            {
                ClientGroupID = reader.GetInt32("clients_in_groups_id"),
                Client = reader.GetString("client"),
                Group = reader.GetString("group_name")
            };
            _clientGroups.Add(currentClientGroup);
        }
        _database.closeConnection();
        ClientGroupDataGrid.ItemsSource = _clientGroups;
    }

    private void AddClientGroup_OnClick(object? sender, RoutedEventArgs e)
    {
        AddClientGroup addClientGroup = new AddClientGroup();
        addClientGroup.Show();
    }

    public void Delete(int id)
    {
        _database.openConnection();
        string sql = "delete from practice.clients_in_groups where clients_in_groups_id = @clientGroupId;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@clientGroupId", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
    }
    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ClientGroup selectedClientGroup = ClientGroupDataGrid.SelectedItem as ClientGroup;
        if (selectedClientGroup != null)
        {
            var warning = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить?", ButtonEnum.YesNo);
            var result = await warning.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                Delete(selectedClientGroup.ClientGroupID);
                ShowTable(fullTable);
                var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Успешно удален!", ButtonEnum.Ok);
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
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите поле для удаления!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }
    
}