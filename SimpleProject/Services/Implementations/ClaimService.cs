using SimpleProject.Models.Identity;
using SimpleProject.Services.Interfaces;
using SimpleProject.UnitOfWorks;

namespace SimpleProject.Services.Implementations
{
    public class ClaimService : IClaimService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region constructors
        public ClaimService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #endregion
        #region Handle Functions
        public async Task<string> AddClaimAsync(Claim claim)
        {
            try
            {
                await _unitOfWork.Repository<Claim>().AddAsync(claim);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "  --  "+ex.InnerException!.Message;
            }

        }

        public async Task<string> DeleteClaimAsync(Claim claim)
        {
            try
            {
                await _unitOfWork.Repository<Claim>().Deletesync(claim);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "  --  "+ex.InnerException!.Message;
            }
        }

        public async Task<List<Claim>> GetClaimsAsync()
        {
            return await _unitOfWork.Repository<Claim>().GetListAsync();
        }

        public async Task<string> UpdateClaimAsync(Claim claim)
        {
            try
            {
                await _unitOfWork.Repository<Claim>().Updatesync(claim);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "  --  "+ex.InnerException!.Message;
            }
        }
        #endregion
    }
}
