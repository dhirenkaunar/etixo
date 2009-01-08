<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminTasks.ascx.cs" Inherits="OxiteSite.Views.Shared.AdminTasks" %>
                        <ul>
                            <li><a href="<%=Url.RouteUrl("AdminAddPost") %>"><%=Localize("Create Post") %></a></li>
                            <li><a href="<%=Url.RouteUrl("PageAdd") %>"><%=Localize("Create Page") %></a></li>
                            <li><a href="<%=Url.RouteUrl("AdminComments") %>"><%=Localize("Manage Comments") %></a></li>
                            <li><a href="<%=Url.RouteUrl("AdminSettings") %>"><%=Localize("Settings") %></a></li>
                            <!--<li><a href="<%=Url.RouteUrl("AdminAreas") %>"><%=Localize("Manage Areas") %></a></li>-->
                            <!--<li><a href="">Manage Pages</a></li>-->
                            <!--<li><a href="<%=Url.RouteUrl("AdminFiles") %>"><%=Localize("Manage Files") %></a></li>-->
                        </ul>
