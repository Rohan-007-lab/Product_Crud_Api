using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCrudApi.Application.DTOs;
using ProductCrudApi.Domain.Entities;
using ProductCrudApi.Infrastructure.Data;

namespace ProductCrudApi.Controllers
{
    [ApiController]
    [Route("api/items")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------- GET: api/items ----------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetAll()
        {
            var items = await _context.Items
                .Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
                .ToListAsync();

            return Ok(items);
        }

        // ---------- GET: api/items/5 ----------
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponseDto>> GetById(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(new { message = $"Item with Id {id} not found." });
            }

            return Ok(new ItemResponseDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        // ---------- GET: api/products/5/items (related resources) ----------
        [HttpGet("/api/products/{productId}/items")]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetByProductId(int productId)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == productId);

            if (!productExists)
            {
                return NotFound(new { message = $"Product with Id {productId} not found." });
            }

            var items = await _context.Items
                .Where(i => i.ProductId == productId)
                .Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
                .ToListAsync();

            return Ok(items);
        }

        // ---------- POST: api/items ----------
        [HttpPost]
        public async Task<ActionResult<ItemResponseDto>> Create(CreateItemRequestDto request)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == request.ProductId);

            if (!productExists)
            {
                return BadRequest(new { message = $"Product with Id {request.ProductId} does not exist." });
            }

            var item = new Item
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = item.Id },
                new ItemResponseDto { Id = item.Id, ProductId = item.ProductId, Quantity = item.Quantity });
        }

        // ---------- PUT: api/items/5 ----------
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemResponseDto>> Update(int id, UpdateItemRequestDto request)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(new { message = $"Item with Id {id} not found." });
            }

            item.Quantity = request.Quantity;
            await _context.SaveChangesAsync();

            return Ok(new ItemResponseDto { Id = item.Id, ProductId = item.ProductId, Quantity = item.Quantity });
        }

        // ---------- DELETE: api/items/5 ----------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(new { message = $"Item with Id {id} not found." });
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}