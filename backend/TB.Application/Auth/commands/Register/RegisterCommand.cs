using MediatR;
using TB.Application.Auth;
namespace TB.Application.Auth.commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string Role,
    string? CompanyName,
    string? Location
) : IRequest<AuthResponse>;