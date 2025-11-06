using AutoMapper;
using Domain.Entities;
using Domain.DTO.Responses;

namespace Infrastructure.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, ClientResponse>()
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));

        CreateMap<Product, ProductResponse>();
        CreateMap<Order, OrderResponse>();
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
    }
}