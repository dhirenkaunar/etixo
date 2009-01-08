<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommentsOnly.aspx.cs" Inherits="OxiteSite.Views.Admin.CommentsOnly" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%
    IPageOfAList<IComment> comments = (IPageOfAList<IComment>)ViewData["Comments"];
%>
            <div class="sections">
                <div class="primary">
                    <h2><%= Localize("Manage Comments") %></h2>
                    <% Html.RenderPartial("Comments", comments, ViewData); %>
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
