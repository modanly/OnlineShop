using curs.Areas.Admin.Models;

namespace curs
{
    public interface IRolesRepository
    {
        void Add(Roles role);
        void Del(Roles role);
        List<Roles> GetAllRoles();
    }
}
