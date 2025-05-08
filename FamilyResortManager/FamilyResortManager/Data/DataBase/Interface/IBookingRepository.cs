using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Data.DataBase.Interface;

public interface IBookingRepository
{
    Task<BookingResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<BookingResponseDto>> GetAllAsync();
    Task<PagedResponseDto<BookingResponseDto>> QueryAsync(BookingQueryDto query);
    Task<BookingResponseDto> CreateAsync(BookingCreateRequestDto request);
    Task<BookingResponseDto> UpdateAsync(BookingUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteBookingsRequestDto request);
}