using Microsoft.AspNetCore.Http;
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
    public class IzdavacSmjestajaController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        public IzdavacSmjestajaController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult Snimi([FromForm] IzdavacSmjestajaVM x)
        {
            try
            {
                bool edit = false;
                IzdavacSmjestaja izdavac;
                if (x.id == 0)
                {
                    izdavac = new IzdavacSmjestaja();
                    _dbContext.Add(izdavac);
                }
                else
                {
                    izdavac = _dbContext.IzdavacSmjestaja.Find(x.id);
                    edit = true;
                }
                izdavac.Username = x.username;
                izdavac.Password = x.password;
                izdavac.Ime = x.ime;
                izdavac.Prezime = x.prezime;
                izdavac.Telefon = x.broj_telefona;
                izdavac.Email = x.email;
                if (x.slika != null)
                {
                    if (x.slika.Length > 500 * 1000)
                        return BadRequest("maksimalna velicina fajla je 500 KB");

                    if (edit == true)
                    {
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/Slike/" + izdavac.Slika;

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
                    izdavac.Slika = filename;
                    myFile.Close();
                }

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
            var data = _dbContext.IzdavacSmjestaja.
                OrderBy(x => x.Ime)
                .Select(x => new
                {
                    id = x.ID,
                    password = x.Password,
                    username = x.Username,
                    ime = x.Ime,
                    slika = x.Slika,
                    prezime = x.Prezime,
                    broj_telefona = x.Telefon,
                    email = x.Email
                }).AsQueryable();
            return Ok(data);
        }

        [HttpGet]
        public ActionResult GetByID(int id)
        {
            var x = _dbContext.IzdavacSmjestaja.FirstOrDefault(s => s.ID == id);
            if (x != null)
            {
                var izdavac = new
                {
                    id = x.ID,
                    password = x.Password,
                    username = x.Username,
                    ime = x.Ime,
                    slika = x.Slika,
                    prezime = x.Prezime,
                    broj_telefona = x.Telefon,
                    email = x.Email
                };
                return Ok(izdavac);
            }

            else return BadRequest();
        }

        [HttpPost]
        public ActionResult Obrisi([FromBody] int id)
        {
            IzdavacSmjestaja izdavac = _dbContext.IzdavacSmjestaja.Find(id);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var fullPath = webRootPath + "/Slike/" + izdavac.Slika;

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _dbContext.Remove(izdavac);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
