using MediatR;
using TB.Application.Auth.DTOs;

namespace TB.Application.Auth.commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;