using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.Models;
using MyFirstProject.Logic;

namespace MyFirstProject.Controllers
{
    public class EpisodesController : Controller
    {
        private readonly MyFirstProjectContext _context;

        public EpisodesController(MyFirstProjectContext context)
        {
            _context = context;    
        }

        // GET: Episodes
        //This should be async if possible, not sure how to do that.
        public IActionResult Index()
        {
            //Build list of episodes 

            return View(Connector.getTrendingEpisodes().Union(Connector.getLatestEpisodes()));
        }

        //This should be async if possible, not sure how to do that.
        public IActionResult Index_alt()
        {
            //How to inject lists as separate elements in razor ? I want to lists in the UI, one for trending episodes and one for latest episodes
            return View(Connector.getTrendingEpisodes().Union(Connector.getLatestEpisodes()));
        }

        //Original code, using async.
        //public async Task<IActionResult> Index()
        //{
        //    //Build list of episodes 
        //    List<Episode> anilinkzTrending = MyFirstProject.Logic.Connector.getTrendingEpisodes(_context);
        //    return View(anilinkzTrending);
        //    //return View(await _context.Episode.ToListAsync());
        //}

        public IActionResult ViewVideo(string detailsURL)
        {
            ViewData["videoLink"] = Connector.getVideoLink(detailsURL);
            return View();
        }

        // GET: Episodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var episode = await _context.Episode
                .SingleOrDefaultAsync(m => m.ID == id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        // GET: Episodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Episodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Anime,Number,Added")] Episode episode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(episode);
        }

        // GET: Episodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var episode = await _context.Episode.SingleOrDefaultAsync(m => m.ID == id);
            if (episode == null)
            {
                return NotFound();
            }
            return View(episode);
        }

        // POST: Episodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Anime,Number,Added")] Episode episode)
        {
            if (id != episode.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(episode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodeExists(episode.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(episode);
        }

        // GET: Episodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var episode = await _context.Episode
                .SingleOrDefaultAsync(m => m.ID == id);
            if (episode == null)
            {
                return NotFound();
            }

            return View(episode);
        }

        // POST: Episodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var episode = await _context.Episode.SingleOrDefaultAsync(m => m.ID == id);
            _context.Episode.Remove(episode);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EpisodeExists(int id)
        {
            return _context.Episode.Any(e => e.ID == id);
        }
    }
}
