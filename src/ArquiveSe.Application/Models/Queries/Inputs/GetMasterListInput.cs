using ArquiveSe.Application.Models.Projections;
using System.Linq.Expressions;

namespace ArquiveSe.Application.Models.Queries.Inputs;
public class GetMasterListInput : BaseQueryInput<DocumentProjection>
{
    public string[] Status { get; set; } = null!;

    public override Expression<Func<DocumentProjection, bool>> Query => 
        document => !Status.Any() || Status.Contains(document.Status);
}