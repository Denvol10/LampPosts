using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.Collections.ObjectModel;
using LampPosts.Models;
using LampPosts.Infrastructure;

namespace LampPosts
{
    public class RevitModelForfard
    {
        private UIApplication Uiapp { get; set; } = null;
        private Application App { get; set; } = null;
        private UIDocument Uidoc { get; set; } = null;
        private Document Doc { get; set; } = null;

        public RevitModelForfard(UIApplication uiapp)
        {
            Uiapp = uiapp;
            App = uiapp.Application;
            Uidoc = uiapp.ActiveUIDocument;
            Doc = uiapp.ActiveUIDocument.Document;
        }

        #region Выбор dwg файла
        public ImportInstance DwgFile { get; set; }

        private string _dwgFileUniqueId;
        public string DwgFileUniqueId
        {
            get => _dwgFileUniqueId;
            set => _dwgFileUniqueId = value;
        }

        public void GetDWGFileBySelection()
        {
            DwgFile = RevitGeometryUtils.GetDWGFileBySelection(Uiapp, out _dwgFileUniqueId);
        }

        public bool IsDwgFileExistInModel(string dwgFileUniqueId)
        {
            return RevitGeometryUtils.IsDwgFileExistInModel(Doc,dwgFileUniqueId);
        }

        public void GetDWGFileBySettings(string dwgFileUniqueId)
        {
            DwgFile = RevitGeometryUtils.GetDWGFileBySettings(Doc, dwgFileUniqueId);
        }

        public string GetDwgFileName()
        {
            string name = DwgFile.Category.Name;
            return name;
        }
        #endregion

        #region Список названий типоразмеров семейств
        public ObservableCollection<FamilySymbolSelector> GetFamilySymbolNames()
        {
            var familySymbolNames = new ObservableCollection<FamilySymbolSelector>();
            var allFamilies = new FilteredElementCollector(Doc).OfClass(typeof(Family)).OfType<Family>();
            var genericModelFamilies = allFamilies.Where(f => f.FamilyCategory.Id.IntegerValue == (int)BuiltInCategory.OST_GenericModel);
            if (genericModelFamilies.Count() == 0)
                return familySymbolNames;

            foreach (var family in genericModelFamilies)
            {
                foreach (var symbolId in family.GetFamilySymbolIds())
                {
                    var familySymbol = Doc.GetElement(symbolId);
                    familySymbolNames.Add(new FamilySymbolSelector(family.Name, familySymbol.Name));
                }
            }

            return familySymbolNames;
        }
        #endregion

        #region Создание экземпляров семейств
        public void CreatePostFamilyInstances(FamilySymbolSelector postFamilySymbol)
        {
            FamilySymbol postFSymbol = RevitGeometryUtils.GetFamilySymbolByName(Doc, postFamilySymbol);
            var locations = RevitGeometryUtils.GetLampPostLocation(DwgFile).Distinct(new LampPostLocationIEqualityComparer());

            using (Transaction trans = new Transaction(Doc, "Create LampPosts"))
            {
                trans.Start();
                if(!postFSymbol.IsActive)
                {
                    postFSymbol.Activate();
                }

                foreach(var location in locations)
                {
                    FamilyInstance postFamilyInstance = Doc.Create.NewFamilyInstance(location.Point,
                                                                                     postFSymbol,
                                                                                     Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    location.RotatePost();
                    postFamilyInstance.Location.Rotate(Line.CreateUnbound(location.Point, XYZ.BasisZ), location.RotationAngle);
                }
                trans.Commit();
            }
        }
        #endregion
    }
}
