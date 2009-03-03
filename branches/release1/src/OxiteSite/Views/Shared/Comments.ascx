<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comments.ascx.cs" Inherits="OxiteSite.Views.Shared.Comments" %>
<% 
    IEnumerable<IComment> comments = (IEnumerable<IComment>)ViewData.Model;
    EntityState? commentState = ViewData["CommentState"] as EntityState?;
%><div class="comments">
	<div class="status">
	    <div><a name="comments"></a></div>
	    <% if (comments != null)
        {%>
		<h3><%= string.Format(comments.Count() == 1 ? Localize("{0} Comment") : Localize("{0} Comments"), comments.Count())%></h3>
		<% } %>
		<div><a href="#comment">leave your own</a></div>
	</div>
	<% if (comments != null && comments.Count() > 0) { %><ul class="commented">
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

               sbClass.Append("comment ");
               
               sbClass.Append(comment.CreatorUser.IsAnonymous ? "anon " : string.Format("{0} {1} ", comment.CreatorUser.ID == ((IPost)ViewData["Post"]).CreatorUser.ID ? "author" : "user", comment.CreatorUser.Username.CleanAttribute()));

               %><li<%= sbClass.Length > 0 ? string.Format(" class=\"{0}\"", sbClass.Remove(sbClass.Length - 1, 1)) : "" %>>
            <% Html.RenderPartial("Comment", comment, ViewData); %>
        </li><%
               counter++;
           } %>
    </ul><%
      } %>
    <%
    if (commentState == EntityState.PendingApproval)
    { %>
    <div id="message pending"><%=Localize("Your comment is pending approval. Please hold.") %></div><%
    }
    if (User.IsAnonymous)
    {
        if (ViewData["CommentAnonymous"] != null)
        {
            Html.RenderPartial("CommentFormAnonymous", ViewData["CommentAnonymous"], ViewData);
        }
        else
        {
            Html.RenderPartial("CommentFormAnonymous");
        }
    }
    else
    {
        Html.RenderPartial("CommentFormAuthenticated", User, ViewData);
    } %>
</div>