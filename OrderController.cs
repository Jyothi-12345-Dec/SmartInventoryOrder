using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Data;
using SmartInventory.Models;
using System.Security.Claims;

namespace SmartInventarOrderApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("place")]

        public IActionResult PlaceOrder()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = _context.Carts
                .FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartItems = _context.CartItems
                .Where(x => x.CartId == cart.CartId)
                .ToList();

            if (cartItems.Count == 0)
            {
                return BadRequest("Cart is empty");
            }
            foreach (var item in cartItems)
            {
                var product = _context.Products
                    .Find(item.ProductId);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                if (product.Stock < item.Quantity)
                {
                    return BadRequest(new
                    {
                        message = "Not enough stock",
                        productId = product.Id,
                        productName = product.Name,
                        availableStock = product.Stock,
                        requestedQuantity = item.Quantity
                    });
                }
            }
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = 0,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            decimal total = 0;

            foreach (var item in cartItems)
            {
                var product = _context.Products
                    .Find(item.ProductId);

                if (product == null)
                {
                    return NotFound("Product not found");
                }

                decimal itemTotal =
                    product.Price * item.Quantity;

                total += itemTotal;

                // Reduce stock
                product.Stock -= item.Quantity;

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                _context.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;
            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Order placed successfully",
                orderId = order.Id,
                userId = userId,
                totalAmount = order.TotalAmount,
                status = order.Status
            });
        }

        // 2. GET MY ORDERS
        [HttpGet("myorders")]
        public IActionResult GetMyOrders()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = _context.Orders
                .Where(x => x.UserId == userId)
                .ToList();

            if (orders.Count == 0)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = _context.Orders
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var orderItems = _context.OrderItems
                .Where(x => x.OrderId == id)
                .ToList();

            return Ok(new
            {
                orderId = order.Id,
                userId = order.UserId,
                orderDate = order.OrderDate,
                totalAmount = order.TotalAmount,
                status = order.Status,
                items = orderItems
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders
                .ToList();

            if (orders.Count == 0)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("status/{id}")]
        public IActionResult UpdateStatus(int id, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Status is required");
            }

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
