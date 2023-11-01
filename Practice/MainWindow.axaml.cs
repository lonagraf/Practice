using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice;

public partial class MainWindow : Window
{
    private Database _database = new Database();
    public MainWindow()
    {
        InitializeComponent();
        Width = 1200;
        Height = 450;
    }
    
    private void ClientBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ClientWindow clientWindow = new ClientWindow();
        MainPanel.Children.Add(clientWindow);
    }

    private void CourseBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        CourseWindow courseWindow = new CourseWindow();
        MainPanel.Children.Add(courseWindow);
    }

    private void PaymentBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        PaymentWindow paymentWindow= new PaymentWindow();
        MainPanel.Children.Add(paymentWindow);
    }

    private void ReportBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ReportWindow reportWindow = new ReportWindow();
        MainPanel.Children.Add(reportWindow);
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
    public void DisplayWelcomeMessage(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            WelcomeTxt.Text = $"Добро пожаловать, {username}!";
        }
        else
        {
            WelcomeTxt.Text = "Добро пожаловать!";
        }
    }
}