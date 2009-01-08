<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OxiteSite.Views.Area.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%  IPageOfAList<IPost> posts = (IPageOfAList<IPost>)ViewData["Posts"];
    Dictionary<Guid, int> postCounts = (Dictionary<Guid, int>)ViewData["PostCounts"];
    IArea area = ((IArea)ViewData["Area"]); %>
            <div class="sections">
                <div class="primary">
                    <h2 class="title"><%= !string.IsNullOrEmpty(area.DisplayName) ? area.DisplayName : area.Name %></h2>
                    <ul class="posts"><%
    foreach (IPost post in posts)
    {
        string className = string.Empty;
        if (post.Equals(posts.First())) className += "first ";
        if (post.Equals(posts.Last())) className += "last "; %>
                        <li<%= !string.IsNullOrEmpty(className) ? string.Format(" class=\"{0}\"", className.Trim()) : string.Empty %>>
                            <h2 class="title"><a href="<%=Url.Post(post) %>"><%= post.Title %></a></h2>
                            <div class="posted"><%=ConvertToLocalTime(post.Published.Value).ToLongDateString() %></div>
                            <div class="content"><%=post.GetBodyShort() %></div>
                            <div class="more"><%        
        int commentCount = postCounts[post.ID];
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
        }
        %> | <a href="<%=Url.Post(post) %>#comments"><%=commentCount %> comment<%=commentCount == 1 ? "" : "s"%></a> <a href="<%=Url.Post(post) %>" class="arrow">&raquo;</a></div>
                        </li><%
    } %>
                    </ul><%
    if (posts.TotalPageCount > 1)
    { %>
                    <div class="pager">
                        <%=Html.SimplePager(posts, this, string.Format("PageOfA{0}", ((IArea)ViewData["Area"]).Type), new { areaName = ((IArea)ViewData["Area"]).Name }) %>
                    </div><%
    } %>
                </div>
                <div class="secondary"><%
    Html.RenderPartial("Search");
    Html.RenderPartial("AdminTasksContainer");
    Html.RenderPartial("Archives", (IEnumerable<KeyValuePair<DateTime, int>>)ViewData["Months"], ViewData); %>
                </div>
            </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="StringResourcesContent">
    <% RenderStringResources(); %>
</asp:Content>
