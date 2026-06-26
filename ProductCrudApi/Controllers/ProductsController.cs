using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCrudApi.Application.DTOs;
using ProductCrudApi.Domain.Entities;
using ProductCrudApi.Infrastructure.Data;

namespace ProductCrudApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]   // हा संपूर्ण controller JWT token शिवाय access होणार नाही
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------- GET: api/products ----------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Items)
                .Select(p => MapToResponseDto(p))
                .ToListAsync();

            return Ok(products);
        }

        // ---------- GET: api/products/5 ----------
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with Id {id} not found." });
            }

            return Ok(MapToResponseDto(product));
        }

        // ---------- POST: api/products ----------
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(CreateProductRequestDto request)
        {
            var currentUser = User.Identity?.Name ?? "system";

            var product = new Product
            {
                ProductName = request.ProductName,
                CreatedBy = currentUser,
                CreatedOn = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                MapToResponseDto(product));
        }

        // ---------- PUT: api/products/5 ----------
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDto>> Update(int id, UpdateProductRequestDto request)
        {
            var product = await _context.Products
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with Id {id} not found." });
            }

            var currentUser = User.Identity?.Name ?? "system";

            product.ProductName = request.ProductName;
            product.ModifiedBy = currentUser;
            product.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToResponseDto(product));
        }

        // ---------- DELETE: api/products/5 ----------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = $"Product with Id {id} not found." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ---------- Helper: Entity -> DTO mapping ----------
        private static ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn,
                ModifiedBy = product.ModifiedBy,
                ModifiedOn = product.ModifiedOn,
                Items = product.Items.Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}