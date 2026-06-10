<%@ Page Title="Search Tasks" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="SearchTasks.aspx.cs" Inherits="TaskManagementSystem.Web.Admin.SearchTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Custom CSS for Search Tasks page -->
    <link href="../Content/custom/search-tasks.css" rel="stylesheet" />

    <div class="container mt-4">
        
        <!-- Page Header -->
        <div class="page-header fade-in">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h1 class="mb-2"><i class="fas fa-search me-3"></i>Task Explorer</h1>
                    <p class="mb-0 opacity-75">Search, filter, and manage all tasks in the system</p>
                </div>
                <div>
                    <i class="fas fa-tasks fa-3x opacity-50"></i>
                </div>
            </div>
        </div>
        
        <!-- Error Display -->
        <div id="errorDiv" runat="server" class="alert alert-danger fade-in" style="display:none; border-radius:15px;">
            <i class="fas fa-exclamation-triangle me-2"></i>
            <asp:Literal ID="litError" runat="server" />
        </div>
        
        <!-- Advanced Filter Section -->
        <div class="filter-card fade-in">
            <h5 class="mb-4"><i class="fas fa-sliders-h me-2 text-primary"></i>Advanced Filters</h5>
            <div class="row g-3">
                <div class="col-md-5">
                    <label class="form-label fw-semibold">Assigned To</label>
                    <asp:DropDownList ID="ddlAssignedTo" runat="server" CssClass="form-select search-box">
                        <asp:ListItem Text="-- All Members --" Value="" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-5">
                    <label class="form-label fw-semibold">Task Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select search-box">
                        <asp:ListItem Text="-- All Statuses --" Value="" />
                        <asp:ListItem Text="New" Value="New" />
                        <asp:ListItem Text="In Progress" Value="InProgress" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 d-flex align-items-end">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-gradient w-100" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
        
        <!-- Quick Filter Section -->
        <div class="filter-card fade-in">
            <div class="row align-items-center">
                <div class="col-md-7">
                    <label class="form-label fw-semibold"><i class="fas fa-search me-2 text-info"></i>Instant Filter</label>
                    <div class="input-group">
                        <span class="input-group-text bg-white" style="border-radius: 50px 0 0 50px;">
                            <i class="fas fa-filter text-muted"></i>
                        </span>
                        <input type="text" id="txtQuickSearch" class="form-control search-box" style="border-radius: 0;" placeholder="Type to filter by ID, Title, Assignee, or Status..." onkeyup="filterTasks();" />
                        <button type="button" class="clear-btn" style="border-radius: 0 50px 50px 0;" onclick="clearFilter();">
                            <i class="fas fa-times me-1"></i> Clear
                        </button>
                    </div>
                    <small class="text-muted mt-2 d-block">
                        <i class="fas fa-info-circle me-1"></i> Filters in real-time - no page reload needed
                    </small>
                </div>
                <div class="col-md-5 text-end">
                    <div id="filterCount" class="filter-count bg-light d-inline-block">
                        <i class="fas fa-chart-line me-2 text-primary"></i>Showing all tasks
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Results Header -->
        <div class="results-header fade-in">
            <div>
                <h4 class="mb-0"><i class="fas fa-list me-2 text-primary"></i>Task Results</h4>
            </div>
            <div>
                <asp:Literal ID="litResultCount" runat="server" />
            </div>
        </div>
        
        <!-- Loading Spinner -->
        <div id="loadingSpinner" class="loading-spinner">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2 text-muted">Loading tasks...</p>
        </div>
        
        <!-- Tasks Grid -->
        <div class="table-responsive fade-in">
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-custom"
                AllowPaging="True" PageSize="10"
                OnPageIndexChanging="gvTasks_PageIndexChanging"
                EmptyDataText="No tasks found matching your criteria"
                ClientIDMode="Static">
                <Columns>
                    <asp:BoundField DataField="TaskId" HeaderText="#" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="Title" HeaderText="Task Title" />
                    <asp:BoundField DataField="AssignedToName" HeaderText="Assignee" HeaderStyle-Width="150px" />
                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <span class='badge-status <%# GetStatusClass(Eval("Status").ToString()) %>'>
                                <%# GetStatusDisplay(Eval("Status").ToString()) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <a href='TaskDetails.aspx?id=<%# Eval("TaskId") %>' class="action-btn">
                                <i class="fas fa-eye me-1"></i> View
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="p-3" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        
        <div class="mt-4 text-center fade-in">
            <a href="Dashboard.aspx" class="btn btn-outline-secondary rounded-pill px-4">
                <i class="fas fa-arrow-left me-2"></i>Back to Dashboard
            </a>
        </div>
        
    </div>
    
    <!-- Custom JavaScript for Search Tasks page -->
    <script src="../Scripts/custom/search-tasks.js"></script>
    
</asp:Content>