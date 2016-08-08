using FileUploadWithFormData.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FileUploadWithFormData.Controllers
{
    public class DealController : ApiController
    {
        Database1Entities dbContext = new Database1Entities();
        public IHttpActionResult Post()
        {
            string path = HttpContext.Current.Server.MapPath("~/Files/");

            var model = HttpContext.Current.Request.Form["DealModel"];

            var deal = JsonConvert.DeserializeObject<Deal>(model);

            if (deal == null)
            {
                return BadRequest();
            }
            // This is for demo. So I want to make this simple.
            deal.Id = Guid.NewGuid();

            var retval = dbContext.Deals.Add(deal);
            

            var files = HttpContext.Current.Request.Files;

            if (retval != null)
            {
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileName = UploadFile(file, path);
                        dbContext.Deal_Image.Add(new Deal_Image { Id = Guid.NewGuid(), DealId = deal.Id, ImageUrl = "/Files/"+fileName });
                    }
                }
                dbContext.SaveChanges();
                
            }
            return Ok();

        }

        private string UploadFile(HttpPostedFile file, string mapPath)
        {

            string fileName = new FileInfo(file.FileName).Name;

            if (file.ContentLength > 0)
            {
                Guid id = Guid.NewGuid();

                var filePath = Path.GetFileName(id.ToString() + "_" + fileName);

                if (!File.Exists(mapPath + filePath))
                {

                    file.SaveAs(mapPath + filePath);
                    return filePath;
                }
                return null;
            }
            return null;

        }

    }
}

