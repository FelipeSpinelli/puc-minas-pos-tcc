using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Enumerators;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Application.UseCases.Commands;
public class CreateDocumentUseCase : BaseCommandUseCase, ICreateDocumentUseCase
{
    private readonly IFolderReadDbPort _folderReadDb;

    public CreateDocumentUseCase(
        IPersistenceDbPort persistenceDb,
        IFolderReadDbPort folderReadDb)
        : base(persistenceDb)
    {
        _folderReadDb = folderReadDb;
    }

    public async Task<CreateDocumentOutput> Execute(CreateDocumentInput input)
    {
        var folder = await _folderReadDb.GetFolderById(input.FolderId)
            ?? throw new ApplicationException($"Folder {input.FolderId} was not found!");

        var sequential = await _persistenceDb.GetNextDocumentSequentialToFolder(folder.Id);
        var permissions = input.InheritFolderPermissions ?
            folder.Permissions.Combine(input.CustomPermissions) :
            input.CustomPermissions ?? new();

        var document = new Document
        (
            input.ExternalId,
            folder.Id,
            folder.Code,
            sequential,
            input.Name,
            (EDocumentType)Enum.Parse(typeof(EDocumentType), input.Type),
            new Permissions(permissions.Reviewers, permissions.Approvers),
            input.ExpectedSize
        );

        await PersistEventsOf(document);

        return new()
        {
            Id = document.Id,
            Code = document.Code
        };
    }
}
