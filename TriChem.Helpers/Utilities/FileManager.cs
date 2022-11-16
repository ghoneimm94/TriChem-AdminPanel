using System;
using System.IO;
using System.Web;

namespace TriChem.Helpers.Utilities
{
    public static class FileManager
    {
        public static string Upload(HttpPostedFileBase file, string directory)
        {
            if (file != null)
            {
                string fileName = DateTime.Now.ToString("yyMMddHHmmssfffffff");
                System.Threading.Thread.Sleep(1);
                var directoryPath = HttpContext.Current.Server.MapPath("~/" + directory);
                var extension = Path.GetExtension(file.FileName);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var fullPath = Path.Combine(directoryPath, fileName + extension);
                file.SaveAs(fullPath);
                return "http://trichem-eg.com" + directory + "/" + fileName + extension;
                // return  directory + "/" + fileName + extension;
            }
            return "";
        }

        public static void Delete(string filePath)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(filePath)))
                File.Delete(HttpContext.Current.Server.MapPath(filePath));
        }
    }
}
