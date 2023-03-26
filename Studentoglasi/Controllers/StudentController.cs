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
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public StudentController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult Snimi([FromForm] StudentVM x)
        {
            try
            {
                bool edit = false;
                Student student;
                if (x.id == 0)
                {
                    student = new Student();
                    _dbContext.Add(student);
                }
                else
                {
                    student = _dbContext.Student.Find(x.id);
                    edit = true;
                }
                student.Username = x.username;
                student.Password = x.password;
                student.Ime = x.ime;
                student.Prezime = x.prezime;
                student.BrojIndeksa = x.broj_indeksa;
                student.FakultetID = x.fakultetID;
                student.GodinaStudija = x.godinaStudija;
                student.NacinStudiranja = x.nacin_studiranja;
                if (x.slika != null)
                {
                    if (x.slika.Length > 500 * 1000)
                        return BadRequest("maksimalna velicina fajla je 500 KB");

                    if (edit == true)
                    {
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        var fullPath = webRootPath + "/Slike/" + student.Slika;

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
                    student.Slika = filename;
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
            var data = _dbContext.Student
                .OrderBy(s => s.BrojIndeksa)
                .Select(s => new 
                {
                    id = s.ID,
                    password = s.Password,
                    username = s.Username,
                    ime = s.Ime,
                    prezime = s.Prezime,
                    broj_indeksa = s.BrojIndeksa,
                    nacin_studiranja = s.NacinStudiranja,
                    godinaStudija = s.GodinaStudija,
                    fakultetID = s.FakultetID,
                    naziv_fakulteta = s.Fakultet.Naziv,
                    slika = s.Slika
                }).AsQueryable();
            return Ok(data);
        }

        [HttpGet]
        public ActionResult GetByID(int id)
        {
            var student = _dbContext.Student.FirstOrDefault(s => s.ID == id);
            if (student != null)
            {
                var s = new
                {
                    id = student.ID,
                    password = student.Password,
                    username = student.Username,
                    ime = student.Ime,
                    prezime = student.Prezime,
                    broj_indeksa = student.BrojIndeksa,
                    nacin_studiranja = student.NacinStudiranja,
                    godinaStudija = student.GodinaStudija,
                    fakultetID = student.FakultetID,
                    naziv_fakulteta = _dbContext.Fakultet.Find(student.FakultetID)?.Naziv,
                    slika = student.Slika
                };
                return Ok(s);
            }

            else return BadRequest();
        }


        [HttpPost]
        public ActionResult Obrisi([FromBody] int id)
        {
            Student s = _dbContext.Student.Find(id);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var fullPath = webRootPath + "/Slike/" + s.Slika;
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _dbContext.Remove(s);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
