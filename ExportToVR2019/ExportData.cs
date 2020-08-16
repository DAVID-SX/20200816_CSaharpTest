using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExportToVR2020
{
    [Transaction(TransactionMode.Manual)]
    class ExportData : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 获取句柄
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // 调出存储文件的对话框
            string saveFilePathAndNameText = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = null;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            saveFilePathAndNameText = saveFileDialog.FileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
                }
            }



            string text2 = "_MouseOver";
            string directoryName = Path.GetDirectoryName(saveFilePathAndNameText);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(saveFilePathAndNameText);
            string str = fileNameWithoutExtension + text2;
            string path = null;
            path = string.Concat(new string[]
            {
                directoryName,
                "\\",
                fileNameWithoutExtension,
                text2,
                ".cs"
            });
            string path2 = null;
            if (File.Exists(saveFilePathAndNameText))
            {
                File.Delete(saveFilePathAndNameText);
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<double> list4 = new List<double>();
            List<string> list5 = new List<string>();
            List<string> list6 = new List<string>();
            List<string> list7 = new List<string>();
            List<string> list8 = new List<string>();
            List<string> list9 = new List<string>();
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            Hashtable hashtable3 = new Hashtable();
            Hashtable hashtable4 = new Hashtable();
            List<string> list10 = new List<string>();
            Hashtable hashtable5 = new Hashtable();
            try
            {
                // 收集元素并将其存放在ICollection中
                FilteredElementCollector familySymbolCollector = new FilteredElementCollector(doc);
                ICollection<Element> familySymbolCollection = familySymbolCollector.OfClass(typeof(FamilySymbol)).ToElements();
                FilteredElementCollector wallCollector = new FilteredElementCollector(doc);
                ICollection<Element> wallCollection = wallCollector.OfClass(typeof(Wall)).ToElements();
                FilteredElementCollector familyInstanceCollector = new FilteredElementCollector(doc);
                ICollection<Element> familyInstanceCollection = familyInstanceCollector.OfClass(typeof(FamilyInstance)).ToElements();


                Wall wall = null;
                List<Element> elementList = new List<Element>();
                List<Element> list12 = new List<Element>();
                List<ElementId> list13 = new List<ElementId>();
                List<string> list14 = new List<string>();

                // 获取项目中的所有实例，并将其存放在elementList
                FilteredElementCollector notElementTypeCollector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
                foreach (Element elem in notElementTypeCollector)
                {
                    if (elem.Category != null && elem.Category.HasMaterialQuantities)
                    {
                        elementList.Add(elem);
                    }
                    if (elem.Category != null && elem.Category.Id.IntegerValue == -2000171)
                    {
                        elementList.Add(elem);
                    }
                }

                // 获取项目中所有的三维视图并将其存放在_3DViewCollection
                FilteredElementCollector _3DViewCollector = new FilteredElementCollector(doc);
                ICollection<Element> _3DViewCollection = _3DViewCollector.OfClass(typeof(View3D)).ToElements();

                // 获取当前的三维视图，并将其存放在activeView3D
                View3D activeView3D = null;
                bool IsTemplate3DView = false;
                // this.m_AllViews.ExportProperties = true;
                if (doc.ActiveView.ViewType != ViewType.ThreeD)
                {
                    MessageBox.Show("The active view must be a 3D view type.");
                    return Result.Cancelled;
                }
                if (doc.ActiveView.ViewType == ViewType.ThreeD & doc.ActiveView.IsTemplate)
                {
                    MessageBox.Show("The active view is a template view and is not exportable.");
                    IsTemplate3DView = true;
                    return Result.Cancelled;
                }
                if (doc.ActiveView.ViewType == ViewType.ThreeD & !doc.ActiveView.IsTemplate)
                {
                    activeView3D = (doc.ActiveView as View3D);
                }

                // 获取族实例有面积参数且族类型有部件代码参数
                Element element = null;
                Element elementType = null;
                ICollection collection = null;
                List<string> list = new List<string>();

                if (saveFilePathAndNameText != null)
                {

                    foreach (Element elem in elementList)
                    {
                        element = doc.GetElement(elem.Id);
                        elementType = doc.GetElement(elem.GetTypeId());
                        if (element != null)
                        {
                            foreach (Parameter elemParameter in element.Parameters)
                            {
                                if (elemParameter.Definition.Name == "Area" || elemParameter.Definition.Name == "面积")
                                {
                                    foreach (Parameter elemTypeParameter in elementType.Parameters)
                                    {
                                        if (elemTypeParameter.Definition.Name == "Assembly Code" || elemTypeParameter.Definition.Name == "部件代码")
                                        {
                                            // 如果族类型的参数是字符串形式则判断
                                            if (elemTypeParameter.AsString() != null)
                                            {
                                                if (list.Contains(elemTypeParameter.AsString()))
                                                {
                                                    foreach (string text3 in collection)
                                                    {
                                                        if (elemTypeParameter.AsString() == text3)
                                                        {
                                                            double num = Convert.ToDouble(hashtable[text3]);
                                                            hashtable[text3] = num + elemParameter.AsDouble();
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (!list.Contains(elemTypeParameter.AsString()))
                                                {
                                                    hashtable.Add(elemTypeParameter.AsString(), elemParameter.AsDouble());
                                                    collection = hashtable.Keys;
                                                    list.Add(elemTypeParameter.AsString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    int num2 = 3;
                    bool checked2 = true;
                    if (checked2)
                    {
                        foreach (Element current3 in elementList)
                        {
                            element = doc.GetElement(current3.Id);
                            elementType = doc.GetElement(current3.GetTypeId());
                            bool flag19 = element != null;
                            if (flag19)
                            {
                                foreach (Parameter parameter3 in element.Parameters)
                                {
                                    bool flag20 = parameter3.Definition.Name == "Area";
                                    if (flag20)
                                    {
                                        bool flag21 = !list.Contains(element.Id.ToString());
                                        if (flag21)
                                        {
                                            hashtable.Add(num2.ToString(), element.Id.ToString());
                                            collection = hashtable.Keys;
                                            list.Add(element.Id.ToString());
                                            hashtable4.Add(num2.ToString(), parameter3.AsDouble());
                                            num2++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    int num3 = 3;
                    int num4 = 3;
                    
                        foreach (Element elem in elementList)
                        {
                            element = doc.GetElement(elem.Id);
                            elementType = doc.GetElement(elem.GetTypeId());
                            if (element != null)
                            {
                                if (!list.Contains(element.Id.ToString()))
                                {
                                    hashtable.Add(num3.ToString(), element.Id.ToString());
                                    collection = hashtable.Keys;
                                    list.Add(element.Id.ToString());
                                    num3++;
                                }
                            }
                        }
                        bool flag24 = true;
                        if (flag24)
                        {
                            foreach (string text4 in this.listBoxParameters.SelectedItems)
                            {
                                foreach (Element current5 in elementList)
                                {
                                    element = doc.GetElement(current5.Id);
                                    elementType = doc.GetElement(current5.GetTypeId());
                                    bool flag25 = element != null;
                                    if (flag25)
                                    {
                                        foreach (Parameter parameter4 in element.Parameters)
                                        {
                                            bool flag26 = parameter4.Definition.ParameterType == (ParameterType)4;
                                            if (flag26)
                                            {
                                                bool flag27 = parameter4.Definition.Name == text4;
                                                if (flag27)
                                                {
                                                    bool flag28 = !list2.Contains(text4);
                                                    if (flag28)
                                                    {
                                                        hashtable3.Add(num4.ToString(), text4);
                                                        ICollection keys = hashtable3.Keys;
                                                        list2.Add(text4);
                                                        num4++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    
                    bool checked4 = true;
                    if (checked4)
                    {
                        string str2 = "using UnityEngine;\nusing System.Collections.Generic;\n\npublic class " + fileNameWithoutExtension + " : MonoBehaviour\n{\nprivate List<string> paramNameList = new List<string>();\n\n";
                        string str3 = "public List<string> ParamNameList\n{\nget\n{\nreturn paramNameList;\n}\nset\n{\nparamNameList = value;\n}\n}\n\n";
                        string str4 = "public void TheList(string value)\n{\nif (value != null)\n{\nparamNameList.Add(ParamByName(value));\nparamNameList.Sort();\n}\n}\n\n";
                        string str5 = "private string ParamByName(string d)\n{\nswitch (d)\n{\n\n";
                        string arg = "\n\n}\nreturn d;\n}\n}\n";
                        string arg2 = str2 + str3 + str4 + str5;
                        string text5 = "using UnityEngine;\nusing System.Collections;\nusing System.Collections.Generic;\n\npublic class " + str + " : MonoBehaviour\n{\n";
                        string text6 = "public Color MouseOverColor = new Color(0.1F, 0.3F, 0.5F, 1.0F);\npublic int TextSize = 14;\nprivate string theText = \"\";\nprivate Hashtable ht_r;\nprivate Hashtable ht_g;\nprivate Hashtable ht_b;\nprivate Hashtable ht_a;\nprivate ICollection key_ht;\nbool showText = false;\n";
                        string text7 = "void OnMouseUp ()\n{\nshowText = false;\n}\n\nvoid OnMouseDown ()\n{\nshowText = true;\n}\n\n";
                        string text8 = string.Concat(new string[]
                        {
                            "void Start ()\n{\nthis.gameObject.AddComponent<MeshCollider>();\nthis.gameObject.AddComponent<",
                            fileNameWithoutExtension,
                            "> ();\nthis.gameObject.GetComponent<",
                            fileNameWithoutExtension,
                            ">().TheList(this.gameObject.name.ToString());\nht_r = new Hashtable ();\nht_g = new Hashtable ();\nht_b = new Hashtable ();\nht_a = new Hashtable ();\n"
                        });
                        string text9 = "for (int i = 0; i < GetComponent<Renderer> ().materials.Length; i++)\n{\nht_r.Add (i, GetComponent<Renderer> ().materials [i].color.r);\nkey_ht = ht_r.Keys;\nht_g.Add (i, GetComponent<Renderer> ().materials [i].color.g);\nht_b.Add (i, GetComponent<Renderer> ().materials [i].color.b);\nht_a.Add (i, GetComponent<Renderer> ().materials [i].color.a);\n}\n}";
                        string text10 = "void  OnMouseOver()\n{for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++){GetComponent<Renderer>().materials[i].color = MouseOverColor;\n}\nshowText = true;\n}\n";
                        string text11 = "void  OnMouseExit()\n{for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++){foreach (int n in key_ht) {\nif (i == n) {\nColor originalcolor = new Color ((float)ht_r [n], (float)ht_g [n], (float)ht_b [n], (float)ht_a [n]);\nGetComponent<Renderer> ().materials [i].color = originalcolor;\n}\n}\n}\nshowText = false;\n}";
                        string text12 = "void OnGUI ()\n{\nif (showText) {\nif (this.name.ToString () != null) {\nstring mess = null;\nforeach (string p in this.GetComponent<" + fileNameWithoutExtension + ">().ParamNameList) {\nmess += \"\\n\" + p;\ntheText = this.name.ToString () + \"\\n\" + mess;\nGUIStyle customGuiStyle = new GUIStyle ();\ncustomGuiStyle.fontSize = TextSize;\ncustomGuiStyle.fontStyle = FontStyle.Normal;\ncustomGuiStyle.normal.textColor = MouseOverColor;\nGUI.Label (new Rect (10, 10, Screen.width - 20, 50), theText, customGuiStyle);\n}\n}\n}\n}\n}";
                        string value = string.Concat(new string[]
                        {
                            text5,
                            text6,
                            text7,
                            text8,
                            text9,
                            text10,
                            text11,
                            text12
                        });
                        this.pBar1.Maximum = elementList.Count;
                        this.pBar1.Visible = true;
                        bool flag29 = !IsTemplate3DView;
                        if (flag29)
                        {
                            bool flag30 = activeView3D != null;
                            if (flag30)
                            {
                                CheckExportContext checkExportContext = new CheckExportContext(doc, this.m_AllViews);
                                CustomExporter customExporter = new CustomExporter(doc, checkExportContext);
                                customExporter.set_IncludeGeometricObjects(false);
                                customExporter.set_ShouldStopOnError(false);
                                customExporter.Export(activeView3D);
                                bool flag31 = checkExportContext.ListElementID01.Count != 0;
                                if (flag31)
                                {
                                    foreach (string current6 in checkExportContext.ListElementID01)
                                    {
                                        bool flag32 = !this.VerifyIdInstNameListfromView.Contains(current6);
                                        if (flag32)
                                        {
                                            this.VerifyIdInstNameListfromView.Add(current6);
                                        }
                                    }
                                }
                                bool flag33 = checkExportContext.ListLINKID01.Count != 0;
                                if (flag33)
                                {
                                    foreach (string current7 in checkExportContext.ListLINKID01)
                                    {
                                        bool flag34 = !this.VerifyIdLinkNameListfromView.Contains(current7);
                                        if (flag34)
                                        {
                                            this.VerifyIdLinkNameListfromView.Add(current7);
                                        }
                                    }
                                }
                            }
                        }
                        foreach (Element current8 in elementList)
                        {
                            this.pBar1.Step = 1;
                            this.pBar1.PerformStep();
                            element = doc.GetElement(current8.Id);
                            elementType = doc.GetElement(current8.GetTypeId());
                            bool flag35 = element != null & elementType != null;
                            if (flag35)
                            {
                                this.ParamInfos(element, elementType);
                            }
                        }
                        foreach (string current9 in this.VerifyIdLinkNameListfromView)
                        {
                            FilteredElementCollector filteredElementCollector6 = new FilteredElementCollector(doc);
                            IList<Element> list15 = filteredElementCollector6.OfCategory(-2001352).OfClass(typeof(RevitLinkType)).ToElements();
                            int num5 = Convert.ToInt32(current9);
                            ElementId elementId = new ElementId(num5);
                            RevitLinkType revitLinkType = doc.GetElement(elementId) as RevitLinkType;
                            string name = revitLinkType.Name;
                            bool flag36 = list15 != null;
                            if (flag36)
                            {
                                bool flag37 = list15.Count > 0 & revitLinkType != null;
                                if (flag37)
                                {
                                    foreach (Document document2 in doc.Application.Documents)
                                    {
                                        bool flag38 = name.Contains(document2.Title);
                                        if (flag38)
                                        {
                                            FilteredElementCollector filteredElementCollector7 = new FilteredElementCollector(document2).WhereElementIsNotElementType();
                                            foreach (Element current10 in filteredElementCollector7)
                                            {
                                                element = document2.GetElement(current10.Id);
                                                elementType = document2.GetElement(current10.GetTypeId());
                                                bool flag39 = element != null & elementType != null;
                                                if (flag39)
                                                {
                                                    this.ParamInfos(element, elementType);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        bool flag40 = !this.checkBoxToVU.Checked;
                        if (flag40)
                        {
                            using (StreamWriter streamWriter = new StreamWriter(saveFilePathAndNameText, true))
                            {
                                streamWriter.WriteLine(arg2 + this.MESSsbuilder + arg);
                            }
                            using (StreamWriter streamWriter2 = new StreamWriter(path, true))
                            {
                                streamWriter2.WriteLine(value);
                            }
                        }
                        bool checked5 = this.checkBoxToVU.Checked;
                        if (checked5)
                        {
                            using (StreamWriter streamWriter3 = new StreamWriter(path2, true))
                            {
                                streamWriter3.WriteLine(this.VUsbuilder);
                            }
                        }
                    }
                    bool checked6 = this.checkBoxRoomToMono.Checked;
                    if (checked6)
                    {
                        foreach (Element current11 in elementList)
                        {
                            bool flag41 = current11.Category.Name == "Generic Models";
                            if (flag41)
                            {
                                FamilyInstance familyInstance = current11 as FamilyInstance;
                                bool flag42 = familyInstance.Name == "GenBubbleDiagramRect01";
                                if (flag42)
                                {
                                    element = doc.GetElement(current11.Id);
                                    elementType = doc.GetElement(current11.GetTypeId());
                                    string text13 = null;
                                    string str6 = null;
                                    bool flag43 = element != null;
                                    if (flag43)
                                    {
                                        bool flag44 = !this.VerifyIdInstNameList.Contains(element.Id.ToString());
                                        if (flag44)
                                        {
                                            this.VerifyIdInstNameList.Add(element.Id.ToString());
                                            List<string> list16 = new List<string>();
                                            string text14 = Convert.ToString(element.get_Parameter(-1002051).AsValueString()) + " " + Convert.ToString(element.get_Parameter(-1002050).AsValueString());
                                            string text15 = text14.Replace("\"", "\\\"");
                                            str6 = string.Concat(new string[]
                                            {
                                                "case \"",
                                                text15,
                                                " [",
                                                element.Id.ToString(),
                                                "]\":\nd = "
                                            });
                                            bool flag45 = this.listBoxParameters.SelectedItems.Count > 0;
                                            if (flag45)
                                            {
                                                foreach (string text16 in this.listBoxParameters.SelectedItems)
                                                {
                                                    foreach (Parameter parameter5 in element.Parameters)
                                                    {
                                                        bool flag46 = parameter5.Definition.ParameterType == (ParameterType)4;
                                                        if (flag46)
                                                        {
                                                            bool flag47 = parameter5.Definition.Name == text16;
                                                            if (flag47)
                                                            {
                                                                bool flag48 = !list16.Contains(text16);
                                                                if (flag48)
                                                                {
                                                                    bool hasValue = parameter5.HasValue;
                                                                    if (hasValue)
                                                                    {
                                                                        bool flag49 = parameter5.AsDouble().ToString() != null;
                                                                        if (flag49)
                                                                        {
                                                                            double value2 = 0.0;
                                                                            bool flag50 = this.UniteSuffixe == " mm";
                                                                            if (flag50)
                                                                            {
                                                                                value2 = parameter5.AsDouble() * 0.3048 * 1000.0;
                                                                            }
                                                                            bool flag51 = this.UniteSuffixe == " m";
                                                                            if (flag51)
                                                                            {
                                                                                value2 = parameter5.AsDouble() * 0.3048;
                                                                            }
                                                                            bool flag52 = this.UniteSuffixe == " ft";
                                                                            if (flag52)
                                                                            {
                                                                                value2 = parameter5.AsDouble();
                                                                            }
                                                                            bool flag53 = this.UniteSuffixe == " in";
                                                                            if (flag53)
                                                                            {
                                                                                value2 = parameter5.AsDouble() * 12.0;
                                                                            }
                                                                            double value3 = Math.Round(value2, 2);
                                                                            string text17 = text16.Replace("\"", "\\\"");
                                                                            string text18 = Convert.ToString(value3);
                                                                            string text19 = text18.Replace("\"", "\\\"");
                                                                            text13 = string.Concat(new string[]
                                                                            {
                                                                                text13,
                                                                                text17,
                                                                                " : ",
                                                                                text19,
                                                                                this.UniteSuffixe,
                                                                                " \\n"
                                                                            });
                                                                            list16.Add(text16);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        bool flag54 = parameter5.Definition.ParameterType == (ParameterType)5;
                                                        if (flag54)
                                                        {
                                                            bool flag55 = parameter5.Definition.Name == text16;
                                                            if (flag55)
                                                            {
                                                                bool flag56 = !list16.Contains(text16);
                                                                if (flag56)
                                                                {
                                                                    bool hasValue2 = parameter5.HasValue;
                                                                    if (hasValue2)
                                                                    {
                                                                        bool flag57 = parameter5.AsDouble().ToString() != null;
                                                                        if (flag57)
                                                                        {
                                                                            double value4 = 0.0;
                                                                            bool flag58 = this.UniteSuffixe == " mm" | this.UniteSuffixe == " m";
                                                                            if (flag58)
                                                                            {
                                                                                value4 = parameter5.AsDouble() * Math.Pow(0.3048, 2.0);
                                                                            }
                                                                            bool flag59 = this.UniteSuffixe == " ft" | this.UniteSuffixe == " in";
                                                                            if (flag59)
                                                                            {
                                                                                value4 = parameter5.AsDouble();
                                                                            }
                                                                            double value5 = Math.Round(value4, 2);
                                                                            string text20 = text16.Replace("\"", "\\\"");
                                                                            string text21 = Convert.ToString(value5);
                                                                            string text22 = text21.Replace("\"", "\\\"");
                                                                            text13 = string.Concat(new string[]
                                                                            {
                                                                                text13,
                                                                                text20,
                                                                                " : ",
                                                                                text22,
                                                                                this.AreaUniteSuffixe,
                                                                                " \\n"
                                                                            });
                                                                            list16.Add(text16);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        bool flag60 = parameter5.Definition.ParameterType == (ParameterType)3;
                                                        if (flag60)
                                                        {
                                                            bool flag61 = parameter5.Definition.Name == text16;
                                                            if (flag61)
                                                            {
                                                                bool flag62 = !list16.Contains(text16);
                                                                if (flag62)
                                                                {
                                                                    bool hasValue3 = parameter5.HasValue;
                                                                    if (hasValue3)
                                                                    {
                                                                        bool flag63 = parameter5.AsDouble().ToString() != null;
                                                                        if (flag63)
                                                                        {
                                                                            double value6 = parameter5.AsDouble();
                                                                            double value7 = Math.Round(value6, 2);
                                                                            string text23 = text16.Replace("\"", "\\\"");
                                                                            string text24 = Convert.ToString(value7);
                                                                            string text25 = text24.Replace("\"", "\\\"");
                                                                            text13 = string.Concat(new string[]
                                                                            {
                                                                                text13,
                                                                                text23,
                                                                                " : ",
                                                                                text25,
                                                                                " \\n"
                                                                            });
                                                                            list16.Add(text16);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        bool flag64 = parameter5.Definition.ParameterType == (ParameterType)1;
                                                        if (flag64)
                                                        {
                                                            bool flag65 = parameter5.Definition.Name == text16;
                                                            if (flag65)
                                                            {
                                                                bool hasValue4 = parameter5.HasValue;
                                                                if (hasValue4)
                                                                {
                                                                    bool flag66 = parameter5.AsString() != null;
                                                                    if (flag66)
                                                                    {
                                                                        string value8 = parameter5.AsString();
                                                                        string text26 = text16.Replace("\"", "\\\"");
                                                                        string text27 = Convert.ToString(value8);
                                                                        string text28 = text27.Replace("\"", "\\\"");
                                                                        text13 = string.Concat(new string[]
                                                                        {
                                                                            text13,
                                                                            text26,
                                                                            " : ",
                                                                            text28,
                                                                            " \\n"
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    foreach (Parameter parameter6 in elementType.Parameters)
                                                    {
                                                        bool flag67 = parameter6.Definition.ParameterType == (ParameterType)4;
                                                        if (flag67)
                                                        {
                                                            bool flag68 = parameter6.Definition.Name == text16;
                                                            if (flag68)
                                                            {
                                                                bool flag69 = !list16.Contains(text16);
                                                                if (flag69)
                                                                {
                                                                    bool hasValue5 = parameter6.HasValue;
                                                                    if (hasValue5)
                                                                    {
                                                                        bool flag70 = parameter6.AsDouble().ToString() != null;
                                                                        if (flag70)
                                                                        {
                                                                            double value9 = 0.0;
                                                                            bool flag71 = this.UniteSuffixe == " mm";
                                                                            if (flag71)
                                                                            {
                                                                                value9 = parameter6.AsDouble() * 0.3048 * 1000.0;
                                                                            }
                                                                            bool flag72 = this.UniteSuffixe == " m";
                                                                            if (flag72)
                                                                            {
                                                                                value9 = parameter6.AsDouble() * 0.3048;
                                                                            }
                                                                            bool flag73 = this.UniteSuffixe == " ft";
                                                                            if (flag73)
                                                                            {
                                                                                value9 = parameter6.AsDouble();
                                                                            }
                                                                            bool flag74 = this.UniteSuffixe == " in";
                                                                            if (flag74)
                                                                            {
                                                                                value9 = parameter6.AsDouble() * 12.0;
                                                                            }
                                                                            double value10 = Math.Round(value9, 2);
                                                                            string text29 = text16.Replace("\"", "\\\"");
                                                                            string text30 = Convert.ToString(value10);
                                                                            string text31 = text30.Replace("\"", "\\\"");
                                                                            text13 = string.Concat(new string[]
                                                                            {
                                                                                text13,
                                                                                text29,
                                                                                " : ",
                                                                                text31,
                                                                                this.UniteSuffixe,
                                                                                " \\n"
                                                                            });
                                                                            list16.Add(text16);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        bool flag75 = parameter6.Definition.ParameterType == (ParameterType)5;
                                                        if (flag75)
                                                        {
                                                            bool flag76 = parameter6.Definition.Name == text16;
                                                            if (flag76)
                                                            {
                                                                bool flag77 = !list16.Contains(text16);
                                                                if (flag77)
                                                                {
                                                                    bool hasValue6 = parameter6.HasValue;
                                                                    if (hasValue6)
                                                                    {
                                                                        bool flag78 = parameter6.AsDouble().ToString() != null;
                                                                        if (flag78)
                                                                        {
                                                                            double value11 = 0.0;
                                                                            bool flag79 = this.UniteSuffixe == " mm" | this.UniteSuffixe == " m";
                                                                            if (flag79)
                                                                            {
                                                                                value11 = parameter6.AsDouble() * Math.Pow(0.3048, 2.0);
                                                                            }
                                                                            bool flag80 = this.UniteSuffixe == " ft" | this.UniteSuffixe == " in";
                                                                            if (flag80)
                                                                            {
                                                                                value11 = parameter6.AsDouble();
                                                                            }
                                                                            double value12 = Math.Round(value11, 2);
                                                                            string text32 = text16.Replace("\"", "\\\"");
                                                                            string text33 = Convert.ToString(value12);
                                                                            string text34 = text33.Replace("\"", "\\\"");
                                                                            text13 = string.Concat(new string[]
                                                                            {
                                                                                text13,
                                                                                text32,
                                                                                " : ",
                                                                                text34,
                                                                                this.AreaUniteSuffixe,
                                                                                " \\n"
                                                                            });
                                                                            list16.Add(text16);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        bool flag81 = parameter6.Definition.ParameterType == (ParameterType)3;
                                                        if (flag81)
                                                        {
                                                            bool flag82 = parameter6.Definition.Name == text16;
                                                            if (flag82)
                                                            {
                                                                bool flag83 = !list16.Contains(text16);
                                                                if (flag83)
                                                                {
                                                                    bool hasValue7 = parameter6.HasValue;
                                                                    if (hasValue7)
                                                                    {
                                                                        bool flag84 = parameter6.AsDouble().ToString() != null;
                                                                        if (flag84)
                                                                        {
                                                                            double value13 = parameter6.AsDouble();
                                                                            double value14 = Math.Round(value13, 2);
                                                                            string text35 = text16.Replace("\"", "\\\"");
                                                                            string text36 = Convert.ToString(value14);
                                                                            string text37 = text36.Replace("\"", "\\\"");
                                                                            text13 = string.Concat(new string[]
                                                                            {
                                                                                text13,
                                                                                text35,
                                                                                " : ",
                                                                                text37,
                                                                                " \\n"
                                                                            });
                                                                            list16.Add(text16);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        bool flag85 = parameter6.Definition.ParameterType == (ParameterType)1;
                                                        if (flag85)
                                                        {
                                                            bool flag86 = parameter6.Definition.Name == text16;
                                                            if (flag86)
                                                            {
                                                                bool hasValue8 = parameter6.HasValue;
                                                                if (hasValue8)
                                                                {
                                                                    bool flag87 = parameter6.AsString() != null;
                                                                    if (flag87)
                                                                    {
                                                                        string value15 = parameter6.AsString();
                                                                        string text38 = text16.Replace("\"", "\\\"");
                                                                        string text39 = Convert.ToString(value15);
                                                                        string text40 = text39.Replace("\"", "\\\"");
                                                                        text13 = string.Concat(new string[]
                                                                        {
                                                                            text13,
                                                                            text38,
                                                                            " : ",
                                                                            text40,
                                                                            " \\n"
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            using (StreamWriter streamWriter4 = new StreamWriter(saveFilePathAndNameText, true))
                                            {
                                                streamWriter4.WriteLine(str6 + "\"" + text13 + "\";\nbreak;");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    bool checked7 = this.cBoxOuvertures.Checked;
                    if (checked7)
                    {
                        foreach (Element current12 in elementList)
                        {
                            element = doc.GetElement(current12.Id);
                            elementType = doc.GetElement(current12.GetTypeId());
                            bool flag88 = element != null;
                            if (flag88)
                            {
                                bool flag89 = element.Category.Name == "Walls";
                                if (flag89)
                                {
                                    wall = (element as Wall);
                                    bool flag90 = wall != null;
                                    if (flag90)
                                    {
                                        bool flag91 = wall.CurtainGrid == null;
                                        if (flag91)
                                        {
                                            foreach (Parameter parameter7 in element.Parameters)
                                            {
                                                bool flag92 = parameter7.Definition.Name == "Area";
                                                if (flag92)
                                                {
                                                    foreach (Parameter parameter8 in element.Parameters)
                                                    {
                                                        bool flag93 = parameter8.Definition.Name == "Mur Orientation";
                                                        if (flag93)
                                                        {
                                                            bool flag94 = parameter8.AsString() != null;
                                                            if (flag94)
                                                            {
                                                                bool flag95 = list.Contains(parameter8.AsString());
                                                                if (flag95)
                                                                {
                                                                    foreach (string text41 in collection)
                                                                    {
                                                                        bool flag96 = parameter8.AsString() == text41;
                                                                        if (flag96)
                                                                        {
                                                                            double num6 = Convert.ToDouble(hashtable[text41]);
                                                                            hashtable[text41] = num6 + parameter7.AsDouble();
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                bool flag97 = !list.Contains(parameter8.AsString());
                                                                if (flag97)
                                                                {
                                                                    hashtable.Add(parameter8.AsString(), parameter7.AsDouble());
                                                                    collection = hashtable.Keys;
                                                                    list.Add(parameter8.AsString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        bool flag98 = wall.CurtainGrid != null;
                                        if (flag98)
                                        {
                                            foreach (Parameter parameter9 in element.Parameters)
                                            {
                                                bool flag99 = parameter9.Definition.Name == "Area";
                                                if (flag99)
                                                {
                                                    foreach (Parameter parameter10 in element.Parameters)
                                                    {
                                                        bool flag100 = parameter10.Definition.Name == "Mur Orientation";
                                                        if (flag100)
                                                        {
                                                            bool flag101 = parameter10.AsString() != null;
                                                            if (flag101)
                                                            {
                                                                bool flag102 = list10.Contains(parameter10.AsString());
                                                                if (flag102)
                                                                {
                                                                    foreach (string text42 in collection)
                                                                    {
                                                                        bool flag103 = parameter10.AsString() == text42;
                                                                        if (flag103)
                                                                        {
                                                                            double num7 = Convert.ToDouble(hashtable5[text42]);
                                                                            hashtable5[text42] = num7 + parameter9.AsDouble();
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                bool flag104 = !list10.Contains(parameter10.AsString());
                                                                if (flag104)
                                                                {
                                                                    hashtable5.Add(parameter10.AsString(), parameter9.AsDouble());
                                                                    ICollection keys2 = hashtable5.Keys;
                                                                    list10.Add(parameter10.AsString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                bool flag105 = element.Category.Name == "Doors";
                                if (flag105)
                                {
                                    foreach (Parameter parameter11 in element.Parameters)
                                    {
                                        bool flag106 = parameter11.Definition.Name == "Area";
                                        if (flag106)
                                        {
                                            foreach (Parameter parameter12 in element.Parameters)
                                            {
                                                bool flag107 = parameter12.Definition.Name == "Mur Orientation";
                                                if (flag107)
                                                {
                                                    bool flag108 = parameter12.AsString() != null;
                                                    if (flag108)
                                                    {
                                                        bool flag109 = list10.Contains(parameter12.AsString());
                                                        if (flag109)
                                                        {
                                                            foreach (string text43 in collection)
                                                            {
                                                                bool flag110 = parameter12.AsString() == text43;
                                                                if (flag110)
                                                                {
                                                                    double num8 = Convert.ToDouble(hashtable5[text43]);
                                                                    hashtable5[text43] = num8 + parameter11.AsDouble();
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        bool flag111 = !list10.Contains(parameter12.AsString());
                                                        if (flag111)
                                                        {
                                                            hashtable5.Add(parameter12.AsString(), parameter11.AsDouble());
                                                            ICollection keys2 = hashtable5.Keys;
                                                            list10.Add(parameter12.AsString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Transaction transaction = new Transaction(doc);
                transaction.Start("WriteToText");
                doc.Regenerate();
                transaction.Commit();
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            return Result.Succeeded;
        }
    }
}
