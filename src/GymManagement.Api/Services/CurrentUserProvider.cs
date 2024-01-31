﻿using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using System.Security.Claims;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        Guid idClaim = GetClaimValues("id")
            .Select(Guid.Parse)
            .First();

        var permissions = GetClaimValues("permissions");

        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(
            Id: idClaim, 
            Permissions: permissions,
            Roles: roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType) => 
        _httpContextAccessor.HttpContext!.User.Claims
         .Where(claim => claim.Type == claimType)
         .Select(claim => claim.Value)
         .ToList();
}
