using FalconSoftChallenge.Business.DTO;
using FalconSoftChallenge.Business.Interfaces;
using FalconSoftChallenge.DAL;
using Microsoft.EntityFrameworkCore;

namespace FalconSoftChallenge.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(FalconSoftDbContext context) : base(context) { }
       
        public async Task<UserDTO> GetByEmailAndPassword(string email, string password)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            var user = await _context
                .Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Password == password);

            if (user == null) throw new UnauthorizedAccessException("User not found");

            return new UserDTO(user.Id, user.Email, user.Password);
        }
    }
}
