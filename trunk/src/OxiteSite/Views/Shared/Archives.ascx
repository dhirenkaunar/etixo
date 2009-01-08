<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Archives.ascx.cs" Inherits="OxiteSite.Views.Shared.Archives" %>
<%  IEnumerable<KeyValuePair<DateTime, int>> months = (IEnumerable<KeyValuePair<DateTime, int>>)ViewData.Model;
    string archiveRouteName = ViewData["Area"] != null ? string.Format("{0}Archive", ((IArea)ViewData["Area"]).Type) : "FullArchive";
    if (months != null && months.Count() > 0)
    { %>
                    <div class="sub archives">
                        <h3>Archives</h3><%
                months = months.OrderByDescending(m => m.Key);
                if (months.Count() > 20)
                {                    
                      int lastYear = months.First().Key.Year;
                      int firstYear = months.Last().Key.Year;
                    %>
                        <ul class="yearList"><%
                    for (int year = lastYear; year >= firstYear; year--)
                    {
                        var yearMonths = months.Where(m => m.Key.Year == year);                         
                        %>
                        <% if (year == lastYear) { 
                               %>
                            <li><h4><%= year %></h4><%
                           } else { 
                               %>
                            <li class="previous"><h4><%= year %> <span>(<%= yearMonths.Sum( ym => ym.Value) %>)</span></h4><%
                           } %>
                           <%=Html.UnorderedList(
                            yearMonths,
                            t => Html.RouteLink(
                                string.Format("{0:MMMM} ({1})", t.Key, t.Value),
                                archiveRouteName,
                                new { archiveData = string.Format("{0}/{1}", t.Key.Year, t.Key.Month) },
                                null),
                            "archiveMonthList")%></li><% 
                    }
                    %>
                        </ul><%
                  }
                  else
                  {
                       %>
                    <%= Html.UnorderedList(
                    months,
                    t => Html.RouteLink(
                        string.Format("{0:MMMM yyyy} ({1})", t.Key, t.Value),
                        archiveRouteName,
                        new { archiveData = string.Format("{0}/{1}", t.Key.Year, t.Key.Month) },
                        null),
                    "archiveMonthList") %><%
                } %>
                    </div><%
    } %>