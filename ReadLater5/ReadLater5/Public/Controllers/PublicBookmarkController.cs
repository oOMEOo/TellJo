using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.Auth;
using ReadLater5.Public.Models;
using Services;
using System.Collections.Generic;

namespace ReadLater5.Public.Controllers
{
    [ApiController]
    [Route("public/bookmark/")]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
    public class PublicBookmarkController : Controller
    {
        IBookmarkService _bookmarkService;
        public PublicBookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        private IActionResult ResponseObject(int statusCode, object value)
        {
            /*var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };*/

            return new JsonResult(value) { StatusCode = statusCode, /*SerializerSettings = settings*/ };
        }

        // GET: Bookmark/Get/
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            return ResponseObject(StatusCodes.Status202Accepted, _bookmarkService.GetBookmarks(User.Identity.Name));
        }

        // GET: Bookmark/Get/5
        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Get([FromRoute] int? id = null)
        {
            return ResponseObject(StatusCodes.Status202Accepted, _bookmarkService.GetBookmark((int)id, User.Identity.Name));
        }

        // POST: Bookmark/Create
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Create(PublicBookmark publicBookmark)
        {
            if (publicBookmark == null)
            {
                // add other validation here for the individual fields of bookmark
                return ResponseObject(StatusCodes.Status400BadRequest, new GenericResponse() { IsSuccess = false, Errors = new List<string>() { "Bookmark cannot be null" } });
            }
            var bookmark = publicBookmark.GetFullBookmark();
            bookmark.UserId = User.Identity.Name;
            bookmark.Category.UserId = User.Identity.Name;

            _bookmarkService.CreateBookmark(bookmark, User.Identity.Name);
            return ResponseObject(StatusCodes.Status202Accepted, bookmark);
        }

        // PUT: Bookmark/Edit/5
        [Route("")]
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Edit(PublicBookmark bookmark)
        {
            if (bookmark == null)
            {
                // add other validation here for the individual fields of bookmark
                return ResponseObject(StatusCodes.Status400BadRequest, new GenericResponse() { IsSuccess = false, Errors = new List<string>() { "Bookmark cannot be null" } });
            }
            _bookmarkService.UpdateBookmark(bookmark.GetFullBookmark(), User.Identity.Name);
            return ResponseObject(StatusCodes.Status202Accepted, bookmark);
        }

        // DELETE: Bookmark/Delete/5
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            var userId = User.Identity.Name;
            Bookmark bookmark = _bookmarkService.GetBookmark(id, userId);
            _bookmarkService.DeleteBookmark(bookmark, userId);
            return ResponseObject(StatusCodes.Status202Accepted, new { IsSuccess = true });
        }
    }
}
