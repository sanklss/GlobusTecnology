using System;
using System.Collections.Generic;

namespace GlobusT.Models;

public partial class User
{
    public int Id { get; set; }

    public int IdRole { get; set; }

    public string FullName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdCompany { get; set; }

    public string ContactEmal { get; set; } = null!;

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
