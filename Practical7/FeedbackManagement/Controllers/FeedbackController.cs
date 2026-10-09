using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FeedbackManagement.Models;

namespace FeedbackManagement.Controllers
{
    public class FeedbackController : Controller
    {
        // In-memory store (no database needed for this practical)
        private static List<Feedback> feedbacks = new List<Feedback>
        {
            new Feedback { Id = 1, Name = "Aarav Shah", Email = "aarav@example.com",
                Category = "Course", Rating = 5,
                Message = "The .NET lab sessions were very practical.",
                SubmittedOn = new DateTime(2026, 10, 5) },
            new Feedback { Id = 2, Name = "Diya Patel", Email = "diya@example.com",
                Category = "Faculty", Rating = 4,
                Message = "Explanations are clear, more examples would help.",
                SubmittedOn = new DateTime(2026, 10, 6) }
        };

        // GET: Feedback
        public ActionResult Index()
        {
            return View(feedbacks.OrderByDescending(f => f.SubmittedOn).ToList());
        }

        // GET: Feedback/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Id = feedbacks.Count + 1;
                feedback.SubmittedOn = DateTime.Now;
                feedbacks.Add(feedback);
                TempData["Success"] = "Thank you! Your feedback has been submitted.";
                return RedirectToAction("Index");
            }
            return View(feedback);
        }
    }
}
