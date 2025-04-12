namespace Todo_App.Domain.Entities;

public class TodoItem : BaseAuditableEntity
{
    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }
    // ------------------------------------------------------------
    /// The background color to be applied to the Todo item.
    /// Should be provided in hexadecimal format (e.g., "#ffcc00").
    public string? BackgroundColor { get; set; }
    // ------------------------------------------------------------

    public PriorityLevel Priority { get; set; }





    public DateTime? Reminder { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public TodoList List { get; set; } = null!;
}
