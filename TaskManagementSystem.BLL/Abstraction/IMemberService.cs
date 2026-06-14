using System;
using System.Collections.Generic;
using TaskManagementSystem.BLL.DTOs;

namespace TaskManagementSystem.BLL.Abstraction
{
    public interface IMemberService : IDisposable
    {
        DashboardStatisticsDto GetMemberStatistics(int userId);
        List<MemberTaskDto> GetMemberTasks(int userId);
        List<OverdueTaskDto> GetOverdueNotifications(int userId);
    }
}