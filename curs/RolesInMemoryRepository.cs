using curs.Areas.Admin.Models;

namespace curs
{
    public class RolesInMemoryRepository:IRolesRepository
    {
        public List<Roles> roles = new List<Roles>() { new Roles("Admin"), new Roles("User") };

        public List<Roles> GetAllRoles()
        {
            return roles;
        }

        public void Add(Roles role)
        {
            roles.Add(role);
        }

        public void Del(Roles role)
        {
            roles.Remove(role);
        }
    }
}
