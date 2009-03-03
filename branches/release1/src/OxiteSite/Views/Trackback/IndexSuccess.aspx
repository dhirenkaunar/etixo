<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexSuccess.aspx.cs" Inherits="OxiteSite.Views.Trackback.IndexSuccess" %>
<response>
    <error>0</error>
    <rss version="0.91">
        <channel>
            <title><%=((IPost)ViewData["Post"]).Title %></title>
            <link><%=ViewData["Url"] %></link>
            <description></description>
            <language>en</language>
        </channel>
    </rss>
</response>