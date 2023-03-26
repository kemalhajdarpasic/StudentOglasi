using System.Drawing;

namespace StudentOglasi.Helper
{
    public class Config
    {
        public static string SlikePutanja = "https://localhost:7296/Slike/";
        public static string CVPutanja = "https://localhost:7296/CV/";
        public static string Slike => "Slike/";
        public static string SlikeFolder => "wwwroot/" + Slike;
        public static string CV => "CV/";
        public static string CVFolder => "wwwroot/" + CV;
    }
}
