
using curs.Areas.Admin.Models;

namespace curs.Models
{
    public class User
    {
     
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Roles Role { get; set; }


        public User(string name, string password, string firstName, string lastName, string phone)
        {
            Id = Guid.NewGuid();
            UserName = name;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Role = new Roles("User");
        }
    }
}
