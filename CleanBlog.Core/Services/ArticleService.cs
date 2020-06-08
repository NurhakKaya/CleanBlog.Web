using CleanBlog.Core.Helpers;
using CleanBlog.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace CleanBlog.Core.Services
{
    public class ArticleService : IArticleService
    {
        public IPublishedContent GetArticleListPage(IPublishedContent siteRoot)
        {
            return siteRoot.Descendants().Where(x => x.ContentType.Alias == "articleList").FirstOrDefault();
        }

        public IEnumerable<IPublishedContent> GetLatestArticles(IPublishedContent siteRoot)
        {
            var articleList = GetArticleListPage(siteRoot);

            return articleList.Descendants()
                .Where(x => x.ContentType.Alias == "article" && x.IsVisible())
                .OrderByDescending(y => y.Value<DateTime>("articleDate"));
        }

        public ArticleResultSet GetLatestArticles(IPublishedContent currentContntentItem, HttpRequestBase request)
        {
            // TODO: Do some null checks

            var siteRoot = currentContntentItem.Root();
            var articleList = GetArticleListPage(siteRoot);
            var articles = GetLatestArticles(siteRoot);

            var isArticleListPage = articleList.Id == currentContntentItem.Id;
            var fallbackPageSize = isArticleListPage ? 10 : 3;

            var pageNumber = QueryStringHelper.GetIntFromQueryString(request, "page", 1);
            var pageSize = QueryStringHelper.GetIntFromQueryString(request, "size", fallbackPageSize);

            var pageOfArticles = articles.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var totalItemCount = articles.Count();
            var pageCount = totalItemCount > 0 ? Math.Ceiling((double)totalItemCount / pageSize) : 1;

            return new ArticleResultSet()
            {
                PageCount = pageCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Results = pageOfArticles,
                IsArticleListPage=isArticleListPage
            };
        }
    }
}
