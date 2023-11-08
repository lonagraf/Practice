using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using Practice.Teachers;

namespace Practice.Payments;

public partial class ReceiptWindow : Window
{
    private Database _database = new Database();
    private List<Payment> _payments = new List<Payment>();

    private string _fullTable =
        "select payment_id,course_name, concat(surname, ' ', firstname) as client, sum, method_name from payment " +
        "join practice.course c on payment.course = c.course_id " +
        "join practice.client c2 on c2.client_id = payment.client " +
        "join practice.payment_method pm on pm.payment_method_id = payment.payment_method;";
    public ReceiptWindow()
    {
        InitializeComponent();
        ShowTable(_fullTable);
    }
    public void ShowTable(string sql)
    {
        _payments = new List<Payment>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentPayment = new Payment()
            {
                PaymentID = reader.GetInt32("payment_id"),
                CourseName = reader.GetString("course_name"),
                Client = reader.GetString("client"),
                PaymentSum = reader.GetDouble("sum"),
                Method = reader.GetString("method_name")
            };
            _payments.Add(currentPayment);
        }
        _database.closeConnection();
        ReceiptDataGrid.ItemsSource = _payments;
    }
}