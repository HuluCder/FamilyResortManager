using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation
{
    #region Booking Service

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IValidator<BookingCreateRequestDto> _createValidator;
        private readonly IValidator<BookingUpdateRequestDto> _updateValidator;

        public BookingService(IBookingRepository repo,
                              IValidator<BookingCreateRequestDto> createValidator,
                              IValidator<BookingUpdateRequestDto> updateValidator)
        {
            _repo = repo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public Task<BookingResponseDto> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<IEnumerable<BookingResponseDto>> GetAllAsync() => _repo.GetAllAsync();
        public Task<PagedResponseDto<BookingResponseDto>> QueryAsync(BookingQueryDto query) => _repo.QueryAsync(query);

        public async Task<BookingResponseDto> CreateAsync(BookingCreateRequestDto request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid) throw new ValidationException(result.Errors);
            return await _repo.CreateAsync(request);
        }

        public async Task<BookingResponseDto> UpdateAsync(BookingUpdateRequestDto request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid) throw new ValidationException(result.Errors);
            return await _repo.UpdateAsync(request);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task BulkDeleteAsync(BulkDeleteBookingsRequestDto request) => _repo.BulkDeleteAsync(request);
    }

    #endregion

    #region Staff Service

    #endregion

    #region Shift Service

    #endregion

    #region CleaningTask Service

    #endregion

    #region Notification Service

    #endregion

    #region AuditLog Service

    #endregion
}
