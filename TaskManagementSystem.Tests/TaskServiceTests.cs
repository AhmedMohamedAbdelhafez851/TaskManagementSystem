using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.Domain.Constants;

namespace TaskManagementSystem.Tests
{
    [TestClass]
    public class TaskServiceTests
    {
        [TestMethod]
        public void SearchCriteriaDto_DefaultValues_ShouldBeCorrect()
        {
            var criteria = new SearchCriteriaDto();
            Assert.AreEqual(1, criteria.PageNumber);
            Assert.AreEqual(10, criteria.PageSize);
        }

        [TestMethod]
        public void CreateTaskDto_CanBeInitialized()
        {
            var dto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Description",
                AssignedToUserId = 2
            };
            Assert.AreEqual("Test Task", dto.Title);
            Assert.AreEqual("Description", dto.Description);
            Assert.AreEqual(2, dto.AssignedToUserId);
        }

        [TestMethod]
        public void UpdateTaskDto_CanBeInitialized()
        {
            var dto = new UpdateTaskDto
            {
                TaskId = 1,
                Title = "Updated",
                Status = TaskStatusConstants.InProgress
            };
            Assert.AreEqual(1, dto.TaskId);
            Assert.AreEqual("Updated", dto.Title);
            Assert.AreEqual(TaskStatusConstants.InProgress, dto.Status);
        }

        [TestMethod]
        public void TaskStatusConstants_ShouldBeValid()
        {
            Assert.IsTrue(TaskStatusConstants.IsValid(TaskStatusConstants.New));
            Assert.IsTrue(TaskStatusConstants.IsValid(TaskStatusConstants.InProgress));
            Assert.IsTrue(TaskStatusConstants.IsValid(TaskStatusConstants.Completed));
            Assert.IsFalse(TaskStatusConstants.IsValid("Invalid"));
        }
    }
}