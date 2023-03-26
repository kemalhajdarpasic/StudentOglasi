using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentOglasi.Data;
using StudentOglasi.Helper;
using StudentOglasi.Models;
using StudentOglasi.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace StudentOglasi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SmjestajController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SmjestajController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult Snimi([FromForm] SmjestajVM x)
        {
            try
            {
                bool edit = false;
                Smjestaj smjestaj = new Smjestaj();

                if (x.id == 0)
                {
                    smjestaj = new Smjestaj();
                    _dbContext.Add(smjestaj);
                    smjestaj.VrijemeObjave = DateTime.Now;
                }
                else
                {
                    smjestaj = _dbContext.Smjestaj.Find(x.id);
                    edit = true;
                }

                smjestaj.RokPrijave = x.rokPrijave;
                smjestaj.Naslov = x.naslov;
                smjestaj.Opis = x.opis;
                smjestaj.Cijena = x.cijena;
                smjestaj.Kapacitet = x.kapacitet;
                smjestaj.DodatneUsluge = x.dodatneUsluge;
                smjestaj.BrojSoba = x.brojSoba;
                smjestaj.Parking = x.parking;
                smjestaj.NacinGrijanja = x.nacinGrijanja;
                smjestaj.GradID = x.gradID;
                smjestaj.IzdavacID = x.izdavacID;

                if (x.slika != null)
                {
                    if (x.slika.Length > 500 * 1000)
                        return BadRequest("max velicina fajla je 500 KB");

                    if (edit == true)
                    {
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/Slike/" + smjestaj.Slika;

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    string ekstenzija = Path.GetExtension(x.slika.FileName);
                    string contetntType = x.slika.ContentType;

                    var filename = $"{Guid.NewGuid()}{ekstenzija}";

                    var myFile = new FileStream(Config.SlikeFolder + filename, FileMode.Create);
                    x.slika.CopyTo(myFile);
                    smjestaj.Slika = filename;
                    myFile.Close();
                }
                _dbContext.SaveChanges();
                return Ok(smjestaj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException);
            }
        }

        [HttpGet]
        public ActionResult Get(int? izdavacID)
        {
            var data = _dbContext.Smjestaj.
                Where(x => izdavacID == null || x.IzdavacID == izdavacID)
                .OrderBy(x => x.ID)
                .Select(x => new
                {
                    id = x.ID,
                    naslov = x.Naslov,
                    rokPrijave = x.RokPrijave,
                    opis = x.Opis,
                    slika = Config.SlikePutanja + x.Slika,
                    vrijemeObjave = x.VrijemeObjave.ToString("dd.MM.yyyy H:mm"),
                    izdavacID = x.IzdavacID,
                    izdavac = x.Izdavac.Ime + ' ' + x.Izdavac.Prezime,
                    nacinGrijanja=x.NacinGrijanja,
                    cijena=x.Cijena,
                    kapacitet=x.Kapacitet,
                    dodatneUsluge=x.DodatneUsluge,
                    brojSoba=x.BrojSoba,
                    parking=x.Parking,
                    gradID=x.GradID,
                    grad_naziv=x.Grad.Naziv
                })
                .AsQueryable();
            return Ok(data);
        }

        [HttpPost]
        public ActionResult Obrisi([FromBody] int id)
        {
            Smjestaj x = _dbContext.Smjestaj.Find(id);
                string webRootPath = _hostingEnvironment.WebRootPath;
                var fullPath = webRootPath + "/Slike/" + x.Slika;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

            _dbContext.Remove(x);
            _dbContext.SaveChanges();

            return Ok();
        }

    }
}
