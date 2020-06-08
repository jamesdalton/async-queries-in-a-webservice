using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/anime")]
    [ApiController]
    public class AnimeController : ControllerBase
    {
        IAnimeDataAccess dataAccess;

        public AnimeController(IAnimeDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        [HttpGet]
        public ActionResult<IEnumerable<long>> Get()
        {
            return Ok(dataAccess.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<Anime> Get(int id)
        {
            return Ok(dataAccess.Get(id));
        }

        [HttpGet("async/{id}")]
        public async Task<ActionResult<Anime>> GetAsync(int id)
        {
            return Ok(await dataAccess.GetAsync(id));
        }
    }
}
