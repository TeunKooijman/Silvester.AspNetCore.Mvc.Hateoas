using Microsoft.Extensions.DependencyInjection;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Collection
{
    public interface IHateoasCollectionBuilderFactory
    {
        IHateoasCollectionBuilder<TResource> Create<TResource>();
        IHateoasCollectionBuilder<TResource, TEntity> Create<TResource, TEntity>();
    }

    public class HateoasCollectionBuilderFactory : IHateoasCollectionBuilderFactory
    {
        private IServiceProvider Services { get; }

        public HateoasCollectionBuilderFactory(IServiceProvider services)
        {
            Services = services;
        }

        public IHateoasCollectionBuilder<TResource> Create<TResource>()
        {
            return ActivatorUtilities.CreateInstance<IHateoasCollectionBuilder<TResource>>(Services);
        }

        public IHateoasCollectionBuilder<TResource, TEntity> Create<TResource, TEntity>()
        {
            return ActivatorUtilities.CreateInstance<IHateoasCollectionBuilder<TResource, TEntity>>(Services);
        }
    }
}
