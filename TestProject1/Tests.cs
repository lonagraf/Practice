using System;
using System.Collections.Generic;
using ClassLibrary1;
using NUnit.Framework;

namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void DisplayWelcomeMessage_WithUsername()
        {
            //Arrange
            string username = "John";
            string expected = $"Добро пожаловать, {username}!";
            var message = new WelcomeMessage();
            
            //Act
            string actual = message.DisplayWelcomeMessage(username);
            
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DisplayWelcomeMessage_WithoutUsername()
        {
            //Arrange
            string expected = "Добро пожаловать!";
            var message = new WelcomeMessage();
            
            //Act
            string actual = message.DisplayWelcomeMessage("");
            
            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        
    }
}