﻿using Authorization;
using Authorization.DependencyInjection;
using Checks.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Shared.Infrastructure;

public static class Extensions
{
    private static void AddDbContext<TContext>(this IHostApplicationBuilder builder, string serviceName, Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext: DbContext
    {
        builder.AddNpgsqlDbContext<TContext>(serviceName, configureDbContextOptions:  optionsAction);
    }

    public static void AddDbContexts(this IHostApplicationBuilder builder)
    {
        builder.AddDbContext<ChecksContext>("HomeAccounting");
        builder.AddDbContext<AuthorizationContext>("HomeAccounting", options => options.SetUpAuthorizationForDevelopment());
    }
}