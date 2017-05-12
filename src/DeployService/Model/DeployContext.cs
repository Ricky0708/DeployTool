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
    public class DeployContext
    {
        //[Required]
        //public IFormFile File { get; set; }
        [Required]
        public string SaveZipFolder { get; set; }
        [Required]
        public string DeployFolder { get; set; }
        [Required]
        public string DeployDescription { get; set; }
        [Required]
        public string DeployKey { get; set; }
        public string WebSite { get; set; }
        public string AppPoolName { get; set; }
        public string ZipFileName { get; set; }
        public Stream ZipStream { get; set; }
        public string AllowDeployKey { get; set; }
    }


}
