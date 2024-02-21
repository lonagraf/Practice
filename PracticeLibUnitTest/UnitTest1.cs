using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using Practice;
using PracticeLib;

namespace PracticeLibUnitTest;

public class Tests
{
    private Library _library;
    [SetUp]
    public void Setup()
    {
        _library = new Library();
    }
    
    

    [Test]
    public void DisplayWelcomeMessage_WithUsername()
    {
        //Arrange
        string username = "John";
        string expected = $"Добро пожаловать, {username}!";
        
            
        //Act
        string actual = _library.DisplayWelcomeMessage(username);
            
        //Assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void DisplayWelcomeMessage_WithoutUsername()
    {
        //Arrange
        string expected = "Добро пожаловать!";
        
            
        //Act
        string actual = _library.DisplayWelcomeMessage("");
            
        //Assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Delete_ExistingClient()
    {
        //Arrange
        int clientId = 20;
        
        //Act
        bool result = _library.Delete(clientId);
        
        //Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Delete_UnexistingClient()
    {
        //Arrange
        int clientId = -1;
       
        
        //Act
        bool result = _library.Delete(clientId);
        
        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void Auth_ExistingUser()
    {
        //Arrange
        string login = "admin";
        string password = "admin";
        
        //Act
        bool result = _library.Auth(login, password);
        
        //Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Auth_UnexistingUser()
    {
        //Arrange
        string login = "bibaboba";
        string password = "helpme";
        
        //Act
        bool result = _library.Auth(login, password);
        
        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void InsertPayment_ValidData()
    {
        //Arrange
        int course = 1;
        int client = 1;
        double price = 1000;
        int method = 1;
        
        //Act
        bool result = _library.InsertPaymentData(course, client, price, method);
        
        //Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void InsertPayment_InvalidData()
    {
        //Arrange
        int course = -1;
        int client = -1;
        double price = -1000;
        int method = -1;
        
        //Act
        bool result = _library.InsertPaymentData(course, client, price, method);
        
        //Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void InsertCourse_ValidData()
    {
        //Arrange
        string name = "course";
        string desc = "description";
        int language = 1;
        double price = 10000;
        
        //Act
        bool result = _library.AddCourse(name, desc, language, price);
        
        //Assert
        Assert.IsTrue(result);
    }
    
    [Test]
    public void InsertCourse_InvalidData()
    {
        //Arrange
        string name = "course";
        string desc = "description";
        int language = -1;
        double price = -10000;
        
        //Act
        bool result = _library.AddCourse(name, desc, language, price);
        
        //Assert
        Assert.IsFalse(result);
    }
}