using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    public class CurriculumController : Controller
    {
        // GET: CurriculumController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CurriculumController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CurriculumController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurriculumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: CurriculumController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CurriculumController/Edit/5
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

        // GET: CurriculumController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CurriculumController/Delete/5
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
    }
}
