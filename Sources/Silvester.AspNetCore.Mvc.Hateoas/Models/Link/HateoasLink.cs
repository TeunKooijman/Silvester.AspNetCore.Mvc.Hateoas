using System;

namespace Silvester.AspNetCore.Mvc.Hateoas.Models.Link
{
    public interface IHateoasLink
    {
        Uri Href { get; set; }
    }

    public class HateoasLink : IHateoasLink
    {
        public Uri Href { get; set; }

        public HateoasLink(Uri href)
        {
            Href = href;
        }
    }
}
