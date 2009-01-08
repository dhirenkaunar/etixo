<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="OxiteSite.Views.Page.Manage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%  
    IPost post = (IPost)ViewData["Post"];
    IUser creator = (IUser)ViewData["Creator"];
    string returnUri = ViewData["ReturnUri"] as string; %>
<div class="post editPage" id="page">
    <form method="post" action="">
	    <h2 class="title">
	        <%= Html.ValidationMessage("Post.Title", Localize("Title?!")) %>
	        <label for="post_bodyShort"><%= Localize("Title")%></label>
	        <%= Html.TextBox("title", !string.IsNullOrEmpty(Request.Form["title"]) ? Request["title"] : post.Title ?? string.Empty, new { id = "post_title", @class = "text", tabindex = "1", size = "60", title = Localize("Enter a title...", "post_title", true) }) %>
	    </h2>
        <% Html.RenderPartial("EditPrimaryMetadata"); %>
	    <div class="content"><%
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
    <%=RegisterScript("jquery-ui-20081126-1.5.2.js", "jquery-ui-20081126-1.5.2.min.js") %>
</asp:Content>
