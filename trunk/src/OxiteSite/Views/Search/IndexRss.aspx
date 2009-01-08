<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexRss.aspx.cs" Inherits="OxiteSite.Views.Search.IndexRss" %><%
    IEnumerable<IFeedItem> items = (IEnumerable<IFeedItem>)ViewData["FeedItems"];
%><rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <channel>
        <title><%=Html.Encode(ViewData["Title"]) %></title>
        <description><%=ViewData["Description"] %></description>
        <link><%=GetAbsolutePath(Url.RouteUrl("Search", new { term = ViewData["Term"] })) %></link>
        <language><%=Config.Site.LanguageDefault %></language><%
    foreach (IFeedItem item in items)
    {
        string itemUrl = GetAbsolutePath(Url.Post((IPost)item)); %>
        <item>
            <dc:creator><%=Html.Encode(item.CreatorName)%></dc:creator>
            <title><%=Html.Encode(item.Title)%></title>
            <description><%=Html.Encode(item.Body)%></description>
            <link><%=itemUrl%></link>
            <guid isPermaLink="true"><%=itemUrl %></guid>
            <pubDate><%=item.Published.Value.ToStringForFeed()%></pubDate>
            <category><%=item.Area.Name%></category><%
        foreach (ITag tag in item.Tags)
        { %>
            <category><%=tag.Name %></category><%
        } %>
        </item><%
    } %>
    </channel>
</rss>
