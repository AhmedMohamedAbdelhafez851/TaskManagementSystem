using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.BLL.DTOs;
using TaskManagementSystem.BLL.Services;
using TaskManagementSystem.Domain.Constants;

namespace TaskManagementSystem.Tests
{
    [TestClass]
    public class TaskServiceTests
    {
        private TaskService _taskService;

        [TestInitialize]
        public void Setup()
        {
            _taskService = new TaskService();
        }

        [TestMethod]
        public void SearchCriteriaDto_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var criteria = new SearchCriteriaDto();

            // Assert
            Assert.AreEqual(1, criteria.PageNumber, "Default page number should be 1");
            Assert.AreEqual(10, criteria.PageSize, "Default page size should be 10");
            Assert.AreEqual("CreatedDate", criteria.SortBy, "Default sort should be by CreatedDate");
            Assert.AreEqual("DESC", criteria.SortDirection, "Default sort direction should be DESC");
        }

        [TestMethod]
        public void CreateTaskDto_CanBeInitialized()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "This is a test task",
                AssignedToUserId = 2
            };

            // Assert
            Assert.AreEqual("Test Task", dto.Title);
            Assert.AreEqual("This is a test task", dto.Description);
            Assert.AreEqual(2, dto.AssignedToUserId);
        }

        [TestMethod]
        public void UpdateTaskDto_CanBeInitialized()
        {
            // Arrange
            var dto = new UpdateTaskDto
            {
                TaskId = 5,
                Title = "Updated Title",
                Description = "Updated Description",
                AssignedToUserId = 3,
                Status = TaskStatusConstants.InProgress
            };

            // Assert
            Assert.AreEqual(5, dto.TaskId);
            Assert.AreEqual("Updated Title", dto.Title);
            Assert.AreEqual("Updated Description", dto.Description);
            Assert.AreEqual(3, dto.AssignedToUserId);
            Assert.AreEqual(TaskStatusConstants.InProgress, dto.Status);
        }

        [TestMethod]
        public void TaskStatusConstants_ShouldContainValidStatuses()
        {
            // Assert
            CollectionAssert.Contains(TaskStatusConstants.All, TaskStatusConstants.New);
            CollectionAssert.Contains(TaskStatusConstants.All, TaskStatusConstants.InProgress);
            CollectionAssert.Contains(TaskStatusConstants.All, TaskStatusConstants.Completed);
            Assert.AreEqual(3, TaskStatusConstants.All.Length);
        }

        [TestMethod]
        public void TaskStatusConstants_IsValid_ShouldReturnTrueForValidStatus()
        {
            // Assert
            Assert.IsTrue(TaskStatusConstants.IsValid(TaskStatusConstants.New));
            Assert.IsTrue(TaskStatusConstants.IsValid(TaskStatusConstants.InProgress));
            Assert.IsTrue(TaskStatusConstants.IsValid(TaskStatusConstants.Completed));
        }

        [TestMethod]
        public void TaskStatusConstants_IsValid_ShouldReturnFalseForInvalidStatus()
        {
            // Assert
            Assert.IsFalse(TaskStatusConstants.IsValid("InvalidStatus"));
            Assert.IsFalse(TaskStatusConstants.IsValid(""));
            Assert.IsFalse(TaskStatusConstants.IsValid(null));
        }

        [TestMethod]
        public void TaskStatusConstants_GetDisplayName_ShouldReturnCorrectValue()
        {
            // Assert
            Assert.AreEqual("New", TaskStatusConstants.GetDisplayName(TaskStatusConstants.New));
            Assert.AreEqual("In Progress", TaskStatusConstants.GetDisplayName(TaskStatusConstants.InProgress));
            Assert.AreEqual("Completed", TaskStatusConstants.GetDisplayName(TaskStatusConstants.Completed));
        }
    }
}