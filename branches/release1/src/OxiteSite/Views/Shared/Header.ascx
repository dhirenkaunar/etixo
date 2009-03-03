<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="OxiteSite.Views.Shared.Header" 
%>            <div id="title">
                <h1><a href="<%= Url.RouteUrl("Home") %>"><%= Config.Site.Name %></a></h1>
            </div>
            <div id="logindisplay"><% Html.RenderPartial("LoginUserControl"); %></div>
            <div id="menucontainer">
<% 
                RouteValueDictionary routeValues = ViewContext.RouteData.Values;
                string selected = " class=\"selected\"";
                string controller = routeValues["controller"] as string;
                string pagePath = (routeValues["pagePath"] as string) ?? string.Empty;
%>                <ul id="menu">
                    <li<%= controller == "Home" ? selected : string.Empty %>><%= Html.RouteLink(Localize("Home"), "Home", new { }) %></li>
                    <li<%= controller == "Page" && string.Compare(pagePath, "about", true) == 0 ? selected : string.Empty %>><%= Html.RouteLink(Localize("About"), "Page", new { pagePath = "About" }) %></li><%
                    if (ViewData["UserCanSeeAdmin"] != null && (bool)ViewData["UserCanSeeAdmin"]) { %>
                    <li<%= controller == "Admin" ? selected : string.Empty %>><%= Html.RouteLink(Localize("Admin"), "AdminHome", new { }) %></li><%
                    } %>
                </ul>
            </div>