using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiplomApi.Models;

namespace DiplomApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DiplombdContext _context;

        public OrdersController(DiplombdContext context)
        {
            _context = context;
        }
        [HttpGet("getOrders")]
        public async Task<ActionResult<IEnumerable<Review>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpPost("addOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order is null.");
            }

            try
            {
                if (string.IsNullOrEmpty(order.OrderState))
                {
                    order.OrderState = "Pending"; 
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Заказ создан"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("updateToDone/{id}")]
        public async Task<IActionResult> UpdateOrderToDone(Guid id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                if (order.OrderState != "Pending")
                {
                    return BadRequest(new { message = "Order state must be 'Pending' to update to 'Done'." });
                }

                order.OrderState = "Done";
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Order state updated to 'Done'."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("updateToReviewed/{id}")]
        public async Task<IActionResult> UpdateOrderToReviewed(Guid id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                if (order.OrderState != "Done")
                {
                    return BadRequest(new { message = "Order state must be 'Done' to update to 'Reviewed'." });
                }

                order.OrderState = "Reviewed";
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Order state updated to 'Reviewed'."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpGet("getOrdersDone/{name}")]
        public async Task<IActionResult> GetOrdersDone(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "The 'name' parameter cannot be null or empty."
                });
            }

            if (_context.Orders == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Orders not found in the database."
                });
            }

            try
            {
                var doneOrders = await _context.Orders
                    .Where(order => order.OrderState == "Done" && order.ClientName == name)
                    .ToListAsync();

                if (!doneOrders.Any())
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = $"No orders found for client '{name}' with state 'Done'."
                    });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Orders retrieved successfully.",
                    data = doneOrders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }
        [HttpDelete("deleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Order deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }




    }
}
