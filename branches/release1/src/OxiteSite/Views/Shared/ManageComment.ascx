<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageComment.ascx.cs" Inherits="OxiteSite.Views.Shared.ManageComment" 
%><%
    IComment comment = ViewData.Model as IComment;
    //todo(nheskew): still kinda lame to be passing around a bool so...make that not necessary sometime
    if (comment != null && (bool)ViewData["UserCanSeeAdmin"])
    { 
%><div class="flags">
    <form class="flag remove" method="post" action="<%= Url.RouteUrl("AdminRemoveComment") %>">
        <input type="image" class="button remove" src="<%=ViewData["SkinPath"] %>/images/delete.png" title="<%= Localize("Remove") %>" />
        <input type="hidden" name="commentId" value="<%= comment.ID %>" />
        <input type="hidden" name="returnUri" value="<%= Request.Url.AbsoluteUri %>" />
    </form>
    <%  if (comment.State == (byte)EntityState.PendingApproval) { %>
    <form class="flag approve" method="post" action="<%= Url.RouteUrl("AdminApproveComment") %>">
        <input type="image" class="button approve" src="<%=ViewData["SkinPath"] %>/images/accept.png" title="<%= Localize("Approve") %>" />
        <input type="hidden" name="commentId" value="<%= comment.ID %>" />
        <input type="hidden" name="returnUri" value="<%= Request.Url.AbsoluteUri %>" />
    </form>
    <%  } %>
</div><%
    } %>