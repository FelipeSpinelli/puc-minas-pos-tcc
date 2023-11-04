namespace ArquiveSe.Application.Models.Dtos;

public record PermissionsDto
{
    public string[] Reviewers { get; set; }
    public string[] Approvers { get; set; }

    public PermissionsDto()
    {
        Reviewers = Array.Empty<string>();
        Approvers = Array.Empty<string>();
    }

    public PermissionsDto Combine(params PermissionsDto?[] @params)
    {
        var reviewers = new List<string>(Reviewers);
        var approvers = new List<string>(Approvers);

        foreach (var permissions in @params.Where(p => p is not null))
        {
            reviewers.AddRange(permissions!.Reviewers);
            approvers.AddRange(permissions!.Approvers);
        }

        return new()
        {
            Reviewers = reviewers.ToArray(),
            Approvers = approvers.ToArray()
        };
    }
}