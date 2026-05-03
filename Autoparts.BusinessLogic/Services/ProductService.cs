using AutoMapper;
using Autoparts.BusinessLogic.Interfaces;
using Autoparts.DataAccess;
using Autoparts.Domains.DTOs;
using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.BusinessLogic.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _db.Products.ToListAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        return product is null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(string category)
    {
        var products = await _db.Products.Where(p => p.Category == category).ToListAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return null;
        _mapper.Map(dto, product);
        await _db.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
}
