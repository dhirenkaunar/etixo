<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comments.ascx.cs" Inherits="OxiteSite.Views.Shared.Comments" %>
<% IPageOfAList<IComment> comments = (IPageOfAList<IComment>)ViewData.Model;
%><div class="comments"><% 
    if (ViewData.ContainsKey("Post")) { %>
	<div class="status">
	    <div><a name="comments"></a></div>
		<h3><%= comments != null ? comments.Count().ToString() : "0"%> Comment<%= comments != null && comments.Count() == 1 ? "" : "s"%></h3>
		<div><a href="<%=Url.Post((IPost)ViewData["Post"]) %>#comment">leave your own</a></div>
	</div><% 
    } %>
	<% if (comments.Count() > 0) { %><ul class="commented">
        <% int counter = 0;
           foreach (IComment comment in comments)
           {
               StringBuilder sbClass = new StringBuilder(40);

               if (counter == 0)
                   sbClass.Append("first ");
               if (comment.Equals(comments.Last()))
                   sbClass.Append("last ");

               if (counter % 2 != 0)
                   sbClass.Append("odd ");
               
               sbClass.Append(comment.CreatorUser.IsAnonymous ? "anon " : comment.CreatorUser.ID == comment.Post.CreatorUser.ID ? "author " : "user ");

               %><li<%= sbClass.Length > 0 ? string.Format(" class=\"{0}\"", sbClass.Remove(sbClass.Length - 1, 1)) : "" %>>
            <% Html.RenderPartial("Comment", comment, ViewData); %>
        </li><%
               counter++;
           }%>
    </ul>
    <div class="pager"><%= Html.SimplePager<IComment>(comments, this, "PageOfAnAdminComments", new { }) %></div>
<%      } else { %>
    <h3><%= Localize("No comments here.") %></h3>
<%      } %>
</div>