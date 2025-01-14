using FalconSoftChallenge.DAL;

namespace FalconSoftChallenge.Business.Services
{
    public abstract class BaseService
    {
        protected FalconSoftDbContext _context { get; init; }

        protected BaseService(FalconSoftDbContext context)
        {
            _context = context;   
        }
    }
}
