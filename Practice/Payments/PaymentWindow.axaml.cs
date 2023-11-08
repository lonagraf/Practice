using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using Practice.Payments;

namespace Practice;

public partial class PaymentWindow : UserControl
{
    private int AuthorizedClientId;
    private Database _database = new Database();
    private List<Payment> _payments;

    private string _courseTable =
        "select course_name, price from course;";

    public PaymentWindow(int authorizedClientId)
    {
        InitializeComponent();
        ShowTable(_courseTable);
        LoadDataPaymentMethodCmb();
        AuthorizedClientId = authorizedClientId;
    }

    public void ShowTable(string sql)
    {
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            CourseListBox.Items.Add(reader["course_name"].ToString() + " - " + reader["price"].ToString() + " руб.");
        }

        _database.closeConnection();
    }

    private void CourseListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (CourseListBox.SelectedItem != null)
        {
            string selectedCourse = CourseListBox.SelectedItem.ToString();
            CartListBox.Items.Add(selectedCourse);
        }
        UpdateTotalAmount();
    }

    private void CartListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (CartListBox.SelectedItem != null)
        {
            string selectedItem = CartListBox.SelectedItem.ToString();
            CartListBox.Items.Remove(selectedItem);
        }
        UpdateTotalAmount();
    }

    private void UpdateTotalAmount()
    {
        double totalAmount = 0;
        foreach (var item in CartListBox.Items)
        {
            string itemString = item.ToString();
            string priceString = itemString.Split('-').Last().Trim().Split(' ')[0];
            double price;
            if (double.TryParse(priceString, out price))
            {
                totalAmount += price;
            }
        }

        SumTextBlock.Text = "Итого: " + totalAmount.ToString("C");
    }

    private void PayBtn_OnClick(object? sender, RoutedEventArgs e)
    {
            if (PaymentMethodCmb.SelectedItem != null && CartListBox.Items.Count > 0 && AuthorizedClientId > 0)
            {
                foreach (var item in CartListBox.Items)
                {
                    string selectedItem = item.ToString();
                    string courseName = selectedItem.Split('-')[0].Trim();
                    int courseId = GetCourseIdFromDatabase(courseName);
                    string paymentMethodName = PaymentMethodCmb.SelectedItem.ToString();
                    int paymentMethodId = GetPaymentMethodIdFromDatabase(paymentMethodName);
                    double price = double.Parse(selectedItem.Split('-').Last().Trim().Split(' ')[0]);
                    InsertPaymentData(courseId, AuthorizedClientId, price, paymentMethodId);
                }
                CartListBox.Items.Clear();
                UpdateTotalAmount();
                var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Успешно оплачено!", ButtonEnum.Ok);
                var result = success.ShowAsync();
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка",
                    "Выберите способ оплаты или добавьте товары в корзину!!", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }
    }
    public void LoadDataPaymentMethodCmb()
    {
        _database.openConnection();
        string sql = "select method_name from payment_method;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            PaymentMethodCmb.Items.Add(reader["method_name"].ToString());
        }
        _database.closeConnection();
    }

    private void InsertPaymentData(int courseName, int clientId, double price, int paymentMethod)
    {
        _database.openConnection();
        string sql =
            "INSERT INTO payment (course, client, sum, payment_method) VALUES (@course, @client, @sum, @payment_method);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());

        command.Parameters.AddWithValue("@course", courseName);
        command.Parameters.AddWithValue("@client", clientId);
        command.Parameters.AddWithValue("@sum", price);
        command.Parameters.AddWithValue("@payment_method", paymentMethod);
        command.ExecuteNonQuery();

        _database.closeConnection();
    }

    private int GetCourseIdFromDatabase(string courseName)
    {
        int courseId = -1;
        _database.openConnection();
        string sql = "select course_id from course where course_name = @courseName;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.Add("@courseName", MySqlDbType.VarChar).Value = courseName;
        object result = command.ExecuteScalar();
        if (result != null)
        {
            courseId = Convert.ToInt32(result);
        }
        _database.closeConnection();
        return courseId;
    }

    private int GetPaymentMethodIdFromDatabase(string methodName)
    {
        int methodId = -1;
        _database.openConnection();
        string sql = "select payment_method_id from payment_method where method_name = @methodName;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.Add("@methodName", MySqlDbType.VarChar).Value = methodName;
        object result = command.ExecuteScalar();
        if (result != null)
        {
            methodId = Convert.ToInt32(result);
        }
        _database.closeConnection();
        return methodId;
    }
    

    private void ReceiptBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ReceiptWindow receiptWindow = new ReceiptWindow();
        receiptWindow.Show();
    }
}
