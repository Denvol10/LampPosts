using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampPosts.Models
{
    public class RevitGeometryUtils
    {
        // Выбор файла DWG
        public static ImportInstance GetDWGFileBySelection(UIApplication uiapp, out string dwgFileUniqueId)
        {
            Selection sel = uiapp.ActiveUIDocument.Selection;
            var selectedElement = sel.PickObject(ObjectType.Element, new ImportInstanceFilter(), "Select Dwg File");
            ImportInstance dwgFile = uiapp.ActiveUIDocument.Document.GetElement(selectedElement) as ImportInstance;
            dwgFileUniqueId = dwgFile.UniqueId;
            return dwgFile;
        }
    }
}
