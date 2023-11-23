using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Models.Dtos;
using ArquiveSe.Application.Models.Projections;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Commands;
using ArquiveSe.Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;

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
            .AddScoped<IJoinDocumentFileChunksUseCase, JoinDocumentFileChunksUseCase>()
            .AddTransient<IRequestHandler<JoinDocumentFileChunksInput, NoOutput>, JoinDocumentFileChunksUseCase>()
            .AddScoped<IAddDocumentReviewUseCase, AddDocumentReviewUseCase>()
            .AddTransient<IRequestHandler<AddDocumentReviewInput, NoOutput>, AddDocumentReviewUseCase>()
            .AddScoped<IGetDocumentDetailUseCase, GetDocumentDetailUseCase>()
            .AddScoped<IGetDocumentStreamUseCase, GetDocumentStreamUseCase>()
            .AddScoped<IGetMasterListUseCase, GetMasterListUseCase>();
    }

    public static IApplicationBuilder UseSeeds(this IApplicationBuilder app)
    {
        const string FLOW_ID = "be9b09c294d44f04bda8d628a962b039";
        const string FOLDER_ID = "6f99a3ca628c413090be0ef6840bdfc5";
        using var scope = app.ApplicationServices.CreateScope();
        var flowReadDb = scope.ServiceProvider.GetRequiredService<IFlowReadDbPort>()!;
        var folderReadDb = scope.ServiceProvider.GetRequiredService<IFolderReadDbPort>()!;

        flowReadDb.Upsert(new FlowProjection
        {
            Id = FLOW_ID,
            Name = "Default",
            CreatedAt = DateTime.UtcNow,
            Description = "Default document analysis flow",
            Permissions = new PermissionsDto
            {
                Reviewers = new[] { "review@email.com" },
                Approvers = new[] { "approver@email.com" }
            }
        });
        folderReadDb.Upsert(new FolderProjection
        {
            Id = FOLDER_ID,
            Name = "Default",
            CreatedAt = DateTime.UtcNow,
            FlowId = FLOW_ID,
            Permissions = new PermissionsDto
            {
                Reviewers = new[] { "review@email.com" },
                Approvers = new[] { "approver@email.com" }
            }
        });

        return app;
    }
}
