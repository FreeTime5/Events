using Events.Application.Mapping;
using Events.Application.Models.Account;
using Events.Application.Models.Event;
using Events.Application.Models.Member;
using Events.Application.UseCases.AccountUseCases.AddAdminUseCase;
using Events.Application.UseCases.AccountUseCases.AddAdminUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.CheckIsLoginUseCase;
using Events.Application.UseCases.AccountUseCases.CheckIsLoginUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.DeleteUserUseCase;
using Events.Application.UseCases.AccountUseCases.DeleteUserUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.GetUserUseCase;
using Events.Application.UseCases.AccountUseCases.GetUserUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.LoginUseCase;
using Events.Application.UseCases.AccountUseCases.LoginUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.LogoutUseCase;
using Events.Application.UseCases.AccountUseCases.LogoutUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.RefreshTokenUseCase;
using Events.Application.UseCases.AccountUseCases.RefreshTokenUseCase.Implementations;
using Events.Application.UseCases.AccountUseCases.RegisterUserAndLoginUseCase;
using Events.Application.UseCases.AccountUseCases.RegisterUserAndLoginUseCase.Implementations;
using Events.Application.UseCases.CategoryUseCases.AddCategoryUseCase;
using Events.Application.UseCases.CategoryUseCases.AddCategoryUseCase.Implementations;
using Events.Application.UseCases.CategoryUseCases.DeleteCategoryUseCase;
using Events.Application.UseCases.CategoryUseCases.DeleteCategoryUseCase.Implementations;
using Events.Application.UseCases.CategoryUseCases.GetAllCategoriesUseCase;
using Events.Application.UseCases.CategoryUseCases.GetAllCategoriesUseCase.Implementations;
using Events.Application.UseCases.CategoryUseCases.GetCategoryByIdUseCase;
using Events.Application.UseCases.CategoryUseCases.GetCategoryByIdUseCase.Implementaions;
using Events.Application.UseCases.CategoryUseCases.GetCategoryByNameUseCase;
using Events.Application.UseCases.CategoryUseCases.GetCategoryByNameUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.CreateEventUseCase;
using Events.Application.UseCases.EventUseCases.CreateEventUseCase.implementations;
using Events.Application.UseCases.EventUseCases.DeleteEventUseCase;
using Events.Application.UseCases.EventUseCases.DeleteEventUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.GetAllEventsUseCase;
using Events.Application.UseCases.EventUseCases.GetAllEventsUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.GetAllUsersRegistredOnEventUseCase;
using Events.Application.UseCases.EventUseCases.GetAllUsersRegistredOnEventUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.GetEventByIdUseCase;
using Events.Application.UseCases.EventUseCases.GetEventByIdUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.GetEventByNameUseCase;
using Events.Application.UseCases.EventUseCases.GetEventByNameUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.GetEventsWithPaginationUseCase;
using Events.Application.UseCases.EventUseCases.GetEventsWithPaginationUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.GetFilteredEventsUseCase;
using Events.Application.UseCases.EventUseCases.GetFilteredEventsUseCase.Implementations;
using Events.Application.UseCases.EventUseCases.UpdateEventUseCase;
using Events.Application.UseCases.EventUseCases.UpdateEventUseCase.Implementations;
using Events.Application.UseCases.MemberUseCases.AddMemberToEventUseCase;
using Events.Application.UseCases.MemberUseCases.AddMemberToEventUseCase.Implementations;
using Events.Application.UseCases.MemberUseCases.GetMembersOfEventUseCase;
using Events.Application.UseCases.MemberUseCases.GetMembersOfEventUseCase.Implementations;
using Events.Application.UseCases.MemberUseCases.RemoveMemberFromEventUseCase;
using Events.Application.UseCases.MemberUseCases.RemoveMemberFromEventUseCase.Implementations;
using Events.Application.UseCases.MemberUseCases.UpdateMemberInformationUseCase;
using Events.Application.UseCases.MemberUseCases.UpdateMemberInformationUseCase.Implementations;
using Events.Application.Validators.Account;
using Events.Application.Validators.Category;
using Events.Application.Validators.Event;
using Events.Application.Validators.Member;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Events.Application.Extensions;

public static class ExtensionMethods
{
    public static IServiceCollection AddAppUseCases(this IServiceCollection services)
    {
        services.AddValidators();

        services.AddAccountUseCases()
            .AddCategoryUseCases()
            .AddEventUseCases()
            .AddMemberUseCases();


        services.AddAutoMapper(typeof(ApplicationProfile));

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateEventRequestDTO>, CreateEventRequestValidator>();
        services.AddScoped<IValidator<UpdateEventRequestDTO>, UpdateEventRequestValidator>();
        services.AddScoped<IValidator<DeleteAndAddMemberRequestDTO>, DeleteAndAddMemberRequestValidator>();
        services.AddScoped<IValidator<LogInRequestDTO>, LogInRequestValidator>();
        services.AddScoped<IValidator<RegisterRequestDTO>, RegisterRequestValidator>();
        services.AddScoped<CategoryNameValidator>();
        services.AddScoped<PageValidator>();
        services.AddScoped<FilterItemValidator>();

        return services;
    }

    private static IServiceCollection AddAccountUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAddAdminUseCase, AddAdminUseCase>();
        services.AddScoped<ICheckIsLoginUseCase, CheckIsLoginUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<IGetUserUseCase, GetUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<ILogoutUseCase, LogoutUseCase>();
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddScoped<IRegisterUserAndLoginUseCase, RegisterUserAndLoginUseCase>();

        return services;
    }

    private static IServiceCollection AddCategoryUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAddCategoryUseCase, AddCategoryUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
        services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IGetCategoryByNameUseCase, GetCategoryByNameUseCase>();

        return services;
    }

    private static IServiceCollection AddEventUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
        services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();
        services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
        services.AddScoped<IGetAllUsersRegistredOnEventUseCase, GetAllUsersRegistredOnEventUseCase>();
        services.AddScoped<IGetEventByIdUseCase, GetEventByIdUseCase>();
        services.AddScoped<IGetEventByNameUseCase, GetEventByNameUseCase>();
        services.AddScoped<IGetEventsWithPaginationUseCase, GetEventsWithPaginationUseCase>();
        services.AddScoped<IGetFilteredEventsUseCase, GetFilteredEventsUseCase>();
        services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();

        return services;
    }

    private static IServiceCollection AddMemberUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAddMemberToEventUseCase, AddMemberToEventUseCase>();
        services.AddScoped<IGetMembersOfEventUseCase, GetMembersOfEventUseCase>();
        services.AddScoped<IRemoveMemberFromEventUseCase, RemoveMemberFromEventUseCase>();
        services.AddScoped<IUpdateMemberInformationUseCase, UpdateMemberInformationUseCase>();

        return services;
    }
}
