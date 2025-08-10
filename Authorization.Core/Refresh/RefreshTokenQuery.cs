using Authorization.Contracts;
using Authorization.Core.Login;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.Refresh;

public record RefreshTokenQuery(string RefreshToken) : IResultRequest<LoginResponse>;