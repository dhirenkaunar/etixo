<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPost.aspx.cs" Inherits="OxiteSite.Views.Admin.EditPost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%
    IArea area = (IArea)ViewData["Area"];
    IPost post = (IPost)ViewData["Post"];
    IEnumerable<IComment> comments = (IEnumerable<IComment>)ViewData["Comments"];
    IUser creator = (IUser)ViewData["Creator"];
    string returnUri = ViewData["ReturnUri"] as string; %>
<div class="post editPost" id="post">
    <form method="post" action="">
        <div class="avatar"><%= Html.Gravatar(creator, "48", Config.Site.GravatarDefault) %></div>
	    <h2 class="title">
	        <%= Html.ValidationMessage("Post.Title", Localize("Title?!")) %>
	        <label for="post_bodyShort"><%= Localize("Title") %></label>
	        <%= Html.TextBox("title", !string.IsNullOrEmpty(Request.Form["title"]) ? Request["title"] : post.Title ?? string.Empty, new { id = "post_title", @class = "text", tabindex = "1", size = "60", title = Localize("Enter a title...", "post_title", true) }) %>
	    </h2>
        <% Html.RenderPartial("EditPrimaryMetadata"); %>
	    <div class="content"><%
    if (ViewData.ModelState.ContainsKey("Post.BodyShort"))
    { %>
	        <h4><%= Html.ValidationMessage("Post.BodyShort", Localize("Li'l Body?!")) %></h4><%
    } %>
	        <label for="post_bodyShort"><%= Localize("Excerpt") %></label>
	        <%= Html.TextArea("bodyShort", (!string.IsNullOrEmpty(Request.Form["bodyShort"]) ? Request["bodyShort"] : post.BodyShort ?? string.Empty), 6, 120, new { id = "post_bodyShort", tabindex = "4", title = Localize("Enter an excerpt...", "post_bodyShort", true) }) %><%
	if (ViewData.ModelState.ContainsKey("Post.Body"))
    { %>
	        <h4><%= Html.ValidationMessage("Post.Body", Localize("Body?!"))%></h4><%
    } %>
	        <label for="post_body"><%= Localize("Body Content") %></label>
	        <%= Html.TextArea("body", (!string.IsNullOrEmpty(Request.Form["body"]) ? Request["body"] : post.Body ?? string.Empty), 24, 120, new { id = "post_body", tabindex = "3", title = Localize("Enter body content...", "post_body", true) }) %>
	    </div>
        <% Html.RenderPartial("EditSecondaryMetadata"); %>
        <%= Html.AntiForgeryToken(ViewData["AntiForgeryToken"] as string) %>
        <%= Html.AntiForgeryTicks(ViewData["AntiForgeryTicks"] as string)%>
        <%= Html.Hidden("returnUri", returnUri)%>
        <% RenderStringResources(); %>
	</form>
</div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadCssFiles">
    <%=RegisterCssFile("jquery.css") %>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ScriptVariablesPre">
    <script type="text/javascript">
        <%=RegisterSkinPathVariable() %>
    </script>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>
