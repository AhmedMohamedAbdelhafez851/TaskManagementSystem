using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.BLL.Services;

namespace TaskManagementSystem.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        private PasswordService _passwordService;

        [TestInitialize]
        public void Setup()
        {
            _passwordService = new PasswordService();
        }

        [TestMethod]
        public void HashPassword_Should_GenerateHashAndSalt()
        {
            // Arrange
            string password = "TestPassword123";

            // Act
            _passwordService.HashPassword(password, out string hash, out string salt);

            // Assert
            Assert.IsNotNull(hash, "Hash should not be null");
            Assert.IsNotNull(salt, "Salt should not be null");
            Assert.IsTrue(hash.Length > 0, "Hash should not be empty");
            Assert.IsTrue(salt.Length > 0, "Salt should not be empty");
        }

        [TestMethod]
        public void VerifyPassword_ValidPassword_ShouldReturnTrue()
        {
            // Arrange
            string password = "MySecretPassword";
            _passwordService.HashPassword(password, out string hash, out string salt);

            // Act
            bool result = _passwordService.VerifyPassword(password, hash, salt);

            // Assert
            Assert.IsTrue(result, "Valid password should verify successfully");
        }

        [TestMethod]
        public void VerifyPassword_InvalidPassword_ShouldReturnFalse()
        {
            // Arrange
            string password = "CorrectPassword";
            string wrongPassword = "WrongPassword";
            _passwordService.HashPassword(password, out string hash, out string salt);

            // Act
            bool result = _passwordService.VerifyPassword(wrongPassword, hash, salt);

            // Assert
            Assert.IsFalse(result, "Invalid password should not verify");
        }

        [TestMethod]
        public void HashPassword_Should_GenerateDifferentHashesForSamePassword()
        {
            // Arrange
            string password = "SamePassword";

            // Act
            _passwordService.HashPassword(password, out string hash1, out string salt1);
            _passwordService.HashPassword(password, out string hash2, out string salt2);

            // Assert
            Assert.AreNotEqual(hash1, hash2, "Hashes should be different due to different salts");
            Assert.AreNotEqual(salt1, salt2, "Salts should be different");
        }

        [TestMethod]
        public void HashPassword_EmptyPassword_ShouldStillGenerateHash()
        {
            // Arrange
            string password = "";

            // Act
            _passwordService.HashPassword(password, out string hash, out string salt);

            // Assert
            Assert.IsNotNull(hash);
            Assert.IsNotNull(salt);
            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod]
        public void VerifyPassword_EmptyPassword_ShouldReturnFalseWhenHashExists()
        {
            // Arrange
            string password = "RealPassword";
            _passwordService.HashPassword(password, out string hash, out string salt);

            // Act
            bool result = _passwordService.VerifyPassword("", hash, salt);

            // Assert
            Assert.IsFalse(result);
        }
    }
}