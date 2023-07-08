﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentOglasi.Data;
using StudentOglasi.Models;
using StudentOglasi.ViewModels;
using StudentOglasi.Helper;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace StudentOglasi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UposlenikFirmeController : ControllerBase
    {

        private readonly AppDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UposlenikFirmeController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult Snimi([FromForm] UposlenikFirmeVM x)
        {
            try
            {
                bool edit = false;
                UposlenikFirme uposlenik;
                if (x.id == 0)
                {
                    uposlenik = new UposlenikFirme();
                    _dbContext.Add(uposlenik);
                }
                else
                {
                    uposlenik = _dbContext.UposlenikFirme.Find(x.id);
                    edit = true;
                }
                uposlenik.Username = x.username;
                uposlenik.Password = x.password;
                uposlenik.Ime = x.ime;
                uposlenik.Prezime = x.prezime;
                uposlenik.Email = x.email;
                uposlenik.FirmaID = x.firmaID;
                uposlenik.Pozicija = x.pozicija;

                uposlenik.Slika = DodajFile.UploadSlike(x.slika, uposlenik.Slika, _hostingEnvironment);

                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException);
            }
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = _dbContext.UposlenikFirme.
                OrderBy(u => u.Ime)
                .Select(u => new 
                {
                    id = u.ID,
                    password = u.Password,
                    username = u.Username,
                    ime = u.Ime,
                    prezime = u.Prezime,
                    email = u.Email,
                    firmaID = u.FirmaID,
                    naziv_firme = u.Firma.Naziv,
                    pozicija=u.Pozicija,
                    slika = u.Slika
                }).AsQueryable();
            return Ok(data);
        }

        [HttpGet]
        public ActionResult GetByID(int id)
        {
            var x = _dbContext.UposlenikFirme.FirstOrDefault(s => s.ID == id);
            if (x != null)
            {
                var uposlenik = new
                {
                    id = x.ID,
                    password = x.Password,
                    username = x.Username,
                    ime = x.Ime,
                    prezime = x.Prezime,
                    email = x.Email,
                    firmaID = x.FirmaID,
                    naziv_firme = _dbContext.Firma.Find(x.FirmaID)?.Naziv,
                    pozicija = x.Pozicija,
                    slika = x.Slika
                };
                return Ok(uposlenik);
            }

            else return BadRequest();
        }

        [HttpPost]
        public ActionResult Obrisi([FromBody] int id)
        {
            UposlenikFirme u = _dbContext.UposlenikFirme.Find(id);
            DodajFile.ObrisiSliku(u.Slika, _hostingEnvironment);

            _dbContext.Remove(u);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
