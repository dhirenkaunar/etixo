﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ViewPage.aspx.cs" Inherits="OxiteSite.Views.Page.Index" %>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<% IPost post = (IPost)ViewData["Post"]; %>
<div class="post page">
<% Html.RenderPartial("ManagePage", post, ViewData); %>
    <h2 class="title"><%= post.Title %></h2>
    <%= post.Body %>
</div>
</asp:Content>