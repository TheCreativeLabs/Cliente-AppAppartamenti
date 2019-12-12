using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppAppartamentiWebCoreMvc.Models;
using Microsoft.Extensions.Configuration;
using AppAppartamenti.Api;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using AppAppartamentiWebCoreMvc.AppAppartamentiApiClient;
using static AppAppartamenti.Api.ApiHelper;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using AppAppartamentiWebCoreMvc.Extensions;
using AppAppartamentiWebCoreMvc.Utility;
using System.Drawing;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        IConfiguration _configuration;

        public SettingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> DetailAsync()
        {
            return View();
        }
    }
}
