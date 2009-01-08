<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OxiteSite.Views.SiteMap.Index" %>
<%@ OutputCache Duration="21600" VaryByParam="none" %>
<?xml version="1.0" encoding="UTF-8"?>
<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><%
    UriBuilder builder = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port);
    foreach (DateTime dt in (IEnumerable<DateTime>)ViewData["YearMonths"])
    {
        builder.Path = Url.RouteUrl("SiteMap", new { year = dt.Year, month = dt.Month }); %>
   <sitemap>
      <loc><%=builder.Uri.ToString() %></loc><%--
      Add last modified date for all but the current month's sitemap --%>
   </sitemap><%
    } %>
</sitemapindex>
