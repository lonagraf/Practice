using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Teachers;

public partial class AddTeacherWindow : Window
{
    private Database _database = new Database();
    
    public AddTeacherWindow()
    {
        InitializeComponent();
        Width = 400;
        Height = 450;
    }

    private void AddTeacherBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _database.openConnection();
        string sql =
            "insert into pro1_4.teacher (surname, firstname, birthday, phone_number, email) values (@Surname, @Firstname, @Birthday, @Phone, @Email);";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@Surname", SurnameTxt.Text);
        command.Parameters.AddWithValue("@Firstname", NameTxt.Text);
        command.Parameters.AddWithValue("@Birthday", BirthdayPicker.SelectedDate.GetValueOrDefault());
        command.Parameters.AddWithValue("@Phone", PhoneTxt.Text);
        command.Parameters.AddWithValue("@Email", EmailTxt.Text);
        command.ExecuteNonQuery();
        _database.closeConnection();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Учитель успешно добавлен!", ButtonEnum.Ok);
        var result = box.ShowAsync();
    }
}