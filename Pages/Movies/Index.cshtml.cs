using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;

        }

        public IList<Movie> Movie { get; set; }

        //若要将属性绑定在 GET 请求上，请将 [BindProperty] 特性的 SupportsGet 属性设置为 true
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }


        public async Task OnGetAsync()
        {
            //Movie = await _context.Movie.ToListAsync();

            //var movies = from m in _context.Movie select m;
            //if (!string.IsNullOrEmpty(SearchString))
            //{
            //    movies = movies.Where(movie => movie.Title.Contains(SearchString));
            //}
            //Movie = await movies.ToListAsync()  ;

            //从数据库中搜索所有的Genre(类型)
            IQueryable<string> genreQuery = from m in _context.Movie orderby m.Genre select m.Genre;
            
            var movies = from m in _context.Movie select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(movie => movie.Genre == MovieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
