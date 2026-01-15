using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace MyBudgets;

internal sealed class BearerAuthenticationSchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        // JwtBearerDefaults.AuthenticationScheme == "Bearer"
        if (authenticationSchemes.All(s => s.Name != JwtBearerDefaults.AuthenticationScheme))
            return;

        // В .NET 10 пример использует IDictionary<string, IOpenApiSecurityScheme>
        var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            [JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",                // важно: HTTP auth scheme, обычно lowercase
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "Please insert JWT token"
                // Name = "Authorization" // для Http scheme обычно не требуется
            }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = securitySchemes;

        // Применяем требование ко всем операциям (как в доке для .NET 10)
        foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations ?? []))
        {
            operation.Value.Security ??= [];
            operation.Value.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
            });
        }
    }
}
