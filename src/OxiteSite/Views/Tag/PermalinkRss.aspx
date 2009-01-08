<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermalinkRss.aspx.cs" Inherits="OxiteSite.Views.Tag.PermalinkRss" %><%
    ITag tag = (ITag)ViewData["Tag"];
    IEnumerable<IFeedItem> items = (IEnumerable<IFeedItem>)ViewData["FeedItems"];
%><rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <channel>
        <title><%=Html.Encode(ViewData["Title"]) %></title>
        <description><%=ViewData["Description"] %></description>
        <link><%=Url.RouteUrl("TagPermalink", new { id = tag.Name })%></link>
        <language><%=Config.Site.LanguageDefault %></language><%
    foreach (IFeedItem item in items)
    {
        string itemUrl = GetAbsolutePath(Url.Post((IPost)item)); %>
        <item>
            <dc:creator><%=Html.Encode(item.CreatorName)%></dc:creator>
            <title><%=Html.Encode(item.Title)%></title>
            <description><%=Html.Encode(item.Body)%></description>
            <link><%=itemUrl %></link>
            <guid isPermaLink="true"><%=itemUrl %></guid>
            <pubDate><%=item.Published.Value.ToStringForFeed()%></pubDate>
            <category><%=item.Area.Name%></category><%
        foreach (ITag postTag in item.Tags)
        { %>
            <category><%=postTag.Name %></category><%
        } %>
        </item><%
    } %>
    </channel>
</rss>
