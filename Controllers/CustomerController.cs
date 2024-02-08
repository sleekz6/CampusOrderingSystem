using CampusOrdering.Models;
using Microsoft.AspNetCore.Mvc;
using CampusOrdering.Data;

namespace CampusOrdering.Controllers
{
    public class CustomerController : Controller
    {

        private readonly AuthDbContext _context;


        public CustomerController(AuthDbContext context)
        {
            _context = context;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public IActionResult Settings(int customerId)
        {
            var customer = _context.Customers.SingleOrDefault(x => x.Id == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        public IActionResult Edit(int customerId)
        {
            var customer = _context.Customers.SingleOrDefault(m => m.Id == customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }



        [HttpPost]
        public IActionResult Save(Customer editedCustomer)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _context.Customers.Find(editedCustomer.Id);

                if (existingCustomer.Id != 0)
                {

                    existingCustomer.Name = editedCustomer.Name;
                    existingCustomer.Birthdate = editedCustomer.Birthdate;
                    existingCustomer.Email = editedCustomer.Email;
                    existingCustomer.Password = editedCustomer.Password;


                }
                else
                {

                    _context.Customers.Add(editedCustomer);
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Settings", new { customerId = editedCustomer.Id });
        }

        public ActionResult Index() { 
        
            var customers = _context.Customers.ToList();

        return View(customers);
        }

        public ActionResult PastOrder() {
           
            return View();
        }

        public ActionResult simple() {
            return View();
        }


    }


}