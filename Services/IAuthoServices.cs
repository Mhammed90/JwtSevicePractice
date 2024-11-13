

namespace JWTPractice.Services
{
    public interface IAuthoServices
    {

        Task<AuthoModel> RegisterModle(RegisterModelDto model);
        Task<AuthoModel> Login(LoginModelDto model);
        Task<string> AddRole(RoleModelDto model);
    }
}
