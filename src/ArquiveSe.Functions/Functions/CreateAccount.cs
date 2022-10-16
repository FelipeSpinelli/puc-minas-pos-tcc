using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace ArquiveSe.Functions.Functions
{
    public class CreateAccount
    {
        private readonly ICreateAccountUseCase _useCase;

        public CreateAccount(
            ICreateAccountUseCase useCase)
        {
            _useCase = useCase;
        }

        [FunctionName("CreateAccount")]
        [OpenApiOperation(operationId: "CreateAccount", tags: new[] { "CreateAccount" })]
        [OpenApiSecurity("openid", SecuritySchemeType.OpenIdConnect, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateAccountRequest), Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "accounts")] HttpRequest req)
        {
            var request = JsonConvert.DeserializeObject<CreateAccountRequest>(await req.ReadAsStringAsync());

            return new OkObjectResult(await _useCase.Execute(request));
        }
    }
}

