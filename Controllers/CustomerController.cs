using CampusOrdering.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusOrdering.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController()
        {
            _customerRepository = new CustomerRepository();
        }

        public IActionResult Settings(int customerId = 0)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        public IActionResult Edit(int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound(); 
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer editedCustomer)
        {
            if (ModelState.IsValid)
            {
                _customerRepository.UpdateCustomer(editedCustomer);
                return RedirectToAction("Settings", new { customerId = editedCustomer.Id });
            }

            return View(editedCustomer);
        }

    }


}

