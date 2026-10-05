using MediatR;
using TB.Application.Auth.DTOs;
using TB.Application.Common.Interfaces;
namespace TB.Application.Auth.commands.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return _authService.RegisterAsync(
            request.Email,
            request.Password,
            request.FullName,
            request.Role,
            request.CompanyName,
            request.Location,
            cancellationToken);
    }
}