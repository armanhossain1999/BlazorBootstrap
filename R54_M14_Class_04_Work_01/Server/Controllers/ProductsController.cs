using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R54_M14_Class_04_Work_01.Shared.Models;
using R54_M14_Class_04_Work_01.Shared.ViewModels;

namespace R54_M14_Class_04_Work_01.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment env;
        public ProductsController(ProductDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env; 
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.ToListAsync();
        }
        [HttpGet("Sales/Include")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsWithSales()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.Include(x => x.Sales).ToListAsync();
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [HttpGet("{id}/Include")]
        public async Task<ActionResult<Product>> GetProductWithSales(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Include(x => x.Sales).FirstOrDefaultAsync(x => x.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProductDbContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }
        [HttpPost("VM")]
        public async Task<ActionResult<ProductInputModel>> PostProductVM(ProductInputModel data)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProductDbContext.Products'  is null.");
            }
            var product = new Product { 
                ProductId = data.ProductId, 
                ProductName=data.ProductName,
                Size = data.Size,
                Picture = data.Picture,
                Price = data.Price,
                OnSale = data.OnSale,
            };
           foreach(var s in data.Sales)
            {
                product.Sales.Add(s);
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            data.ProductId = product.ProductId;
            return data;
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("Upload")]
        public async Task<UploadResponse> Upload(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName);
            string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            //string s = env.WebRootPath;
            string savePath = Path.Combine(env.WebRootPath, "Pictures", f);
            using FileStream fs = new (savePath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return new UploadResponse { ImageName = f };
        }
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
