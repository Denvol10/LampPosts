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

        // Проверка на то существует ли файл dwg в модели
        public static bool IsDwgFileExistInModel(Document doc, string dwgFileUniqueId)
        {
            if(dwgFileUniqueId is null || doc.GetElement(dwgFileUniqueId) is null)
            {
                return false;
            }

            return true;
        }

        // Получение dwg файла из settings
        public static ImportInstance GetDWGFileBySettings(Document doc, string dwgFileUniqueId)
        {
            ImportInstance dwgFile = doc.GetElement(dwgFileUniqueId) as ImportInstance;

            return dwgFile;
        }
    }
}
