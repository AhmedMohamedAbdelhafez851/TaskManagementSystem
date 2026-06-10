<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskManagementSystem.Web.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Custom CSS for Admin Dashboard -->
    <link href="../Content/custom/admin-dashboard.css" rel="stylesheet" />

    <div class="container mt-4">
        
        <!-- Welcome Card -->
        <div class="welcome-card fade-in-up">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h2>Welcome back, <asp:Literal ID="litUserName" runat="server" />!</h2>
                    <p class="mb-0 opacity-75">Here's what's happening with your tasks today.</p>
                </div>
                <div>
                    <i class="fas fa-chart-line fa-3x opacity-50"></i>
                </div>
            </div>
        </div>
        
        <!-- Statistics Cards Row -->
        <div class="row mb-4">
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-1">
                <div class="stat-card">
                    <div class="stat-icon total">
                        <i class="fas fa-tasks"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Literal ID="litTotalTasks" runat="server" />
                    </div>
                    <div class="stat-label">Total Tasks</div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-2">
                <div class="stat-card">
                    <div class="stat-icon new">
                        <i class="fas fa-plus-circle"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Literal ID="litNewTasks" runat="server" />
                    </div>
                    <div class="stat-label">New Tasks</div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-3">
                <div class="stat-card">
                    <div class="stat-icon progress">
                        <i class="fas fa-spinner"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Literal ID="litInProgressTasks" runat="server" />
                    </div>
                    <div class="stat-label">In Progress</div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 mb-4 fade-in-up delay-4">
                <div class="stat-card">
                    <div class="stat-icon completed">
                        <i class="fas fa-check-circle"></i>
                    </div>
                    <div class="stat-value">
                        <asp:Literal ID="litCompletedTasks" runat="server" />
                    </div>
                    <div class="stat-label">Completed</div>
                </div>
            </div>
        </div>
        
        <!-- Action Buttons Row -->
        <div class="row mb-4">
            <div class="col-md-6 mb-4">
                <div class="action-card fade-in-up delay-1">
                    <div class="action-icon create">
                        <i class="fas fa-plus-circle"></i>
                    </div>
                    <div class="action-title">Create New Task</div>
                    <div class="action-desc">Create and assign new tasks to members</div>
                    <asp:Button ID="btnCreateTask" runat="server" Text="Create Task" CssClass="btn-custom btn-custom-primary" OnClick="btnCreateTask_Click" />
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="action-card fade-in-up delay-2">
                    <div class="action-icon search">
                        <i class="fas fa-search"></i>
                    </div>
                    <div class="action-title">Search Tasks</div>
                    <div class="action-desc">Search, filter, and manage existing tasks</div>
                    <asp:Button ID="btnSearchTasks" runat="server" Text="Search Tasks" CssClass="btn-custom btn-custom-success" OnClick="btnSearchTasks_Click" />
                </div>
            </div>
        </div>
        
        <!-- Recent Tasks Section -->
        <div class="recent-card fade-in-up delay-3">
            <div class="recent-header">
                <h4><i class="fas fa-clock me-2 text-primary"></i> Recent Tasks</h4>
                <asp:Button ID="btnViewAll" runat="server" Text="View All Tasks" CssClass="btn btn-sm btn-outline-primary" OnClick="btnSearchTasks_Click" />
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvRecentTasks" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-custom"
                    EmptyDataText="No tasks found.">
                    <Columns>
                        <asp:BoundField DataField="TaskId" HeaderText="ID" />
                        <asp:BoundField DataField="Title" HeaderText="Task Title" />
                        <asp:BoundField DataField="AssignedToName" HeaderText="Assigned To" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='badge-status badge-<%# GetBadgeClass(Eval("Status").ToString()) %>'>
                                    <%# GetStatusDisplayName(Eval("Status").ToString()) %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href='TaskDetails.aspx?id=<%# Eval("TaskId") %>' class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-eye"></i> View
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        
    </div>
</asp:Content>