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
    public AuthWindow()
    {
        InitializeComponent();
        Width = 400;
        Height = 400;
    }

    private void RegistrationTxt_OnTapped(object? sender, TappedEventArgs e)
    {
        this.Hide();
        AddWindow registrationWindow = new AddWindow();
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
            var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Вы успешно вошли", ButtonEnum.Ok);
            var result = box.ShowAsync();
            this.Hide();
            string username = GetUserNameFromDatabase(loginUser); 
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.DisplayWelcomeMessage(username); 
            mainWindow.Show();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ошибка входа", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }
    
}