using Silvester.AspNetCore.Mvc.Hateoas.Models.Collection;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Resource;
using System;
using System.Collections.Generic;

namespace Silvester.AspNetCore.Mvc.Hateoas
{
    public interface IHateoasService
    {
        IHateoasResource<TResource> CreateResource<TResource>(TResource resource, Action<IHateoasResourceBuilder<TResource>> buildAction);
        IHateoasCollection<TResource> CreateCollection<TResource>(IEnumerable<TResource> resources, Action<IHateoasCollectionBuilder<TResource>> buildAction);
    }

    public class HateoasService : IHateoasService
    {
        private IHateoasResourceBuilderFactory ResourceBuilderFactory { get; }

        private IHateoasCollectionBuilderFactory CollectionBuilderFactory { get; }

        public HateoasService(IHateoasResourceBuilderFactory resourceBuilderFactory, IHateoasCollectionBuilderFactory collectionBuilderFactory)
        {
            ResourceBuilderFactory = resourceBuilderFactory;
            CollectionBuilderFactory = collectionBuilderFactory;
        }

        public IHateoasResource<TResource> CreateResource<TResource>(TResource resource, Action<IHateoasResourceBuilder<TResource>> buildAction)
        {
            IHateoasResourceBuilder<TResource> builder = ResourceBuilderFactory.Create<TResource>();
            buildAction.Invoke(builder);

            return builder.Build(resource);
        }

        public IHateoasResource<TResource> CreateResource<TResource, TEntity>(TEntity entity, Func<TEntity, TResource> transformation, Action<IHateoasResourceBuilder<TResource, TEntity>> buildAction)
        {
            IHateoasResourceBuilder<TResource, TEntity> builder = ResourceBuilderFactory.Create<TResource, TEntity>();
            buildAction.Invoke(builder);

            return builder.Build(entity, transformation);
        }

        public IHateoasCollection<TResource> CreateCollection<TResource>(IEnumerable<TResource> resources, Action<IHateoasCollectionBuilder<TResource>> buildAction)
        {
            IHateoasCollectionBuilder<TResource> builder = CollectionBuilderFactory.Create<TResource>();
            buildAction.Invoke(builder);

            return builder.Build(resources);
        }

        public IHateoasCollection<TResource> CreateCollection<TResource, TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TResource> transformation, Action<IHateoasCollectionBuilder<TResource, TEntity>> buildAction)
        {
            IHateoasCollectionBuilder<TResource, TEntity> builder = CollectionBuilderFactory.Create<TResource, TEntity>();
            buildAction.Invoke(builder);

            return builder.Build(entities, transformation);
        }
    }
}
