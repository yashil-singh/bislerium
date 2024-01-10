
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace bislerium.Data
{
    public class PDFService
    {
        public static void SavePDF()
        {
            var filePath = Utils.GetAppDirectoryPath();
            if(File.Exists(filePath))
            {

            }
        }
    }
}
