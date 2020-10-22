using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Data;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    [Authorize()]
    public class HomeController : Controller
    {
        private readonly ILogService _logService;
        private readonly ITargetAppService _service;

        public HomeController(ILogService logService, ITargetAppService service)
        {
            _logService = logService;
            _service = service;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return View("Index_Logged");
            }
            return View();
        }



        [HttpPost]
        public IActionResult List(Helpers.Datatable.DataRequest model)
        {
            var _result = _service.List().ToDataResult(model);
            return Ok(_result);
        }

        [HttpGet]
        public IActionResult GetItem(Guid id)
        {
            var _data = _service.Get(id);
            return Ok(_data);
        }

        [HttpPost]
        public IActionResult Save(TargetApp model)
        {
            _service.Add(model);
            return Ok(true);
        }

        [HttpPost]
        public IActionResult Update(TargetApp model)
        {
            _service.Update(model);
            return Ok(true);
        }

        [HttpGet]
        public IActionResult DeleteItem(Guid id)
        {
            _service.Delete(id);
            return Ok(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
