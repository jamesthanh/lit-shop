using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastucture.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepository;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepository;
        private readonly IGenericRepository<ProductType> _productsTypeRepository;
        public ProductsController(IGenericRepository<Product> productsRepository, IGenericRepository<ProductBrand> productsBrandRepository,
            IGenericRepository<ProductType> productsTypeRepository)
        {
            _productsRepository = productsRepository;
            _productsBrandRepository = productsBrandRepository;
            _productsTypeRepository = productsTypeRepository;
        }

        [HttpGet]
        // using task to pass the request to a delegate and it does not wait
        public async Task<ActionResult<List<Product>>>  GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepository.ListAsync(spec);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            return await _productsRepository.GetEntityWithSpec(spec);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productsBrandRepository.ListAllAsync());
        }
        
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productsTypeRepository.ListAllAsync());
        }
    }
}  