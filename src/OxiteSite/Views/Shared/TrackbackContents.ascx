<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackbackContents.ascx.cs" Inherits="OxiteSite.Views.Shared.TrackbackContents" %><%
    if (ViewData["Trackback.Url"] != null)
    { %>
    <!--
    <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:trackback="http://madskills.com/public/xml/rss/module/trackback/">
        <rdf:Description rdf:about="<%=ViewData["Trackback.Post.Url"] %>" dc:identifier="<%=ViewData["Trackback.Post.Url"] %>" dc:title="<%=ViewData["Trackback.Post.Title"] %>" trackback:ping="<%=ViewData["Trackback.Url"] %>" />
    </rdf:RDF>
    --><%
    } %>