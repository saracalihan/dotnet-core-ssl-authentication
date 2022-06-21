using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication5.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            LogHelper.LogError(string.Concat(LogHelper.LogType.Info, " ", MethodBase.GetCurrentMethod().Name, " Controller'a girdi."));

            return "Serifika doğrulamalı giriş başarıyla yapıldı";
        }
    }
}
