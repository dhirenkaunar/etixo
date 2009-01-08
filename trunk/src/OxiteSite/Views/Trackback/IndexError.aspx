<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexError.aspx.cs" Inherits="OxiteSite.Views.Trackback.IndexError" %>
<response>
    <error><%=ViewData["ErrorCode"] %></error>
<%  if (ViewData["ErrorText"] != null)
    { %>
    <message><%=ViewData["ErrorText"] %></message>
<%  } %>
</response>