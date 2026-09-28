using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Data;

namespace SmartInventarOrderApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET ALL ORDERS
        [HttpGet("orders")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.ToList();

            if (orders.Count == 0)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }

        // 2. UPDATE ORDER STATUS
        [HttpPut("order-status/{id}")]
        public IActionResult UpdateOrderStatus(
            int id,
            string status)
        {
            var order = _context.Orders
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            order.Status = status;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Order status updated successfully",
                orderId = order.Id,
                status = order.Status
            });
        }
    }
}
