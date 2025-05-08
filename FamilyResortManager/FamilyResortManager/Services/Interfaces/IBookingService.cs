using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<BookingResponseDto>> GetAllAsync();
    Task<PagedResponseDto<BookingResponseDto>> QueryAsync(BookingQueryDto query);
    Task<BookingResponseDto> CreateAsync(BookingCreateRequestDto request);
    Task<BookingResponseDto> UpdateAsync(BookingUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteBookingsRequestDto request);
}