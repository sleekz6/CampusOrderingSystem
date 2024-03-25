using CampusOrdering.Interfaces;
using CampusOrdering.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampusOrdering.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CampusOrdering.Controllers
{
    public class UserController : Controller
    {

        private readonly AuthDbContext _db;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;




        public UserController(AuthDbContext db, IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _db = db;
            _userRepository = userRepository;
            _userManager = userManager;

        }



        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            

            foreach (var user in users)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;

                }
            }

            return View(users);

        }

        /*
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();         
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    GenreId = user.GenreId,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Birthday = user.Birthdate,
                   
                };
                result.Add(userViewModel);
            }
            return View(result);
        }
        */
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                RestaurantId = user.RestaurantID,
                ProfileImageUrl = user.ProfileImageUrl,
                Birthday = user.Birthdate,
            };
            return View(userDetailViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                RestaurantId = user.RestaurantID,
                ProfileImageUrl = user.ProfileImageUrl,
                Birthday = user.Birthdate,
                roleId = user.RoleId,
                RoleList = roles.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // If validation fails, return to the edit view with errors
            }

            var user = await _userRepository.GetUserById(model.Id);
            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == model.Id);

            // Update user properties

            user.UserName = model.UserName;
            user.Name = model.Name;
            user.RestaurantID = model.RestaurantId;
            user.ProfileImageUrl = model.ProfileImageUrl;
            user.Birthdate = model.Birthday;
            user.RoleId = model.roleId;
            user.RoleList = model.RoleList;


            if (userRole != null)
            {
                var previousRoleName = _db.Roles.Where(u => u.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                await _userManager.RemoveFromRoleAsync(user, previousRoleName);

            }

            await _userManager.AddToRoleAsync(user, _db.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);




            // Update user in database
            _userRepository.Update(user);

            return RedirectToAction("Index"); // Redirect to the index page after successful edit
        }




        public async Task<IActionResult> Settings()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(UserId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public async Task<IActionResult> EditAccount(string id)
        {
            var user = await _userRepository.GetUserById(id);
           

            if (user == null)
            {
                return NotFound();
            }



            return View(user);
        }



        [HttpPost]
        public async Task<IActionResult> SaveAsync(AppUser editedUser)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _userRepository.GetUserById(editedUser.Id);

                if (existingCustomer != null)
                {

                    existingCustomer.Name = editedUser.Name;
                    existingCustomer.Birthdate = editedUser.Birthdate;
                    existingCustomer.Email = editedUser.Email;
                    existingCustomer.UserName = editedUser.UserName;



                }

                _userRepository.Update(existingCustomer);


                return RedirectToAction("Settings");
            }

            return View(editedUser);
        }

        public async Task<IActionResult> PastOrder()
        {


            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(currentUserId);

            if (user == null)
            {
                return View("~/Views/Shared/_SignInRequired.cshtml");
            }




            var orders = _db.Orders
                .Include(o => o.PurchasedItems) // Include PurchasedItems navigation property
                .Where(o => o.purchasingUser == user) // Filter orders by purchasingUserId
                .ToList();


            return View(orders);
        }

    }
    }
