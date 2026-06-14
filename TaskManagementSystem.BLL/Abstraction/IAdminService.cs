using System.Collections.Generic;
using TaskManagementSystem.BLL.DTOs;

namespace TaskManagementSystem.BLL.Abstraction
{
    public interface IAdminService
    {
        DashboardStatisticsDto GetDashboardStatistics();
        List<RecentTaskDto> GetRecentTasks(int count = 5);
    }
}