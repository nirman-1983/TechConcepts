using AutoMapper;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Mappings;

/// <summary>
/// AutoMapper profile for Content bounded context
/// </summary>
public class ContentMappingProfile : Profile
{
    public ContentMappingProfile()
    {
        // ProductCategory mappings
        CreateMap<ProductCategory, GetProductCategoryResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products ?? new List<Product>()));

        CreateMap<AddProductCategoryRequest, ProductCategory>()
            .ConstructUsing(src => new ProductCategory(src.ProductCategoryName));

        // Product mappings
        CreateMap<Product, GetProductResponse>()
            .ForMember(dest => dest.ProductCategoryName, opt => opt.MapFrom(src => src.ProductCategory != null ? src.ProductCategory.ProductCategoryName : string.Empty));

        CreateMap<AddProductRequest, Product>()
            .ConstructUsing(src => new Product(src.ProductName, src.ProductCategoryId));

    }
}
