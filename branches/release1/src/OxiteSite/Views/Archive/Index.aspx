<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OxiteSite.Views.Home.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%
    IPageOfAList<IPost> posts = ((IPageOfAList<IPost>)ViewData["Posts"]);
    Dictionary<Guid, int> postCounts = (Dictionary<Guid, int>)ViewData["PostCounts"];
    IArea area = (IArea)ViewData["Area"]; %>
            <div class="sections">
                <div class="primary">
                    <h2 class="title"><%=
                        (
                            ((int)ViewData["Year"]).ToString() 
                            + ((int)ViewData["Month"] > 0 
                                ? " / " + new DateTime((int)ViewData["Year"], (int)ViewData["Month"], 1).ToString("MMMM")
                                : "") 
                            + ((int)ViewData["Day"] > 0 
                                ? " / " + ((int)ViewData["Day"]).ToString()
                                : "")
                        ) 
                        + (area != null ? " / " + (!string.IsNullOrEmpty(area.DisplayName) ? area.DisplayName : area.Name) : "")
                    %> / <%=Localize("Archives") %></h2>
                    <ul class="posts"><%
    foreach (IPost post in posts)
    {
        IArea postArea = post.Area;
        string className = string.Empty;
        if (post.Equals(posts.First())) className += "first ";
        if (post.Equals(posts.Last())) className += "last "; %>
                        <li<%= !string.IsNullOrEmpty(className) ? string.Format(" class=\"{0}\"", className.Trim()) : string.Empty %>>
                            <h2 class="title"><a href="<%=Url.Post(post) %>"><%=post.Title %></a></h2>
                            <div class="posted"><%=ConvertToLocalTime(post.Published.Value).ToLongDateString() %></div>
                            <div class="content"><%=post.GetBodyShort() %></div>
                            <div class="more"><%
        if ((int)ViewData["AreaCount"] > 1)
            Response.Write(string.Format(Localize("From the {0} {1}."), string.Format("<a href=\"{1}\">{0}</a>", post.Area.Name, Url.Area(post.Area)), post.Area.Type));
        
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
                        <%=Html.SimpleArchivePager(posts, this, "FullArchive", new { })%>
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
