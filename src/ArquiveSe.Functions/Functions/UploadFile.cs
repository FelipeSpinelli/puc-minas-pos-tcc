using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Functions.Authorization;
using ArquiveSe.Functions.Authorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Net.Http;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "files/upload")] HttpRequest req)
        {
            req.Headers.TryGetValue("Authorization", out var authorizationHeader);
            var authenticatedUser = await Authorizer.ValidateTokenAsync(authorizationHeader.ToString());
            if (!authenticatedUser.IsAuthenticated ||
                !authenticatedUser.IsAuthorized("UploadFile"))
            {
                return new UnauthorizedResult();
            }

            var accountId = req.Headers["AccountId"].ToString();
            var userId = req.Headers["UserId"].ToString();
            await req.ReadFormAsync();

            return new OkObjectResult(await _useCase.Execute(new(accountId, userId, req.Form)));
        }
    }
}

