<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Permalink.aspx.cs" Inherits="OxiteSite.Views.Area.Permalink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%  IPost post = (IPost)ViewData["Post"];
    IEnumerable<IComment> comments = (IEnumerable<IComment>)ViewData["Comments"];
    IUser creator = (IUser)ViewData["Creator"]; %>
<div class="post">
<% Html.RenderPartial("ManagePost", post, ViewData); %>
    <div class="avatar"><%= Html.Gravatar(creator, "48", Config.Site.GravatarDefault) %></div>
    <h2 class="title"><%= post.Title %></h2>
    <div class="metadata">
        <div class="posted"><%
    if (post.Published.HasValue)
    {
        Response.Write(ConvertToLocalTime(post.Published.Value).ToLongDateString());
    }
    else
    {
        Response.Write(Localize("Draft"));
    } %>
        </div><%
        IEnumerable<ITag> tags = post.Tags;
        if (tags.Count() > 0)
        {
            Response.Write(
                string.Format(
                    " {0} {1}",
                    Localize("Filed under"),
                    Html.UnorderedList(
                        tags,
                        (t, i) =>
                            string.Format(
                                "<a href=\"{1}\" rel=\"tag\">{0}</a>",
                                Html.Encode(t.Name),
                                Url.Tag(t)
                            ),
                        "tags"
                    )
                )
            );
        } %>
    </div>
    <div class="content"><%=post.Body %></div>
    <% Html.RenderPartial("Comments", comments, ViewData); %>
</div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ScriptVariablesPre">
    <script type="text/javascript">
        <%=RegisterScriptVariable("computeHashPath", Url.RouteUrl("ComputeHash")) %>
    </script>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>
