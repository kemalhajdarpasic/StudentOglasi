﻿using StudentOglasi.Models;

namespace StudentOglasi.ViewModels
{
    public class PrijavaPraksaVM
    {
       
        public int StudentId { get; set; }
        public string studentIme{ get; set; }
        public int PraksaId { get; set; }
        public IFormFile? propratnoPismo { get; set; }
        public IFormFile? CV { get; set; }
        public IFormFile? certifikati { get; set; }
    }
}