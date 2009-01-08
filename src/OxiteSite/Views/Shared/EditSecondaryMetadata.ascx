<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSecondaryMetadata.ascx.cs" Inherits="OxiteSite.Views.Admin.EditSecondaryMetadata" %>
<% 
    IPost post = (IPost)ViewData["Post"];
    RequestMode mode = (RequestMode)ViewData["RequestMode"];
    bool urlIsLocked = (bool)ViewData["UrlIsLocked"];
    string returnUri = ViewData["ReturnUri"] as string;
%>        <div class="admin metadata">
            <ul>
                <li class="input slug"><% 
                    if (ViewData.ModelState.ContainsKey("Post.Slug")) {
                        %><%= Html.ValidationMessage("Post.Slug", Localize("Slug?!"))%><% 
                    } else { 
                        %><label for="post_slug"><%= Localize("Slug") %></label><% 
                    } 
                    %> <%= Html.TextBox("slug", !string.IsNullOrEmpty(Request.Form["slug"]) ? Request["slug"] : post.Slug ?? string.Empty, new { id = "post_slug", @class = "text", tabindex = "2", size = "72", title = Localize("Enter slug...", "post_slug", true) }, !urlIsLocked) %></li>
                <% if (!urlIsLocked) { %><li class="input draft"><input type="radio" name="isPublished" value="false" id="post_stateDraft" tabindex="6"<%= !(post.Published.HasValue || (string.Compare(Request.Form["isPublished"], "true", true) == 0)) ? " checked='checked'" : string.Empty %>/> <label for="post_stateDraft" class="radio"><%= Localize("Draft", "post_stateDraft", true) %></label></li><% } %>
                <li class="input publish">
                    <fieldset>
                        <legend><%= Localize("Publish") %></legend>
                        <%= Html.RadioButton("isPublished", true, post.Published.HasValue || (string.Compare(Request.Form["isPublished"], "true", true) == 0), new { id = "post_statePublished", tabindex = "6" }, !urlIsLocked)%> <label for="post_statePublished" class="radio"><%= Localize(string.Format("Publish{0}", post.Published.HasValue && ConvertToLocalTime(post.Published.Value) < DateTime.Now ? "ed" : string.Empty), "post_statePublished", true) %></label>
                        <label for="post_publishDate"><%= Localize("Publish Date") %></label><% 
                        if (ViewData.ModelState.ContainsKey("Post.PublishDate")) { 
                            %><%= Html.ValidationMessage("Post.PublishDate") %><% 
                        } 
                        %><%= Html.TextBox("publishDate", post.Published != null && post.Published.HasValue && post.Published.Value > default(DateTime) ? post.Published.Value.ToStringForEdit() : string.Empty, new { id = "post_publishDate", @class = "text date", tabindex="8", size="22", title = Localize("Enter publish date...", "post_publishDate", true) }, !urlIsLocked) %>
                        <input type="hidden" name="postState" value="<%=((byte)EntityState.Normal).ToString() %>" />
                    </fieldset>
                </li>
            </ul>
        <div class="admin buttons">
            <input type="submit" value="<%= Localize("Save") %>" class="button submit" tabindex="" />
            <%= Html.Button("cancel", Localize("Cancel"), new { @class = "cancel", tabindex = "", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Localize("really?"), returnUri ?? Url.RouteUrl("AdminHome")) })%>
            <%= Html.Link(Localize("Cancel"), returnUri ?? Url.RouteUrl("AdminHome"), new { @class = "cancel", tabindex = "" })%>
        </div>
</div>