using Microsoft.Extensions.DependencyInjection;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Resource
{
    public interface IHateoasResourceBuilderFactory
    {
        IHateoasResourceBuilder<TResource> Create<TResource>();
    }

    public class HateoasResourceBuilderFactory : IHateoasResourceBuilderFactory
    {
        private IServiceProvider Services { get; }

        public HateoasResourceBuilderFactory(IServiceProvider services)
        {
            Services = services;
        }

        public IHateoasResourceBuilder<TResource> Create<TResource>()
        {
            return ActivatorUtilities.CreateInstance<HateoasResourceBuilder<TResource>>(Services);
        }
    }
}
