using System;
using System.Data;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice;

public partial class AuthWindow : MainWindow
{
    private Database _database = new Database();
    public AuthWindow()
    {
        InitializeComponent();
        Width = 400;
        Height = 400;
        Icon = new WindowIcon("WinIcon/clown.png");
    }

    private void RegistrationTxt_OnTapped(object? sender, TappedEventArgs e)
    {
        AddClientWindow registrationWindow = new AddClientWindow();
        registrationWindow.Show();
    }

    private void AuthBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        string loginUser = LoginTxt.Text;
        string passUser = PassTxt.Text;
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        string sql = "select * from client where login = @loginUser and password = @passUser";
        MySqlCommand command = new MySqlCommand(sql, database.getConnection());
        command.Parameters.Add("@loginUser", MySqlDbType.VarChar).Value = loginUser;
        command.Parameters.Add("@passUser", MySqlDbType.VarChar).Value = passUser;
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            int authorizedClientId = GetClientIdFromDatabase(loginUser);
            var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Вы успешно вошли", ButtonEnum.Ok);
            var result = box.ShowAsync();
            this.Hide();
            string username = GetUserNameFromDatabase(loginUser); 
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.DisplayWelcomeMessage(username); 
            mainWindow.AuthorizedClientId = authorizedClientId;
            mainWindow.Show();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ошибка входа", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }
    public string GetUserNameFromDatabase(string login)
    {
        string username = ""; 
        _database.openConnection(); 
        string sql = "SELECT firstname FROM practice.client WHERE login = @login;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@login", login);
        var result = command.ExecuteScalar();
        if (result != null) 
        {
            username = result.ToString();
        }
        _database.closeConnection();
        return username;
    }
    private int GetClientIdFromDatabase(string username)
    {
        int userId = -1;
        _database.openConnection();
        string sql = "select client_id from client where login = @username;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
        object result = command.ExecuteScalar();
        if (result != null)
        {
            userId = Convert.ToInt32(result);
        }
        _database.closeConnection();
        return userId;
    }
}