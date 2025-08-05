using MoviesApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
public class UserRepository:IUserRepository
{
    protected readonly CinemaDbContext _context;
    public async Task<IResult> GetUserRole(string id)
    {
      
      var roles = await _context.Roles
            .FromSqlInterpolated($@"
                SELECT r.*
                FROM AspNetRoles r
                INNER JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
                WHERE ur.UserId = {id}")
            .Select(r => r.Name)
            .ToListAsync();
        if (roles == null)
        {
            return TypedResults.NotFound($"There is no role for the user Id {id}");
        }

        return TypedResults.Ok(roles);
    } 
}