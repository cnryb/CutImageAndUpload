using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication11.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(dynamic dyn)
        {
            int x = int.Parse(Request.Form["x"]);
            int y = int.Parse(Request.Form["y"]);
            int w = int.Parse(Request.Form["w"]);
            int h = int.Parse(Request.Form["h"]);

            HttpPostedFileBase file = Request.Files[0];

            CropImage(file, w, h, x, y);

            return View();

        }

        private bool CropImage(HttpPostedFileBase originaImgPath, int width, int height, int x, int y)
        {
            byte[] cropImage = Crop(originaImgPath, width, height, x, y);

            System.IO.File.WriteAllBytes(Server.MapPath("t.jpg"), cropImage);
            return true;
        }

        /// <summary>
        /// 剪裁图像
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="file"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private byte[] Crop(HttpPostedFileBase file, int width, int height, int x, int y)
        {
            try
            {
                //file.InputStream

                using (var originalImage = new Bitmap(file.InputStream))
                {
                    using (var bmp = new Bitmap(width, height, originalImage.PixelFormat))
                    {
                        bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                        using (Graphics graphic = Graphics.FromImage(bmp))
                        {
                            graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphic.DrawImage(originalImage, new Rectangle(0, 0, width, height), x, y, width, height,
                                              GraphicsUnit.Pixel);
                            var ms = new MemoryStream();
                            bmp.Save(ms, originalImage.RawFormat);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }
    }
}
