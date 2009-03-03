<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Files.aspx.cs" Inherits="OxiteSite.Views.Admin.Files" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h1><%=User.DisplayName %>'s Files</h1><%
    if (ViewData["Files"] != null && ((IEnumerable<IFileResource>)ViewData["Files"]).Count() > 0)
    { %>
        <ul><%
        foreach (IFileResource file in (IEnumerable<IFileResource>)ViewData["Files"])
        {
            string filePath = file.Path;

            if (filePath != "")
                filePath = "/" + filePath;

            filePath = "/" + file.Name;

            filePath = Url.RouteUrl("UserFile", new { username = User.Username, filePath = filePath }); %>
            <li>
                <form action="<%=Url.Action("FileModify", new { type = "remove" }) %>" method="post">
                    <div><input type="hidden" name="fileID" value="<%=file.ID.ToString("N") %>" /></div>
                    <div><input type="image" class="button" src="<%=ViewData["SkinPath"] %>/images/clear.gif" /></div>
                </form>
                <a href="<%=filePath %>"><%=filePath %></a>
            </li><%
        } %>
        </ul><%
    }
    else
    { %>
    <p>No files found.</p><%
    } %>
    <form action="<%=Url.Action("Files") %>" method="post" enctype="multipart/form-data">
        <div><input type="file" name="file" /></div>
        <div><%=Html.TextBox("path") %></div>
        <div><input type="submit" value="<%= Localize("Add File") %>" /></div>
    </form>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts">
    <%=RegisterScript("site.js") %>
</asp:Content>