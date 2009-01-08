<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotFound.aspx.cs" Inherits="OxiteSite.Views.Shared.NotFound" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p><%=Localize("The url you requested could not be found.") %></p><%
    if (!string.IsNullOrEmpty((string)ViewData["Description"]))
    { %>
    <p><%=ViewData["Description"] %></p><%
    } %>
</asp:Content>
