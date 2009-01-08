//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Oxite.Data;
using Oxite.Mvc.Views;

namespace Oxite.Mvc
{
    public static class HtmlHelperExtensions
    {
        #region AntiForgeryToken

        public static string AntiForgeryToken(this HtmlHelper htmlHelper, string token)
        {
            return htmlHelper.Hidden(Mvc.AntiForgeryToken.TokenInputName, token);
        }

        public static string AntiForgeryTicks(this HtmlHelper htmlHelper, string ticks)
        {
            return htmlHelper != null ? htmlHelper.Hidden(Mvc.AntiForgeryToken.TicksInputName, ticks) : string.Empty;
        }

        #endregion

        #region Gravatar

        public static string Gravatar(this HtmlHelper htmlHelper, IUser user, string size, string defaultImage)
        {
            return htmlHelper.Gravatar(user.HashedEmail, user.DisplayName, size, defaultImage);
        }

        public static string Gravatar(this HtmlHelper htmlHelper, string id, string name, string size,
                                      string defaultImage)
        {
            return htmlHelper.Image(
                string.Format("http://www.gravatar.com/avatar/{0}?s={1}&default={2}", id, size, defaultImage),
                string.Format("{0} (gravatar)", name),
                new RouteValueDictionary(new {width = size, height = size, @class = "gravatar"})
                );
        }

        #endregion

        #region HeadLink

