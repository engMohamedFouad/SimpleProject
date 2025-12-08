using SimpleProject.Models.Identity;

namespace SimpleProject.Services.Interfaces
{
    public interface IClaimService
    {
        public Task<string> AddClaimAsync(Claim claim);
        public Task<string> DeleteClaimAsync(Claim claim);
        public Task<string> UpdateClaimAsync(Claim claim);
        public Task<List<Claim>> GetClaimsAsync();
    }
}
