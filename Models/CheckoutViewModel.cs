using System.ComponentModel.DataAnnotations;
using LuhnNet;

namespace CampusOrdering.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Credit card number is required.")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be a 3-digit number")]
        public string CVV { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsCardNumberValid()
        {
            // Remove spaces and non-numeric characters from the card number
            string cleanedCardNumber = new string(CardNumber.Where(char.IsDigit).ToArray());

            // Log the cleaned card number for debugging
            Console.WriteLine("Cleaned card number: " + cleanedCardNumber);

            // Check if the cleaned card number is empty or null
            if (string.IsNullOrEmpty(cleanedCardNumber))
            {
                ErrorMessage = "Invalid credit card number.";
                return false;
            }

            // Perform Luhn algorithm validation
            bool isValid = Luhn.IsValid(cleanedCardNumber);

            // Log the result of Luhn validation for debugging
            Console.WriteLine("Luhn validation result: " + isValid);

            // Set error message if credit card number is invalid
            if (!isValid)
            {
                ErrorMessage = "Invalid credit card number.";
            }

            // Return the result of Luhn validation
            return isValid;
        }
    }
}
