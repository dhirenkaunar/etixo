<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePost.ascx.cs" Inherits="OxiteSite.Views.Shared.ManagePost" 
%><% 
    IPost post = (IPost)ViewData.Model;
    bool urlIslocked = post.State == (byte)EntityState.Normal && post.Published.HasValue && post.Published.Value.AddHours(Config.Site.PostEditTimeout) < DateTime.Now.ToUniversalTime();

    if ((bool)ViewData["UserCanSeeAdmin"])
    {
%><div class="admin manage buttons">
<%      if (post.State != (byte)EntityState.Removed)
        {
%>    <a href="<%= Url.RouteUrl("AdminBlogPermalinkEdit", new { areaName = post.Area.Name, slugToEdit = post.Slug }) %>" title="<%= Localize("Edit") %>" class="button edit"><img src="<%=ViewData["SkinPath"] %>/images/page_edit.png" alt="<%= Localize("Edit") %>" width="16" height="16" /></a>
<%          if (!urlIslocked)
            { 
%>  <form class="remove post" method="post" action="<%= Url.RouteUrl("AdminBlogPermalinkRemove", new { areaName = post.Area.Name, slug = post.Slug }) %>">
        <fieldset>
            <input type="image" src="<%=ViewData["SkinPath"] %>/images/page_delete.png" alt="<%= Localize("Remove") %>" title="<%= Localize("Remove") %>" class="button remove" />
            <%= Html.Hidden("returnUri", Request.Url.AbsoluteUri)%>
            <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
            <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>
        </fieldset>
    </form>
<%          } 
        }
        else
        {
%>  <span class="message"><%=Localize("This post has been removed") %></span>
<%        }
%></div>
<%  } %>