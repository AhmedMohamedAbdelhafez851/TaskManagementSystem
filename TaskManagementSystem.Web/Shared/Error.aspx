<%@ Page Title="Error" Language="C#" MasterPageFile="~/MasterPages/Empty.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TaskManagementSystem.Web.Shared.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 text-center">
                <div class="card shadow">
                    <div class="card-body">
                        <i class="fas fa-exclamation-triangle fa-5x text-danger mb-3"></i>
                        <h2 class="text-danger">Oops! Something went wrong</h2>
                        <p class="lead">An unexpected error has occurred.</p>
                        <hr />
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="text-muted" />
                        <div class="mt-4">
                            <a href="~/Account/Login.aspx" class="btn btn-primary">Return to Login</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>