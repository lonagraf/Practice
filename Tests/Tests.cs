using System;
using NUnit.Framework;
using Pra

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void AuthBtn_Click_Success()
        {
            //Arrange
            AuthWindow authWindow = new AuthWindow();
            
            //Act
            authWindow.AuthBtn_OnClick(null, null);
        }
    }
}