<%@ Page Title="Task Details" Language="C#" MasterPageFile="~/MasterPages/Member.Master" AutoEventWireup="true" CodeBehind="TaskDetails.aspx.cs" Inherits="TaskManagementSystem.Web.Member.TaskDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Custom CSS for Member Task Details page -->
    <link href="../Content/custom/member-task-details.css" rel="stylesheet" />

    <div class="container mt-4">
        
        <!-- Page Header -->
        <div class="page-header fade-in">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h1 class="mb-2"><i class="fas fa-info-circle me-3"></i>Task Details</h1>
                    <p class="mb-0 opacity-75">View and update your task information</p>
                </div>
                <div>
                    <i class="fas fa-tasks fa-3x opacity-50"></i>
                </div>
            </div>
        </div>
        
        <!-- Details Card -->
        <div class="details-card fade-in">
            
            <!-- Success Message -->
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="alert-custom alert-success-custom">
                <i class="fas fa-check-circle fa-lg"></i>
                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
            </asp:Panel>
            
            <!-- Error Message -->
            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert-custom alert-danger-custom">
                <i class="fas fa-exclamation-triangle fa-lg"></i>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </asp:Panel>
            
            <!-- Task ID and Status -->
            <div class="row mb-4">
                <div class="col-md-6">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-hashtag me-2 text-success"></i>Task ID</div>
                        <div class="info-value"><asp:Literal ID="litTaskId" runat="server" /></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-tag me-2 text-success"></i>Status</div>
                        <div class="info-value">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Text="New" Value="New" />
                                <asp:ListItem Text="In Progress" Value="InProgress" />
                                <asp:ListItem Text="Completed" Value="Completed" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Title -->
            <div class="info-row">
                <div class="info-label"><i class="fas fa-heading me-2 text-success"></i>Title</div>
                <div class="info-value"><asp:Literal ID="litTitle" runat="server" /></div>
            </div>
            
            <!-- Description -->
            <div class="info-row">
                <div class="info-label"><i class="fas fa-align-left me-2 text-success"></i>Description</div>
                <div class="info-value"><asp:Literal ID="litDescription" runat="server" /></div>
            </div>
            
            <!-- Assigned To -->
            <div class="row">
                <div class="col-md-6">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-user-check me-2 text-success"></i>Assigned To</div>
                        <div class="info-value"><asp:Literal ID="litAssignedTo" runat="server" /></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="info-row">
                        <div class="info-label"><i class="fas fa-calendar-alt me-2 text-success"></i>Assigned Date</div>
                        <div class="info-value"><asp:Literal ID="litAssignedDate" runat="server" /></div>
                    </div>
                </div>
            </div>
            
            <!-- Created Date -->
            <div class="info-row">
                <div class="info-label"><i class="fas fa-calendar-plus me-2 text-success"></i>Created Date</div>
                <div class="info-value"><asp:Literal ID="litCreatedDate" runat="server" /></div>
            </div>
            
            <!-- Attachment -->
            <div class="info-row">
                <div class="info-label"><i class="fas fa-paperclip me-2 text-success"></i>Attachment</div>
                <div class="info-value">
                    <asp:HyperLink ID="lnkAttachment" runat="server" Target="_blank" CssClass="btn-download">
                        <i class="fas fa-download me-2"></i>Download Attachment
                    </asp:HyperLink>
                    <asp:Label ID="lblNoAttachment" runat="server" Text="No attachment" CssClass="text-muted" Visible="false" />
                </div>
            </div>
            
            <!-- Action Buttons -->
            <div class="mt-4 text-center">
                <a href="Dashboard.aspx" class="btn-back">
                    <i class="fas fa-arrow-left me-2"></i>Back to Dashboard
                </a>
            </div>
            
        </div>
        
    </div>
</asp:Content>