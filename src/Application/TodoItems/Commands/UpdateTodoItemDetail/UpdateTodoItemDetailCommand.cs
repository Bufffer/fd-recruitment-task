using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
    public string BackgroundColor { get; set; }

    public List<string> Tags { get; set; } = new();
}

public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
           .Include(t => t.Tags) // tags
           .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        if (request.Tags != null)
        {
            var existingTags = await _context.Tags
                .Where(t => request.Tags.Contains(t.Name))
                .ToListAsync(cancellationToken);

            var newTagNames = request.Tags.Except(existingTags.Select(t => t.Name)).ToList();

            foreach (var tagName in newTagNames)
            {
                var newTag = new Tag { Name = tagName };
                _context.Tags.Add(newTag);
                existingTags.Add(newTag);
            }

            entity.Tags = existingTags;
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
