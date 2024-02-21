using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    public class WelcomeMessage
    {
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
    }

    
}