//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class PingbackController : Controller
    {
        public PingbackController()
            : base()
        {
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AppSettings = new AppSettingsHelper(ConfigurationManager.AppSettings);

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            PostRepository = dataProvider.PostRepository;
            TrackbackRepository = dataProvider.TrackbackRepository;
        }

        public PingbackController(IOxiteConfiguration config, NameValueCollection appSettings,
                                  IPostRepository postRepository, ITrackbackRepository trackbackRepository)
            : base()
        {
            Config = config;
            AppSettings = new AppSettingsHelper(appSettings);
            PostRepository = postRepository;
            TrackbackRepository = trackbackRepository;
        }

        public IOxiteConfiguration Config { get; private set; }
        public AppSettingsHelper AppSettings { get; private set; }
        protected IPostRepository PostRepository { get; private set; }
        protected ITrackbackRepository TrackbackRepository { get; private set; }

        public virtual ActionResult Index(Guid id)
        {
            if (!Config.Site.TrackbacksEnabled)
            {
                return new TrackbackErrorResult(0, "Pingbacks are not enabled");
            }

            IPost post = PostRepository.GetPost(id);

            if (post == null)
            {
                return new TrackbackErrorResult(0, "ID is invalid or missing");
            }

            if (Request.RequestType == "POST")
            {
                string requestBody;
                StreamReader sr = new StreamReader(Request.InputStream);

                requestBody = sr.ReadToEnd();

                if (requestBody.Contains(".info"))
                {
                    return Redirect("http://prisoner.iana.org");
                }
                else
                {
                    //TODO: (erikpo) Convert to use XLINQ
                    XmlDocument xdoc = new XmlDocument();
                    string sourceUrl = "";
                    string targetUrl = "";

                    try
                    {
                        xdoc.LoadXml(requestBody);

                        XmlNodeList paramList = xdoc.GetElementsByTagName("param");
                        if (paramList.Count == 2)
                        {
                            sourceUrl = paramList[0]["value"]["string"].InnerText;
                            targetUrl = paramList[1]["value"]["string"].InnerText;
                        }
                    }
                    catch (Exception err)
                    {
                        return new TrackbackErrorResult(0, err.Message);
                    }

                    if (targetUrl == "")
                    {
                        return new TrackbackErrorResult(32, "The specified target URI does not exist.");
                    }

                    if (targetUrl == sourceUrl)
                    {
                        return new TrackbackErrorResult(0,
                                                        "The source URI and the target URI cannot both point to the same resource.");
                    }

                    ITrackback trackback = TrackbackRepository.GetTrackback(post, sourceUrl);

                    try
                    {
                        if (trackback == null)
                        {
                            trackback = TrackbackRepository.CreateTrackback();

                            trackback.Post = post;
                            trackback.Title = "";
                            trackback.Body = "";
                            trackback.Url = sourceUrl;
                            trackback.BlogName = "";
                            trackback.Source = "";
                            trackback.Created = trackback.Modified = DateTime.Now.ToUniversalTime();

                            TrackbackRepository.AddTrackback(trackback);
                        }
                        else
                        {
                            trackback.IsTargetInSource = null;
                            trackback.Modified = DateTime.Now.ToUniversalTime();
                        }

                        TrackbackRepository.SubmitChanges();

                        return new PingbackSuccessResult(sourceUrl, targetUrl);
                    }
                    catch
                    {
                        return new TrackbackErrorResult(0, "Failed to save Pingback.");
                    }
                }
            }
            else
            {
                return new TrackbackErrorResult(0, "no POST data found, please try harder!");
            }
        }
    }
}