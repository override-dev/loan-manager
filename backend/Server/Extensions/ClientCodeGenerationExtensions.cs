using FastEndpoints.ClientGen.Kiota;
using Kiota.Builder;

namespace Server.Extensions;

public static class ClientCodeGenerationExtensions
{
    public static async Task<WebApplication> GenerateClientCodeAsync(this WebApplication app)
    {
        await app.GenerateApiClientsAndExitAsync(
         c =>
         {
             c.SwaggerDocumentName = "v1";
             c.Language = GenerationLanguage.TypeScript;
             c.OutputPath = "../../frontend/app-client/openApi";
             c.ClientNamespaceName = "MyCompanyName";
             c.ClientClassName = "MyTsClient";
         });

        return app;
    }
}
