using CampingAPI2.Models;

namespace CampingAPI2.Data
{
    public interface IDataContext
    {
        Task<User> RegisterUser(User user);
        Task<Owner> RegisterOwner(Owner owner);
        Task<User> GetUserByEmail(string email);
        Task<Owner> GetOwnerByEmail(string email);
        Task<Owner> AuthenticateOwner(string email, string password);
        Task<User> AuthenticateUser(string email, string password);
        Task<User> UpdateUser(User user);
        Task<Owner> UpdateOwner(Owner owner);
        Task<CampingSite> AddCampingSite(CampingSite campingSite);
        Task<IEnumerable<CampingSite>> GetSites();
        Task<IEnumerable<CampingSite>> GetCampingSitesByOwnerId(int OwnerId);
        Task<bool> DeleteCampingSite(string Name);


    }
}
