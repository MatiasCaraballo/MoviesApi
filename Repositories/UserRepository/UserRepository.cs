using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
public class UserRepository:IUserRepository
{
    protected readonly CinemaDbContext _context;
    public async Task<IResult> GetUserRole(string id)
    {
        var roles = await (from r in _context.Roles
                           join ur in _context.UserRoles on r.Id equals ur.RoleId
                           where ur.UserId == id
                           select r.Name).ToListAsync();
        if (roles == null)
        {
            return TypedResults.NotFound($"There is no role for the user Id {id}");
        }

        return TypedResults.Ok(roles);
    } 
}