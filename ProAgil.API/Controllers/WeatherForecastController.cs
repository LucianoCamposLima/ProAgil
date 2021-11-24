using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.API.Data;
using ProAgil.API.Model;

namespace ProAgil.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public readonly DataContext _context;

        public WeatherForecastController(DataContext context)
        {
            _context = context;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        // public WeatherForecastController(ILogger<WeatherForecastController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("v1")]
        public IEnumerable<Evento> GetEventos()
        {
            return new Evento[] {
                new Evento()
                {
                    EventoId = 1,
                    Tema = "Angular e .Net Core",
                    Local = "BH",
                    Lote = "1º Lote",
                    QtdPessoas = 100,
                    DataEvento = DateTime.Now.AddDays(2).ToString("dd/mm/yyyy")
                },
                new Evento()
                {
                    EventoId = 2,
                    Tema = "Java",
                    Local = "BH",
                    Lote = "1º Lote",
                    QtdPessoas = 10,
                    DataEvento = DateTime.Now.AddDays(2).ToString("dd/mm/yyyy")
                }
            };
        }

        [HttpGet("v1/{id}")]
        public ActionResult<Evento> GetEventosById(int id)
        {
            return new Evento[] {
                new Evento()
                {
                    EventoId = 1,
                    Tema = "Angular e .Net Core",
                    Local = "BH",
                    Lote = "1º Lote",
                    QtdPessoas = 100,
                    DataEvento = DateTime.Now.AddDays(2).ToString("dd/mm/yyyy")
                },
                new Evento()
                {
                    EventoId = 2,
                    Tema = "Java",
                    Local = "BH",
                    Lote = "1º Lote",
                    QtdPessoas = 10,
                    DataEvento = DateTime.Now.AddDays(2).ToString("dd/mm/yyyy")
                }
            }.FirstOrDefault(e => e.EventoId == id);
        }

        [HttpGet("v2")]
        public ActionResult<IEnumerable<Evento>> GetEventosContext()
        {
            return _context.Eventos.ToList();
        }

        [HttpGet("v2/{id}")]
        public ActionResult<Evento> GetEventosContextById(int id)
        {
            return _context.Eventos.FirstOrDefault(e => e.EventoId == id);
        }

        [HttpGet("v3/{id}")]
        public ActionResult<Evento> GetEventosContextByIdTryCatch(int id)
        {
            try
            {
                var results = _context.Eventos.FirstOrDefault(e => e.EventoId == id); 
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database error!");
            }     
        }

        [HttpGet("v4")]
        public async Task<ActionResult<Evento>> GetEventosAsync(int id)
        {
            try
            {
                var results = await _context.Eventos.ToListAsync(); 
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database error!");
            }     
        }
    }
}
