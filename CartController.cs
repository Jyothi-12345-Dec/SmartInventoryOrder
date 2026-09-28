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
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }



        [HttpPost("add")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            var product = _context.Products
                .Find(productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (product.Stock < quantity)
            {
                return BadRequest(new
                {
                    message = "Not enough stock",
                    productId = product.Id,
                    productName = product.Name,
                    availableStock = product.Stock,
                    requestedQuantity = quantity
                });
            }

        
            var cart = _context.Carts
                .FirstOrDefault(x => x.UserId == userId);

          
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            
            var existingItem = _context.CartItems
                .FirstOrDefault(x =>
                    x.CartId == cart.CartId &&
                    x.ProductId == productId);

            if (existingItem != null)
            {
            
                int totalQuantity =
                    existingItem.Quantity + quantity;

                
                if (product.Stock < totalQuantity)
                {
                    return BadRequest(new
                    {
                        message = "Not enough stock",
                        productId = product.Id,
                        productName = product.Name,
                        availableStock = product.Stock,
                        requestedQuantity = totalQuantity
                    });
                }

                existingItem.Quantity = totalQuantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };

                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges();

            return Ok(new
            {
                message = "Product added to cart successfully",
                cartId = cart.CartId,
                userId = userId,
                productId = productId,
                quantity = quantity
            });
        }

        [HttpGet]
        public IActionResult GetCart()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = _context.Carts
                .FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var items = _context.CartItems
                .Where(x => x.CartId == cart.CartId)
                .ToList();

            return Ok(new
            {
                cartId = cart.CartId,
                userId = userId,
                items = items
            });
        }


      

        [HttpPut("update")]
        public IActionResult UpdateCart(
            int productId,
            int quantity)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            // Find cart
            var cart = _context.Carts
                .FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(x =>
                    x.CartId == cart.CartId &&
                    x.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in cart");
            }

            var product = _context.Products
                .Find(productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (product.Stock < quantity)
            {
                return BadRequest(new
                {
                    message = "Not enough stock",
                    productId = product.Id,
                    productName = product.Name,
                    availableStock = product.Stock,
                    requestedQuantity = quantity
                });
            }
            cartItem.Quantity = quantity;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Cart updated successfully",
                cartId = cart.CartId,
                userId = userId,
                productId = productId,
                quantity = quantity
            });
        }

        [HttpDelete("remove")]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var cart = _context.Carts
                .FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }
            var cartItem = _context.CartItems
                .FirstOrDefault(x =>
                    x.CartId == cart.CartId &&
                    x.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in cart");
            }
            _context.CartItems.Remove(cartItem);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Product removed from cart successfully",
                cartId = cart.CartId,
                userId = userId,
                productId = productId
            });
        }
    }
}