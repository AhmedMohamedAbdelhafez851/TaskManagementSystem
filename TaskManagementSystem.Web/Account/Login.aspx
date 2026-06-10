<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TaskManagementSystem.Web.Account.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
    <meta name="theme-color" content="#667eea" />
    <title>Login | TaskFlow</title>
    
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <!-- Font Awesome 6 -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet" />
    
    <!-- Custom Login CSS -->
    <link href="../Content/custom/login.css" rel="stylesheet" />    
</head>
<body>
    
    <!-- Animated Background Circles -->
    <div class="bg-circle circle-1"></div>
    <div class="bg-circle circle-2"></div>
    <div class="bg-circle circle-3"></div>
    
    <form id="form1" runat="server">
        <div class="login-card">
            
            <!-- Header -->
            <div class="login-header">
                <div class="logo-icon">
                    <i class="fas fa-tasks"></i>
                </div>
                <h1>Welcome Back</h1>
                <p>Sign in to your account to continue</p>
            </div>
            
            <!-- Body -->
            <div class="login-body">
                
                <!-- Error Message -->
                <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert-custom alert-danger-custom">
                    <i class="fas fa-exclamation-triangle"></i>
                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                </asp:Panel>
                
                <!-- Username Field -->
                <div class="input-group-custom">
                    <i class="fas fa-envelope input-icon"></i>
                    <asp:TextBox ID="txtUsername" runat="server" placeholder="Username" />
                </div>
                
                <!-- Password Field -->
                <div class="input-group-custom">
                    <i class="fas fa-lock input-icon"></i>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password" />
                    <i class="fas fa-eye password-toggle" id="togglePassword"></i>
                </div>
                
                <!-- Remember Me & Forgot Password -->
                <div class="form-options">
                    <label class="checkbox-label">
                        <input type="checkbox" id="rememberMe" /> Remember me
                    </label>
<%--                    <a href="#" class="forgot-link">Forgot Password?</a>--%>
                </div>
                
                <!-- Login Button -->
                <asp:Button ID="btnLogin" runat="server" Text="Sign In" OnClick="btnLogin_Click" CssClass="login-btn" UseSubmitBehavior="true" />
                
            </div>
            
            <!-- Footer -->
            <div class="login-footer">
                <div class="demo-credentials">
                    <i class="fas fa-info-circle me-1"></i> Demo Credentials:<br />
                    <strong>Admin:</strong> admin / Admin@123 &nbsp;|&nbsp;
                    <strong>Member:</strong> ahmed.ali / Member@123
                </div>
            </div>
            
        </div>
    </form>
    
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <!-- Custom Login JavaScript -->
    <script src="../Scripts/custom/login.js"></script>    
</body>
</html>