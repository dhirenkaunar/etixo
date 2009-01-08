<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Areas.aspx.cs" Inherits="OxiteSite.Views.Admin.Areas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <div class="sections">
                <div class="primary">
                    <h2 class="title">Areas</h2><%
    if (!string.IsNullOrEmpty((string)ViewData["Message"]))
    { %>
                    <div class="message"><%=ViewData["Message"] %></div><%
    } %>
                    <div><a href="<%=Url.RouteUrl("AdminAreasAdd") %>"><%=Localize("Create Area") %></a></div>
                    <form method="post" action="<%=Url.RouteUrl("AdminAreas") %>">
                        <div>Find Area: <input type="text" name="areaNameSearch" value="<%=ViewData["AreaNameSearch"] != null ? (string)ViewData["AreaNameSearch"] : "" %>" /> <input type="submit" name="findArea" value="Find" /></div>
                    </form><%
    IEnumerable<IArea> areas = (IEnumerable<IArea>)ViewData["Areas"];
    if (areas != null && areas.Count() > 0)
    {
        Response.Write(
            Html.UnorderedList(
                areas,
                a => string.Format("<a href=\"{1}\">{0}</a>",
                    !string.IsNullOrEmpty(a.DisplayName) ? a.DisplayName : a.Name,
                    Url.AdminArea(a)
                )
            )
        );
    }
    else if (ViewData["AreaNameSearch"] != null)
    { %>
                    <p><%=Localize("No areas found.") %></p><%
    } %>
                </div>
                <div class="secondary">
                    <% Html.RenderPartial("Search"); %>
                    <div class="sub admin manage">
                        <h3><%=Localize("Tasks") %></h3>
<% Html.RenderPartial("AdminTasks"); %>
                    </div>
                </div>
            </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>