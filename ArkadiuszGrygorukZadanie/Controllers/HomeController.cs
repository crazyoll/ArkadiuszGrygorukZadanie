using ArkadiuszGrygorukZadanie.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArkadiuszGrygorukZadanie.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private static string IMDbId = null;
        //private static object obj = new object();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Moves_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Move> moves = db.Moves;
            DataSourceResult result = moves.ToDataSourceResult(request, move => new {
                Id = move.Id,
                IMDbId = move.IMDbId,
                Title = move.Title,
                Rating = move.Rating,
                ImageUrl = move.ImageUrl,
                ReleaseDate = move.ReleaseDate,
                Description = move.Description
            });

            return Json(result);
        }

        public async Task<JsonResult> MovieSearchResult(string text)
        {
            const string baseUrl = "https://imdb8.p.rapidapi.com/title/auto-complete?q=";
            if (text == null || text == "")
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
                text.Replace(" ", "%20");

            string url = baseUrl + text;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", "af1d9b5a35mshaf2fb7a5f8d3bb8p104cb1jsn08ecf7f60894" },
                    { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                },
            };
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            string data = await response.Content.ReadAsStringAsync();
            JObject parsedData = JObject.Parse(data);
            var selectedData = parsedData["d"];
            List<Move> output = new List<Move>();
            foreach (var d in selectedData)
            {
                output.Add(new Move
                {
                    IMDbId = (string)d["id"],
                    ImageUrl = (string)d["i"]["imageUrl"],
                    Title = (string)d["l"]
                });
            }

               return Json(output, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> CreateFromApi()
        {
            //string s;
            //if (Monitor.IsEntered(obj))
            //    s = "s";
            //else
            //    s = "p";
            if (IMDbId == "" || IMDbId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var client = new HttpClient();
            string url = "https://imdb8.p.rapidapi.com/title/get-overview-details?tconst=" + IMDbId + "&currentCountry=US";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", "af1d9b5a35mshaf2fb7a5f8d3bb8p104cb1jsn08ecf7f60894" },
                    { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                JObject parsedData = JObject.Parse(data);
                Move output = new Move()
                {
                    IMDbId = IMDbId,
                    Title = (string)parsedData["title"]["title"],
                    ImageUrl = (string)parsedData["title"]["image"]["url"],
                    Rating = (float)parsedData["ratings"]["rating"],
                    Description = (string)parsedData["plotSummary"]["text"],
                    ReleaseDate = (string)parsedData["releaseDate"]
                };
                return View(output);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFromApi([Bind(Include = "Id,IMDbId,Title,Rating,ImageUrl,ReleaseDate,Description")] Move move)
        {
            if (ModelState.IsValid)
            {
                db.Moves.Add(move);
                db.SaveChanges();
                IMDbId = null;
                return RedirectToAction("Index");
            }
            return View(move);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Move move = db.Moves.Find(id);
            if (move == null)
            {
                return HttpNotFound();
            }
            return View(move);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IMDbId,Title,Rating,ImageUrl,ReleaseDate,Description")] Move move)
        {
            if (ModelState.IsValid)
            {
                db.Moves.Add(move);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(move);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Move move = db.Moves.Find(id);
            if (move == null)
            {
                return HttpNotFound();
            }
            return View(move);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IMDbId,Title,Rating,ImageUrl,ReleaseDate,Description")] Move move)
        {
            if (ModelState.IsValid)
            {
                db.Entry(move).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(move);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Move move = db.Moves.Find(id);
            if (move == null)
            {
                return HttpNotFound();
            }
            return View(move);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Move move = db.Moves.Find(id);
            db.Moves.Remove(move);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public void SetTempData(string id)
        {
            IMDbId = id;
        }
    }
}