## Silvester.AspNetCore.Mvc.Hateoas
This library contains utilities and extensions for working with `Hypermedia As The Engine Of Application State` (`HATEOAS`) in `AspNetCore MVC`. In its current form it distinguishes three entities:
- HateoasLink
- HateoasResource
- HateoasCollection

## Dependency Injection
The `Silvester.AspNetCore.Mvc.Hateoas.DependencyInjection` namespace contains `IServiceCollection` extensions for injecting the `IHateoasService` and the required entity factories into your inversion-of-control container.
```
services.AddHateoas();
```

## ControllerBase Extensions
The `Silvester.AspNetCore.Mvc.Hateoas.Extensions` namespace contains `ControllerBase` extensions for working with HATEOAS resources and collections from an API controller.

### Resources
```
[HttpGet]
[Route("/users/{userId}", Name=nameof(GetUser))]
public IActionResult GetUser([FromRoute] Guid userId)
{
	User user = GetUserFromSomewhere(userId);
    return this.HateoasResource(user, hateoas => 
	{
		hateoas.Links.AddLink("self", nameof(GetUser), new {userId});
		hateoas.Links.AddLink("roles", nameof(GetRoles), new {userId});
	});
}
```

### Collection
```
[HttpGet]
[Route("/users", Name=nameof(GetUsers))]
public IActionResult GetUsers()
{
	IEnumerable<User> users = GetUsersFromSomewhere();
	return this.HateoasCollection(users, hateoas => 
	{
		hateoas.Collection.AddLink("self", nameof(GetUsers), (collection) => null);
		
		hateoas.Resources.AddLink("self", nameof(GetUser), (resource) => new {userId = resource.Id});
		hateoas.Resources.AddLink("roles", nameof(GetRoles), (resources) => new {userId = resource.Id});
	});
}
```