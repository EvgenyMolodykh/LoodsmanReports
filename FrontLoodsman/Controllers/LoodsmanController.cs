using System.Web.Mvc;
using Loodsman.Core;   // ссылка на библиотеку

namespace FrontLoodsman.Controllers
{
    public class LoodsmanController : Controller
    {
        private readonly LoodsmanApi _api;

        public LoodsmanController()
        {
            // хост/порт как в тесте
            _api = new LoodsmanApi("ascon", 8076);
        }

        // Шаг 1: выбор БД (GET)
        [HttpGet]
        public ActionResult SelectDb()
        {
            string code;
            string dbList = _api.GetDatabaseList(out code);

            ViewBag.DbList = dbList;
            ViewBag.ErrCode = code;

            return View();
        }

        // Шаг 1: выбор БД (POST)
        [HttpPost]
        public ActionResult SelectDb(string dbName)
        {
            // сохраняем выбранную БД между запросами
            TempData["DbName"] = dbName;
            return RedirectToAction("Search");
        }

        // Шаг 2: форма поиска (GET)
        [HttpGet]
        public ActionResult Search()
        {
            if (TempData["DbName"] == null)
                return RedirectToAction("SelectDb");

            ViewBag.DbName = TempData["DbName"].ToString();
            TempData.Keep("DbName");
            return View();
        }

        // Шаг 2: выполнение поиска (POST)
        [HttpPost]
        public ActionResult Search(string typeName, string condition)
        {
            if (TempData["DbName"] == null)
                return RedirectToAction("SelectDb");

            string dbName = TempData["DbName"].ToString();
            TempData.Keep("DbName");

            string errCode, errMsg;

            // пока без проектов/сортировки/контекста — пустые строки
            object result = _api.FindObjects(
                dbName,
                typeName,
                projects: "",
                condition: condition,
                sort: "",
                context: "",
                user: "",
                options: "",
                out errCode,
                out errMsg);

            ViewBag.DbName = dbName;
            ViewBag.Type = typeName;
            ViewBag.Cond = condition;
            ViewBag.ErrCode = errCode;
            ViewBag.ErrMsg = errMsg;
            ViewBag.Result = result == null ? "null" : result.ToString();

            return View();
        }
    }
}
