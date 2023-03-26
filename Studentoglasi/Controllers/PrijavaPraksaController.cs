using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using StudentOglasi.Data;
using StudentOglasi.Helper;
using StudentOglasi.Models;
using StudentOglasi.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace StudentOglasi.Controllers
{
    public class PrijavaPraksaController : Controller
    {
          private readonly AppDbContext _dbContext;
          private readonly IHostingEnvironment _hostingEnvironment;
        public PrijavaPraksaController(AppDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
                _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

            [HttpPost]
            public ActionResult Snimi([FromForm] PrijavaPraksaVM x)
            {
                try
                {
                   
                    PrijavaPraksa prijava_praksa = new PrijavaPraksa();

                    if (x.StudentId == 0)
                    {
                        prijava_praksa = new PrijavaPraksa();
                        _dbContext.Add(prijava_praksa);
                    }
                    else
                    {
                        prijava_praksa = _dbContext.PrijavaPraksa.Find(x.StudentId);
                    }

               

                prijava_praksa.PraksaId = x.PraksaId;
                prijava_praksa.StudentId = x.StudentId;
                prijava_praksa.PropratnoPismo=x.propratnoPismo;
                prijava_praksa.CV = Config.CVPutanja + x.CV;
                prijava_praksa.Certifikati = x.certifikati;

              
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
                var data = _dbContext.PrijavaPraksa
                    .OrderBy(x => x.StudentId)
                    .Select(x => new
                    {
                        praksaID = x.PraksaId,
                        propratnoPismo = x.PropratnoPismo,
                        CV = x.CV,
                        certifikati = x.Certifikati,
                        studentIme=x.Student.Ime,
                        studentID=x.StudentId
                    })
                    .AsQueryable();
                return Ok(data);
            }

            [HttpPost]
            public ActionResult Obrisi([FromBody] int id)
            {
                PrijavaPraksa x = _dbContext.PrijavaPraksa.Find(id);

                string webRootPath = _hostingEnvironment.WebRootPath;
                var fullPath = webRootPath + "/CV/" + x.CV;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _dbContext.Remove(x);
                _dbContext.SaveChanges();

                return Ok();
            }
       
        
        [HttpPost("{id}")]
        public ActionResult AddCV(int id, [FromForm] PrijavaPraksaVM x)
        {
            try
            {
                PrijavaPraksa prijava = _dbContext.PrijavaPraksa.FirstOrDefault(p => p.StudentId == id);

                if (x.CV != null && prijava != null)
                {
                    if (x.CV.Length > 300 * 1000)
                        return BadRequest("max velicina fajla je 300 KB");

                    string ekstenzija = Path.GetExtension(x.CV.FileName);

                    var filename = $"{Guid.NewGuid()}{ekstenzija}";

                    x.CV.CopyTo(new FileStream(Config.CVFolder + filename, FileMode.Create));
                    prijava.CV = Config.CVPutanja + filename;
                    _dbContext.SaveChanges();
                }

                return Ok(prijava);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException);
            }
        }
    }
    }



