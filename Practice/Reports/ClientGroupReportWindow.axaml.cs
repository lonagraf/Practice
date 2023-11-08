using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice.Reports;

public partial class ClientGroupReportWindow : Window
{
    private Database _database = new Database();
    private List<Report> _reports = new List<Report>();

    private string _sql = "select group_name, count(`client`) as total_clients from clients_in_groups " +
                          "join practice.`group` g on g.group_id = clients_in_groups.`group` " +
                          "group by group_name;";
    public ClientGroupReportWindow()
    {
        Width = 237;
        Height = 300;
        InitializeComponent();
        ShowTable(_sql);
    }
    private void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentClientGroup = new Report()
            {
                Group = reader.GetString("group_name"),
                TotalClients = reader.GetInt32("total_clients")
            };
            _reports.Add(currentClientGroup);
        }
        _database.closeConnection();
        ClientGroupDataGrid.ItemsSource = _reports;
    }
}