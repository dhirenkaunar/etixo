<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="OxiteSite.Views.Admin.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%=Localize("Settings") %></h2>
    <ul>
        <li><a href="<%=Url.RouteUrl("AdminBlogML", new { areaName = ((IArea)ViewData["Area"]).Name }) %>">BlogML</a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>