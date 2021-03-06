﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MrFixIt.Controllers
{
    public class WorkersController : Controller
    {
        private MrFixItContext db = new MrFixItContext();
        // GET: /<controller>/
        public IActionResult Index()
        {
            var thisWorker = db.Workers.Include(i =>i.Jobs).FirstOrDefault(i => i.UserName == User.Identity.Name);
            if (thisWorker != null)
            {
                return View(thisWorker);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Worker worker)
        {
            worker.UserName = User.Identity.Name;
            db.Workers.Add(worker); 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CompleteJob(int JobId)
        {
            var job = db.Jobs.FirstOrDefault(items => items.JobId == JobId);
            job.Completed = true;
            var worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            worker.Avaliable = true;
            db.Entry(job).State = EntityState.Modified;
            db.Entry(worker).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

        [HttpPost]
        public IActionResult ActiveJob(int JobId)
        {
            var job = db.Jobs.FirstOrDefault(items => items.JobId == JobId);
            var worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            job.Pending = true;
            worker.Avaliable = false;
            db.Entry(job).State = EntityState.Modified;
            db.Entry(worker).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

    }
}
