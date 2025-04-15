using System;
using System.Collections.Generic;

namespace imdb.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string? OriginalTitle { get; set; }

    public int? Budget { get; set; }

    public int? Popularity { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public long? Revenue { get; set; }

    public string? Title { get; set; }

    public double? VoteAverage { get; set; }

    public int? VoteCount { get; set; }

    public string? Overview { get; set; }

    public string? Tagline { get; set; }

    public int? Uid { get; set; }

    public int DirectorId { get; set; }
}
