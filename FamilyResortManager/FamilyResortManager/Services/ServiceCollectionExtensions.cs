using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Repository;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Implementation;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.Validators;
using FluentValidation;

namespace FamilyResortManager.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Репозитории
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<ICleaningTaskRepository, CleaningTaskRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            // Сервисы
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<ICleaningTaskService, CleaningTaskService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<EmailService>();
            

            // Валидаторы
            services.AddScoped<IValidator<RoomCreateRequestDto>, RoomCreateRequestDtoValidator>();
            services.AddScoped<IValidator<RoomUpdateRequestDto>, RoomUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<ClientCreateRequestDto>, ClientCreateRequestDtoValidator>();
            services.AddScoped<IValidator<ClientUpdateRequestDto>, ClientUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<BookingCreateRequestDto>, BookingCreateRequestDtoValidator>();
            services.AddScoped<IValidator<BookingUpdateRequestDto>, BookingUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<StaffCreateRequestDto>, StaffCreateRequestDtoValidator>();
            services.AddScoped<IValidator<StaffUpdateRequestDto>, StaffUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<ShiftCreateRequestDto>, ShiftCreateRequestDtoValidator>();
            services.AddScoped<IValidator<ShiftUpdateRequestDto>, ShiftUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<CleaningTaskCreateRequestDto>, CleaningTaskCreateRequestDtoValidator>();
            services.AddScoped<IValidator<CleaningTaskUpdateRequestDto>, CleaningTaskUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<NotificationCreateRequestDto>, NotificationCreateRequestDtoValidator>();
            services.AddScoped<IValidator<NotificationUpdateRequestDto>, NotificationUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<AuditLogCreateRequestDto>, AuditLogCreateRequestDtoValidator>();

            return services;
        }
    }
}