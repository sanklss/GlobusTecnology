using System;
using System.Collections.Generic;

namespace GlobusT.Models;

public partial class DevelopmentArea
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
