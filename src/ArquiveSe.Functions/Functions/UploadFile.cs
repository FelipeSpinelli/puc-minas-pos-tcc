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
    public class UploadFile
    {
        private readonly IUploadFileUseCase _useCase;


        public UploadFile(
            IUploadFileUseCase useCase)
        {
            _useCase = useCase;
        }

        [FunctionName("UploadFile")]
        [OpenApiOperation(operationId: "UploadFile", tags: new[] { "UploadFile" })]
        [OpenApiSecurity("openid", SecuritySchemeType.OpenIdConnect, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(IFormCollection), Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "files/upload")] HttpRequest req)
        {
            var accountId = req.Headers["AccountId"].ToString();
            var userId = req.Headers["UserId"].ToString();
            await req.ReadFormAsync();

            return new OkObjectResult(await _useCase.Execute(new(accountId, userId, req.Form)));
        }
    }
}

