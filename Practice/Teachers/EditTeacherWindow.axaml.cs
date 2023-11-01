using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace Practice.Teachers;

public partial class EditTeacherWindow : Window
{
    private Database _database = new Database();
    private Teacher _teacher;
    public EditTeacherWindow(Teacher teacher)
    {
        InitializeComponent();
        Width = 400;
        Height = 450;
        _teacher = teacher;
        EditSurnameTxt.Text = _teacher.Surname;
        EditFirstnameTxt.Text = _teacher.Firstname;
        EditPhoneTxt.Text = _teacher.Phone;
        EditEmailtxt.Text = _teacher.Email;
        
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        int id = _teacher.TeacherID;
        _database.openConnection();
        string sql = "update pro1_4.teacher set surname = @Surname, firstname = @Firstname, phone_number = @Phone, email = @Email where teacher_id = @id;";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.AddWithValue("@Surname", EditSurnameTxt.Text);
        command.Parameters.AddWithValue("@Firstname", EditFirstnameTxt.Text);
        command.Parameters.AddWithValue("@Phone", EditPhoneTxt.Text);
        command.Parameters.AddWithValue("@Email", EditEmailtxt.Text);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
        _database.closeConnection();
        var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно изменены!", ButtonEnum.Ok);
        var result = box.ShowAsync();
    }
}