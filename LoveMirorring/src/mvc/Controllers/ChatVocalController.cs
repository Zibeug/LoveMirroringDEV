using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace mvc.Controllers
{
    public class ChatVocalController : Controller
    {
        public IStringLocalizer<MatchingController> _localizer;
        public ChatVocalController(IStringLocalizer<MatchingController> localizer)
        {
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}