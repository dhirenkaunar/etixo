<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminTasksContainer.ascx.cs" Inherits="OxiteSite.Views.Shared.AdminTasksContainer" %>
<%  if (ViewData["UserCanSeeAdmin"] != null && (bool)ViewData["UserCanSeeAdmin"])
    { %>
                    <div class="sub admin manage">
                        <h3><%=Localize("Admin") %></h3>
<% Html.RenderPartial("AdminTasks"); %>
                    </div><%
    } %>