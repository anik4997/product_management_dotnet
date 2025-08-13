using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCrud.Models;

namespace ProductCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DAL _dal;
        private readonly string _documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductionDocuments");
        public ProductsController(DAL dal)
        {
            _dal = dal;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] Products product, IFormFile ProductionDocuments)
        {
            if(ProductionDocuments != null && ProductionDocuments.Length > 0)
            {
                //string fileName = $"{Guid.NewGuid()}_{ProductionDocuments.FileName}";
                //string filePath = Path.Combine(_documentPath, fileName);
                //if (!Directory.Exists(filePath))
                //{
                //    Directory.CreateDirectory(filePath);
                //}
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await ProductionDocuments.CopyToAsync(stream);
                //}
                string fileName = $"{Guid.NewGuid()}_{ProductionDocuments.FileName}";
                string filePath = Path.Combine(_documentPath, fileName);

                string directoryPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductionDocuments.CopyToAsync(stream);
                }

                product.ProductionDocuments = $"/ProductionDocuments/{fileName}";
            }
            else
            {
                return BadRequest("Production document is required.");
            }
            int newRecord = _dal.InsertProducts(product);
            if (newRecord > 0)
            {
                return Ok(new { Message = "Product created successfully", ProductId = newRecord });
            }
            else
            {
                return BadRequest("Failed to create product.");
            }
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            List<Products> ListProducts = _dal.GetAllProducts();
            return Ok(ListProducts);
        }
        [HttpGet("{id}")]
        public IActionResult GetProducts(int id)
        {
            Products product = _dal.GetProducts(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound($"Product with ID {id} not found.");
            }
        }
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] Products product, IFormFile ProductionDocuments)
        {
            if (ProductionDocuments != null && ProductionDocuments.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{ProductionDocuments.FileName}";
                string filePath = Path.Combine(_documentPath, fileName);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductionDocuments.CopyToAsync(stream);
                }
                product.ProductionDocuments = $"/ProductionDocuments/{fileName}";
            }
            else
            {
                return BadRequest("Production document is required.");
            }
            bool isUpdated = _dal.UpdateProduct(product) > 0;
            if (isUpdated)
            {
                return Ok(new { Message = "Product updated successfully" });
            }
            else
            {
                return BadRequest("Failed to update product.");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            bool isDeleted = _dal.DeleteProducts(id) > 0;
            if (isDeleted)
            {
                return Ok(new { Message = "Product deleted successfully" });
            }
            else
            {
                return NotFound($"Product with ID {id} not found.");
            }
        }
    }
}
