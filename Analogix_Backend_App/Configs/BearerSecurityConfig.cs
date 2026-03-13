using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Analogix_Backend_App.Presentation.WebAPI.Configs
{
    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {   
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authentictionSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (authentictionSchemes.Any(authScheme => authScheme.Name == "Bearer")) 
            {
                var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme> 
                {

                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                        Description = "Json Web Token"

                    }



                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = securitySchemes;

            }
           
        }


    }
}
