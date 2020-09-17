using Microsoft.AspNetCore.Mvc;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Resource;
using System.Collections.Generic;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Collection
{
    public interface IHateoasCollectionBuilder<TResource>
    {
        IHateoasLinksBuilder<IEnumerable<TResource>> Collection { get; }
        IHateoasLinksBuilder<TResource> Resources { get; }

        IHateoasCollection<TResource> Build(IEnumerable<TResource> resources);
    }

    public class HateoasCollectionBuilder<TResource> : IHateoasCollectionBuilder<TResource>
    {
        private IHateoasResourceBuilderFactory ResourceBuilderFactory { get; }

        public IHateoasLinksBuilder<IEnumerable<TResource>> Collection { get; }

        public IHateoasLinksBuilder<TResource> Resources { get; }

        public HateoasCollectionBuilder(IHateoasLinksBuilderFactory collection, IHateoasLinksBuilderFactory resources, IHateoasResourceBuilderFactory resourceBuilderFactory)
        {
            ResourceBuilderFactory = resourceBuilderFactory;
            Collection = collection.Create<IEnumerable<TResource>>();
            Resources = resources.Create<TResource>();
        }

        public IHateoasCollection<TResource> Build(IEnumerable<TResource> resources)
        {
            return new HateoasCollection<TResource>(GetResources(resources), Collection.Build(resources));
        }

        private IEnumerable<IHateoasResource<TResource>> GetResources(IEnumerable<TResource> resources)
        {
            foreach (TResource resource in resources)
            {
                IHateoasResourceBuilder<TResource> builder = ResourceBuilderFactory.Create<TResource>();
                builder.Links.AddLinks(Resources.Build(resource));

                yield return builder.Build(resource);
            }
        }
    }
}
