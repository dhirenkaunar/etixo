using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Web.Routing;
using Oxite.Mvc.Tests.Fakes;
using System.Collections.Specialized;
using System.Web.Mvc;
using MvcFakes;
using System.Web;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexFillsViewData()
        {
            FakeSite site = new FakeSite();
            RouteCollection routes = new RouteCollection();
            routes.Add("HomeRsd", new Route("Rsd", null));
            routes.Add("AllCommentsAtom", new Route("AllCommentsAtom", null));
            routes.Add("AllCommentsRss", new Route("AllCommentsRss", null));

            FakePostRepository postRepository = new FakePostRepository();
            for (int x = 0; x < 20; x++)
            {
                postRepository.Posts.Add(new FakePost() { ID = Guid.NewGuid() });
            }

            FakeAreaRepository areaRepository = new FakeAreaRepository(new FakeArea(), new FakeArea());
            
            HomeController controller = new HomeController(routes, 
                new FakeConfig(site), 
                new NameValueCollection(), 
                areaRepository, 
                new FakeMembershipRepository(), 
                postRepository, 
                new FakeResourceRepository());

            FakeHttpContext rawContext = new FakeHttpContext(new Uri("http://test"), "~/");
            FakeHttpContextWrapper context = new FakeHttpContextWrapper(rawContext);

            RequestContext reqContext = new RequestContext(context, new RouteData());
            controller.ControllerContext = new ControllerContext(reqContext, controller);
            controller.Url = new UrlHelper(reqContext, routes);

            ViewResult result = controller.Index(1) as ViewResult;

            Assert.Equal(2, result.ViewData["AreaCount"]);

            Assert.NotNull(result.ViewData["Posts"]);
            Assert.IsAssignableFrom<IPageOfAList<IPost>>(result.ViewData["Posts"]);
            IPageOfAList<IPost> posts = result.ViewData["Posts"] as IPageOfAList<IPost>;
            Assert.Equal(10, posts.Count);
            Assert.Equal(0, posts.PageIndex);
            Assert.Equal(10, posts.PageSize);
            Assert.Equal(20, posts.TotalItemCount);

            Assert.NotNull(result.ViewData["PostCounts"]);
            Assert.IsType<Dictionary<Guid, int>>(result.ViewData["PostCounts"]);
            Dictionary<Guid, int> postCounts = result.ViewData["PostCounts"] as Dictionary<Guid, int>;
            Assert.Equal(10, postCounts.Count);
            foreach (var postCount in postCounts)
                Assert.Equal(2, postCount.Value);

            Assert.IsAssignableFrom<IEnumerable<KeyValuePair<DateTime, int>>>(result.ViewData["Months"]);
            IEnumerable<KeyValuePair<DateTime, int>> months = result.ViewData["Months"] as IEnumerable<KeyValuePair<DateTime, int>>;
            Assert.Equal(0, months.Count());
            Assert.Equal("http://test/Rsd", result.ViewData["RsdLink"]);
        }
    }
}
