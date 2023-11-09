using CampusOrdering.Interfaces;
using CampusOrdering.Models;
namespace CampusOrdering.Data
{
    public class RestaurantRepository : IRestaurantRepo
    {
        private List<Restaurant> _restaurants;

        public RestaurantRepository()
        {
            // Initialize the restaurant data (you can load data from a database here)
            _restaurants = new List<Restaurant>
        {
            new Restaurant { Id = 1, Name = "Chick-Fil-A",  LogoUrl = "~/Images/Chick-fil-A-Logo.png" },
            new Restaurant { Id = 2, Name = "Panda Express",  LogoUrl = "~/Images/1200px-Panda_Express_logo.svg.png" },
            // Add more restaurants
        };
        }

        public Restaurant GetRestaurantById(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return _restaurants;
        }

        public void AddRestaurant(Restaurant restaurant)
        {
            restaurant.Id = _restaurants.Max(r => r.Id) + 1;
            _restaurants.Add(restaurant);
        }

        public void UpdateRestaurant(Restaurant restaurant)
        {
            var existingRestaurant = _restaurants.FirstOrDefault(r => r.Id == restaurant.Id);
            if (existingRestaurant != null)
            {
                existingRestaurant.Name = restaurant.Name;
                existingRestaurant.LogoUrl = restaurant.LogoUrl;
                // Update other properties as needed
            }
        }

        public void DeleteRestaurant(int id)
        {
            var restaurant = _restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                _restaurants.Remove(restaurant);
            }
        }
    }
}
