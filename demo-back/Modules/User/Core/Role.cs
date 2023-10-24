namespace back.Core;
public static class Role
{
    static ExampleContext dbContext = new();
     public static Array GET()
    {
        dbContext = new ();
        List<Models.Role> roles = dbContext.Roles.ToList();
        return roles.ToArray();
    }

   public static Array GET(int id)
    {
         dbContext = new ();
        List<Models.Role> roles = dbContext.Roles.ToList();
        return roles.Where(i => i.Id == id).ToArray();
    }
    public static IResult POST(Models.Role oRole)
    {
        dbContext.Roles.Add(oRole);
        dbContext.SaveChanges();
        return Results.Created($"/roles/{oRole.Id}", oRole);
    }

    public static IResult PUT(int id, Models.Role oRole)
    {
        var roles = dbContext.Roles.Find(id);
        if (roles is null) return Results.NotFound();
        roles.Title = oRole.Title;
        roles.Updated_at = DateTime.UtcNow;



        dbContext.SaveChanges();
        return Results.NoContent();
    }

    public static IResult DELETE(int id)
    {
        if (dbContext.Roles.Find(id) is Models.Role roles)
        {
            dbContext.Roles.Remove(roles);
            dbContext.SaveChanges();
            return Results.Ok(roles);
        }
        return Results.NotFound();
    }
}