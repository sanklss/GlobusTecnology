using System;
using System.Collections.Generic;

namespace GlobusT.Models;

public partial class Request
{
    public int Id { get; set; }

    public int IdService { get; set; }

    public int IdUser { get; set; }

    public DateTime Date { get; set; }

    public int IdStatusRequest { get; set; }

    public int CountNeedSpecialists { get; set; }

    public int TotalPrice { get; set; }

    public string? Comment { get; set; }

    public virtual Service IdServiceNavigation { get; set; } = null!;

    public virtual RequestStatus IdStatusRequestNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
