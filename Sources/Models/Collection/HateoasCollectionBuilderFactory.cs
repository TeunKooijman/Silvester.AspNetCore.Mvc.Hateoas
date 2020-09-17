

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Collection
{
    public interface IHateoasCollectionBuilderFactory
    {
        IHateoasCollectionBuilder<TResource> Create<TResource>();
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
    }
}
