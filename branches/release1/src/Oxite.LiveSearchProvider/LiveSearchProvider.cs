//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using Oxite.Data;
using Oxite.Search.Live.Search.Soap;

namespace Oxite.Search
{
    public class LiveSearchProvider : ISearchProvider
    {
        #region ISearchProvider Members

        public PageOfAList<ISearchResultItem> GetSearchResults(string term, int page, int count,
                                                               NameValueCollection properties)
        {
            MSNSearchService searchService = new MSNSearchService();
            SearchRequest searchRequest = new SearchRequest();

            List<SourceRequest> sourceRequests = new List<SourceRequest>()
                                                 {
                                                     new SourceRequest()
                                                     {
                                                         Source = SourceType.Spelling,
                                                         ResultFields = ResultFieldMask.Title
                                                     },
                                                     new SourceRequest()
                                                     {
                                                         Source = SourceType.Web,
                                                         ResultFields =
                                                             ResultFieldMask.Title | ResultFieldMask.Url |
                                                             ResultFieldMask.DisplayUrl | ResultFieldMask.Description,
                                                         Offset = page * count,
                                                         Count = count * 2
                                                     }
                                                 };

            searchRequest.Query = string.Format("site:{0} {1}", properties["siteDomainName"], term);
            searchRequest.Requests = sourceRequests.ToArray();

            searchRequest.AppID = properties["appID"];
            searchRequest.CultureInfo = properties["cultureInfo"];
            searchService.Timeout = int.Parse(properties["timeout"]);

            SearchResponse searchResponse = searchService.Search(searchRequest);
            List<string> suggestions = new List<string>(10);
            List<LiveSearchResultItem> results = new List<LiveSearchResultItem>(count);

            // spelling
            foreach (Result spellingResult in searchResponse.Responses[0].Results)
            {
                suggestions.Add(spellingResult.Title);
            }

            // web
            int retrieveCount = 0;
            foreach (Result webResult in searchResponse.Responses[1].Results)
            {
                if (retrieveCount++ >= count)
                {
                    break;
                }

                results.Add(
                    new LiveSearchResultItem()
                    {
                        Title = webResult.Title,
                        Permalink = webResult.Url,
                        DisplayPermalink = webResult.DisplayUrl,
                        BodyShort = webResult.Description
                    }
                    );
            }

            return new LiveSearchResults(results, suggestions, page, count, searchResponse.Responses[1].Total);
        }

        #endregion
    }
}