﻿namespace OlympicsWebApplication.Models.Olympics;

public partial class Event
{
    public int Id { get; set; }

    public int? SportId { get; set; }

    public string? EventName { get; set; }

    public virtual Sport? Sport { get; set; }
}
