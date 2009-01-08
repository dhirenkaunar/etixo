<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermalinkRss.aspx.cs" Inherits="OxiteSite.Views.Area.PermalinkRss" %><%
    IArea area = (IArea)ViewData["Area"];
    IPost post = (IPost)ViewData["Post"];
    IEnumerable<IFeedItem> items = (IEnumerable<IFeedItem>)ViewData["FeedItems"];
%><rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <channel>
        <title><%=Html.Encode(ViewData["Title"]) %></title>
        <description><%=ViewData["Description"] %></description>
        <link><%=GetAbsolutePath(Url.Post(post))%></link>
        <language><%=Config.Site.LanguageDefault %></language><%
    foreach (IFeedItem item in items)
    {
        string itemUrl = GetAbsolutePath(Url.Comment(((IComment)item))).Replace("%23", "#"); %>
        <item>
            <dc:creator><%=Html.Encode(item.CreatorName) %></dc:creator>
            <title>Re: <%=Html.Encode(item.Title) %></title>
            <description><%=Html.Encode(item.Body) %></description>
            <link><%=itemUrl %></link>
            <guid isPermaLink="true"><%=itemUrl %></guid>
            <pubDate><%=item.Published.Value.ToStringForFeed()%></pubDate>
        </item><%
    } %>
    </channel>
</rss>
