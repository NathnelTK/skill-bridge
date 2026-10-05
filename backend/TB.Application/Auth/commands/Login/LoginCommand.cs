using MediatR;

namespace TB.Application.Auth.commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;