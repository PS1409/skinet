using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specificatons;
using API.Dtos;
using System.Linq;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo
        , IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }
        [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        // {
        //     var spec =  new ProductWithTypesAndBrandsSpecification();
        //     //var products = await _productRepo.ListAllAsync();
        //      var products = await _productRepo.ListAsync(spec);  
        //     return Ok(products);
        // }
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec =  new ProductWithTypesAndBrandsSpecification();
            //var products = await _productRepo.ListAllAsync();
             var products = await _productRepo.ListAsync(spec);  
            // return products.Select(product => new ProductToReturnDto{
            //      Id= product.Id,
            //      Name = product.Name,
            //      Description = product.Description,
            //      PictureUrl = product.PictureUrl,
            //      Price = product.Price,
            //      ProductType = product.ProductType.Name,
            //      ProductBrand = product.ProductBrand.Name
            // }).ToList();
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Product>> GetProduct(int id)
        // {
        //     var spec = new ProductWithTypesAndBrandsSpecification(id);
        //     var product = await _productRepo.GetEntityWithSpecifications(spec);
        //     return Ok(product);
        // }
         [HttpGet("{id}")]
         public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
         {
             var spec = new ProductWithTypesAndBrandsSpecification(id);
             var product = await _productRepo.GetEntityWithSpecifications(spec);

            //  return new ProductToReturnDto{
            //      Id= product.Id,
            //      Name = product.Name,
            //      Description = product.Description,
            //      PictureUrl = product.PictureUrl,
            //      Price = product.Price,
            //      ProductType = product.ProductType.Name,
            //      ProductBrand = product.ProductBrand.Name
            //  };
            return _mapper.Map<Product, ProductToReturnDto>(product);
         }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands =  await _productBrandRepo.ListAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await  _productTypeRepo.ListAllAsync();
            return Ok(types);
        }
    }
}