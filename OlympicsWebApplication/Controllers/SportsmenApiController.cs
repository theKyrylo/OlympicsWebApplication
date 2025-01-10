using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlympicsWebApplication.Models.Olympics;

namespace OlympicsWebApplication.Controllers;

[Route("/api/sportsmen")]
[ApiController]
public class SportsmenApiController(OlympicsDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetFilteredSportsmen(string filter)
    {
        return Ok(context.People
            .Where(o => o.FullName != null && o.FullName.Contains(filter.ToLower()))
            .OrderBy(o => o.FullName)
            .AsNoTracking()
            .AsEnumerable());
    }
}