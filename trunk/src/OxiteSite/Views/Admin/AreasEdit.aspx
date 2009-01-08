<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreasEdit.aspx.cs" Inherits="OxiteSite.Views.Admin.AreasEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%  IArea area = (IArea)ViewData["Area"]; %>
            <div class="sections">
                <div class="primary">
                    <h2 class="title">Areas</h2><%
    if (!string.IsNullOrEmpty((string)ViewData["Message"]))
    { %>
                    <div class="message"><%=ViewData["Message"] %></div><%
    } %>
                    <form action="<%=area == null ? Url.RouteUrl("AdminAreasAdd") : Url.RouteUrl("AdminAreasEdit") %>" method="post"><%
    if (area != null)
    { %>
                        <div><input type="hidden" name="areaID" value="<%=area.ID.ToString() %>" /></div><%
    } %>
                        <div>Name: <input type="text" name="areaName" value="<%=ViewData["AreaName"] != null ? (string)ViewData["AreaName"] : area != null ? area.Name : "" %>" /></div>
                        <div>Display Name: <input type="text" name="areaDisplayName" value="<%=ViewData["AreaDisplayName"] != null ? (string)ViewData["AreaDisplayName"] : area != null ? area.DisplayName : "" %>" /></div>
                        <div>Description: <textarea name="areaDescription" cols="50" rows="10"><%=ViewData["AreaDescription"] != null ? (string)ViewData["AreaDescription"] : area != null ? area.Description : "" %></textarea></div>
                        <div>Type: <input type="text" name="areaType" value="<%=ViewData["AreaType"] != null ? (string)ViewData["AreaType"] : area != null ? area.Type : "" %>" /></div>
                        <div><input type="submit" name="createArea" value="<%=area == null ? Localize("Create Area") : Localize("Edit Area") %>" /></div>
                    </form>
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