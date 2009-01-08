<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPrimaryMetadata.ascx.cs" Inherits="OxiteSite.Views.Admin.EditPrimaryMetadata" %>
<%  IEnumerable<IArea> areas = (IEnumerable<IArea>)ViewData["Areas"]; 
    IArea area = (IArea)ViewData["Area"];
    IPost post = (IPost)ViewData["Post"];
    Dictionary<Guid, string> pages = (Dictionary<Guid, string>)ViewData["Pages"];
    string type = (string)ViewData["Type"];
    RequestMode mode = (RequestMode)ViewData["RequestMode"];
    bool urlIsLocked = (bool)ViewData["UrlIsLocked"];
    string returnUri = ViewData["ReturnUri"] as string;
%>        <div class="admin metadata">
            <ul><% 
                if (areas != null) { %>
                <li class="input area">
                    <fieldset>
                        <legend><%= Localize("Blog Relationship") %></legend>
                        <label for="post_area"><%= Localize("In") %></label>
                        <%= Html.DropDownList(
                                "areaID",
                                new SelectList(areas, "ID", "Name", area != null ? area.ID.ToString() : null),
                                new { id = "post_area", tabindex = "5" },
                                !urlIsLocked
                            ) %>
                    </fieldset>
                </li><% 
                }  
                if (pages != null) { %>
                <li class="input page">
                    <fieldset>
                        <legend><%= Localize("Page Relationship") %></legend>
                        <label for="post_parent"><%= Localize("A page under") %></label>
                        <%= Html.DropDownList(
                                "parentID",
                                new SelectList(pages, "Key", "Value", post.Parent != null && post.Parent.ID != post.ID ? post.Parent.ID.ToString() : (ViewData["NewParentPostID"] != null ? ((Guid)ViewData["NewParentPostID"]).ToString() : string.Empty)),
                                new { id = "post_parent", tagindex = "6" },
                                mode == RequestMode.Add
                            ) %>
                    </fieldset>
                </li><% 
                } %>
                <li class="input tags">
                    <label for="post_tags"><%= Localize("Tags") %></label> <%= Html.ValidationMessage("Post.Tags")%>
                    <%= Html.TextBox(
                            "tags", 
                            post != null ? string.Join(", ", post.Tags.Select<ITag, string>((tag, name) => tag.Name).ToArray()) : string.Empty, 
                            new { id = "post_tags", @class="text", tabindex = "9", size = "60", title = Localize("Enter comma-delimited list of tags...", "post_tags", true) }
                        ) %>
                </li>
            </ul>
            <div class="admin buttons">
                <input type="submit" value="<%= Localize("Save") %>" class="button submit" tabindex="" />
                <%= Html.Button("cancel", Localize("Cancel"), new { @class = "cancel", tabindex = "", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Localize("really?"), returnUri ?? Url.RouteUrl("AdminHome")) }) %>
                <%= Html.Link(Localize("Cancel"), returnUri ?? Url.RouteUrl("AdminHome"), new { @class = "cancel", tabindex = "" }) %>
            </div>
        </div>