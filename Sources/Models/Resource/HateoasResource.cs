using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;
using System.Collections.Generic;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Resource
{
    public interface IHateoasResource<TResource>
    {
        IDictionary<string, HateoasLink> Links { get; set; }
        TResource Resource { get; set; }
    }

    public class HateoasResource<TResource> : IHateoasResource<TResource>
    {
        public TResource Resource { get; set; }

        public IDictionary<string, HateoasLink> Links { get; set; }

        public HateoasResource(TResource resource, IDictionary<string, HateoasLink> links)
        {
            Resource = resource;
            Links = links;
        }
    }
}
