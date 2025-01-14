using FalconSoftChallenge.Business.DTO;

namespace FalconSoftChallenge.Business.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetByEmailAndPassword(string email, string password);
    }
}
