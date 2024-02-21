using Data;
using Entity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark Bookmark, string userId);
        List<Bookmark> GetBookmarks(string userId);
        Bookmark GetBookmark(int Id, string userId);
        void UpdateBookmark(Bookmark Bookmark, string userId);
        void DeleteBookmark(Bookmark Bookmark, string userId);
    }

    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        private readonly ILogger _logger;

        public BookmarkService(ReadLaterDataContext readLaterDataContext, ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(typeof(UserApiKeyService));
            _ReadLaterDataContext = readLaterDataContext;
        }

        public Bookmark CreateBookmark(Bookmark bookmark, string userId)
        {
            var existingCategory = _ReadLaterDataContext.Categories.Where(b => b.Name.ToLower() == bookmark.Category.Name.ToLower() && b.UserId.ToLower() == userId.ToLower()).FirstOrDefault();
            if (existingCategory != null)
            {
                bookmark.Category = existingCategory;
                bookmark.CategoryId = existingCategory.ID;
            } else
            {
                var newCategory = new Category()
                {
                    Name = bookmark.Category.Name
                };

                var addedCategory = _ReadLaterDataContext.Add(newCategory);
                _ReadLaterDataContext.SaveChanges();
                bookmark.Category = addedCategory.Entity;
                bookmark.CategoryId = addedCategory.Entity.ID;
            }

            bookmark.UserId = userId;

            _ReadLaterDataContext.Add(bookmark);
            _ReadLaterDataContext.SaveChanges();
            return bookmark;
        }

        public void UpdateBookmark(Bookmark bookmark, string userId)
        {
            var foundBookmark = _ReadLaterDataContext.UserApiKey.Where(u => u.ID == bookmark.ID && u.UserId.ToLower() == userId.ToLower()).FirstOrDefault();
            if (foundBookmark is null)
            {
                _logger.LogError("Unable to match bookmark with id {BookmarkId} with userId {userId}", bookmark.ID, userId);
                return;
            }
            _ReadLaterDataContext.Update(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Bookmark> GetBookmarks(string userId)
        {
            var bookmarks = _ReadLaterDataContext.Bookmark.Where(b => b.UserId.ToLower() == userId.ToLower()).ToList();
            foreach(var bookmark in bookmarks)
            {
                bookmark.Category = GetCategoryForBookmark(bookmark);
            }

            return bookmarks;
        }

        public Bookmark GetBookmark(int Id, string userId)
        {
            var bookmark = _ReadLaterDataContext.Bookmark.Where(b => b.ID == Id && b.UserId.ToLower() == userId.ToLower()).FirstOrDefault();
            bookmark.Category = GetCategoryForBookmark(bookmark);
            return bookmark;
        }

        private Category GetCategoryForBookmark(Bookmark bookmark)
        {
            return _ReadLaterDataContext.Categories.Where(c => c.ID == bookmark.CategoryId).FirstOrDefault();
        }

        public void DeleteBookmark(Bookmark bookmark, string userId)
        {
            var foundBookmark = _ReadLaterDataContext.Bookmark.Where(b => b.ID == bookmark.ID && b.UserId.ToLower() == userId.ToLower()).FirstOrDefault();
            if (foundBookmark != null)
            {
                _ReadLaterDataContext.Bookmark.Remove(foundBookmark);
            } else
            {
                _logger.LogError("Unable to match bookmark with id {BookmarkId} with userId {userId}", bookmark.ID, userId);
                return;
            }
            _ReadLaterDataContext.SaveChanges();
        }
    }
}
