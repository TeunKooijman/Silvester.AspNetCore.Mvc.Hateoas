using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Resource
{
    public interface IHateoasResourceBuilder<TResource>
    {
        IHateoasLinksBuilder<TResource> Links { get; }

        IHateoasResource<TResource> Build(TResource resource);
    }

    public class HateoasResourceBuilder<TResource> : IHateoasResourceBuilder<TResource>
    {
        public IHateoasLinksBuilder<TResource> Links { get; }

        public HateoasResourceBuilder(IHateoasLinksBuilderFactory factory)
        {
            Links = factory.Create<TResource>();
        }

        public IHateoasResource<TResource> Build(TResource resource)
        {
            return new HateoasResource<TResource>(resource, Links.Build(resource));
        }
    }
}
