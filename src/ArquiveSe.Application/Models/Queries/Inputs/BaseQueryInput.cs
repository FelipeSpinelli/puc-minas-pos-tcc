using System.Linq.Expressions;

namespace ArquiveSe.Application.Models.Queries.Inputs;

public abstract class BaseQueryInput<TProjection>
{
    public ushort Page { get; set; }
    public ushort Size { get; set; }
    public ushort Offset => (ushort)((Page - 1) * Size);

    public abstract Expression<Func<TProjection, bool>> Query { get; }
}
