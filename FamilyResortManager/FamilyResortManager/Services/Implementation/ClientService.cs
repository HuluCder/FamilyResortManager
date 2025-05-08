using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation
{
    #region Client Service

    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;
        private readonly IValidator<ClientCreateRequestDto> _createValidator;
        private readonly IValidator<ClientUpdateRequestDto> _updateValidator;

        public ClientService(
            IClientRepository repo,
            IValidator<ClientCreateRequestDto> createValidator,
            IValidator<ClientUpdateRequestDto> updateValidator)
        {
            _repo = repo;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public Task<ClientResponseDto> GetByIdAsync(int id) =>
            _repo.GetByIdAsync(id);

        public Task<IEnumerable<ClientResponseDto>> GetAllAsync() =>
            _repo.GetAllAsync();

        public Task<PagedResponseDto<ClientResponseDto>> QueryAsync(ClientQueryDto query) =>
            _repo.QueryAsync(query);

        public async Task<ClientResponseDto> CreateAsync(ClientCreateRequestDto request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            return await _repo.CreateAsync(request);
        }

        public async Task<ClientResponseDto> UpdateAsync(ClientUpdateRequestDto request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            return await _repo.UpdateAsync(request);
        }

        public Task DeleteAsync(int id) =>
            _repo.DeleteAsync(id);

        public Task BulkDeleteAsync(BulkDeleteClientsRequestDto request) =>
            _repo.BulkDeleteAsync(request);
    }

    #endregion

    #region Staff Service

    // …

    #endregion

    #region Shift Service

    // …

    #endregion

    #region CleaningTask Service

    // …

    #endregion

    #region Notification Service

    // …

    #endregion

    #region AuditLog Service

    // …

    #endregion
}
