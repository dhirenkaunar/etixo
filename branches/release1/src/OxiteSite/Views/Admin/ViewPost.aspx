<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ViewPost.aspx.cs" Inherits="OxiteSite.Views.Admin.ViewPost" %>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<%  IArea area = (IArea)ViewData["Area"];
    IPost post = (IPost)ViewData["Post"];
    IEnumerable<IComment> comments = (IEnumerable<IComment>)ViewData["Comments"];
    IUser creator = (IUser)ViewData["Creator"]; %>
<div class="post">
<% Html.RenderPartial("ManagePost", post, ViewData); %>
    <div class="avatar"><%= Html.Gravatar(creator, "48", Config.Site.GravatarDefault) %></div>
	<h2 class="title"><%= post.Title %></h2>
	<div class="metadata">
		<p class="posted"><%
		    if (post.Published.HasValue)
            {
                if (post.Published.Value > DateTime.Now.ToUniversalTime())
                {
                    %>Will Publish<%
                }
                else
                {
                    %>Published: <%
                    Response.Write(ConvertToLocalTime(post.Published.Value).ToLongDateString());
                }
            }
		    if (post.State != (byte)EntityState.Normal)
            {
                if (post.State == (byte)EntityState.PendingApproval)
                {
                    %>Pending Approval<%
                }
                else if (post.State == (byte)EntityState.Removed)
                {
                    %>Removed<%
                }
            } %>
        </p><%
    IEnumerable<ITag> tags = post.Tags;

    if (tags.Count() > 0)
    {
        Response.Write(
            Html.UnorderedList(
                tags,
                t => string.Format(
                    "<a href=\"{1}\" rel=\"tag\">{0}</a>",
                    Html.Encode(t.Name),
                    t.GetAdminUrl(ViewContext)),
                "tags")
        );
    } 
%>	</div>
	<div class="content"><%=post.Body %></div>
	<% Html.RenderPartial("Comments", comments, ViewData); %>
</div>
</asp:Content>
