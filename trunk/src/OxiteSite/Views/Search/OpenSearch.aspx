<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenSearch.aspx.cs" Inherits="OxiteSite.Views.Search.OpenSearch" %><%
    UriBuilder builder = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port);
    builder.Path = Url.RouteUrl("Search", new { term = "{searchTerms}" }); %>
<OpenSearchDescription xmlns="http://a9.com/-/spec/opensearch/1.1/">
    <ShortName><%=ViewData["OpenSearch.ShortName"] %></ShortName> 
    <Description><%=ViewData["OpenSearch.Description"] %></Description> 
    <Url type="text/html" template="<%=Server.UrlDecode(builder.Uri.ToString()) %>" /> 
</OpenSearchDescription>
