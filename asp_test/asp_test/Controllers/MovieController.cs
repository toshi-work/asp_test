using Microsoft.AspNetCore.Mvc;
using asp_test.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace asp_test.Controllers
{
    public class MovieController : BaseController
    {
        private readonly string err_msg = "データが見つかりません";

        // 親クラスのコンストラクタに渡す引数を設定
        public MovieController(asp_testContext context, ILogger<MovieController> logger) : base(context, logger){}


        public IActionResult Index()
        {
            try
            {
                IEnumerable<Movie> data = _context.Movies.ToArray();

                return View(data);
            } 
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        // GET: Movies/Details/3
        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }

                // idと一致するデータをMoviesテーブルから取得する
                Movie? data = _context.Movies.FirstOrDefault(d => d.Id == id);

                if (data == null)
                {
                    throw new Exception(err_msg);
                }

                return View(data);
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movies)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(movies);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(movies);
            }
            catch(Exception ex)
            {
                return this.Error(ex);
            }
        }

        // GET: Movies/Edit/3
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }

                Movie? movies = _context.Movies.Find(id);
                if (movies == null)
                {
                    throw new Exception(err_msg);
                }
                return View(movies);
            }
            catch(Exception ex)
            {
                return this.Error(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movies)
        {
            try
            {
                if (id != movies.Id)
                {
                    throw new Exception(err_msg);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(movies);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        if (!_context.Movies.Any(x => x.Id == movies.Id))
                        {
                            throw new Exception(err_msg);
                        }
                        else
                        {
                            throw new Exception(e.Message);
                        }
                    }
                    // 更新がうまく行けばIndex画面へリダイレクト
                    return RedirectToAction(nameof(Index));
                }
                // 更新処理が走らなかった場合は、そのままEditページを再表示
                return View(movies);
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        // GET: Movies/Delete/3
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }

                Movie? movies = _context.Movies.FirstOrDefault(m => m.Id == id);
                if (movies == null)
                {
                    throw new Exception(err_msg);
                }

                return View(movies);
            }
            catch(Exception ex)
            {
                return this.Error(ex);
            }
        }

        // POST: Movies/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                Movie? movies = _context.Movies.Find(id);
                
                if (movies == null)
                {
                    throw new Exception(err_msg);
                }

                _context.Movies.Remove(movies);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return this.Error(ex);
            }
        }
    }
}
