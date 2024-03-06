
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusOrdering.Models;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string? Name { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int? RestaurantID { get; set; }
    public DateTime? Birthdate { get; set; }

    [NotMapped]
    public string? RoleId { get; set; }
    [NotMapped]
    public string? Role { get; set; }
    [NotMapped]
    public IEnumerable<SelectListItem>? RoleList { get; set; }
}

