using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practice.Reports;

namespace Practice;

public partial class ReportWindow : UserControl
{
    public ReportWindow()
    {
        InitializeComponent();
    }

    private void CourseReportBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        CourseReportWindow courseReportWindow = new CourseReportWindow();
        courseReportWindow.Show();
    }

    private void AttendanceReportBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AttendanceReportWindow attendanceReportWindow = new AttendanceReportWindow();
        attendanceReportWindow.Show();
    }

    private void ClientGroupReportBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ClientGroupReportWindow clientGroupReportWindow = new ClientGroupReportWindow();
        clientGroupReportWindow.Show();
    }
}