using System;
using MySql.Data.MySqlClient;

namespace Practice.Model;

public class Client
{
    public int ClientID { get; set; }
    public string Surname { get; set; }
    public string Firstname { get; set; }
    public DateTime Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string LanguageExperience { get; set; }
    public string LanguageLevel { get; set; }
}