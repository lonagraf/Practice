using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Practice;

public partial class MainWindow : Window
{
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
}