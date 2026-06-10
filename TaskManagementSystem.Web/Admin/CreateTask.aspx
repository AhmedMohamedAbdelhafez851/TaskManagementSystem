<%@ Page Title="Create Task" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="CreateTask.aspx.cs" Inherits="TaskManagementSystem.Web.Admin.CreateTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Custom CSS for Create Task page -->
    <link href="../Content/custom/create-task.css" rel="stylesheet" />

    <div class="container mt-4">
        
        <!-- Page Header -->
        <div class="page-header fade-in">
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <h1 class="mb-2"><i class="fas fa-plus-circle me-3"></i>Create New Task</h1>
                    <p class="mb-0 opacity-75">Create and assign tasks to team members</p>
                </div>
                <div>
                    <i class="fas fa-tasks fa-3x opacity-50"></i>
                </div>
            </div>
        </div>
        
        <!-- Form Card -->
        <div class="form-card fade-in">
            
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
            
            <!-- Title Field -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-heading me-2 text-primary"></i>Task Title
                    <span class="required-star">*</span>
                </label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control-custom" placeholder="Enter task title" />
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" 
                    ControlToValidate="txtTitle" ErrorMessage="Title is required" 
                    CssClass="validation-error" Display="Dynamic" />
            </div>
            
            <!-- Description Field -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-align-left me-2 text-primary"></i>Description
                </label>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                    CssClass="form-control-custom" placeholder="Enter task description (optional)" />
            </div>
            
            <!-- Assign To Field -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-user-check me-2 text-primary"></i>Assign To
                    <span class="required-star">*</span>
                </label>
                <asp:DropDownList ID="ddlAssignedTo" runat="server" CssClass="form-select-custom">
                    <asp:ListItem Text="-- Select Member --" Value="0" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvAssignedTo" runat="server" 
                    ControlToValidate="ddlAssignedTo" ErrorMessage="Please select a member" 
                    CssClass="validation-error" Display="Dynamic" InitialValue="0" />
            </div>
            
            <!-- Attachment Field -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-paperclip me-2 text-primary"></i>Attachment (Optional)
                </label>
                <div class="upload-area" id="uploadArea">
                    <div class="upload-icon">
                        <i class="fas fa-cloud-upload-alt"></i>
                    </div>
                    <div class="upload-text">
                        <strong>Click to upload</strong> or drag and drop<br />
                        <small>PDF, DOC, DOCX, JPG, PNG (Max 5MB)</small>
                    </div>
                </div>
                <asp:FileUpload ID="fuAttachment" runat="server" CssClass="d-none" />
                <div id="fileInfo" class="file-info" style="display: none;">
                    <i class="fas fa-file-alt"></i>
                    <span id="fileName"></span>
                    <i class="fas fa-times-circle" style="cursor: pointer; color: #dc3545;" onclick="clearFile()"></i>
                </div>
            </div>
            
            <!-- Action Buttons -->
            <div class="d-flex gap-3 mt-4">
                <asp:Button ID="btnCreate" runat="server" Text="Create Task" 
                    CssClass="btn-custom btn-primary-custom" OnClick="btnCreate_Click" CausesValidation="true" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                    CssClass="btn-custom btn-secondary-custom" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
        </div>
        
    </div>
    
    <!-- Custom JavaScript for Create Task page -->
    <script src="../Scripts/custom/create-task.js"></script>
    
</asp:Content>