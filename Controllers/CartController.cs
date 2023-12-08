using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CampusOrdering.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CampusOrdering.Controllers
{
    public class CartController : Controller
    {
        private readonly string SessionKey = "ShoppingCart";
        private readonly OrderingContext _context;

        public CartController(OrderingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItem> _cart = GetCartFromSession();
            return View(_cart);
        }

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

        public IActionResult Checkout()
        {
            List<CartItem>  cart = GetCartFromSession();

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
                TotalPrice = cart.Sum(item => item.Price * item.Quantity)
            };

            _context.Receipts.Add(receipt);
            _context.SaveChanges();

            ClearCart();

            return RedirectToAction("ThankYou");
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
            Console.WriteLine($"PurchasedItems Count: {receipt?.PurchasedItems?.Count}");

            // Return the receipt details view
            return View(receipt);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
