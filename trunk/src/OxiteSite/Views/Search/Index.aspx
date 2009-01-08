<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OxiteSite.Views.Search.Index" 
%><asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%
    IPageOfAList<ISearchResultItem> searchResults = (IPageOfAList<ISearchResultItem>)ViewData["SearchResults"];
    Dictionary<Guid, int> postCounts = (Dictionary<Guid, int>)ViewData["PostCounts"]; %>
            <form method="get" action="<%= Url.RouteUrl(new { controller = "Search", action = "Index" }) %>" class="search main">
                <div class="main search">
                    <div class="search-box">
                        <fieldset>
                            <label for="search_term"><%=Localize("Search this site...", "search_term", true) %></label>
                            <input id="search_term" name="Term" class="text" type="text" size="42" value="<%= Request["term"].CleanAttribute() %>" />
                            <input class="button" type="submit" value="Search" />
                        </fieldset>
                        <% RenderStringResources(); %>
                   </div><%
                    
    if (searchResults != null && searchResults.TotalItemCount > 0)
    {
%>
                    <ul class="posts"><%
        foreach (IPost post in searchResults)
        {
            IArea area = post.Area;
            string className = string.Empty;
            if (post.Equals(searchResults.First())) className += "first ";
            if (post.Equals(searchResults.Last())) className += "last "; %>
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
            } %> | <a href="<%=Url.Post(post) %>#comments"><%=commentCount %> comment<%=commentCount == 1 ? "" : "s"%></a> <a href="<%=Url.Post(post) %>" class="arrow">&raquo;</a></div>
                        </li><%
        } %>
                    </ul><%
        if (searchResults.TotalPageCount > 1)
        { %>
                    <div class="pager">
                        <%=Html.SimplePager(searchResults, this, "PageOfASearch", new { controller = "Search" })%>
                    </div><%
        }
    }
    else
    { %>
                    <div class="message info"><%=Localize("None found") %></div><%        
    } %>
                </div>
            </form>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>