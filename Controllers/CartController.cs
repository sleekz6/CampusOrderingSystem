using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CampusOrdering.Models;

using Newtonsoft.Json;
using System.Security.Claims;

namespace CampusOrdering.Controllers
{
    public class CartController : Controller
    {
        private readonly string SessionKey = "ShoppingCart";
        private readonly AuthDbContext _context;

        public CartController(AuthDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItem> _cart = GetCartFromSession();
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

        [HttpPost]
        public IActionResult ProcessCheckout(CheckoutViewModel model)
        {

            if (!model.IsCardNumberValid())
            {
                ModelState.AddModelError("CardNumber", "Invalid credit card number.");
            }

            // Check for overall model validity (including other properties)
            //if (!ModelState.IsValid)
            //{
                // Return the Checkout view with the invalid model
              //  return View("Checkout", model);
            //}

            /*
            code below finds the current customer that is logged in and assigns them to the order. This will be set up after we have authorization implemented.

             var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentCustomer = _context.Customers.SingleOrDefault(c => c.Id.ToString() == currentUserId);
            */

            var defaultCustomerId = 2;
            var currentCustomer = _context.Customers.SingleOrDefault(c => c.Id == defaultCustomerId);

            List<CartItem> cart = GetCartFromSession();

            List<CartItem> clonedCart = cart.Select(item => new CartItem
            {
                MenuItemName = item.MenuItemName,
                Price = item.Price,
                Quantity = item.Quantity
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
                purchasingCustomer = currentCustomer,
                JSONstring = JsonConvert.SerializeObject(cart)
            };

            _context.Receipts.Add(receipt);

            _context.Orders.Add(order);

            _context.SaveChanges();
            // Validate the model

            // Perform payment processing (e.g., using a payment gateway)

            // Clear the cart after successful checkout
            ClearCart();

            // Redirect to a Thank You page or another appropriate page
            return RedirectToAction("ThankYou");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();

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

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
