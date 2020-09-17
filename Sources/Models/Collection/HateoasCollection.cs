using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Resource;
using System.Collections.Generic;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Collection
{
    public interface IHateoasCollection<TResource>
    {
        IDictionary<string, HateoasLink> Links { get; set; }
        IEnumerable<IHateoasResource<TResource>> Resources { get; set; }
    }

    public class HateoasCollection<TResource> : IHateoasCollection<TResource>
    {
        public IEnumerable<IHateoasResource<TResource>> Resources { get; set; }

        public IDictionary<string, HateoasLink> Links { get; set; }

        public HateoasCollection(IEnumerable<IHateoasResource<TResource>> resources, IDictionary<string, HateoasLink> links)
        {
            Resources = resources;
            Links = links;
        }
    }
}
