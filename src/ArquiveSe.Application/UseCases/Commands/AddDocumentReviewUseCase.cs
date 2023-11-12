using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Application.UseCases.Commands;

public class AddDocumentReviewUseCase : BaseCommandUseCase<AddDocumentReviewInput, NoOutput>, IAddDocumentReviewUseCase
{
    public AddDocumentReviewUseCase(IPersistenceDbPort persistenceDb) : base(persistenceDb)
    {
    }

    public override async Task<NoOutput> Execute(AddDocumentReviewInput input)
    {
        var document = await _persistenceDb.LoadAggregate<Document>(input.Id)
            ?? throw new ApplicationException($"Document {input.Id} was not found!");

        document.AddReview(new Review
        {
            ReviewerId = input.ReviewerId,
            RequiresChanges = input.RequiresChanges,
            Comments = input.Comments,
            IsResolved = !input.RequiresChanges
        });

        await PersistEventsOf(document);

        return NoOutput.Value;
    }
}
