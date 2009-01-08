//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Security.Application;
using Oxite.Configuration;

namespace Oxite
{
    public static class StringExtensions
    {
        private static IOxiteConfiguration _config;
        private static readonly Regex cleanWhitespace = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.Singleline);
        private static Dictionary<string, Regex> regularExpressions;

        private static IOxiteConfiguration config
        {
            get
            {
                if (_config == null)
                {
                    _config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
                }

                return _config;
            }
        }

        private static Regex getRegex(string name)
        {
            Regex regex = null;

            if (regularExpressions == null)
            {
                regularExpressions = new Dictionary<string, Regex>(10);
            }

            if (regularExpressions.ContainsKey(name))
            {
                regex = regularExpressions[name];
            }
            else
            {
                if (config != null)
                {
                    foreach (IValidationConfiguration validation in config.Validation)
                    {
                        if (validation.Name == name)
                        {
                            regex = new Regex(HttpUtility.HtmlDecode(validation.Regex),
                                              (RegexOptions)validation.RegexOptions);
                            break;
                        }
                    }

                    if (regex != null)
                    {
                        regularExpressions.Add(name, regex);
                    }
                    else
                    {
                        throw new Exception(string.Format("Could not find a regular expression for validation '{0}'",
                                                          name));
                    }
                }
            }

            return regex;
        }

        public static string IsRequired(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ValidationException(string.Format("String is required: {0}", s));
            }

            return s;
        }

        public static string IsSlug(this string s)
        {
            Regex regex = getRegex("IsSlug");

            if (regex != null && !regex.IsMatch(s))
            {
                throw new ValidationException(string.Format("String is not a valid Slug: {0}", s));
            }

            return s;
        }

        public static string IsTag(this string s)
        {
            Regex regex = getRegex("IsTag");

            if (regex != null && !regex.IsMatch(s))
            {
                throw new ValidationException(string.Format("String is not a valid Tag: {0}", s));
            }

            return s;
        }

        public static string IsEmail(this string s)
        {
            Regex regex = getRegex("IsEmail");

            if (regex != null && !regex.IsMatch(s))
            {
                throw new ValidationException(string.Format("String is not a valid Email: {0}", s));
            }

            return s;
        }

        public static string IsUrl(this string s)
        {
            Regex regex = getRegex("IsUrl");

            if (!(s.StartsWith("http://") || s.StartsWith("https://")))
            {
                s = string.Format("http://{0}", s);
            }

            if (regex != null && !regex.IsMatch(s))
            {
                throw new ValidationException(string.Format("String is not a valid Url: {0}", s));
            }

            return s;
        }

        public static string CleanText(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string CleanHtml(this string s)
        {
            //AntiXss library from Microsoft 
            //(http://codeplex.com/antixss)
            string encodedText = AntiXss.HtmlEncode(s);
            //convert line breaks into an html break tag
            return encodedText.Replace("&#13;&#10;", "<br />");
        }

        public static string CleanAttribute(this string s)
        {
            return AntiXss.HtmlAttributeEncode(s);
        }

        public static string CleanHref(this string s)
        {
            return HttpUtility.HtmlAttributeEncode(s);
        }

        public static string CleanWhitespace(this string s)
        {
            return cleanWhitespace.Replace(s, " ");
        }

        public static bool GuidTryParse(this string s, out Guid result)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            try
            {
                result = new Guid(s);
                return true;
            }
            catch (FormatException)
            {
                result = Guid.Empty;
                return false;
            }
            catch (OverflowException)
            {
                result = Guid.Empty;
                return false;
            }
        }

        public static string Slugify(this string title)
        {
            string slug = "";

            if (!string.IsNullOrEmpty(title))
            {
                Regex regex = getRegex("SlugReplace");

                slug = title.Trim();
                slug = slug.Replace(' ', '-');
                slug = slug.Replace("---", "-");
                slug = slug.Replace("--", "-");
                if (regex != null)
                {
                    slug = regex.Replace(slug, "");
                }

                if (slug.Length * 2 < title.Length)
                {
                    return "";
                }

                if (slug.Length > 100)
                {
                    slug = slug.Substring(0, 100);
                }
            }

            return slug;
        }
    }
}