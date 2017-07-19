using System.Collections.Generic;
using AutoMapper;
using EggsAndHoney.Domain.Models;
using EggsAndHoney.WebApi.ViewModels;

namespace EggsAndHoney.WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderViewModel>().ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.OrderType.Name));
			CreateMap<IList<Order>, IList<OrderViewModel>>();
			CreateMap<ResolvedOrder, ResolvedOrderViewModel>().ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.OrderType.Name));
			CreateMap<IList<ResolvedOrder>, IList<ResolvedOrderViewModel>>();
		}
    }
}
