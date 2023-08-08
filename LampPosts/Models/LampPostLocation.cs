using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampPosts.Models
{
    public class LampPostLocation
    {
        public XYZ Point { get; set; }
        public double RotationAngle { get; set; }
        public XYZ Vector { get; set; }

        public void RotatePost()
        {
            if (RotationAngle >= Math.PI / 2 && RotationAngle <= Math.PI && Vector.X > 0)
            {
                RotationAngle = -(RotationAngle - Math.PI);
            }
            else if (RotationAngle >= 0 && RotationAngle <= Math.PI / 2 && Vector.X > 0)
            {
                RotationAngle = Math.PI - RotationAngle;
            }
            else if (RotationAngle >= 0 && RotationAngle <= Math.PI / 2 && Vector.X < 0)
            {
                RotationAngle = Math.PI + RotationAngle;
            }
            else if (RotationAngle >= Math.PI / 2 && RotationAngle <= Math.PI && Vector.X < 0)
            {
                RotationAngle = RotationAngle + Math.PI;
            }
            else if (Vector.X == 0)
            {
                RotationAngle = RotationAngle + Math.PI;
            }
        }
    }
}
