using System.Collections.Generic;
using System.Linq;
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
        //Create Restaurant
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

        //Get list of restaurants
        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.Include(r => r.Ratings).ToListAsync();
            List<RestaurantListItem> restaurantList = restaurants.Select(r => new RestaurantListItem
            {
                Id = r.Id,
                Name = r.Name,
                Location = r.Location,
                AvgRating = r.AvgRating
            }).ToList();
            return Ok(restaurantList);
        }

        //Get one restaurant by id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants.Include(r => r.Ratings).FirstOrDefaultAsync(r => r.Id == id);
            if(restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        //Update a Restaurant
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] Restaurant model)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var oldRestaurant = await _context.Restaurants.FindAsync(id);
            if(oldRestaurant == null)
            {
                return NotFound();
            }
            try
            {
                if(!string.IsNullOrEmpty(model.Name))
                {
                    oldRestaurant.Name = model.Name;
                }
                if(!string.IsNullOrEmpty(model.Location))
                {
                    oldRestaurant.Location = model.Location;
                }
            }
            catch
            {
                return BadRequest("Name or Location is null or empty");
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        //Delete Restaurants
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