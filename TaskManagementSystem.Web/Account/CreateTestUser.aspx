<%@ Page Title="Create Test User" Language="C#" MasterPageFile="~/MasterPages/Empty.Master" AutoEventWireup="true" CodeBehind="CreateTestUser.aspx.cs" Inherits="TaskManagementSystem.Web.Account.CreateTestUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row justify-content-center mt-5">
            <div class="col-md-6">
                <div class="card shadow-lg">
                    <div class="card-header bg-success text-white text-center">
                        <h3 class="mb-0">Create Test Users</h3>
                        <small>Password Reset Tool</small>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-success">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </asp:Panel>
                        
                        <div class="alert alert-info">
                            <strong>Note:</strong> This tool will reset all user passwords.
                        </div>
                        
                        <div class="d-grid gap-2">
                            <asp:Button ID="btnCreateAdmin" runat="server" Text="Create / Reset Admin User (admin / Admin@123)" 
                                CssClass="btn btn-primary btn-lg" OnClick="btnCreateAdmin_Click" />
                            
                            <asp:Button ID="btnCreateMembers" runat="server" Text="Create / Reset Member Users (Password: Member@123)" 
                                CssClass="btn btn-secondary btn-lg" OnClick="btnCreateMembers_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>