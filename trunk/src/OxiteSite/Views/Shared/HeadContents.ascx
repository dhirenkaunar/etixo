<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeadContents.ascx.cs" Inherits="OxiteSite.Views.Shared.HeadContents" %>
    <meta name="language" content="<%=Config.Site.LanguageDefault %>" /><%
    if (!string.IsNullOrEmpty(Config.Site.SEORobots))
    { %>
    <meta name="robots" content="<%=Config.Site.SEORobots %>" /><%
    }
    if (!string.IsNullOrEmpty((string)ViewData["Meta.Description"]))
    { %>
    <meta name="description" content="<%=((string)ViewData["Meta.Description"]).CleanText()%>" /><%
    }
    if (ViewData["Pingback.Url"] != null)
    {
        Response.Write("\r\n    ");
        Response.Write(Html.HeadLink("pingback", (string)ViewData["Pingback.Url"], "", ""));
    }
    if (ViewData["RsdLink"] != null)
    {
        Response.Write("\r\n    ");
        Response.Write(Html.HeadLink("EditURI", (string)ViewData["RsdLink"], "application/rsd+xml", "RSD"));
        Response.Write("\r\n    ");
        Response.Write(Html.HeadLink("wlwmanifest", "/LiveWriterManifest.xml", "application/wlwmanifest+xml", ""));
    }
    if (ViewData["FeedDiscovery"] != null)
    {
        foreach (HeadLink headLink in ((IEnumerable<HeadLink>)ViewData["FeedDiscovery"]).Reverse())
        {
            Response.Write("\r\n    ");
            Response.Write(Html.HeadLink(headLink));
        }
    }
    if (ViewData["OpenSearch.Title"] != null && ViewData["OpenSearch.Url"] != null)
    {
        Response.Write("\r\n    ");
        Response.Write(Html.HeadLink("search", (string)ViewData["OpenSearch.Url"], "application/opensearchdescription+xml", (string)ViewData["OpenSearch.Title"]));
    } %>