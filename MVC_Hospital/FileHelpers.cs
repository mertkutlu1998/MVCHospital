using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MVC_Hospital
{
    public class FileHelpers
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string name)
        {
            if (file==null||string.IsNullOrEmpty(name)|| string.IsNullOrEmpty(folder))
            {
                return false;
            }
            try
            {
                string path = string.Empty;
                if (file!=null)
                {
                    path=Path.Combine(HttpContext.Current.Server.MapPath(folder),name);

                    WebImage img=new WebImage(file.InputStream);
                    if (img.Width>300)
                    {
                        img.Resize(300, 300);
                        img.Save(path);
                    }
                 
                }
                return true;
            }
            catch 
            {

                return false;
            }
      
        }
    }
}