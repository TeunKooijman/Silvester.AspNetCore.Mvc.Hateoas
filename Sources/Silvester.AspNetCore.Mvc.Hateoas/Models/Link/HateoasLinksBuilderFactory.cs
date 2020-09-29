using Microsoft.Extensions.DependencyInjection;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Link.BuildDelegates;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Link
{
    public interface IHateoasLinksBuilderFactory
    {
        HateoasLinksBuilder Create();
        HateoasLinksBuilder<TResource> Create<TResource>();
        HateoasLinksBuilder<TResource, TEntity> Create<TResource, TEntity>();
    }

    public class HateoasLinksBuilderFactory : IHateoasLinksBuilderFactory
    {
        private IServiceProvider Services { get; }

        public HateoasLinksBuilderFactory(IServiceProvider services)
        {
            Services = services;
        }

        public HateoasLinksBuilder Create()
        {
            return ActivatorUtilities.CreateInstance<HateoasLinksBuilder>(Services);
        }

        public HateoasLinksBuilder<TResource> Create<TResource>()
        {
            return ActivatorUtilities.CreateInstance<HateoasLinksBuilder<TResource>>(Services);
        }

        public HateoasLinksBuilder<TResource, TEntity> Create<TResource, TEntity>()
        {
            return ActivatorUtilities.CreateInstance<HateoasLinksBuilder<TResource, TEntity>>(Services);
        }
    }
}
