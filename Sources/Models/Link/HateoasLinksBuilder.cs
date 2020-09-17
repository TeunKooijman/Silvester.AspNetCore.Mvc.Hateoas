using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Link
{
    public interface IHateoasLinksBuilder<TResource>
    {
        IHateoasLinksBuilder<TResource> AddLink(string reference, Uri uri);
        IHateoasLinksBuilder<TResource> AddLink(string reference, HateoasLink link);
        IHateoasLinksBuilder<TResource> AddLink(string reference, string routeName, object routeParams);
        IHateoasLinksBuilder<TResource> AddLink(string reference, string routeName, Func<TResource, object> routeParametersDelegate);
        IHateoasLinksBuilder<TResource> AddLinks(IDictionary<string, HateoasLink> links);

        IDictionary<string, HateoasLink> Build(TResource resource);
    }

    public class HateoasLinksBuilder<TResource> : IHateoasLinksBuilder<TResource>
    {
        private Dictionary<string, Func<TResource, HateoasLink>> FactoryDelegates { get; }

        private IUrlHelper UrlHelper { get; }

        public HateoasLinksBuilder(IUrlHelper urlHelper)
        {
            UrlHelper = urlHelper;
            FactoryDelegates = new Dictionary<string, Func<TResource, HateoasLink>>();
        }

        public IHateoasLinksBuilder<TResource> AddLink(string reference, string routeName, Func<TResource, object> routeParametersDelegate)
        {
            FactoryDelegates.Add(reference, (e) => new HateoasLink(CreateUri(routeName, routeParametersDelegate.Invoke(e))));
            return this;
        }

        public IHateoasLinksBuilder<TResource> AddLink(string reference, string routeName, object routeParams)
        {
            FactoryDelegates.Add(reference, (e) => new HateoasLink(CreateUri(routeName, routeParams)));
            return this;
        }

        public IHateoasLinksBuilder<TResource> AddLink(string reference, Uri uri)
        {
            return AddLink(reference, new HateoasLink(uri));
        }
        
        public IHateoasLinksBuilder<TResource> AddLink(string reference, HateoasLink link)
        {
            FactoryDelegates.Add(reference, (e) => link);
            return this;
        }

        public IHateoasLinksBuilder<TResource> AddLinks(IDictionary<string, HateoasLink> links)
        {
            foreach(KeyValuePair<string, HateoasLink> pair in links)
            {
                AddLink(pair.Key, pair.Value);
            }

            return this;
        }

        public IDictionary<string, HateoasLink> Build(TResource resource)
        {
            return BuildInternal(resource).ToDictionary(p => p.Key, p => p.Value);
        }

        private IEnumerable<KeyValuePair<string, HateoasLink>> BuildInternal(TResource resource)
        {
            foreach (KeyValuePair<string, Func<TResource, HateoasLink>> pair in FactoryDelegates)
            {
                yield return new KeyValuePair<string, HateoasLink>(pair.Key, pair.Value.Invoke(resource));
            }
        }

        private Uri CreateUri(string routeName, object routeParams = null)
        {
            string href = UrlHelper.Link(routeName, routeParams);
            return new Uri(href, UriKind.Relative);
        }
    }
}
