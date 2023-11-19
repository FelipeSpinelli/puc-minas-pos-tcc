using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Dtos;

namespace ArquiveSe.Api.Contracts.v1.Requests;

public class CreateDocumentRequest :
    IToInputConverter<CreateDocumentInput>,
    IToInputConverter<IEnumerable<AddDocumentFileChunkInput>>
{
    public string FolderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Approvers { get; set; } = null!;
    public string Reviewers { get; set; } = null!;
    public bool InheritFolderPermissions { get; set; }
    public IFormFile? File { get; set; }

    public void ToInput(out CreateDocumentInput input)
    {
        input = new CreateDocumentInput
        {
            ExternalId = Guid.NewGuid().ToString("N"),
            FolderId = FolderId,
            Name = Name,
            Type = Type,
            InheritFolderPermissions = InheritFolderPermissions,
            ExpectedSize = (ulong)File!.Length,
            CustomPermissions = new PermissionsDto
            {
                Approvers = (Approvers ?? string.Empty).Split(','),
                Reviewers = (Reviewers ?? string.Empty).Split(',')
            }
        };
    }

    public void ToInput(out IEnumerable<AddDocumentFileChunkInput> input)
    {
        const int MAX_CHUNK_SIZE = 4096;

        var count = (int)((File!.Length % 4096 == 0 ? 0 : 1) + (File!.Length / 4096));
        var chunks = new List<AddDocumentFileChunkInput>(count);
        var fileBytes = GetFileBytes();

        for (int i = 0; i < count; i++)
        {
            var position = i * MAX_CHUNK_SIZE;
            var chunkBytes = position + MAX_CHUNK_SIZE <= fileBytes.Length ?
                    fileBytes[position..(position + MAX_CHUNK_SIZE)] :
                    fileBytes[position..];

            chunks.Add(new AddDocumentFileChunkInput
            {
                Position = position,
                Base64Chunk = Convert.ToBase64String(chunkBytes)
            });
        }

        input = chunks.ToArray();
    }

    private byte[] GetFileBytes()
    {
        var ms = new MemoryStream();
        File!.CopyTo(ms);
        return ms.ToArray();
    }
}
