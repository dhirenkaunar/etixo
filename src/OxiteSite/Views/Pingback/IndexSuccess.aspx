<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexSuccess.aspx.cs" Inherits="OxiteSite.Views.Pingback.IndexSuccess" %>
<methodResponse>
    <params>
        <param>
            <value>
                <string>Pingback from <%=ViewData["SourceUrl"] %> to <%=ViewData["TargetUrl"] %> registered. Keep the web talking! :-)</string>
            </value>
        </param>
    </params>
</methodResponse>