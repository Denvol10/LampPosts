using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampPosts.Models
{
    internal class ImportInstanceFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem is ImportInstance)
                return true;

            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
