using Microsoft.AspNetCore.Mvc;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Link.BuildDelegates;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Resource;
using System;
using System.Collections.Generic;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Collection
{
    public interface IHateoasCollectionBuilder<TResource, TEntity>
    {
        HateoasLinksBuilder Collection { get; }
        HateoasLinksBuilder<TResource, TEntity> Resources { get; }

        IHateoasCollection<TResource> Build(IEnumerable<TEntity> entities, Func<TEntity, TResource> transformation);
    }

    public class HateoasCollectionBuilder<TResource, TEntity> : IHateoasCollectionBuilder<TResource, TEntity>
    {
        private IHateoasResourceBuilderFactory ResourceBuilderFactory { get; }

        public HateoasLinksBuilder Collection { get; }

        public HateoasLinksBuilder<TResource, TEntity> Resources { get; }

        public HateoasCollectionBuilder(IHateoasLinksBuilderFactory collection, IHateoasLinksBuilderFactory resources, IHateoasResourceBuilderFactory resourceBuilderFactory)
        {
            ResourceBuilderFactory = resourceBuilderFactory;
            Collection = collection.Create();
            Resources = resources.Create<TResource, TEntity>();
        }

        public IHateoasCollection<TResource> Build(IEnumerable<TEntity> entities, Func<TEntity, TResource> transformation)
        {
            return new HateoasCollection<TResource>(GetResources(entities, transformation), Collection.Build());
        }

        private IEnumerable<IHateoasResource<TResource>> GetResources(IEnumerable<TEntity> entities, Func<TEntity, TResource> transformation)
        {
            foreach (TEntity entity in entities)
            {
                TResource resource = transformation.Invoke(entity);

                IHateoasResourceBuilder<TResource> builder = ResourceBuilderFactory.Create<TResource>();
                builder.Links.AddLinks(Resources.Build(resource, entity));

                yield return builder.Build(resource);
            }
        }
    }

    public interface IHateoasCollectionBuilder<TResource> : IHateoasCollectionBuilder<TResource, TResource>
    {
        IHateoasCollection<TResource> Build(IEnumerable<TResource> resources);
    }

    public class HateoasCollectionBuilder<TResource> : HateoasCollectionBuilder<TResource, TResource>, IHateoasCollectionBuilder<TResource>
    {
        public HateoasCollectionBuilder(IHateoasLinksBuilderFactory collection, IHateoasLinksBuilderFactory resources, IHateoasResourceBuilderFactory resourceBuilderFactory)
            : base(collection, resources, resourceBuilderFactory)
        {

        }

        public IHateoasCollection<TResource> Build(IEnumerable<TResource> resources)
        {
            return Build(resources, (id) => id);
        }
    }
}
