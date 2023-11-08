using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using Practice.Groups;
using Practice.Teachers;

namespace Practice;

public partial class MainWindow : Window
{
    private Database _database = new Database();
    public int AuthorizedClientId { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        Width = 1200;
        Height = 450;
        Icon = new WindowIcon("WinIcon/apps.png");
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
        PaymentWindow paymentWindow= new PaymentWindow(AuthorizedClientId);
        MainPanel.Children.Add(paymentWindow);
    }

    private void ReportBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ReportWindow reportWindow = new ReportWindow();
        MainPanel.Children.Add(reportWindow);
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
    

    private void TeacherBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        TeacherWindow teacherWindow = new TeacherWindow();
        MainPanel.Children.Add(teacherWindow);
    }

    private void GroupBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        GroupWindow groupWindow = new GroupWindow();
        MainPanel.Children.Add(groupWindow);
    }

    private void LogOutBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}