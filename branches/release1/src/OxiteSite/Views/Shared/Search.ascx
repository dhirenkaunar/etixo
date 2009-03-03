<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="OxiteSite.Views.Shared.Search" %>
                    <div class="sub search">
                        <form id="search" method="get" action="<%= Url.RouteUrl("Search") %>">
                            <fieldset>
                                <label for="search_term"><%= Localize("Search this site...", "search_term", true) %></label>
                                <%= Html.TextBox("term", "", new { id = "search_term", @class = "text" })%>
                                <input type="submit" value="<%= Localize("Search") %>" class="button" />
                            </fieldset>
                            <% RenderStringResources(); %>
                        </form>
                    </div>