using Microsoft.AspNetCore.Mvc;
using asp_test.Models;
using asp_test.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace asp_test.Controllers
{
    public class CommentController : BaseController
    {
        private readonly string err_msg = "データが見つかりません";
        public CommentController(asp_testContext context, ILogger<CommentController> logger) : base(context, logger) { }

        public IActionResult Index(int? movieid)
        {
            try
            {
                if (movieid == null)
                {
                    throw new ArgumentNullException("movieid");
                }

                var service = new CommentService(_context);

                // 選択した映画のIDに一致するコメントを取得
                IEnumerable<CommentViewModel> data = service.GetComments().Where(x => x.Movieid == movieid).ToArray();

                return View(data);
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        // GET: Movies/Create
        public IActionResult Create(int? movieid)
        {
            try
            {
                if (movieid == null)
                {
                    throw new ArgumentNullException("movieid");
                }

                var service = new CommentService(_context);
                RegisterCommentViewModel data = new RegisterCommentViewModel();

                // MovieidだけはIndexから情報を引き継ぐため、ここで設定
                data.Movieid = (int)movieid;

                // ドロップダウンリスト生成
                data.Userlist = service.GenUserlists();

                return View(data);
            } catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Movieid,Userid,Comment1")] RegisterCommentViewModel model)
        {
            // トランザクション開始
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new Comment();
                    var service = new CommentService(_context);

                    // 保存データの生成
                    data.Movieid = model.Movieid;
                    data.Userid = model.Userid;
                    data.Comment1 = model.Comment1;
                    data.CreatedAt = DateTime.Now;  // 現在日時を設定

                    service.SaveComment(data);

                    // コミット
                    transaction.Commit();

                    // クエリパラメータの設定
                    RouteValueDictionary param = new RouteValueDictionary()
                    {
                        {"movieid",  model.Movieid}
                    };

                    return RedirectToAction("index", "Comment", param);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                // ロールバック
                transaction.Rollback();

                return this.Error(ex);
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }

                var service = new CommentService(_context);

                // 選択したコメントのIDに一致するデータを取得
                CommentViewModel? subdata = service.GetComments().Where(x => x.Id == id).FirstOrDefault();

                if (subdata == null)
                {
                    throw new Exception(err_msg);
                }

                RegisterCommentViewModel data = new RegisterCommentViewModel();

                // Editページ用にカスタマイズ
                data.Id = (int)id;
                data.Movieid = subdata.Movieid;
                data.Userid = subdata.Userid;
                data.Comment1 = subdata.Comment1;

                // ドロップダウンリスト生成
                data.Userlist = service.GenUserlists(data.Userid);

                return View(data);
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Movieid,Userid,Comment1")] RegisterCommentViewModel model)
        {
            // トランザクション開始
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (id != model.Id)
                {
                    throw new Exception(err_msg);
                }

                if (ModelState.IsValid)
                {
                    var data = new Comment();
                    var service = new CommentService(_context);

                    // 保存データの生成
                    data.Id = model.Id;
                    data.Movieid = model.Movieid;
                    data.Userid = model.Userid;
                    data.Comment1 = model.Comment1;
                    data.UpdatedAt = DateTime.Now;  // 現在日時を設定

                    service.UpdateComment(data);
                    
                    // コミット
                    transaction.Commit();

                    // クエリパラメータの設定
                    RouteValueDictionary param = new RouteValueDictionary()
                    {
                        {"movieid",  model.Movieid}
                    };

                    return RedirectToAction("index", "Comment", param);
                }
                // 更新処理が走らなかった場合は、そのままEditページを再表示
                return View(model);
            }
            catch (Exception ex)
            {
                // ロールバック
                transaction.Rollback();

                return this.Error(ex);
            }
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }

                var service = new CommentService(_context);

                // 選択したコメントのIDに一致するデータを取得
                CommentViewModel? data = service.GetComments().Where(x => x.Id == id).FirstOrDefault();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // トランザクション開始
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var service = new CommentService(_context);

                // 削除前にリダイレクトする際に必要となるmovieidを取得
                CommentViewModel? data = service.GetComments().Where(x => x.Id == id).FirstOrDefault();

                if (data == null)
                {
                    throw new Exception(err_msg);
                }

                int movieid = data.Movieid;

                // 削除を実施
                service.DeleteComment(id);

                // コミット
                transaction.Commit();

                // クエリパラメータの設定
                RouteValueDictionary param = new RouteValueDictionary()
                {
                    {"movieid",  movieid}
                };

                return RedirectToAction("index", "Comment", param);
            }
            catch (Exception ex)
            {
                // ロールバック
                transaction.Rollback();

                return this.Error(ex);
            }
        }
    }
}
