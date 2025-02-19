﻿using Demo_ASP.Handlers;
using Demo_ASP.Models.ClientViewModels;
using Demo_BLL.Entities;
using Demo_Common.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_ASP.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository<Client, int> _service;
        private readonly SessionManager _sessionManager;

        public ClientController(IClientRepository<Client, int> service, SessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        // GET: ClientController
        public ActionResult Index()
        {
            IEnumerable<ClientListItem> model = _service.Get().Select(e => e.ToListItem());
            return View(model);
        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientCreateForm form)
        {
            if (!ModelState.IsValid)
            {
                form.pass = null;
                form.confirmPass = null;
                return View(form);
            }
            else
            {
                int id = _service.Insert(form.ToBLL());
                return RedirectToAction("Details", "Client", new { id = id });
            }
        }

        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginForm form)
        {
            if (!ModelState.IsValid) return View();
            int? id = _service.CheckPassword(form.email, form.pass);
            if (id is null) return View();
            CurrentUser currentUser = new CurrentUser()
            {
                IdUser = (int)id,
                Email = form.email,
                LastConnection = DateTime.Now
            };
            _sessionManager.CurrentUser = currentUser;
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            _sessionManager.CurrentUser = null;
            /*
             * OU (Attention, suppression du cookie de session!)
            HttpContext.Session.Clear();
            */
            return RedirectToAction("Index", "Home");
        }
    }
}
