﻿using ArquiveSe.Application.Models.Commands.Outputs;
using MediatR;

namespace ArquiveSe.Application.Models.Commands.Inputs;

public record AddDocumentReviewInput : IRequest<NoOutput>
{
    public string Id { get; set; } = null!;
    public string ReviewerId { get; set; } = null!;
    public bool RequiresChanges { get; set; }
    public string Comments { get; set; } = null!;
}
