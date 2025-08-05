public interface IUserRepository
{
    Task<IResult> GetUserRole(string id);
}