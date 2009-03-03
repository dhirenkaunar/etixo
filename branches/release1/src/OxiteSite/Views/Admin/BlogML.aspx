<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlogML.aspx.cs" Inherits="OxiteSite.Views.Admin.BlogML" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
                    <h2>BlogML</h2>
                    <h3><%=Localize("Import") %></h3><%
    if (!string.IsNullOrEmpty((string)ViewData["Message"]))
    { %>
                    <div class="message info"><%=ViewData["Message"] %></div><%
    } %>
                    <form action="" method="post" enctype="multipart/form-data">
                        <div>BlogML File: <input type="file" name="blogMLFile" /></div>
                        <div>Slug Pattern: <%=Html.TextBox("slugPattern", "/([^/]+)\\.aspx") %></div>
                        <div><%=Localize("(Leave blank if you want post-url attribute to be used as is)") %></div>
                        <div><input type="submit" value="<%= Localize("Upload") %>" /></div>
                    </form>
                    <h3><%=Localize("Export") %></h3>
                    <p><%=Localize("Coming soon") %></p>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>
