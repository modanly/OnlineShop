using curs.Areas.Admin.Models;
using curs.Models;

namespace curs
{
    public interface IUsersRepository
    {
        List<User> GetAll();
        User TryGetById(Guid id);
        User TryGetByName(string name);
        void Del(Guid id);
        void Add(User user);
        void Edit(EditUser user, Guid id);
        void ChangePassword(Guid id, string password);
        void ChangeAccess(Guid id, string roleName);
       
    }
}
