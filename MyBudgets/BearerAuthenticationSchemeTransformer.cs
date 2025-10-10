using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace MyBudgets;

internal sealed class BearerAuthenticationSchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.All(scheme => scheme.Name != JwtBearerDefaults.AuthenticationScheme))
            return;

        var requirements = new Dictionary<string, OpenApiSecurityScheme>
        {
            {
                JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = "Please insert JWT token",
                    Name = "Authorization",
                }
            },
            // {
            //     $"{JwtBearerDefaults.AuthenticationScheme} password", new OpenApiSecurityScheme
            //     {
            //         Type = SecuritySchemeType.Http,
            //         Scheme = "password",
            //         In = ParameterLocation.Header,
            //         
            //     }
            // }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = requirements;
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            }
        );
    }
}