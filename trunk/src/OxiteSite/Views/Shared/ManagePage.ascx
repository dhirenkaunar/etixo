<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePage.ascx.cs" Inherits="OxiteSite.Views.Shared.ManagePage" %><% 
    IPost post = (IPost)ViewData.Model;
    IQueryable<IPost> children = (IQueryable<IPost>)ViewData["Children"];
    bool urlIslocked = post.State == (byte)EntityState.Normal && post.Published.HasValue && post.Published.Value.AddHours(Config.Site.PostEditTimeout) < DateTime.Now.ToUniversalTime();

    if ((bool)ViewData["UserCanSeeAdmin"])
    { %>
<div class="admin manage buttons"><%
        if (post.State != (byte)EntityState.Removed)
        { %>
    <a href="<%= string.Format("{0}/Add", Url.Page(post)) %>" title="<%= Localize("Add") %>" class="button add"><img src="<%=ViewData["SkinPath"] %>/images/page_add.png" alt="<%= Localize("Add") %>" width="16" height="16" /></a>
    <a href="<%= string.Format("{0}/Edit", Url.Page(post)) %>" title="<%= Localize("Edit") %>" class="button edit"><img src="<%=ViewData["SkinPath"] %>/images/page_edit.png" alt="<%= Localize("Edit") %>" width="16" height="16" /></a><%
            if (!urlIslocked && children.Count() < 1)
            { %>
    <form class="remove post" method="post" action="<%= string.Format("{0}/Remove", Url.Page(post)) %>">
        <fieldset>
            <input type="image" src="<%=ViewData["SkinPath"] %>/images/page_delete.png" alt="<%= Localize("Remove") %>" title="<%= Localize("Remove") %>" class="button remove" />
            <%= Html.Hidden("returnUri", Request.Url.AbsoluteUri)%>
            <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
            <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>
        </fieldset>
    </form><%
            } 
        }
        else
        { %>
    <span class="message"><%=Localize("This page has been removed") %></span><%
        } %>
</div><%
    } %>