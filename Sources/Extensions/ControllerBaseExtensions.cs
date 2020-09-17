using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Collection;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Resource;
using System;
using System.Collections.Generic;

namespace Silvester.AspNetCore.Mvc.Hateoas.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult HateoasResource<TResource>(this ControllerBase controller, TResource resource, Action<IHateoasResourceBuilder<TResource>> buildAction)
        {
            IHateoasService service = controller.HttpContext.RequestServices.GetRequiredService<IHateoasService>();
            return controller.Ok(service.CreateResource(resource, buildAction));
        }

        public static IActionResult HateoasCollection<TResource>(this ControllerBase controller, IEnumerable<TResource> resources, Action<IHateoasCollectionBuilder<TResource>> buildAction)
        {
            IHateoasService service = controller.HttpContext.RequestServices.GetRequiredService<IHateoasService>();
            return controller.Ok(service.CreateCollection(resources, buildAction));
        }
    }
}
