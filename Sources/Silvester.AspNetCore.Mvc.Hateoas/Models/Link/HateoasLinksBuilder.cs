using Silvester.AspNetCore.Mvc.Hateoas.Utilities.Links;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Link.BuildDelegates
{
    public interface IHateoasLinksBuilder<TResource, TEntity>
    {
        IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, HateoasLink link);
        IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, string routeName, Func<TResource, object> routeParametersDelegate);
        IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, string routeName, Func<TResource, TEntity, object> routeParametersDelegate);
        IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, string routeName, object routeParams);
        IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, Uri uri);
        IHateoasLinksBuilder<TResource, TEntity> AddLinks(IDictionary<string, HateoasLink> links);

        IDictionary<string, HateoasLink> Build(TResource resource, TEntity entity);
    }

    public class HateoasLinksBuilder<TResource, TEntity> : IHateoasLinksBuilder<TResource, TEntity>
    {
        private IUriBuilder UriBuilder { get; }

        private IDictionary<string, Func<TResource, TEntity, HateoasLink>> Transformations { get; }

        private IDictionary<string, Func<TResource, HateoasLink>> Lambdas { get; }

        private IDictionary<string, HateoasLink> Links { get; }

        public HateoasLinksBuilder(IUriBuilder uriBuilder)
        {
            Transformations = new Dictionary<string, Func<TResource, TEntity, HateoasLink>>();
            Lambdas = new Dictionary<string, Func<TResource, HateoasLink>>();
            Links = new Dictionary<string, HateoasLink>();

            UriBuilder = uriBuilder;
        }

        public virtual IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, string routeName, Func<TResource, object> routeParametersDelegate)
        {
            Lambdas.Add(reference, (e) => new HateoasLink(UriBuilder.CreateUri(routeName, routeParametersDelegate.Invoke(e))));
            return this;
        }

        public virtual IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, string routeName, Func<TResource, TEntity, object> routeParametersDelegate)
        {
            Transformations.Add(reference, (resource, entity) => new HateoasLink(UriBuilder.CreateUri(routeName, routeParametersDelegate.Invoke(resource, entity))));
            return this;
        }

        public virtual IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, string routeName, object routeParams)
        {
            Links.Add(reference, new HateoasLink(UriBuilder.CreateUri(routeName, routeParams)));
            return this;
        }

        public virtual IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, Uri uri)
        {
            return AddLink(reference, new HateoasLink(uri));
        }

        public virtual IHateoasLinksBuilder<TResource, TEntity> AddLink(string reference, HateoasLink link)
        {
            Links.Add(reference, link);
            return this;
        }

        public virtual IHateoasLinksBuilder<TResource, TEntity> AddLinks(IDictionary<string, HateoasLink> links)
        {
            foreach (KeyValuePair<string, HateoasLink> pair in links)
            {
                AddLink(pair.Key, pair.Value);
            }

            return this;
        }

        public IDictionary<string, HateoasLink> Build(TResource resource, TEntity entity)
        {
            return BuildInternal(resource, entity)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        protected virtual IEnumerable<KeyValuePair<string, HateoasLink>> BuildInternal(TResource resource, TEntity entity)
        {
            foreach (KeyValuePair<string, Func<TResource, TEntity, HateoasLink>> pair in Transformations)
            {
                yield return new KeyValuePair<string, HateoasLink>(pair.Key, pair.Value.Invoke(resource, entity));
            }

            foreach (KeyValuePair<string, Func<TResource, HateoasLink>> pair in Lambdas)
            {
                yield return new KeyValuePair<string, HateoasLink>(pair.Key, pair.Value.Invoke(resource));
            }

            foreach (KeyValuePair<string, HateoasLink> link in Links)
            {
                yield return link;
            }
        }
    }

    public interface IHateoasLinksBuilder<TResource> : IHateoasLinksBuilder<TResource, TResource>
    {
        IDictionary<string, HateoasLink> Build(TResource resource);
    }

    public class HateoasLinksBuilder<TResource> : HateoasLinksBuilder<TResource, TResource>, IHateoasLinksBuilder<TResource>
    {
        public HateoasLinksBuilder(IUriBuilder uriBuilder)
            : base(uriBuilder)
        {

        }

        public IDictionary<string, HateoasLink> Build(TResource resource)
        {
            return Build(resource, resource);
        }
    }

    public interface IHateoasLinksBuilder : IHateoasLinksBuilder<object>
    {
        IDictionary<string, HateoasLink> Build();
    }

    public class HateoasLinksBuilder : HateoasLinksBuilder<object>, IHateoasLinksBuilder
    {
        public HateoasLinksBuilder(IUriBuilder uriBuilder)
            : base(uriBuilder)
        {

        }

        public IDictionary<string, HateoasLink> Build()
        {
            return Build(null);
        }
    }
}
