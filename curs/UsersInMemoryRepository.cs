using curs.Models;
using curs.Areas.Admin.Models;

namespace curs
{
    public class UsersInMemoryRepository : IUsersRepository
    {
        private readonly List<User> users= new List<User>() 
        {
            new User("nashatkina@bk.ru", "12345678", "Анастасия", "Ашаткина", "+79222319055")
        };
        public List<User> GetAll()
        {
            return users;
        }

        public User TryGetById(Guid id)
        {
            return users.FirstOrDefault(user=>user.Id==id);
        }


        public User TryGetByName(string name)
        {
            return users.FirstOrDefault(user => user.UserName == name);
        }
        public void Add(User user)
        {
            users.Add(user);
        }

        public void ChangeAccess(Guid id, string roleName)
        {
            var currentUser=TryGetById(id);
            currentUser.Role.Name = roleName;
        }

        public void ChangePassword(Guid id, string password)
        {
            var currentUser = TryGetById(id);
            currentUser.Password=password;
        }

        public void Del(Guid id)
        {
            var user=TryGetById(id);
            users.Remove(user);
        }

        public void Edit(EditUser user, Guid id)
        {
            var currentUser = TryGetById(id);
            currentUser.UserName = user.UserName;
            currentUser.FirstName= user.FirstName;
            currentUser.LastName= user.LastName;
            currentUser.Phone=user.Phone;
        }

        

       
    }
}
