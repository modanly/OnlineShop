namespace OnlineShop.Areas.Admin.Models
{
    public class EditRightsViewModel
    {
        public string Id { get; set; } 
        public string UserName { get; set; }
        public List<RolesViewModel> Roles { get; set; }
        public List<string> AssignedRoles { get; set; }
    }
}
