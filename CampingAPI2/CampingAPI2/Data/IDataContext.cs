using CampingAPI2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampingAPI2.Data
{
    public interface IDataContext
    {
        Task<User> RegisterUser(User user);
        Task<Owner> RegisterOwner(Owner owner);
        Task<User> GetUserByEmail(string email);
        Task<Owner> GetOwnerByEmail(string email);
        Task<Owner> GetOwnerById(int id);
        Task<Owner> AuthenticateOwner(string email, string password);
        Task<User> AuthenticateUser(string email, string password);
        Task<User> UpdateUser(User user);
        Task<Owner> UpdateOwner(Owner owner);
        Task<CampingSite> AddCampingSite(CampingSite campingSite);
        Task<IEnumerable<CampingSite>> GetSites();
        Task<IEnumerable<CampingSite>> GetCampingSitesByOwnerEmail(string ownerEmail);
        Task<bool> DeleteCampingSite(string Name);
        Task<IEnumerable<CampingSite>> GetAvailableSites();

    }
}
