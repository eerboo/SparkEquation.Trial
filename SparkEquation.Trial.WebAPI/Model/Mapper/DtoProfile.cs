using AutoMapper;
using SparkEquation.Trial.WebAPI.Data.Models;
using SparkEquation.Trial.WebAPI.Model.Request;
using System.Linq;

namespace SparkEquation.Trial.WebAPI.Model.Mapper
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandName, cfg => cfg.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Categories, cfg => cfg.MapFrom(src => src.CategoryProducts.Select(c => c.Category)));

            CreateMap<Category, CategoryDto>(MemberList.Destination);

            CreateMap<AddProductRequest, Product>(MemberList.None);

            CreateMap<UpdateProductRequest, Product>(MemberList.None);

        }

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoProfile>();
            });

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}
