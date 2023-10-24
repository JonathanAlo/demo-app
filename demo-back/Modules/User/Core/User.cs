
using back.Models;

namespace back.Core;
public static class User
{
    private static readonly TimeSpan ExpirationTime = TimeSpan.FromHours(24);


    static ExampleContext dbContext = new();
    public static Array GET()
    {
        dbContext = new();
        var users = from us in dbContext.Users

                    join r in dbContext.Roles on us.Id_role equals r.Id
                 

                    select new
                    {
                        us.Id,
                        us.Firstname,
                        us.Lastname,
                        us.Email,
                        us.Password,
                        us.State,
                        us.Status,
                        us.PhoneNumber,
                        us.Id_role,
                        Role = r.Title,
                       
                        us.Created_at,
                        us.Updated_at
                    };

        return users.ToArray();
    }


    public static Array GET(Guid id)
    {
        dbContext = new();
        var users = from us in dbContext.Users
                    where us.Id == id
                    join r in dbContext.Roles on us.Id_role equals r.Id
                    

                    select new
                    {
                        us.Id,
                        us.Firstname,
                        us.Lastname,
                        us.Email,
                        us.Password,
                        us.State,
                        us.Status,
                        us.PhoneNumber,
                        us.Id_role,
                        Role = r.Title,
                       
                        us.Created_at,
                        us.Updated_at
                    };

        return users.ToArray();
    }

    public static async Task<IResult> POST(Models.User oUser)
    {
        
        try
        {
            bool userAlready = dbContext.Users.Any(i => i.Email == oUser.Email);
            if (userAlready)
            {
                return Results.NotFound("El email ya esta en uso ");
            }
            else
            {
                await dbContext.Users.AddAsync(oUser);
                await dbContext.SaveChangesAsync();
              return Results.Created($"/users/{oUser.Id}", oUser);
            }
        }
        catch (System.Exception ex)
        {
            return Results.BadRequest("Error al registrar el usuario" + ex);
        }
    }
       public static IResult PUT(Guid id, Models.User oUser)
    {


        var user = dbContext.Users.Find(id);
        if (user is null) return Results.NotFound();
        user.Firstname = oUser.Firstname;
        user.Lastname = oUser.Lastname;
        user.Email = oUser.Email;
        user.Password = oUser.Password;
        user.PhoneNumber = oUser.PhoneNumber;
        user.State = oUser.State;
        user.Status = oUser.Status;
        user.Id_role = oUser.Id_role;
      
        user.Updated_at = DateTime.UtcNow;
        dbContext.SaveChanges();
        return Results.NoContent();
    }
    public static IResult DELETE(Guid id)
    {
        if (dbContext.Users.Find(id) is Models.User user)
        {
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return Results.Ok(user);
        }
        return Results.NotFound();
    }


   
}