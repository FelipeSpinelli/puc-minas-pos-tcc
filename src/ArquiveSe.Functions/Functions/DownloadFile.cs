using ArquiveSe.Application.UseCases.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace ArquiveSe.Functions.Functions
{
    public class DownloadFile
    {
        private readonly IDownloadFileUseCase _useCase;

        public DownloadFile(
            IDownloadFileUseCase useCase)
        {
            _useCase = useCase;
        }

        [FunctionName("DownloadFile")]
        [OpenApiOperation(operationId: "DownloadFile", tags: new[] { "DownloadFile" })]
        [OpenApiSecurity("openid", SecuritySchemeType.OpenIdConnect, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(object), Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "files/download/{id}")] HttpRequest req, string id)
        {
            return new FileStreamResult(await _useCase.Execute(new(id)), "application/octet-stream");
        }
    }
}

