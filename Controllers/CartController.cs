using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CampusOrdering.Models;

using Newtonsoft.Json;
using System.Security.Claims;
using CampusOrdering.Interfaces;

namespace CampusOrdering.Controllers
{
    public class CartController : Controller
    {
        private readonly string SessionKey = "ShoppingCart";
        private readonly AuthDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CartController> _logger;

        public CartController(AuthDbContext context, IUserRepository userRepository, ILogger<CartController> logger)
        {
            _context = context;
            _userRepository = userRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<CartItem> _cart = GetCartFromSession();
            TimeSpan estimate = CalculateOrderEstimate(_cart, _cart.Count);

            ViewBag.OrderEstimate = estimate;
            return View(_cart);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItem cartItem)
        {
            List<CartItem> _cart = GetCartFromSession();
            _cart.Add(cartItem);
            SaveCartToSession(_cart);

            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        public IActionResult AddToCartPastOrder(string MenuItemName, decimal Price, int Quantity, string ImageUrl)
        {
            CartItem cartItem = new CartItem
            {
                MenuItemName = MenuItemName,
                Price = Price,
                Quantity = Quantity,
                ImageURL = ImageUrl
                
            };

            List<CartItem> _cart = GetCartFromSession();
            _cart.Add(cartItem);
            SaveCartToSession(_cart);

            return RedirectToAction("PastOrder", "User");
        }


        //[RM]
        //Takes a list from Past Orders and adds it to cart.
        [HttpPost]
        public IActionResult AddToCartList(string Json)
        {

            List<CartItem> cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Json);
            List<CartItem> _cart = GetCartFromSession();
            foreach (var cartItem in cartItems)
            {
                _cart.Add(cartItem);
            }
            SaveCartToSession(_cart);


            return RedirectToAction("Index", "Cart");
        }

        private List<CartItem> GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString(SessionKey);
            return string.IsNullOrEmpty(sessionData)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(sessionData);
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            var sessionData = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SessionKey, sessionData);
        }

        public IActionResult CheckoutPage()
        {
            return View();
        }

       [HttpGet]
        public async Task<IActionResult> ThankYou()
        {
            try
            {
                _logger.LogInformation("ThankYou action method called.");

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userRepository.GetUserById(currentUserId);

                List<CartItem> cart = GetCartFromSession();


                List<CartItem> clonedCart = cart.Select(item => new CartItem
                {
                    MenuItemName = item.MenuItemName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageURL = item.ImageURL



                }).ToList();

                Receipt receipt = new Receipt
                {
                    PurchaseDateTime = DateTime.Now,
                    PurchasedItems = cart,
                    TotalPrice = cart.Sum(item => item.Price * item.Quantity),
                    JSONForReceipt = JsonConvert.SerializeObject(cart)
                };

                Order order = new Order
                {

                    PurchaseDateTime = DateTime.Now,
                    TotalPrice = cart.Sum(item => item.Price * item.Quantity),
                    PurchasedItems = cart,
                    isServed = false,
                    purchasingUser = user,
                    GuestName = "Guest",
                    JSONstring = JsonConvert.SerializeObject(cart)
                };

                _context.Receipts.Add(receipt);

                _context.Orders.Add(order);

                _context.SaveChanges();

                TimeSpan estimate = CalculateOrderEstimate(GetCartFromSession(), GetCartFromSession().Count());

                ViewBag.OrderEstimate = estimate;
                
                ClearCart();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ThankYou action.");
                throw;
            }

            // Redirect to a Thank You page or another appropriate page
            //return RedirectToAction("ThankYou", new {estimateTime = estimate});
        }

        [HttpGet]
        public async Task<IActionResult> CheckoutAsync()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(currentUserId);

            
            var model = new CheckoutViewModel();

            //Pass on the current user if they're logged in
            if (user != null)
            {
                model.currUser = user;
            }
            
            model.ErrorMessage = "Enter a valid credit card with no spaces.";
            return View(model);
        }

        public IActionResult Receipts()
        {
            List<Receipt> receipts = _context.Receipts.ToList();

            return View(receipts);
        }

    private void SaveReceipt(Receipt receipt)
        {
            Console.WriteLine($"Receipt ID: {receipt.ReceiptId}");
            Console.WriteLine($"Purchase Date/Time: {receipt.PurchaseDateTime}");
            Console.WriteLine("Purchased Items: ");
            foreach (var item in receipt.PurchasedItems)
            {
                Console.WriteLine($"- {item.Quantity} x {item.MenuItemName} (${item.Price} each)");
            }
            Console.WriteLine($"Total Price: ${receipt.TotalPrice}");
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("ShoppingCart");
        }

        public IActionResult ViewReceipt(int receiptId)
        {
            // Retrieve the receipt from the database
            Receipt receipt = _context.Receipts.Find(receiptId);

            Console.WriteLine($"Receipt ID: {receiptId}");
            Console.WriteLine($"PurchasedItems Count : {receipt?.PurchasedItems?.Count}");

            // Return the receipt details view
            return View(receipt);
        }

        /*  public IActionResult ThankYou(TimeSpan estimateTime)
        {
            ViewBag.OrderEstimate = estimateTime;
            return View();
        } */

        public TimeSpan CalculateOrderEstimate(List<CartItem> cart, int additionalItems)
        {
            int baseMinutes = 5;
            int incrementMinutesPerItem = 1; // Increase by 1 minute per item

            // Calculate additional minutes based on the number of additional items
            int additionalMinutes = additionalItems * incrementMinutesPerItem;

            // Add additional minutes to base estimate
            TimeSpan estimate = TimeSpan.FromMinutes(baseMinutes + additionalMinutes);

            return estimate;
        }
    }
}
