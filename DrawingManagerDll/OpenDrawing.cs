using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace DrawingManagerDll
{
    class OpenDrawing
    {
        public void openDrawing(string filePath)
        {
            Document oDoc = AcadApp.DocumentManager.Open(filePath, false);
        }
    }
}
