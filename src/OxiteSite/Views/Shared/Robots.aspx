<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Robots.aspx.cs" Inherits="OxiteSite.Views.Shared.Robots" %>
<%  if (ViewData["SiteMap"] != null)
    { %>
SiteMap: <%
        Response.Write(GetAbsolutePath((string)ViewData["SiteMap"]));
    } %>