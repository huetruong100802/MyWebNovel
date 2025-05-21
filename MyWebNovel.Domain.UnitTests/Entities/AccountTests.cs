using MyWebNovel.Domain.Entities.Accounts;
using MyWebNovel.Domain.Entities.Accounts.Exceptions;

namespace MyWebNovel.Domain.UnitTests.Entities
{
    public class AccountTests
    {
        [Theory]
        [InlineData("incomplete-email@")]
        [InlineData("sample.email")]
        internal void Should_Throw_Invalid_Email_Domain_Exception_For_Invalid_Email(string invalidEmail)
        {
            Assert.Throws<InvalidEmailDomainException>(() =>
            {
                new Email(invalidEmail);
            });
        }

        [Fact]
        public void Should_Throw_Invalid_Email_Length_Exception_For_Too_Long_Email()
        {
            // Arrange
            var maxLength = AccountConstraint.Account.EmailMaxLength;
            var longEmail = new string('a', maxLength + 1) + "@example.com"; // Adjust to exceed max length

            // Act & Assert
            Assert.Throws<InvalidEmailLengthException>(() => new Email(longEmail));
        }

        [Theory]
        [InlineData("AB")]
        [InlineData("Jonathan Alexander Montgomery-Harrington Fitzwilliam Cunningham-Smithington Kensington Wetherby-Clarkson Paddington-Livingston O'Sullivan Carrington-Rutherford")]
        internal void Should_Throw_Invalid_Username_Exception_For_Too_Short_Or_Too_Long_Username(string username)
        {
            Assert.Throws<InvalidUsernameException>(() =>
            {
                new Username(username);
            });
        }

        [Theory]
        [InlineData("Short1!")] // Too short
        [InlineData("Long1234567891011!")] // Too long
        [InlineData("NoSymbols123")] // Missing symbol or punctuation
        [InlineData("nouppercase1!")] // Missing uppercase character
        [InlineData("!@#$%^&*()")] // No letter or digit
        [InlineData("")] // Empty password
        [InlineData("1234567890!")] // No letter
        [InlineData("Pass Word1")] // space in between
        public void Should_Throw_Invalid_Password(string plainText)
        {
            Assert.Throws<InvalidPasswordException>(() =>
            {
                Password.CreateHashed(plainText);
            });
        }

        [Fact]
        public void Should_Create_Password()
        {
            // Arrange
            var plainText = "Password123!";
            // Act
            var password = Password.CreateHashed(plainText);
            // Assert
            Assert.True(password.Verify(plainText));
        }

        [Fact]
        public void Should_Create_Account()
        {
            // Arrange
            var username = new Username("JohnDoe");
            var email = new Email("testEmail@gmail.com");
            var password = Password.CreateHashed("Password123!");
            var roleId = 1;

            // Act
            var account = Account.Create(username, email, password, roleId);

            // Assert
            Assert.NotNull(account);
            Assert.Equal((Username)username, account.Username);
            Assert.Equal(email, account.Email);
            Assert.True(account.Password.Verify("Password123!"));
            Assert.Equal(roleId, account.RoleId);
        }

        [Fact]
        public void Should_Change_Password()
        {
            // Arrange
            var username = new Username("JohnDoe");
            var email = new Email("testEmail@gmail.com");
            var password = Password.CreateHashed("Password123!");
            var roleId = 1;
            var newPassword = "NewPassword123!";

            // Act
            var account = Account.Create(username, email, password, roleId);
            account.ChangePassword(newPassword);

            // Assert
            Assert.True(account.Password.Verify(newPassword));
        }
    }
}
