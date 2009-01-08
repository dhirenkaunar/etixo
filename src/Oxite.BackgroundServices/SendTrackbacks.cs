//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.BackgroundServices
{
    public class SendTrackbacks : PostBackgroundServiceAction
    {
        private static Guid typeID = new Guid("F762DA8A-E37F-4592-818A-846BE53BC96A");

        public SendTrackbacks(IBackgroundServiceConfiguration backgroundServiceConfiguration)
        {
            LoadFromConfig(backgroundServiceConfiguration);

            Routes = RouteTable.Routes;

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            AreaRepository = dataProvider.AreaRepository;
            BackgroundServiceActionRepository = dataProvider.BackgroundServiceActionRepository;
            PostRepository = dataProvider.PostRepository;
            Actions = new List<IBackgroundServiceAction>(10);

            RegisterRoutes();
        }

        public SendTrackbacks(IBackgroundServiceConfiguration backgroundServiceConfiguration, RouteCollection routes,
                              IOxiteConfiguration config,
                              IBackgroundServiceActionRepository backgroundServiceActionRepository,
                              IAreaRepository areaRepository, IPostRepository postRepository)
            : base(
                backgroundServiceConfiguration, routes, config, backgroundServiceActionRepository, areaRepository,
                postRepository)
        {
        }

        public override Guid TypeID
        {
            get
            {
                return typeID;
            }
        }

        public override void Run()
        {
            base.Run();

            ExecuteActions(a => sendTrackback(GetPost(a)));
        }

        private void sendTrackback(IPost post)
        {
            string areaName = new PageTitleHelper() {Post = post, Area = post.Area}.ToString();
            Regex r =
                new Regex(
                    @"(?<HTML><a[^>]*href\s*=\s*[\""\']?(?<HRef>[^""'>\s]*)[\""\']?[^>]*>(?<Title>[^<]+|.*?)?</a>)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection m = r.Matches(post.Body);
            List<string> links = new List<string>(10);

            foreach (Match mtch in m)
            {
                links.Add(mtch.Groups["HRef"].Value);
            }

            foreach (string link in links)
            {
                sendTrackBackPing(link, post.Title, GetEntryPath(post), areaName, post.Body);
            }
        }

        private static void sendTrackBackPing(string url, string title, string link, string areaName, string description)
        {
            WebClient wc = new WebClient();
            string pageText = wc.DownloadString(url);
            string trackBackItem = getTrackBackText(pageText, url, link);

            if (trackBackItem != null)
            {
                if (!trackBackItem.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    trackBackItem = "http://" + trackBackItem;
                }

                sendPing(trackBackItem,
                         string.Format("title={0}&url={1}&blog_name={2}&excerpt={3}", HttpUtility.HtmlEncode(title),
                                       HttpUtility.HtmlEncode(link), HttpUtility.HtmlEncode(areaName),
                                       HttpUtility.HtmlEncode(description)));
            }
        }

        private static void sendPing(string trackBackItem, string parameters)
        {
            StreamWriter myWriter = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(trackBackItem);

            request.Method = "POST";
            request.ContentLength = parameters.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;

            try
            {
                myWriter = new StreamWriter(request.GetRequestStream());
                myWriter.Write(parameters);
            }
            finally
            {
                myWriter.Close();
            }
        }

        private static string getTrackBackText(string pageText, string url, string postUrl)
        {
            if (!Regex.IsMatch(pageText, postUrl, RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                string sPattern = @"<rdf:\w+\s[^>]*?>(</rdf:rdf>)?";
                Regex r = new Regex(sPattern, RegexOptions.IgnoreCase);
                Match m;
                string text;
                string tbPattern;
                Regex reg;
                Match m2;

                for (m = r.Match(pageText); m.Success; m = m.NextMatch())
                {
                    if (m.Groups.ToString().Length > 0)
                    {
                        text = m.Groups[0].ToString();

                        if (text.IndexOf(url, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            tbPattern = "trackback:ping=\"([^\"]+)\"";
                            reg = new Regex(tbPattern, RegexOptions.IgnoreCase);
                            m2 = reg.Match(text);

                            if (m2.Success)
                            {
                                return m2.Result("$1");
                            }
                            else
                            {
                                return text;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}