<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexError.aspx.cs" Inherits="OxiteSite.Views.Pingback.IndexError" %>
<methodResponse>
    <fault>
        <value>
            <struct>
                <member>
                    <name>faultCode</name>
                    <value>
                        <int><%=ViewData["ErrorCode"] %></int>
                    </value>
                </member>
                <member>
                    <name>faultString</name>
                    <value>
                        <string><%=ViewData["ErrorText"] %></string>
                    </value>
                </member>
            </struct>
        </value>
    </fault>
</methodResponse>