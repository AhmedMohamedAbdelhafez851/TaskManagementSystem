<%@ Page Title="Access Denied" Language="C#" MasterPageFile="~/MasterPages/Empty.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="TaskManagementSystem.Web.Shared.AccessDenied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 text-center">
                <div class="card shadow">
                    <div class="card-body p-5">
                        <i class="fas fa-lock fa-5x text-warning mb-4"></i>
                        <h1 class="text-warning">Access Denied</h1>
                        <p class="lead">You do not have permission to access this page.</p>
                        <hr />
                        <p class="text-muted">Please contact your administrator if you believe this is an error.</p>
                        <a href="~/Account/Login.aspx" class="btn btn-primary mt-3">Return to Login</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>