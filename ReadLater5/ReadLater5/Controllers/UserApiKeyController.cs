using Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;

namespace ReadLater5.Controllers
{
    [Authorize]
    public class UserApiKeyController : Controller
    {
        IUserApiKeyService _userApiKeyService;
        public UserApiKeyController(IUserApiKeyService UserApiKeyService)
        {
            _userApiKeyService = UserApiKeyService;
        }

        // GET: UserApiKey
        public IActionResult Index()
        {
            List<UserApiKey> model = _userApiKeyService.GetUserApiKeys(User.Identity.GetUserId());
            return View(model);
        }

        // GET: UserApiKey/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            UserApiKey UserApiKey = _userApiKeyService.GetUserApiKey((int)id, User.Identity.GetUserId());
            if (UserApiKey == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(UserApiKey);
        }

        // GET: UserApiKey/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserApiKey/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserApiKey UserApiKey)
        {
            if (ModelState.IsValid)
            {
                _userApiKeyService.CreateUserApiKey(UserApiKey, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }

            return View(UserApiKey);
        }

        // GET: UserApiKey/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            UserApiKey UserApiKey = _userApiKeyService.GetUserApiKey((int)id, User.Identity.GetUserId());
            if (UserApiKey == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(UserApiKey);
        }

        // POST: UserApiKey/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserApiKey UserApiKey)
        {
            if (ModelState.IsValid)
            {
                _userApiKeyService.UpdateUserApiKey(UserApiKey, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }
            return View(UserApiKey);
        }

        // GET: UserApiKey/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            UserApiKey UserApiKey = _userApiKeyService.GetUserApiKey((int)id, User.Identity.GetUserId());
            if (UserApiKey == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(UserApiKey);
        }

        // POST: UserApiKey/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            UserApiKey UserApiKey = _userApiKeyService.GetUserApiKey(id, User.Identity.GetUserId());
            _userApiKeyService.DeleteUserApiKey(UserApiKey, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }
    }
}
