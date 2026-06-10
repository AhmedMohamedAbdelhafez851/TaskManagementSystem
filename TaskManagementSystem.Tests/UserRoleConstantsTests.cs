using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.Domain.Constants;

namespace TaskManagementSystem.Tests
{
    [TestClass]
    public class UserRoleConstantsTests
    {
        [TestMethod]
        public void UserRoleConstants_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.AreEqual("Admin", UserRoleConstants.Admin);
            Assert.AreEqual("Member", UserRoleConstants.Member);
        }

        [TestMethod]
        public void UserRoleConstants_IsValid_ShouldReturnTrueForValidRoles()
        {
            // Assert
            Assert.IsTrue(UserRoleConstants.IsValid(UserRoleConstants.Admin));
            Assert.IsTrue(UserRoleConstants.IsValid(UserRoleConstants.Member));
        }

        [TestMethod]
        public void UserRoleConstants_IsValid_ShouldReturnFalseForInvalidRoles()
        {
            // Assert
            Assert.IsFalse(UserRoleConstants.IsValid("SuperAdmin"));
            Assert.IsFalse(UserRoleConstants.IsValid(""));
            Assert.IsFalse(UserRoleConstants.IsValid(null));
        }

        [TestMethod]
        public void UserRoleConstants_IsAdmin_ShouldReturnTrueForAdmin()
        {
            // Assert
            Assert.IsTrue(UserRoleConstants.IsAdmin(UserRoleConstants.Admin));
            Assert.IsFalse(UserRoleConstants.IsAdmin(UserRoleConstants.Member));
        }

        [TestMethod]
        public void UserRoleConstants_IsMember_ShouldReturnTrueForMember()
        {
            // Assert
            Assert.IsTrue(UserRoleConstants.IsMember(UserRoleConstants.Member));
            Assert.IsFalse(UserRoleConstants.IsMember(UserRoleConstants.Admin));
        }
    }
}