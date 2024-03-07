using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CampusOrdering.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? Name { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int? RestaurantId { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        public string? roleId { get; set; }
        public IEnumerable<SelectListItem>? RoleList { get; set; }
        public DateTime? Birthday { get; set; }


    }
}
