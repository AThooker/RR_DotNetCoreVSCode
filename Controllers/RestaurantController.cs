using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterAPI.Models;

namespace RestaurantRaterAPI.Controllers
{
    [ApiController]
    [Route("restaurant")]
    public class RestaurantController : Controller
    {
        private RestaurantDbContext _context;
        public RestaurantController(RestaurantDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> PostRestaurant([FromForm] RestaurantEdit model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            _context.Restaurants.Add(
            new Restaurant
            {
                Name = model.Name,
                Location = model.Location

            });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if(restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRestaurantById([FromRoute] int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            var removeRest = _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}