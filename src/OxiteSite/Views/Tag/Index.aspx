<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OxiteSite.Views.Tag.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="sections">
        <div class="primary">
            <h2 class="title">Tags</h2><%
    if (ViewData["Tags"] != null)
    {
        IEnumerable<KeyValuePair<ITag, int>> tagsWithPostCounts = (IEnumerable<KeyValuePair<ITag, int>>)ViewData["Tags"];
        double? averagePostCount = null;
        double? standardDeviationPostCount = null;
        
        Response.Write(
            Html.UnorderedList(
                tagsWithPostCounts.OrderBy(kvp => kvp.Key.Name),
                t => string.Format(
                    "<a href=\"{2}\" rel=\"tag\" class=\"t{3}\">{0} ({1})</a> ",
                    t.Key.Name,
                    t.Value,
                    Url.Tag(t.Key),
                    t.Key.GetTagWeight(tagsWithPostCounts, ref averagePostCount, ref standardDeviationPostCount)
                ),
                "tagCloud"
            )
        );
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