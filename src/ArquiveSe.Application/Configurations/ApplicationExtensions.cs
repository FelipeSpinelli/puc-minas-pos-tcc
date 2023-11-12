using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Commands;
using ArquiveSe.Application.UseCases.Queries;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ApplicationException).Assembly);
            })
            .AddScoped<ICreateDocumentUseCase, CreateDocumentUseCase>()
            .AddTransient<IRequestHandler<CreateDocumentInput, CreateDocumentOutput>, CreateDocumentUseCase>()
            .AddScoped<IAddDocumentFileChunkUseCase, AddDocumentFileChunkUseCase>()
            .AddTransient<IRequestHandler<AddDocumentFileChunkInput, NoOutput>, AddDocumentFileChunkUseCase>()
            .AddScoped<IAddDocumentReviewUseCase, AddDocumentReviewUseCase>()
            .AddTransient<IRequestHandler<AddDocumentReviewInput, NoOutput>, AddDocumentReviewUseCase>()
            .AddScoped<IGetDocumentDetailUseCase, GetDocumentDetailUseCase>()
            .AddScoped<IGetDocumentStreamUseCase, GetDocumentStreamUseCase>()
            .AddScoped<IGetMasterListUseCase, GetMasterListUseCase>();
    }
}
