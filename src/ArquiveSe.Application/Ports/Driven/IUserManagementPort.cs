using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Ports.Driven;

public interface IUserManagementPort
{
    Task<bool> CanReview(DocumentProjection document);
    Task<bool> CanApprove(DocumentProjection document);
}