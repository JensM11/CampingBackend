using CampingAPI2.Models;
using LiteDB;
using BCrypt.Net;

namespace CampingAPI2.Data
{
    public class DataBase : IDataContext
    {
        LiteDatabase db = new LiteDatabase(@"data.db");

        public async Task<CampingSite> AddCampingSite(CampingSite campingSite)
        {
            try
            {
                var campingsites = db.GetCollection<CampingSite>("Campingsites");
                campingsites.Insert(campingSite);
                return campingSite;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<bool> DeleteCampingSite(string name)
        {
            var campingsites = db.GetCollection<CampingSite>("CampingSites");
            var campsite = campingsites.FindOne(c => c.Name == name);

            if (campsite != null)
            {
                return campingsites.Delete(campsite.Id);
            }

            return false;
        }

        public async Task<IEnumerable<CampingSite>> GetCampingSitesByOwnerEmail(string ownerEmail)
        {
            var campingsites = db.GetCollection<CampingSite>("CampingSites");
            return campingsites.Find(c => c.OwnerEmail == ownerEmail);
        }

        public async Task<IEnumerable<CampingSite>> GetAvailableSites()
        {
            var campingspots = db.GetCollection<CampingSite>("CampingSites");
            // Return only available camping sites
            return campingspots.Find(c => c.IsAvailable);
        }

        public async Task<Owner> GetOwnerByEmail(string email)
        {
            var owners = db.GetCollection<Owner>("Owners");
            return owners.FindOne(o => o.Email == email);
        }
        public async Task<Owner> GetOwnerById(int id)
        {
            var owners = db.GetCollection<Owner>("Owners");
            return owners.FindOne(o => o.Id == id);
        }

        public async Task<IEnumerable<CampingSite>> GetSites()
        {
            var campingspots = db.GetCollection<CampingSite>("CampingSites");
            return campingspots.FindAll();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var users = db.GetCollection<User>("Users");
            return users.FindOne(u => u.Email == email);
        }

        public async Task<Owner> RegisterOwner(Owner owner)
        {
            var owners = db.GetCollection<Owner>("Owners");
            if (owners.Exists(u => u.Email == owner.Email))
            {
                throw new Exception("Email already exists");
            }
                
            owner.Password = BCrypt.Net.BCrypt.HashPassword(owner.Password);
            owners.Insert(owner);
            return owner;
        }

        public async Task<User> RegisterUser(User user)
        {
            var users = db.GetCollection<User>("Users");

            if (users.Exists(u => u.Email == user.Email))
            {
                throw new Exception("Email already exists");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            users.Insert(user);
            return user;
        }

        public async Task<Owner> UpdateOwner(Owner owner)
        {
            var owners = db.GetCollection<Owner>("Owners");
            owners.Update(owner);
            return owner;
        }

        public async Task<User> UpdateUser(User user)
        {
            var users = db.GetCollection<User>("Users");
            users.Update(user);
            return user;
        }

        public async Task<Owner> AuthenticateOwner(string email, string password)
        {
            var owners = db.GetCollection<Owner>("Owners");
            var owner = owners.FindOne(o => o.Email == email);

            if (owner != null && BCrypt.Net.BCrypt.Verify(password, owner.Password))
                return owner;

            return null;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            var users = db.GetCollection<User>("Users");
            var user = users.FindOne(o => o.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null;
        }
        public async Task<CampingSite> GetCampingSiteById(int id)
        {
            var campingsites = db.GetCollection<CampingSite>("CampingSites");
            return campingsites.FindById(id);
        }

        public async Task<bool> UpdateCampingSite(CampingSite campingSite)
        {
            var campingsites = db.GetCollection<CampingSite>("CampingSites");
            return campingsites.Update(campingSite);
        }
        public async Task<IEnumerable<CampingSite>> GetBookedSitesByClientEmail(string clientEmail)
        {
            var campingsites = db.GetCollection<CampingSite>("CampingSites");
            return campingsites.Find(c => c.ClientEmail == clientEmail);
        }
    }
}
