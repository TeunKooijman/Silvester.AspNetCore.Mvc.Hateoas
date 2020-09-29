using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Link.BuildDelegates;
using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Resource
{
    public interface IHateoasResourceBuilder<TResource, TEntity>
    {
        HateoasLinksBuilder<TResource, TEntity> Links { get; }

        IHateoasResource<TResource> Build(TEntity entity, Func<TEntity, TResource> transformation);
    }

    public class HateoasResourceBuilder<TResource, TEntity> : IHateoasResourceBuilder<TResource, TEntity>
    {
        public HateoasLinksBuilder<TResource, TEntity> Links { get; }

        public HateoasResourceBuilder(IHateoasLinksBuilderFactory factory)
        {
            Links = factory.Create<TResource, TEntity>();
        }

        public IHateoasResource<TResource> Build(TEntity entity, Func<TEntity, TResource> transformation)
        {
            TResource resource = transformation.Invoke(entity);
            return new HateoasResource<TResource>(resource, Links.Build(resource, entity));
        }
    }

    public interface IHateoasResourceBuilder<TResource> : IHateoasResourceBuilder<TResource, TResource>
    {
        IHateoasResource<TResource> Build(TResource resource);
    }

    public class HateoasResourceBuilder<TResource> : HateoasResourceBuilder<TResource, TResource>, IHateoasResourceBuilder<TResource>
    {
        public HateoasResourceBuilder(IHateoasLinksBuilderFactory factory)
            : base(factory)
        {

        }

        public IHateoasResource<TResource> Build(TResource resource)
        {
            return Build(resource, (id) => id);
        }
    }
}
