<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexAtom.aspx.cs" Inherits="OxiteSite.Views.Area.IndexAtom" %><%@ Import Namespace="System.Xml" %><%
    IEnumerable<IFeedItem> items = (IEnumerable<IFeedItem>)ViewData["FeedItems"];
    IArea area = (IArea)ViewData["Area"];
%><?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <title type="html"><%=Html.Encode(ViewData["Title"]) %></title>
  <updated><%=XmlConvert.ToString(items.First().Published.Value, XmlDateTimeSerializationMode.RoundtripKind) %></updated><%
    if (!string.IsNullOrEmpty((string)ViewData["Description"]))
    { %>
  <subtitle type="html"><%=Html.Encode(ViewData["Description"])%></subtitle><%
    } %>
  <id><%=Context.Request.Url.ToString().ToLower() %></id>
  <link rel="alternate" type="text/html" hreflang="<%=Config.Site.LanguageDefault %>" href="<%=GetAbsolutePath(Url.Area(area)).ToLower() %>"/>
  <link rel="self" type="application/atom+xml" href="<%=Context.Request.Url.ToString() %>"/>
  <generator uri="http://oxite.net/" version="1.0">Oxite</generator><%
    foreach (IFeedItem item in items)
    {
        string itemUrl = GetAbsolutePath(Url.Post(((IPost)item))); %>
  <entry>
    <title type="html"><%=Html.Encode(item.Title)%></title>
    <link rel="alternate" type="text/html" href="<%=itemUrl %>"/>
    <id><%=itemUrl %></id>
    <updated><%=XmlConvert.ToString(item.Published.Value, XmlDateTimeSerializationMode.RoundtripKind)%></updated>
    <published><%=XmlConvert.ToString(item.Published.Value, XmlDateTimeSerializationMode.RoundtripKind)%></published>
    <author>
      <name><%=Html.Encode(item.CreatorName)%></name>
    </author><%
        foreach (ITag tag in item.Tags)
        { %>
    <category term="<%=tag.Name %>" /><%
        } %>
    <content type="html" xml:lang="<%=Config.Site.LanguageDefault %>">
      <%=Html.Encode(item.Body)%>
    </content>
  </entry><%
    } %>
</feed>
