// Server/Services/Implementation/RoomService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;
        private readonly IValidator<RoomCreateRequestDto> _createValidator;
        private readonly IValidator<RoomUpdateRequestDto> _updateValidator;

        public RoomService(
            IRoomRepository repo,
            IValidator<RoomCreateRequestDto> createValidator,
            IValidator<RoomUpdateRequestDto> updateValidator)
        {
            _repo = repo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public Task<RoomResponseDto> GetByIdAsync(int id) =>
            _repo.GetByIdAsync(id);

        public Task<IEnumerable<RoomResponseDto>> GetAllAsync() =>
            _repo.GetAllAsync();

        public Task<PagedResponseDto<RoomResponseDto>> QueryAsync(RoomQueryDto query) =>
            _repo.QueryAsync(query);

        public async Task<RoomResponseDto> CreateAsync(RoomCreateRequestDto request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            return await _repo.CreateAsync(request);
        }

        public async Task<RoomResponseDto> UpdateAsync(RoomUpdateRequestDto request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            return await _repo.UpdateAsync(request);
        }

        public Task DeleteAsync(int id) =>
            _repo.DeleteAsync(id);

        public Task BulkDeleteAsync(BulkDeleteRoomsRequestDto request) =>
            _repo.BulkDeleteAsync(request);
    }
}
