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
    public class ReferentFakultetaController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReferentFakultetaController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public ActionResult Snimi([FromForm] ReferentFakultetaVM x)
        {
            try
            {
                bool edit = false;
                ReferentFakulteta referent;
                if (x.id == 0)
                {
                    referent = new ReferentFakulteta();
                    _dbContext.Add(referent);
                }
                else
                {
                    referent = _dbContext.ReferentFakulteta.Find(x.id);
                    edit = true;
                }
                referent.Username = x.username;
                referent.Password = x.password;
                referent.Ime = x.ime;
                referent.Prezime = x.prezime;
                referent.Email = x.email;
                referent.FakultetID = x.fakultetID;
                if (x.slika != null)
                {
                    if (x.slika.Length > 500 * 1000)
                        return BadRequest("maksimalna velicina fajla je 500 KB");

                    if (edit == true)
                    {
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/Slike/" + referent.Slika;

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
                    referent.Slika = filename;
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
            var data = _dbContext.ReferentFakulteta.
                OrderBy(x => x.Ime)
                .Select(x => new
                {
                    id = x.ID,
                    password = x.Password,
                    username = x.Username,
                    ime = x.Ime,
                    prezime = x.Prezime,
                    slika = x.Slika,
                    email = x.Email,
                    fakultetID = x.FakultetID,
                    naziv_fakulteta = x.Fakultet.Naziv
                }).AsQueryable();
            return Ok(data);
        }


        [HttpGet]
        public ActionResult GetByID(int id)
        {
            var x = _dbContext.ReferentFakulteta.FirstOrDefault(s => s.ID == id);
            if (x != null)
            {
                var referent = new
                {
                    id = x.ID,
                    password = x.Password,
                    username = x.Username,
                    ime = x.Ime,
                    prezime = x.Prezime,
                    slika = x.Slika,
                    email = x.Email,
                    fakultetID = x.FakultetID,
                    naziv_fakulteta = _dbContext.Fakultet.Find(x.FakultetID)?.Naziv
                };
                return Ok(referent);
            }

            else return BadRequest();
        }

        [HttpPost]
        public ActionResult Obrisi([FromBody] int id)
        {
            ReferentFakulteta r = _dbContext.ReferentFakulteta.Find(id);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var fullPath = webRootPath + "/Slike/" + r.Slika;

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _dbContext.Remove(r);
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
