using System;
using System.Collections.Generic;

namespace imdb.Models;

public partial class Director
{
    public string? Name { get; set; }

    public int Id { get; set; }

    public int? Gender { get; set; }

    public int? Uid { get; set; }

    public string? Department { get; set; }
}
