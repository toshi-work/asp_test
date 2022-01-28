using asp_test.Models.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_test.Models
{
    public class CommentService
    {
        private readonly asp_testContext _context;
        public CommentService(asp_testContext context)
        {
            _context = context;
        }

        /// <summary>
        /// commentsテーブルとusersテーブルを結合してデータを取得
        /// </summary>
        /// <returns></returns>
        public IQueryable<CommentViewModel> GetComments()
        {
            IQueryable<CommentViewModel> query = from c in _context.Comments
                                                 join u in _context.Users
                                                 on c.Userid equals u.Id
                                                 select new CommentViewModel
                                                 {
                                                      Id = c.Id,
                                                      Movieid = c.Movieid,
                                                      Userid = c.Userid,
                                                      Comment1 = c.Comment1,
                                                      CreatedAt = c.CreatedAt,
                                                      UpdatedAt = c.UpdatedAt,
                                                      Name = u.Name,
                                                      Email = u.Email,
                                                      Gender = u.Gender
                                                 };
            return query;
        }

        /// <summary>
        /// Userid選択用のドロップダウンリストを生成
        /// </summary>
        /// <param name="userid">ユーザID</param>
        /// <returns></returns>
        public List<SelectListItem> GenUserlists(int? userid = null)
        {
            var list = new List<SelectListItem>();

            _context.Users.ToList().ForEach(u =>
            {
                list.Add(new SelectListItem
                {
                    Value = u.Id.ToString(),    // 実際の値
                    Text = u.Name,              // 画面上に表示する値
                    Selected = userid != null && u.Id == userid ? true : false
                });
            });

            return list;
        }

        /// <summary>
        /// コメント情報をcommentsテーブルに保存
        /// </summary>
        /// <param name="data"></param>
        public void SaveComment(Comment data)
        {
            string? wkSQL = null;

            wkSQL = "INSERT INTO comments (id, movieid, userid, comment, created_at) ";
            wkSQL += "VALUES (";
            wkSQL += "(SELECT CASE WHEN  MAX(id) is not null THEN MAX(id)+1 ELSE 1 END from comments), ";
            wkSQL += "{0}, {1}, {2}, {3})";    // SQLインジェクション対策

            // SQLを実行
            _context.Database.ExecuteSqlRaw(@wkSQL, data.Movieid, data.Userid, data.Comment1, data.CreatedAt);
        }

        /// <summary>
        /// コメント情報を更新する
        /// </summary>
        /// <param name="data"></param>
        public void UpdateComment(Comment data)
        {
            string? wkSQL = null;

            wkSQL = "UPDATE comments SET ";
            wkSQL += "id = {0}, movieid = {1}, userid = {2}, comment = {3}, updated_at = {4} WHERE id = {0}";

            // SQLを実行
            _context.Database.ExecuteSqlRaw(@wkSQL, data.Id, data.Movieid, data.Userid, data.Comment1, data.UpdatedAt!);
        }

        /// <summary>
        /// コメント情報を削除する
        /// </summary>
        /// <param name="id"></param>
        public void DeleteComment(int id)
        {
            string? wkSQL = null;

            wkSQL = "DELETE FROM comments WHERE id = {0}";

            // SQLを実行
            _context.Database.ExecuteSqlRaw(@wkSQL, id);
        }
    }
}
