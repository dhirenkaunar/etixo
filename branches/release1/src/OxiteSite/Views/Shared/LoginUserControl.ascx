<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginUserControl.ascx.cs" Inherits="OxiteSite.Views.Shared.LoginUserControl" %>
<%  if (Request.IsAuthenticated) { %>
        <%=string.Format(Localize("Welcome, {0}!"), string.Format("<span class=\"username\">{0}</span>", Html.Encode(User.DisplayName))) 
%> <span class="logout">&laquo; <%= Html.RouteLink(Localize("Logout"), "SignOut", new { })%> &raquo;</span><%
    }
    else { 
%><span class="login">&laquo; <%= Html.RouteLink(Localize("Login"), "SignIn", new { ReturnUrl = Request.Url.AbsolutePath })%> &raquo;</span><%
    } %>