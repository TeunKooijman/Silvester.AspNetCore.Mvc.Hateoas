using Microsoft.AspNetCore.Mvc;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Utilities.Links
{
    public interface IUriBuilder
    {
        Uri CreateUri(string routeName, object routeParams = null);
    }

    public class UriBuilder : IUriBuilder
    {
        private IUrlHelper UrlHelper { get; }

        public UriBuilder(IUrlHelper urlHelper)
        {
            UrlHelper = urlHelper;
        }

        public Uri CreateUri(string routeName, object routeParams = null)
        {
            string href = UrlHelper.Link(routeName, routeParams);
            return new Uri(href, UriKind.Relative);
        }
    }
}
