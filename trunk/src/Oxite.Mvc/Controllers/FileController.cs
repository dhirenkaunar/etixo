//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class FileController : BaseController
    {
        public FileController()
        {
            Routes = RouteTable.Routes;
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AppSettings = new AppSettingsHelper(ConfigurationManager.AppSettings);
            PageTitle = new PageTitleHelper(Config);

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();
            AreaRepository = dataProvider.AreaRepository;
            MembershipRepository = dataProvider.MembershipRepository;
            PostRepository = dataProvider.PostRepository;
            ResourceRepository = dataProvider.ResourceRepository;
        }

        public FileController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                              IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                              IPostRepository postRepository, IResourceRepository resourceRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
        }

        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult UserFile(string username, string filePath)
        {
            IUser user = MembershipRepository.GetUser(username);
            IFileResource fileResource;

            if (user == null)
            {
                return NotFound();
            }

            fileResource = ResourceRepository.GetFile(Config.Site.ID, user.ID, filePath);

            if (fileResource == null)
            {
                return NotFound();
            }

            return File(fileResource);
        }
    }
}