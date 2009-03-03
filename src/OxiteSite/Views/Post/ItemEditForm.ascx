﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
    <form method="post" action="">
        <div class="avatar"><%=Html.Gravatar("48") %></div>
	    <h2 class="title">
	        <label for="post_title"><%=Model.Localize("Title")%></label><%=Html.ValidationMessage("Post.Title", Model.Localize("Title isn't valid.")) %>
	        <%=Html.TextBox(
	            "title", 
	            Request["title"] ?? (Model.Item != null ? Model.Item.Title : ""),
	            new { id = "post_title", @class = "text", size = "60", title = Model.Localize("Enter a title...") }
	            ) %>
            <%=Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
	    </h2>
        <% Html.RenderPartial("ItemEditPrimaryMetadata"); %>
	    <div class="content">
	        <label for="post_bodyShort"><%=Model.Localize("Excerpt") %></label><%=Html.ValidationMessage("Post.BodyShort", Model.Localize("Excerpt isn't valid.")) %>
	        <%=Html.TextArea(
	            "bodyShort",
	            Request["bodyShort"] ?? ( Model.Item != null ? Model.Item.BodyShort : ""),
                6 /*rows*/,
                120 /*cols*/,
                new { id = "post_bodyShort", title = Model.Localize("Enter an excerpt...") }
                ) %>
	        <label for="post_body"><%=Model.Localize("Body Content") %></label><%=Html.ValidationMessage("Post.Body", Model.Localize("Body isn't valid.")) %>
	        <%=Html.TextArea(
	            "body",
                Request["body"] ?? (Model.Item != null ? Model.Item.Body : ""),
                24 /*rows*/,
                120 /*cols*/,
                new { id = "post_body", title = Model.Localize("Enter body content...") }
                ) %>
	    </div>
        <% Html.RenderPartial("ItemEditSecondaryMetadata"); %>
	</form>