using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlympicsWebApplication.Models;
using OlympicsWebApplication.Models.Olympics;

namespace OlympicsWebApplication.Controllers
{
    public class PersonController(OlympicsDbContext context) : Controller
    {
        // GET: Person
        public async Task<IActionResult> Index(int page = 1, int size = 20)
        {
            var totalSportsmen = await context.People.CountAsync();

            var pagedSportsmen = await PagingListAsync<AthleteViewModel>.CreateAsync((p, s) => 
                        Task.FromResult(context.People
                .Select(c => new AthleteViewModel()
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Gender = c.Gender,
                    Height = c.Height,
                    Weight = c.Weight,
                    GoldMedals = context.CompetitorEvents
                        .Where(medal => medal.Medal.MedalName == "Gold")
                        .Count(id => id.CompetitorId == c.Id),
                    SilverMedals = context.CompetitorEvents
                        .Where(medal => medal.Medal.MedalName == "Silver")
                        .Count(id => id.CompetitorId == c.Id),
                    BronzeMedals = context.CompetitorEvents
                        .Where(medal => medal.Medal.MedalName == "Bronze")
                        .Count(id => id.CompetitorId == c.Id),
                    StartCount = context.CompetitorEvents.Count(id => id.CompetitorId == c.Id)
                }
                )
                .OrderBy(person => person.Id)
                .Skip((p - 1) * s)
                .Take(s)), totalSportsmen,
                    page,
                    size);
            return View(pagedSportsmen); // Pass the pagedSportsmen directly to the view
        }

        public async Task<IActionResult> SportsmanEvents(int sportsmanId, int page = 1, int size = 20)
        {
            var totalEvents = await context.CompetitorEvents.Where(e => e.CompetitorId == sportsmanId).CountAsync();
            
            var pagedEvents = await PagingListAsync<AthleteParticipationViewModel>.CreateAsync((p, s) =>
                Task.FromResult(context.CompetitorEvents
                    .Select(e => new AthleteParticipationViewModel()
                    {
                        AthleteId = e.CompetitorId,
                        SportName = e.Event.Sport.SportName,
                        EventName = e.Event.EventName,
                        Olympiad = e.Competitor.Games.GamesName,
                        Season = e.Competitor.Games.Season,
                        Age = e.Competitor.Age,
                        Medal = e.Medal.MedalName
                    })
                    .Where(s => s.AthleteId == sportsmanId)
                    .OrderBy(ev => ev.EventName)
                    .Skip((p - 1) * s)
                    .Take(s)), totalEvents,
                page,
                size);
            return View(pagedEvents);
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Gender,Height,Weight")] Person person)
        {
            if (ModelState.IsValid)
            {
                context.Add(person);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Gender,Height,Weight")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(person);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await context.People.FindAsync(id);
            if (person != null)
            {
                context.People.Remove(person);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return context.People.Any(e => e.Id == id);
        }
    }
}
