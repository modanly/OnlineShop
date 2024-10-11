using OnlineShop.Areas.Admin.Models;


namespace OnlineShop.Models
{
    public class UserViewModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
     
        public string Phone { get; set; }
        public List<RolesViewModel> Roles { get; set; }
    }
}
