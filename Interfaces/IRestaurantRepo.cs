using CampusOrdering.Models;
namespace CampusOrdering.Interfaces

{
    public interface IRestaurantRepo
    {
        Restaurant GetRestaurantById(int id);
        List<Restaurant> GetAllRestaurants();
        void AddRestaurant(Restaurant rest);
        void UpdateRestaurant(Restaurant rest);
        void DeleteRestaurant(int id);
    }
}
