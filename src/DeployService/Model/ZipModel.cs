using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DeployService.Model
{
    public class ZipModel
    {
        //[Required]
        //public IFormFile File { get; set; }
        [Required]
        public HttpContent ZipFile { get; set; }
        [Required]
        public string SaveZipFolder { get; set; }
        [Required]
        public string DeployFolder { get; set; }
        [Required]
        public string DeployDescription { get; set; }
        [Required]
        public string DeployKey { get; set; }
    }

}
