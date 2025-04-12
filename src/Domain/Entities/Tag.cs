using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Domain.Entities;
public class Tag

{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<TodoItem> TodoItems { get; set; } = new();
}
