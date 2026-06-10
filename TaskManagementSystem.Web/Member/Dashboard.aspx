<%@ Page Title="My Tasks" Language="C#" MasterPageFile="~/MasterPages/Member.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskManagementSystem.Web.Member.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Custom CSS for Member Dashboard -->
    <link href="../Content/custom/member-dashboard.css" rel="stylesheet" />

    <div class="container mt-4">
        
        <!-- Welcome Card -->
        <div class="welcome-card fade-in-up">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2><i class="fas fa-user-circle me-2"></i> Welcome, <asp:Literal ID="litUserName" runat="server" />!</h2>
                    <p class="mb-0 opacity-75">Here's what's happening with your tasks today.</p>
                </div>
                <div>
                    <i class="fas fa-chart-line fa-3x opacity-50"></i>
                </div>
            </div>
        </div>
        
        <!-- Statistics Cards -->
        <div class="row mb-4">
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-1">
                <div class="stat-card">
                    <div class="stat-icon total"><i class="fas fa-tasks"></i></div>
                    <div class="stat-value"><asp:Literal ID="litTotalTasks" runat="server" /></div>
                    <div class="stat-label">Total Tasks</div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-2">
                <div class="stat-card">
                    <div class="stat-icon new"><i class="fas fa-plus-circle"></i></div>
                    <div class="stat-value"><asp:Literal ID="litNewTasks" runat="server" /></div>
                    <div class="stat-label">New Tasks</div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-3">
                <div class="stat-card">
                    <div class="stat-icon progress"><i class="fas fa-spinner"></i></div>
                    <div class="stat-value"><asp:Literal ID="litInProgressTasks" runat="server" /></div>
                    <div class="stat-label">In Progress</div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-4">
                <div class="stat-card">
                    <div class="stat-icon completed"><i class="fas fa-check-circle"></i></div>
                    <div class="stat-value"><asp:Literal ID="litCompletedTasks" runat="server" /></div>
                    <div class="stat-label">Completed</div>
                </div>
            </div>
        </div>
        
        <!-- Overdue Notification -->
        <asp:Panel ID="pnlNotification" runat="server" Visible="false">
            <div class="notification-card fade-in-up">
                <h5><i class="fas fa-exclamation-triangle me-2"></i> Overdue Tasks Notification</h5>
                <p>The following tasks have been assigned for 3 or more days and are still in "New" status:</p>
                <asp:Repeater ID="rptOverdueTasks" runat="server">
                    <ItemTemplate>
                        <div class="overdue-task">
                            <strong>Task #<%# Eval("TaskId") %>:</strong> <%# Eval("Title") %><br />
                            <small>Assigned on: <%# Convert.ToDateTime(Eval("AssignedDate")).ToString("yyyy-MM-dd") %> (<%# (DateTime.Now - Convert.ToDateTime(Eval("AssignedDate"))).Days %> days ago)</small><br />
                            <a href='TaskDetails.aspx?id=<%# Eval("TaskId") %>' class="btn btn-sm btn-light mt-2">View Task</a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>
        
        <!-- My Tasks Section -->
        <div class="tasks-card fade-in-up">
            <div class="tasks-header">
                <h4><i class="fas fa-list me-2 text-success"></i> My Assigned Tasks</h4>
                <span class="badge bg-secondary"><asp:Literal ID="litTaskCount" runat="server" /> Tasks</span>
            </div>
            
            <div class="task-cards-grid">
                <asp:Repeater ID="rptTasks" runat="server">
                    <ItemTemplate>
                        <div class="task-item-card">
                            <div class="task-id">Task #<%# Eval("TaskId") %></div>
                            <div class="task-title">
                                <a href='TaskDetails.aspx?id=<%# Eval("TaskId") %>'><%# Eval("Title") %></a>
                            </div>
                            <div>
                                <span class='badge-status badge-<%# GetBadgeClass(Eval("Status").ToString()) %>'>
                                    <%# GetStatusDisplayName(Eval("Status").ToString()) %>
                                </span>
                            </div>
                            <div class="task-date">
                                <i class="fas fa-calendar-alt"></i> Created: <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy-MM-dd") %>
                            </div>
                            <div class="task-actions">
                                <a href='TaskDetails.aspx?id=<%# Eval("TaskId") %>' class="btn-sm-custom">View Details</a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            
            <!-- Empty State -->
            <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
                <div class="empty-state">
                    <div class="empty-icon"><i class="fas fa-inbox"></i></div>
                    <div class="empty-text">No tasks assigned to you yet.</div>
                </div>
            </asp:PlaceHolder>
        </div>
        
    </div>
</asp:Content>