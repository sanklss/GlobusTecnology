using System;
using System.Collections.Generic;

namespace GlobusT.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IdDevelopmentArea { get; set; }

    public int Duration { get; set; }

    public DateTime Date { get; set; }

    public int Price { get; set; }

    public int IdTypeCommand { get; set; }

    public int FreeSlot { get; set; }

    public string Image { get; set; } = null!;

    public virtual DevelopmentArea IdDevelopmentAreaNavigation { get; set; } = null!;

    public virtual TypeCommand IdTypeCommandNavigation { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
