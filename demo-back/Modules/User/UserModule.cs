
namespace back.Core;
public class UserModule : IModule
{
    public IServiceCollection RegisterModules(IServiceCollection services)
    {

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users", () => { return User.GET(); }).WithTags("User");
       
        endpoints.MapGet("/api/users/{id}", (Guid id) => { return User.GET(id); }).WithTags("User");
       
        endpoints.MapPost("/api/users", (Models.User oUser) => { return User.POST(oUser); }).WithTags("User");
       
        endpoints.MapPut("/api/users/{id}", (Guid id, Models.User oUser) => { return User.PUT(id, oUser); }).WithTags("User");
        endpoints.MapDelete("/api/users/{id}", (Guid id) => { return User.DELETE(id); }).WithTags("User");

      
        endpoints.MapPost("/api/roles", (Models.Role oRole) => { return Role.POST(oRole); }).WithTags("Role");
        endpoints.MapGet("/api/roles", () => { return Role.GET(); })/**.RequireAuthorization("Nuevo_Ingreso")*/.WithTags("Role");
        endpoints.MapGet("/api/roles/{id}", (int id) => { return Role.GET(id); }).WithTags("Role");
        endpoints.MapPut("/api/roles/{id}", (int id, Models.Role oRole) => { return Role.PUT(id, oRole); }).WithTags("Role");
        endpoints.MapDelete("/api/roles/{id}", (int id) => { return Role.DELETE(id); }).WithTags("Role");


        return endpoints;
    }
}