using Microsoft.Extensions.DependencyInjection;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Link
{
    public interface IHateoasLinksBuilderFactory
    {
        IHateoasLinksBuilder<TResource> Create<TResource>();
    }

    public class HateoasLinksBuilderFactory : IHateoasLinksBuilderFactory
    {
        private IServiceProvider Services { get; }

        public HateoasLinksBuilderFactory(IServiceProvider services)
        {
            Services = services;
        }

        public IHateoasLinksBuilder<TResource> Create<TResource>()
        {
            return ActivatorUtilities.CreateInstance<IHateoasLinksBuilder<TResource>>(Services);
        }
    }
}
