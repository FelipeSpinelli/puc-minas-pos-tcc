using ArquiveSe.Application.Models.Queries.Inputs;

namespace ArquiveSe.Api.Contracts.v1.Requests;

public class GetMasterListRequest : IToInputConverter<GetMasterListInput>
{
    public string[] Status { get; set; } = Array.Empty<string>();

    public void ToInput(out GetMasterListInput input)
    {
        input = new GetMasterListInput
        {
            Page = 1,
            Size = 150,
            Status = Status
        };
    }
}