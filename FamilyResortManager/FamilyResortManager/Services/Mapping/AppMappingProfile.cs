using AutoMapper;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Room mappings
            CreateMap<Room, RoomResponseDto>();
            CreateMap<RoomCreateRequestDto, Room>();
            CreateMap<RoomUpdateRequestDto, Room>();

            // Client mappings
            CreateMap<Data.DataBase.Models.Client, ClientResponseDto>();
            CreateMap<ClientCreateRequestDto, Data.DataBase.Models.Client>();
            CreateMap<ClientUpdateRequestDto, Data.DataBase.Models.Client>();

            // Booking mappings
            CreateMap<Booking, BookingResponseDto>();
            CreateMap<BookingCreateRequestDto, Booking>();
            CreateMap<BookingUpdateRequestDto, Booking>();

            // Staff mappings
            CreateMap<Staff, StaffResponseDto>();
            CreateMap<StaffCreateRequestDto, Staff>();
            CreateMap<StaffUpdateRequestDto, Staff>();

            // Shift mappings
            CreateMap<Shift, ShiftResponseDto>();
            CreateMap<ShiftCreateRequestDto, Shift>();
            CreateMap<ShiftUpdateRequestDto, Shift>();

            // CleaningTask mappings
            CreateMap<CleaningTask, CleaningTaskResponseDto>();
            CreateMap<CleaningTaskCreateRequestDto, CleaningTask>();
            CreateMap<CleaningTaskUpdateRequestDto, CleaningTask>();
        }
    }
}