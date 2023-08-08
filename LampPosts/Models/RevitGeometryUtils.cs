using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (dwgFileUniqueId is null || doc.GetElement(dwgFileUniqueId) is null)
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

        #region Получение типоразмера по имени
        public static FamilySymbol GetFamilySymbolByName(Document doc, FamilySymbolSelector familyAndSymbolName)
        {
            var familyName = familyAndSymbolName.FamilyName;
            var symbolName = familyAndSymbolName.SymbolName;

            Family family = new FilteredElementCollector(doc).OfClass(typeof(Family)).Where(f => f.Name == familyName).First() as Family;
            var symbolIds = family.GetFamilySymbolIds();
            foreach (var symbolId in symbolIds)
            {
                FamilySymbol fSymbol = (FamilySymbol)doc.GetElement(symbolId);
                if (fSymbol.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM).AsString() == symbolName)
                {
                    return fSymbol;
                }
            }
            return null;
        }
        #endregion

        // Получение положения экземпляров семейств
        public static List<LampPostLocation> GetLampPostLocation(ImportInstance dwgFile)
        {
            Options options = new Options();
            var geometry = dwgFile.get_Geometry(options);
            var geomInstance = geometry.OfType<GeometryInstance>().First();
            var arcs = geomInstance.GetInstanceGeometry().OfType<Arc>();
            var lines = geomInstance.GetInstanceGeometry().OfType<Line>();

            var lampPostLocations = new List<LampPostLocation>();

            foreach (var geom in arcs.Zip(lines, Tuple.Create))
            {
                XYZ postOrigin = geom.Item1.Center;

                XYZ lineVector = geom.Item2.GetEndPoint(0) - geom.Item2.GetEndPoint(1);
                double rotationAngle = lineVector.AngleTo(XYZ.BasisY);

                var location = new LampPostLocation { Point = postOrigin, RotationAngle = rotationAngle, Vector = lineVector };
                lampPostLocations.Add(location);
            }

            return lampPostLocations;
        }
    }
}
