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
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be a 3-digit number")]
        public string CVV { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsCardNumberValid()
        {
            // Remove spaces and non-numeric characters from the card number
            string cleanedCardNumber = new string(CardNumber.Where(char.IsDigit).ToArray());

            if (!Luhn.IsValid(cleanedCardNumber))
            {
                ErrorMessage = "Invalid credit card number.";
                return false;
            }
                // Perform Luhn algorithm validation
                return Luhn.IsValid(cleanedCardNumber);
        }
    }
}
