<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Permalink.aspx.cs" Inherits="OxiteSite.Views.SiteMap.Permalink" %><%@ OutputCache Duration="21600" VaryByParam="none"
%><urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><%
    foreach (IPost post in (IEnumerable<IPost>)ViewData["Posts"])
    { %>
   <url>
        <loc><%=GetAbsolutePath(Url.Post(post)) %></loc><%--
        Add last modified date for all but the current month's sitemap --%>
    </url><%
    } %>
</urlset>