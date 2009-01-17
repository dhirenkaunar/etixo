﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OxiteSite.Views.Admin.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%
    IPageOfAList<IPost> posts = ((IPageOfAList<IPost>)ViewData["Posts"]);
    Dictionary<Guid, int> postCounts = (Dictionary<Guid, int>)ViewData["PostCounts"]; %>
            <div class="sections">
                <div class="primary">
                    <h2 class="title">Admin</h2>
                    <ul class="posts"><%
    foreach (IPost post in posts)
    {
        IArea area = post.Area;
        string className = string.Empty;
        if (post.Equals(posts.First())) className += "first ";
        if (post.Equals(posts.Last())) className += "last "; %>
                        <li<%= !string.IsNullOrEmpty(className) ? string.Format(" class=\"{0}\"", className.Trim()) : string.Empty %>>
                            <h2 class="title"><a href="<%=Url.Post(post) %>"><%=post.Title %></a></h2>
                            <div class="posted"><%=(post.Published.HasValue ? ConvertToLocalTime(post.Published.Value).ToLongDateString() : "Draft") %></div>
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
                        t =>
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
                        <%=Html.SimplePager(posts, this, "PageOfAnAdminHome", new { })%>
                    </div><%
    } %>
                </div>
                <div class="secondary">
                    <% Html.RenderPartial("Search"); %>
                    <div class="sub admin manage">
                        <h3>Tasks</h3>
<% Html.RenderPartial("AdminTasks"); %>
                    </div>
                </div>
            </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>