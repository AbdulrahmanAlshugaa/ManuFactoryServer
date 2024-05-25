using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edex.GeneralObjects.GeneralClasses
{

    public static class ControlExtensions
    {
        //public static void SetTabIndexForControls(Control container)
        //{
        //    int tabIndex = 1;

        //    List<Control> interactiveControls = new List<Control>();
        //    foreach (Control control in FindControlsRecursive(container))
        //    {
        //        if (control is TextEdit || control is LookUpEdit)
        //        {
        //            if (control.TabStop)
        //            {
        //                interactiveControls.Add(control);
        //            }
        //        }
        //    }
        //    interactiveControls.Sort((c1, c2) =>
        //    {
        //        int comparison = c2.Top.CompareTo(c1.Top);
        //        if (comparison == 0)
        //        {
        //            comparison = c2.Right.CompareTo(c1.Right);
        //        }
        //        return comparison;
        //    });

        //    foreach (var control in interactiveControls)
        //    {
        //        control.TabIndex = tabIndex++;
        //    }
        //}

        private static IEnumerable<Control> FindControlsRecursive(Control root)
        {
            foreach (Control control in root.Controls)
            {
                yield return control;

                foreach (Control childControl in FindControlsRecursive(control))
                {
                    yield return childControl;
                }
            }
        }

  
        //public static void SetTabIndexForControls(Control container)
        //{
        //    // ابدأ من القيمة 1
        //    int tabIndex = 1;

        //    // حدد كل العناصر التفاعلية في النموذج وقم بترتيبها بناءً على الموقع العلوي والموقع الأيمن
        //    List<Control> interactiveControls = new List<Control>();
        //    foreach (Control control in FindControlsRecursive(container))
        //    {
        //        if (control.TabStop)
        //        {
        //            interactiveControls.Add(control);
        //        }
        //    }
        //    interactiveControls.Sort((c1, c2) =>
        //    {
        //        int comparison = c2.Top.CompareTo(c1.Top); // قارن الموقع العلوي
        //        if (comparison == 0)
        //        {
        //            comparison = c2.Right.CompareTo(c1.Right); // إذا كانت المواقع العلوية متساوية، قارن الموقع الأيمن
        //        }
        //        return comparison;
        //    });  // قم بترتيب العناصر بناءً على الموقع العلوي والموقع الأيمن

        //    // قم بتعيين TabIndex لكل عنصر
        //    foreach (var control in interactiveControls)
        //    {
        //        control.TabIndex = tabIndex++;
        //    }
        //}
        public static void SetTabIndexForControls(Control container)
        {
            int tabIndex = 1;
            List<Control> interactiveControls = new List<Control>();
            foreach (Control control in FindControlsRecursive(container))
            {
                if (control.TabStop)
                {
                    interactiveControls.Add(control);
                }
            }

            interactiveControls.Sort((c1, c2) =>
            {
                int comparison = c1.Top.CompareTo(c2.Top); // ترتيب من الأعلى إلى الأسفل
                if (comparison == 0)
                {
                    comparison = c1.Left.CompareTo(c2.Left); // ترتيب من اليمين إلى اليسار
                }
                return comparison;
            });

            foreach (var control in interactiveControls)
            {
                control.TabIndex = tabIndex++;
            }
        }





    
    }
}
