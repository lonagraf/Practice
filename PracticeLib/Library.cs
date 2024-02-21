using System.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using Practice;

namespace PracticeLib;

public class Library
{
    Database _database = new Database();
    public string DisplayWelcomeMessage(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            return $"Добро пожаловать, {username}!";
        }
        else
        {
            return "Добро пожаловать!";
        }
    }
    
    public bool Delete(int id)
    {
        try
        {
            _database.openConnection();
            string sql = "delete from practice.client where client_id = @clientId;";
            MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
            command.Parameters.AddWithValue("@clientId", id);
            int rowsAffected = command.ExecuteNonQuery();
            _database.closeConnection();
            return rowsAffected > 0; // Возвращаем true, если удаление прошло успешно
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при удалении: " + ex.Message);
            return false; // Возвращаем false в случае ошибки
        }
    }
    
    public bool Auth(string login, string password)
    {
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        string sql = "select * from client where login = @loginUser and password = @passUser";
        MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
        command.Parameters.Add("@loginUser", MySqlDbType.VarChar).Value = login;
        command.Parameters.Add("@passUser", MySqlDbType.VarChar).Value = password;
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool InsertPaymentData(int courseName, int clientId, double price, int paymentMethod)
    {
        try
        {
            _database.openConnection();
            string sql = "INSERT INTO payment (course, client, sum, payment_method) VALUES (@course, @client, @sum, @payment_method);";
            MySqlCommand command = new MySqlCommand(sql, _database.getConnection());

            command.Parameters.AddWithValue("@course", courseName);
            command.Parameters.AddWithValue("@client", clientId);
            command.Parameters.AddWithValue("@sum", price);
            command.Parameters.AddWithValue("@payment_method", paymentMethod);
            command.ExecuteNonQuery();

            return true; // Вернуть true при успешной операции вставки
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inserting payment data: " + ex.Message);
            return false; // Вернуть false в случае ошибки
        }
        finally
        {
            _database.closeConnection();
        }
    }
    
    public bool AddCourse(string name, string desc, int language, double price)
    {
        try
        {
            _database.openConnection();
            string sql =
                "insert into course (course_name, course_description, language_study, price) values (@courseName, @courseDesc, @language, @price);";
            MySqlCommand command = new MySqlCommand(sql, _database.getConnection());
            command.Parameters.AddWithValue("@courseName", name);
            command.Parameters.AddWithValue("@courseDesc", desc);
            command.Parameters.AddWithValue("@language", language);
            command.Parameters.AddWithValue("@price", price);
            command.ExecuteNonQuery();
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        finally
        {
            _database.closeConnection();
        }
        
    }
}