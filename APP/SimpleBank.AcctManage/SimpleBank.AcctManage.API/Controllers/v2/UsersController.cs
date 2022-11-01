using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Mime;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> v2 placeholder </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        /// <summary> Test. </summary>
        public UsersController() { }

        /// <summary> Test. </summary>
        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult ReturnOk() => Ok("Yeehh!");




        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> UploadFile([FromForm] IFormFile file)
        {
            byte[] fileValue;
            using (Stream fileStream = file.OpenReadStream())
            {
                using (BinaryReader br = new BinaryReader(fileStream))
                {
                    fileValue = br.ReadBytes((Int32)fileStream.Length);
                }

                return Ok(new {
                    FilePath = Path.GetTempFileName(),
                    FilePath2 = Path.GetFileName(file.FileName),
                    FileType = file.ContentType, 
                    ContentDisposition = file.ContentDisposition,
                    sizeBy = (double) file.Length,
                    sizeKb = (double) file.Length / 1024,
                    sizeMb = (double) (file.Length / 1024) / 1024,
                    sizeGb = (double) ((file.Length / 1024) / 1024) / 1024,
                    FileValue = fileValue });
            }
        }







    }
}



/*
             using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (BinaryReader br = new BinaryReader(stream))
                {
                    fileValue = br.ReadBytes((Int32)stream.Length);
                }
            }
 */



/*
        [HttpPost("UploadFiles")]
        public IActionResult Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = Path.GetTempFileName();// full path to file in temp location

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }


        protected void Upload(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contentType = FileUpload1.PostedFile.ContentType;
            using (Stream fs = FileUpload1.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "insert into tblFiles values (@Name, @ContentType, @Data)";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@Name", filename);
                            cmd.Parameters.AddWithValue("@ContentType", contentType);
                            cmd.Parameters.AddWithValue("@Data", bytes);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            Response.Redirect(Request.Url.AbsoluteUri);
        }

 */