using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Client = Practice.Model.Client;

namespace Practice;

public partial class EditClientWindow : Window
{
    private Database _database = new Database();
    private Client _client;
    public EditClientWindow(Client client)
    {
        InitializeComponent();
        Width = 400;
        Height = 450;
        _client = client;
        EditSurnameTxt.Text = _client.Surname;
        EditFirstnameTxt.Text = _client.Firstname;
        EditPhoneTxt.Text = _client.PhoneNumber;
        EditEmailtxt.Text = _client.Email;
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        int id = _client.ClientID;
        _database.openConnection();
        string sql =
            "update practice.client set surname = @surname, firstname = @firstname, phone_number = @phonenumber, email = @email where client_id = @id;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@surname", EditSurnameTxt.Text);
        command.Parameters.AddWithValue("@firstname", EditFirstnameTxt.Text);
        command.Parameters.AddWithValue("@phonenumber", EditPhoneTxt.Text);
        command.Parameters.AddWithValue("@email", EditEmailtxt.Text);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно изменены!", ButtonEnum.Ok);
        var result = box.ShowAsync();
    }
}