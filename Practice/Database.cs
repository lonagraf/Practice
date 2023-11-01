using System.Data;
using MySql.Data.MySqlClient;

namespace Practice;

public class Database
{
   private MySqlConnection _connection = new MySqlConnection(@"server=10.10.1.24;database=pro1_4;port=3306;User Id=user_01;password=user01pro");
   public void openConnection() 
   {
      if (_connection.State == ConnectionState.Closed)
      {
         _connection.Open();
      }
   }
   public void closeConnection() 
   {
      if (_connection.State == ConnectionState.Open)
      {
         _connection.Close();
      }
   }
   public MySqlConnection getConnection() 
   {
      return _connection;
   }
}