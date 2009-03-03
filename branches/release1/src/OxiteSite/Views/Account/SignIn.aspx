<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="OxiteSite.Views.Account.Login" %>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <form method="post" action="" class="login">
        <h2 class="title"><%=Localize("Login") %></h2>
        <div><%=Localize("Please enter your username and password below.")%></div>
        <%= Html.ValidationSummary() %>
        <div class="username">
            <label for="login_username"><%= Localize("Username") %></label>
            <%= Html.TextBox("username", Request["username"], new { id = "login_username", @class = "text", tabindex = "1", title = Localize("Your username...") }) %>
            <%= Html.ValidationMessage("username") %>
        </div>
        <div class="password">
            <label for="login_password"><%= Localize("Password") %></label>
            <%= Html.Password("password", Request["password"], new { id = "login_password", @class = "text", tabindex = "2", title = Localize("Your password...") }) %>
            <%= Html.ValidationMessage("password")%>
        </div>
        <div class="remember">
            <%= Html.CheckBox("rememberMe", Request.Form.IsTrue("rememberMe"), new { id = "login_remember", tabindex = "3" })%>
            <label for="login_remember"><%= Localize("Remember me?") %></label>
        </div>
        <div class="submit">
            <input type="submit" value="<%= Localize("Login") %>" id="login_submit" class="submit button" tabindex="4" />
        </div>
    </form>
</asp:Content>