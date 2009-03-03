<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comment.ascx.cs" Inherits="OxiteSite.Views.Shared.Comment" 
%><%  
    IComment comment = ViewData.Model as IComment;
    if (comment != null) { 
    %><% Html.RenderPartial("ManageComment", comment, ViewData); %>
    <div class="name"<%= comment.Published != null ? string.Format(" id=\"{0}\"", comment.Published.Value.GetPermalinkHashValue()) : string.Empty %>>
        <div><%= string.IsNullOrEmpty(comment.CreatorUrl) 
                 ? Html.Gravatar(comment.CreatorHashedEmail.CleanAttribute(), comment.CreatorName.CleanAttribute(), "48", Config.Site.GravatarDefault as string) 
                 : Html.Link(Html.Gravatar(comment.CreatorHashedEmail.CleanAttribute(), comment.CreatorName.CleanAttribute(), "48", Config.Site.GravatarDefault), comment.CreatorUrl.CleanHref(), new { @class = "avatar" })
             %></div>
        <p class="comment"><strong><%= string.IsNullOrEmpty(comment.CreatorUrl)
                        ? comment.CreatorName.CleanText()
                        : Html.Link(comment.CreatorName.CleanText(), comment.CreatorUrl.CleanHref()) 
                        %></strong><%
                        if (comment.Published != null) { 
                            %> <span><%= Localize("said") %><br /><%= Html.Link(ConvertToLocalTime(comment.Published.Value).ToString("MMMM dd, yyyy"), string.Format("#{0}", ConvertToLocalTime(comment.Published.Value).GetPermalinkHashValue())) %></span><%
                        }
        %></p>
    </div>
    <div class="text">
        <p><%= comment.Body %></p>
    </div><%
    } %>