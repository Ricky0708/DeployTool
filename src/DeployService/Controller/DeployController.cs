using DeployService.Model;
using DeployService.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DeployReceiver.Controllers
{
    public class DeployController : ApiController
    {

        public static bool OnDeploying = false;

        [HttpPost]
        //[Produces("multipart/form-data")]
        public string Post()
        {
            try
            {
                List<ISeqTask> tasks = new List<ISeqTask>() {
                    new CheckKey(),
                    new SaveZip(),
                    new StopWebSite(),
                    new StopAppPool(),
                    new Deploy(),
                    new StartAppPool(),
                    new StartWebSite(),
                };
                Console.WriteLine("------Start Deployment " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "------");
                var provider = Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider()).Result;
                var deployContext = new DeployContext()
                {
                    DeployFolder = provider.FormData.GetValues("DeployFolder")[0],
                    SaveZipFolder = provider.FormData.GetValues("SaveZipFolder")[0],
                    DeployKey = provider.FormData.GetValues("DeployKey")[0],
                    DeployDescription = provider.FormData.GetValues("DeployDescription")[0],
                    WebSite = ConfigurationManager.AppSettings["WebSite"],
                    AppPoolName = ConfigurationManager.AppSettings["AppPoolName"],
                    ZipFileName = (provider.FormData.GetValues("SaveZipFolder")[0] + @"\" +
                             provider.Files[0].Headers.ContentDisposition.FileName).Replace(@"\\", @"\")
                            .Replace(".zip", $"_{provider.FormData.GetValues("DeployDescription")[0] + "_" + Guid.NewGuid().ToString()}.zip"),
                    ZipStream = provider.Files[0].ReadAsStreamAsync().Result,
                    AllowDeployKey = ConfigurationManager.AppSettings["AllowDeployKey"]
                };

                if (OnDeploying)
                {
                    Console.WriteLine("Deploy service is working..");
                    return "Deploy service is working..";
                }
                else
                {
                    OnDeploying = true;
                    foreach (var task in tasks)
                    {
                        Console.WriteLine(task.Invoke(deployContext));
                    }
                    OnDeploying = false;
                    Console.WriteLine("Success");
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                OnDeploying = false;
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// 網路copy來的，目的是為了解multi part data，到了mvc core版本可以直接使用Model內放置 IFormFile 的model來接收
        /// </summary>
        public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
        {
            private NameValueCollection _formData = new NameValueCollection();
            private List<HttpContent> _fileContents = new List<HttpContent>();

            // Set of indexes of which HttpContents we designate as form data
            private Collection<bool> _isFormData = new Collection<bool>();

            /// <summary>
            /// Gets a <see cref="NameValueCollection"/> of form data passed as part of the multipart form data.
            /// </summary>
            public NameValueCollection FormData
            {
                get { return _formData; }
            }

            /// <summary>
            /// Gets list of <see cref="HttpContent"/>s which contain uploaded files as in-memory representation.
            /// </summary>
            public List<HttpContent> Files
            {
                get { return _fileContents; }
            }

            public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
            {
                // For form data, Content-Disposition header is a requirement
                ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
                if (contentDisposition != null)
                {
                    // We will post process this as form data
                    _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

                    return new MemoryStream();
                }

                // If no Content-Disposition header was present.
                throw new InvalidOperationException(string.Format("Did not find required '{0}' header field in MIME multipart body part..", "Content-Disposition"));
            }

            /// <summary>
            /// Read the non-file contents as form data.
            /// </summary>
            /// <returns></returns>
            public override async Task ExecutePostProcessingAsync()
            {
                // Find instances of non-file HttpContents and read them asynchronously
                // to get the string content and then add that as form data
                for (int index = 0; index < Contents.Count; index++)
                {
                    if (_isFormData[index])
                    {
                        HttpContent formContent = Contents[index];
                        // Extract name from Content-Disposition header. We know from earlier that the header is present.
                        ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                        string formFieldName = UnquoteToken(contentDisposition.Name) ?? String.Empty;

                        // Read the contents as string data and add to form data
                        string formFieldValue = await formContent.ReadAsStringAsync();
                        FormData.Add(formFieldName, formFieldValue);
                    }
                    else
                    {
                        _fileContents.Add(Contents[index]);
                    }
                }
            }

            /// <summary>
            /// Remove bounding quotes on a token if present
            /// </summary>
            /// <param name="token">Token to unquote.</param>
            /// <returns>Unquoted token.</returns>
            private static string UnquoteToken(string token)
            {
                if (String.IsNullOrWhiteSpace(token))
                {
                    return token;
                }

                if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
                {
                    return token.Substring(1, token.Length - 2);
                }

                return token;
            }
        }
    }
}
