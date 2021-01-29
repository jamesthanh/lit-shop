using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
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
        public async Task<ActionResult<List<ProductToReturnDto>>>  GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepository.ListAsync(spec);
            return products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepository.GetEntityWithSpec(spec);
            return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            };
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