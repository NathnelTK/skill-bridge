using MediatR;
using TB.Application.Auth.DTOs;
using TB.Application.Common.Interfaces;

namespace TB.Application.Auth.commands.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        return _authService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);
    }
}