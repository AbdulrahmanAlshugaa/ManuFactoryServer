//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XamlStaticHelperNamespace {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")]
    internal class _XamlStaticHelper {
        
        private static System.WeakReference schemaContextField;
        
        private static System.Collections.Generic.IList<System.Reflection.Assembly> assemblyListField;
        
        internal static System.Xaml.XamlSchemaContext SchemaContext {
            get {
                System.Xaml.XamlSchemaContext xsc = null;
                if ((schemaContextField != null)) {
                    xsc = ((System.Xaml.XamlSchemaContext)(schemaContextField.Target));
                    if ((xsc != null)) {
                        return xsc;
                    }
                }
                if ((AssemblyList.Count > 0)) {
                    xsc = new System.Xaml.XamlSchemaContext(AssemblyList);
                }
                else {
                    xsc = new System.Xaml.XamlSchemaContext();
                }
                schemaContextField = new System.WeakReference(xsc);
                return xsc;
            }
        }
        
        internal static System.Collections.Generic.IList<System.Reflection.Assembly> AssemblyList {
            get {
                if ((assemblyListField == null)) {
                    assemblyListField = LoadAssemblies();
                }
                return assemblyListField;
            }
        }
        
        private static System.Collections.Generic.IList<System.Reflection.Assembly> LoadAssemblies() {
            System.Collections.Generic.IList<System.Reflection.Assembly> assemblyList = new System.Collections.Generic.List<System.Reflection.Assembly>();
            assemblyList.Add(Load("Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a" +
                        "3a"));
            assemblyList.Add(Load("Microsoft.VisualBasic, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f" +
                        "11d50a3a"));
            assemblyList.Add(Load("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
            assemblyList.Add(Load("System.ComponentModel.DataAnnotations, Version=4.0.0.0, Culture=neutral, PublicKe" +
                        "yToken=31bf3856ad364e35"));
            assemblyList.Add(Load("System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11" +
                        "d50a3a"));
            assemblyList.Add(Load("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
            assemblyList.Add(Load("System.Data.DataSetExtensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b" +
                        "77a5c561934e089"));
            assemblyList.Add(Load("System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
            assemblyList.Add(Load("System.Deployment, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50" +
                        "a3a"));
            assemblyList.Add(Load("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
            assemblyList.Add(Load("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" +
                        ""));
            assemblyList.Add(Load("System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856a" +
                        "d364e35"));
            assemblyList.Add(Load("System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c5619" +
                        "34e089"));
            assemblyList.Add(Load("System.Workflow.ComponentModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=" +
                        "31bf3856ad364e35"));
            assemblyList.Add(Load("System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
            assemblyList.Add(Load("System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e08" +
                        "9"));
            assemblyList.Add(Load("DevExpress.BonusSkins.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Charts.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b" +
                        "88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.CodeParser.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Dashboard.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToke" +
                        "n=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Dashboard.v18.1.Win, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Data.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d1754" +
                        "d700e49a"));
            assemblyList.Add(Load("DevExpress.DataAccess.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.DataAccess.v18.1.UI, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Docs.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d1754" +
                        "d700e49a"));
            assemblyList.Add(Load("DevExpress.ExpressApp.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Images.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d17" +
                        "54d700e49a"));
            assemblyList.Add(Load("DevExpress.Map.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d" +
                        "1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Office.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b" +
                        "88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Pdf.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d" +
                        "1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Persistent.Base.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyTok" +
                        "en=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Persistent.BaseImpl.v18.1, Version=18.1.4.0, Culture=neutral, PublicKe" +
                        "yToken=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.PivotGrid.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToke" +
                        "n=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Printing.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.RichEdit.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.RichEdit.v18.1.Export, Version=18.1.4.0, Culture=neutral, PublicKeyTok" +
                        "en=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Sparkline.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyToke" +
                        "n=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Utils.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d175" +
                        "4d700e49a"));
            assemblyList.Add(Load("DevExpress.Utils.v18.1.UI, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d" +
                        "1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Xpf.NavBar.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Xpf.TreeMap.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b" +
                        "88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.Xpo.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d" +
                        "700e49a"));
            assemblyList.Add(Load("DevExpress.XtraBars.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d" +
                        "1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraCharts.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraCharts.v18.1.UI, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraCharts.v18.1.Wizard, Version=18.1.4.0, Culture=neutral, PublicKeyT" +
                        "oken=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraEditors.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b" +
                        "88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraGauges.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKeyTok" +
                        "en=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraGauges.v18.1.Presets, Version=18.1.4.0, Culture=neutral, PublicKey" +
                        "Token=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraGauges.v18.1.Win, Version=18.1.4.0, Culture=neutral, PublicKeyToke" +
                        "n=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraGrid.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b88d" +
                        "1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraLayout.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraNavBar.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b8" +
                        "8d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraPivotGrid.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraPrinting.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=" +
                        "b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraReports.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=b" +
                        "88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraReports.v18.1.Extensions, Version=18.1.4.0, Culture=neutral, Publi" +
                        "cKeyToken=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraScheduler.v18.1.Core, Version=18.1.4.0, Culture=neutral, PublicKey" +
                        "Token=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraScheduler.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken" +
                        "=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraScheduler.v18.1.Reporting, Version=18.1.4.0, Culture=neutral, Publ" +
                        "icKeyToken=b88d1754d700e49a"));
            assemblyList.Add(Load("DevExpress.XtraTreeList.v18.1, Version=18.1.4.0, Culture=neutral, PublicKeyToken=" +
                        "b88d1754d700e49a"));
            assemblyList.Add(Load("Edex.DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            assemblyList.Add(Load("Edex.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            assemblyList.Add(Load("MySql.Data, Version=8.0.28.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d"));
            assemblyList.Add(Load("MySql.Data.EntityFramework, Version=8.0.28.0, Culture=neutral, PublicKeyToken=c56" +
                        "87fc88969c44d"));
            assemblyList.Add(Load("MySql.Web, Version=8.0.28.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d"));
            assemblyList.Add(Load("ZATKAQREncryption, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
            assemblyList.Add(System.Reflection.Assembly.GetExecutingAssembly());
            return assemblyList;
        }
        
        private static System.Reflection.Assembly Load(string assemblyNameVal) {
            System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName(assemblyNameVal);
            byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
            System.Reflection.Assembly asm = null;
            try {
                asm = System.Reflection.Assembly.Load(assemblyName.FullName);
            }
            catch (System.Exception ) {
                System.Reflection.AssemblyName shortName = new System.Reflection.AssemblyName(assemblyName.Name);
                if ((publicKeyToken != null)) {
                    shortName.SetPublicKeyToken(publicKeyToken);
                }
                asm = System.Reflection.Assembly.Load(shortName);
            }
            return asm;
        }
    }
}
