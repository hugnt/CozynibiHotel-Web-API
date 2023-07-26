using AutoMapper;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageDto, Language>();

            CreateMap<RoomCategory, RoomCategoryDto>();
            CreateMap<RoomCategoryDto, RoomCategory>();

            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();

            CreateMap<RefeshToken, RefeshTokenDto>();
            CreateMap<RefeshTokenDto, RefeshToken>();

            CreateMap<Equipment, EquipmentDto>();
            CreateMap<EquipmentDto, Equipment>();

            CreateMap<FoodCategory, FoodCategoryDto>();
            CreateMap<FoodCategoryDto, FoodCategory>();

            CreateMap<Food, FoodDto>();
            CreateMap<FoodDto, Food>();

            CreateMap<TourTravel, TourTravelDto>();
            CreateMap<TourTravelDto, TourTravel>();


            CreateMap<TourSchedule, TourScheduleDto>();
            CreateMap<TourScheduleDto, TourSchedule>();

            CreateMap<Gallery, GalleryDto>();
            CreateMap<GalleryDto, Gallery>();

            CreateMap<TourPrice, TourPriceDto>();
            CreateMap<TourPriceDto, TourPrice>();

            CreateMap<Inclusion, InclusionDto>();
            CreateMap<InclusionDto, Inclusion>();

            CreateMap<Exclusion, ExclusionDto>();
            CreateMap<ExclusionDto, Exclusion>();

            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();

            CreateMap<NewsCategory, NewsCategoryDto>();
            CreateMap<NewsCategoryDto, NewsCategory>();

            CreateMap<News, NewsDto>();
            CreateMap<NewsDto, News>();

            CreateMap<Page, PageDto>();
            CreateMap<PageDto, Page>();

            CreateMap<Custommer, CustommerDto>();
            CreateMap<CustommerDto, Custommer>();

            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();

            CreateMap<Gallery, GalleryDto>();
            CreateMap<GalleryDto, Gallery>();

            CreateMap<GalleryCategory, GalleryCategoryDto>();
            CreateMap<GalleryCategoryDto, GalleryCategory>();

            CreateMap<Information, InformationDto>();
            CreateMap<InformationDto, Information>();

            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();

            CreateMap<Booking, BookingDto>();
            CreateMap<BookingDto, Booking>();

            CreateMap<FoodOrder, FoodOrderDto>();
            CreateMap<FoodOrderDto, FoodOrder>();

        }
        
    }
}
