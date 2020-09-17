﻿using Microsoft.Extensions.DependencyInjection;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Collection;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Link;
using Silvester.AspNetCore.Mvc.Hateoas.Models.Resource;

namespace Silvester.AspNetCore.Mvc.Hateoas.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHateoas(this IServiceCollection services)
        {
            if(services != null)
            {
                services.AddScoped<IHateoasLinksBuilderFactory, HateoasLinksBuilderFactory>();
                services.AddScoped<IHateoasResourceBuilderFactory, HateoasResourceBuilderFactory>();
                services.AddScoped<IHateoasCollectionBuilderFactory, HateoasCollectionBuilderFactory>();
                services.AddScoped<IHateoasService, HateoasService>();
            }

            return services;
        }
    }
}
