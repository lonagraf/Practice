using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace Practice;

public partial class PaymentWindow : UserControl
{
    private Database _database = new Database();
    private List<Payment> _payments;

    private string _courseTable =
        "select course_name, price from payment\njoin practice.course c on c.course_id = payment.course;";
    public PaymentWindow()
    {
        InitializeComponent();
        ShowTable(_courseTable);
    }
    public void ShowTable(string sql)
    {
        _payments = new List<Payment>();
        _database.openConnection();
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            CourseListBox.Items.Add(reader["course_name"].ToString() +" - " + reader["price"].ToString() + " руб.");
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
        // Проходим по элементам в CartListBox и вычисляем общую сумму
        foreach (var item in CartListBox.Items)
        {
            // Предполагая, что элементы имеют формат "название курса - цена руб."
            string itemString = item.ToString();
            // Проводите парсинг цены из строки элемента и прибавляйте к общей сумме
            // Предполагая, что цена находится после " - " в элементе строки
            string priceString = itemString.Split('-').Last().Trim().Split(' ')[0]; // Получаем цену
            double price;
            if (double.TryParse(priceString, out price))
            {
                totalAmount += price;
            }
        }

        // Обновляем TextBlock с общей суммой
        SumTextBlock.Text = "Итого: " + totalAmount.ToString("C"); // Форматируем сумму как денежное значение
    }
}