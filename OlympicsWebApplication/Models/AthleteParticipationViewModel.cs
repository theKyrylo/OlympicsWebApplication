namespace OlympicsWebApplication.Models;

public class AthleteParticipationViewModel
{
    public int? AthleteId { get; set; }
    public string SportName { get; set; }
    public string EventName { get; set; }
    public string Olympiad { get; set; }
    public string Season { get; set; }
    public int? Age { get; set; }
    public string Medal { get; set; }
}