        public static string HeadLink(this HtmlHelper htmlHelper, HeadLink headLink)
        {
            return htmlHelper.HeadLink(headLink.Rel, headLink.Href, headLink.Type, headLink.Title,
                                       headLink.HtmlAttributes);
        }

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title)
        {
            return htmlHelper.HeadLink(rel, href, type, title, null);
        }

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title,
                                      object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("link");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(rel))
            {
                tagBuilder.MergeAttribute("rel", rel);
            }
            if (!string.IsNullOrEmpty(href))
            {
                tagBuilder.MergeAttribute("href", href);
            }
            if (!string.IsNullOrEmpty(type))
            {
                tagBuilder.MergeAttribute("type", type);
            }
            if (!string.IsNullOrEmpty(title))
            {
                tagBuilder.MergeAttribute("title", title);
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Image

        public static string Image(this HtmlHelper helper, string imageSrc, string alt,
                                   IDictionary<string, object> htmlAttributes)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext);
            string imageUrl = url.Content(imageSrc);

            TagBuilder imageTag = new TagBuilder("img");

            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageTag.MergeAttribute("src", imageUrl);
            }

            if (!string.IsNullOrEmpty(alt))
            {
                imageTag.MergeAttribute("alt", alt);
            }

            imageTag.MergeAttributes(htmlAttributes, true);

            if (imageTag.Attributes.ContainsKey("alt") && !imageTag.Attributes.ContainsKey("title"))
            {
                imageTag.MergeAttribute("title", imageTag.Attributes["alt"] ?? "");
            }

            return imageTag.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Input

        public static string DropDownList(this HtmlHelper htmlHelper, string name, SelectList selectList,
                                          object htmlAttributes, bool isEnabled)
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(htmlHelper.DropDownList(string.Format("{0}_view", name), selectList,
                                                                    htmlAttributeDictionary));
                inputItemBuilder.AppendLine(htmlHelper.Hidden(name, selectList.SelectedValue));
                return inputItemBuilder.ToString();
            }

            return htmlHelper.DropDownList(name, selectList, htmlAttributeDictionary);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked,
                                         object htmlAttributes, bool isEnabled)
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(htmlHelper.RadioButton(string.Format("{0}_view", name), value, isChecked,
                                                                   htmlAttributeDictionary));
                if (isChecked)
                {
                    inputItemBuilder.AppendLine(htmlHelper.Hidden(name, value));
                }
                return inputItemBuilder.ToString();
            }

            return htmlHelper.RadioButton(name, value, isChecked, htmlAttributeDictionary);
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes,
                                     bool isEnabled)
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.Append(htmlHelper.TextBox(string.Format("{0}_view", name), value,
                                                           htmlAttributeDictionary));
                inputItemBuilder.Append(htmlHelper.Hidden(name, value));
                return inputItemBuilder.ToString();
            }

            return htmlHelper.TextBox(name, value, htmlAttributeDictionary);
        }

        public static string Button(this HtmlHelper htmlHelper, string name, string buttonContent, object htmlAttributes)
        {
            return htmlHelper.Button(name, buttonContent, new RouteValueDictionary(htmlAttributes));
        }

        public static string Button(this HtmlHelper htmlHelper, string name, string buttonContent,
                                    IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("button")
                                    {
                                        InnerHtml = buttonContent
                                    };
            tagBuilder.MergeAttributes(htmlAttributes);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region Link

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href)
        {
            return Link(htmlHelper, linkText, href, null);
        }

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href, object htmlAttributes)
        {
            return htmlHelper.Link(linkText, href, new RouteValueDictionary(htmlAttributes));
        }

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href,
                                  IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a")
                                    {
                                        InnerHtml = linkText
                                    };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", href);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region Pager

        public static string SimplePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList, BaseViewPage view,
                                            string routeName, object values)
        {
            return htmlHelper.SimplePager(pageOfAList, s => view.Localize(s), routeName, values);
        }

        public static string SimplePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                            BaseViewUserControl control, string routeName, object values)
        {
            return htmlHelper.SimplePager(pageOfAList, s => control.Localize(s), routeName, values);
        }

        public static string SimplePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                            Func<string, string> localize, string routeName, object values)
        {
            return SimplePager<T>(htmlHelper, pageOfAList, routeName, values, localize("« Newer"), localize("Older »"),
                                  false);
        }

        public static string SimplePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList, string routeName,
                                            object values, string previousText, string nextText,
                                            bool alwaysShowPreviousAndNext)
        {
            StringBuilder sb = new StringBuilder(50);
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary rvd = new RouteValueDictionary();

            foreach (KeyValuePair<string, object> item in viewContext.RouteData.Values)
            {
                rvd.Add(item.Key, item.Value);
            }

            UrlHelper urlHelper = new UrlHelper(viewContext);

            rvd.Remove("controller");
            rvd.Remove("action");
            rvd.Remove("id");

            if (values != null)
            {
                RouteValueDictionary rvd2 = new RouteValueDictionary(values);

                foreach (KeyValuePair<string, object> item in rvd2)
                {
                    rvd[item.Key] = item.Value;
                }
            }

            if (pageOfAList.PageIndex < pageOfAList.TotalPageCount - 1 || alwaysShowPreviousAndNext)
            {
                rvd["page"] = pageOfAList.PageIndex + 2;

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"next\">{0}</a>", nextText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            if (pageOfAList.PageIndex > 0 || alwaysShowPreviousAndNext)
            {
                rvd["page"] = pageOfAList.PageIndex;

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"previous\">{0}</a>", previousText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            return sb.ToString();
        }

        public static string SimpleArchivePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                                   BaseViewPage view, string routeName, object values)
        {
            return htmlHelper.SimpleArchivePager(pageOfAList, s => view.Localize(s), routeName, values);
        }

        public static string SimpleArchivePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                                   BaseViewUserControl control, string routeName, object values)
        {
            return htmlHelper.SimpleArchivePager(pageOfAList, s => control.Localize(s), routeName, values);
        }

        public static string SimpleArchivePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                                   Func<string, string> localize, string routeName, object values)
        {
            return SimpleArchivePager<T>(htmlHelper, pageOfAList, routeName, values, localize("« Newer"),
                                         localize("Older »"), false);
        }

        public static string SimpleArchivePager<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                                   string routeName, object values, string previousText, string nextText,
                                                   bool alwaysShowPreviousAndNext)
        {
            StringBuilder sb = new StringBuilder(50);
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary rvd = new RouteValueDictionary();
            ArchiveData archiveData;

            foreach (KeyValuePair<string, object> item in viewContext.RouteData.Values)
            {
                rvd.Add(item.Key, item.Value);
            }

            UrlHelper urlHelper = new UrlHelper(viewContext);

            rvd.Remove("controller");
            rvd.Remove("action");
            rvd.Remove("id");

            if (values != null)
            {
                RouteValueDictionary rvd2 = new RouteValueDictionary(values);

                foreach (KeyValuePair<string, object> item in rvd2)
                {
                    rvd[item.Key] = item.Value;
                }
            }

            archiveData = new ArchiveData(rvd["archiveData"] as string);

            if (pageOfAList.PageIndex < pageOfAList.TotalPageCount - 1 || alwaysShowPreviousAndNext)
            {
                archiveData.Page = pageOfAList.PageIndex + 2;
                rvd["archiveData"] = archiveData.ToString();

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"next\">{0}</a>", nextText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            if (pageOfAList.PageIndex > 0 || alwaysShowPreviousAndNext)
            {
                archiveData.Page = pageOfAList.PageIndex;
                rvd["archiveData"] = archiveData.ToString();

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"previous\">{0}</a>", previousText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            return sb.ToString();
        }

        #endregion

        #region PageStatus

        public static string PageStatus<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList, BaseViewPage view)
        {
            return pageOfAList.Count > 0
                       ?
                           htmlHelper.PageStatus<T>(
                               pageOfAList,
                               p => string.Format(
                                        view.Localize("Displaying {0}-{1} of {2}"),
                                        p.PageIndex * p.PageSize + 1,
                                        p.PageIndex * p.PageSize + p.Count,
                                        p.TotalItemCount
                                        )
                               )
                       :
                           view.Localize("None found");
        }

        public static string PageStatus<T>(this HtmlHelper htmlHelper, IPageOfAList<T> pageOfAList,
                                           Func<IPageOfAList<T>, string> generateContent)
        {
            return generateContent(pageOfAList);
        }

        #endregion

        #region ScriptBlock

        public static string ScriptBlock(this HtmlHelper htmlHelper, string type, string src)
        {
            return htmlHelper.ScriptBlock(type, src, null);
        }

        public static string ScriptBlock(this HtmlHelper htmlHelper, string type, string src, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("script");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(type))
            {
                tagBuilder.MergeAttribute("type", type);
            }
            if (!string.IsNullOrEmpty(src))
            {
                tagBuilder.MergeAttribute("src", src);
            }

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region UnorderedList

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, string> generateContent)
        {
            return UnorderedList<T>(htmlHelper, items, (t, i) => generateContent(t));
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, string> generateContent, string cssClass)
        {
            return UnorderedList<T>(htmlHelper, items, (t, i) => generateContent(t), cssClass, null, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, int, string> generateContent)
        {
            return UnorderedList<T>(htmlHelper, items, generateContent, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, int, string> generateContent, string cssClass)
        {
            return UnorderedList<T>(htmlHelper, items, generateContent, cssClass, null, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, int, string> generateContent, string cssClass, string itemCssClass,
                                              string alternatingItemCssClass)
        {
            if (items == null || items.Count() == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(100);
            int counter = 0;

            sb.Append("<ul");
            if (!string.IsNullOrEmpty(cssClass))
            {
                sb.AppendFormat(" class=\"{0}\"", cssClass);
            }
            sb.Append(">");
            foreach (T item in items)
            {
                StringBuilder sbClass = new StringBuilder(40);

                if (counter == 0)
                {
                    sbClass.Append("first ");
                }
                if (item.Equals(items.Last()))
                {
                    sbClass.Append("last ");
                }

                if (counter % 2 == 0 && !string.IsNullOrEmpty(itemCssClass))
                {
                    sbClass.AppendFormat("{0} ", itemCssClass);
                }
                else if (counter % 2 != 0 && !string.IsNullOrEmpty(alternatingItemCssClass))
                {
                    sbClass.AppendFormat("{0} ", alternatingItemCssClass);
                }

                sb.Append("<li");
                if (sbClass.Length > 0)
                {
                    sb.AppendFormat(" class=\"{0}\"", sbClass.Remove(sbClass.Length - 1, 1));
                }
                sb.AppendFormat(">{0}</li>", generateContent(item, counter));

                counter++;
            }
            sb.Append("</ul>");

            return sb.ToString();
        }

        #endregion
    }
}