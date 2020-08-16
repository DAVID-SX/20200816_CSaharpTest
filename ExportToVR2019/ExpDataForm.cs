using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ExportToVR
{
	// Token: 0x0200000C RID: 12
	public partial class ExpDataForm : System.Windows.Forms.Form
    {
		// Token: 0x0600009D RID: 157 RVA: 0x000062C4 File Offset: 0x000044C4
		public ExpDataForm(ExternalCommandData commandData, AllViews families)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = families;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000063A8 File Offset: 0x000045A8
		private void UnitButton_Click_MM(object sender, EventArgs e)
		{
			bool @checked = this.mmButton.Checked;
			if (@checked)
			{
				this.unite = 0.0032808;
				this.UniteSuffixe = " mm";
				this.AreaUniteSuffixe = " m2";
			}
			else
			{
				bool checked2 = this.mButton.Checked;
				if (checked2)
				{
					this.unite = 3.2808398;
					this.UniteSuffixe = " m";
					this.AreaUniteSuffixe = " m2";
				}
				else
				{
					bool checked3 = this.feetButton.Checked;
					if (checked3)
					{
						this.unite = 1.0;
						this.UniteSuffixe = " ft";
						this.AreaUniteSuffixe = " ft2";
					}
					else
					{
						bool checked4 = this.inchesButton.Checked;
						if (checked4)
						{
							this.unite = 0.0833333;
							this.UniteSuffixe = " in";
							this.AreaUniteSuffixe = " ft2";
						}
						else
						{
							this.unite = 1.0;
							this.UniteSuffixe = " ft";
							this.AreaUniteSuffixe = " ft2";
						}
					}
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000064C4 File Offset: 0x000046C4
		private static long ConvertFeetToMillimetres(double d)
		{
			bool flag = 0.0 < d;
			long result;
			if (flag)
			{
				result = ((1E-09 > d) ? 0L : ((long)(304.79999999999995 * d + 0.5)));
			}
			else
			{
				result = ((1E-09 > -d) ? 0L : ((long)(304.79999999999995 * d - 0.5)));
			}
			return result;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006538 File Offset: 0x00004738
		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.79999999999995;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006558 File Offset: 0x00004758
		private string ProjectUnits(string s)
		{
			Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
			Document document = this.p_commandData.Application.ActiveUIDocument.Document;
			FormatOptions formatOptions = document.GetUnits().GetFormatOptions(0);
			bool flag = formatOptions.DisplayUnits == DisplayUnitType.DUT_MILLIMETERS;
			string result;
			if (flag)
			{
				result = this.mm;
			}
			else
			{
				bool flag2 = formatOptions.DisplayUnits == 0;
				if (flag2)
				{
					result = this.m;
				}
				else
				{
					bool flag3 = formatOptions.DisplayUnits == DisplayUnitType.DUT_DECIMAL_INCHES;
					if (flag3)
					{
						result = this.inches;
					}
					else
					{
						result = this.ft;
					}
				}
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006600 File Offset: 0x00004800
		private void okButton_Click(object sender, EventArgs e)
		{
			string text = null;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = "c:\\";
			saveFileDialog.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = null;
			bool flag = saveFileDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				try
				{
					Stream stream;
					bool flag2 = (stream = saveFileDialog.OpenFile()) != null;
					if (flag2)
					{
						using (stream)
						{
							text = saveFileDialog.FileName;
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
				}
			}
			string text2 = "_MouseOver";
			string directoryName = Path.GetDirectoryName(text);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
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
			bool flag3 = File.Exists(text);
			if (flag3)
			{
				File.Delete(text);
			}
			bool flag4 = File.Exists(path);
			if (flag4)
			{
				File.Delete(path);
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<double> list4 = new List<double>();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			List<string> list7 = new List<string>();
			List<string> list8 = new List<string>();
			List<string> list9 = new List<string>();
			Hashtable hashtable = new Hashtable();
			ICollection collection = null;
			Hashtable hashtable2 = new Hashtable();
			Hashtable hashtable3 = new Hashtable();
			Hashtable hashtable4 = new Hashtable();
			List<string> list10 = new List<string>();
			Hashtable hashtable5 = new Hashtable();
			try
			{
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
				ICollection<Element> collection2 = filteredElementCollector.OfClass(typeof(FamilySymbol)).ToElements();
				FilteredElementCollector filteredElementCollector2 = new FilteredElementCollector(document);
				ICollection<Element> collection3 = filteredElementCollector2.OfClass(typeof(Wall)).ToElements();
				Wall wall = null;
				FilteredElementCollector filteredElementCollector3 = new FilteredElementCollector(document);
				ICollection<Element> collection4 = filteredElementCollector3.OfClass(typeof(FamilyInstance)).ToElements();
				Element element = null;
				Element element2 = null;
				List<Element> list11 = new List<Element>();
				List<Element> list12 = new List<Element>();
				List<ElementId> list13 = new List<ElementId>();
				List<string> list14 = new List<string>();
				FilteredElementCollector filteredElementCollector4 = new FilteredElementCollector(document).WhereElementIsNotElementType();
				foreach (Element element3 in filteredElementCollector4)
				{
					bool flag5 = element3.Category != null && element3.Category.HasMaterialQuantities;
					if (flag5)
					{
						list11.Add(element3);
					}
					bool flag6 = element3.Category != null && element3.Category.Id.IntegerValue == -2000171;
					if (flag6)
					{
						list11.Add(element3);
					}
				}
				FilteredElementCollector filteredElementCollector5 = new FilteredElementCollector(document);
				ICollection<Element> collection5 = filteredElementCollector5.OfClass(typeof(View3D)).ToElements();
				View3D view3D = null;
				bool flag7 = false;
				this.m_AllViews.ExportProperties = true;
				bool flag8 = document.ActiveView.ViewType != ViewType.ThreeD;
				if (flag8)
				{
					MessageBox.Show("The active view must be a 3D view type.");
					base.Close();
				}
				bool flag9 = document.ActiveView.ViewType == ViewType.ThreeD & document.ActiveView.IsTemplate;
				if (flag9)
				{
					MessageBox.Show("The active view is a template view and is not exportable.");
					flag7 = true;
					base.Close();
				}
				bool flag10 = document.ActiveView.ViewType == ViewType.ThreeD & !document.ActiveView.IsTemplate;
				if (flag10)
				{
					view3D = (document.ActiveView as View3D);
				}
				bool flag11 = text != null;
				if (flag11)
				{
					bool @checked = this.cBoxAssemCode.Checked;
					if (@checked)
					{
						foreach (Element element4 in list11)
						{
							element = document.GetElement(element4.Id);
							element2 = document.GetElement(element4.GetTypeId());
							bool flag12 = element != null;
							if (flag12)
							{
								foreach (object obj in element.Parameters)
								{
									Parameter parameter = (Parameter)obj;
									bool flag13 = parameter.Definition.Name == "Area";
									if (flag13)
									{
										foreach (object obj2 in element2.Parameters)
										{
											Parameter parameter2 = (Parameter)obj2;
											bool flag14 = parameter2.Definition.Name == "Assembly Code";
											if (flag14)
											{
												bool flag15 = parameter2.AsString() != null;
												if (flag15)
												{
													bool flag16 = list.Contains(parameter2.AsString());
													if (flag16)
													{
														foreach (object obj3 in collection)
														{
															string text3 = (string)obj3;
															bool flag17 = parameter2.AsString() == text3;
															if (flag17)
															{
																double num = Convert.ToDouble(hashtable[text3]);
																hashtable[text3] = num + parameter.AsDouble();
																break;
															}
														}
													}
													bool flag18 = !list.Contains(parameter2.AsString());
													if (flag18)
													{
														hashtable.Add(parameter2.AsString(), parameter.AsDouble());
														collection = hashtable.Keys;
														list.Add(parameter2.AsString());
													}
												}
											}
										}
									}
								}
							}
						}
					}
					int num2 = 3;
					bool checked2 = this.checkBoxId.Checked;
					if (checked2)
					{
						foreach (Element element5 in list11)
						{
							element = document.GetElement(element5.Id);
							element2 = document.GetElement(element5.GetTypeId());
							bool flag19 = element != null;
							if (flag19)
							{
								foreach (object obj4 in element.Parameters)
								{
									Parameter parameter3 = (Parameter)obj4;
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
					bool checked3 = this.checkBoxList.Checked;
					if (checked3)
					{
						foreach (Element element6 in list11)
						{
							element = document.GetElement(element6.Id);
							element2 = document.GetElement(element6.GetTypeId());
							bool flag22 = element != null;
							if (flag22)
							{
								bool flag23 = !list.Contains(element.Id.ToString());
								if (flag23)
								{
									hashtable.Add(num3.ToString(), element.Id.ToString());
									collection = hashtable.Keys;
									list.Add(element.Id.ToString());
									num3++;
								}
							}
						}
						bool flag24 = this.listBoxParameters.SelectedItems.Count > 0;
						if (flag24)
						{
							foreach (object obj5 in this.listBoxParameters.SelectedItems)
							{
								string text4 = (string)obj5;
								foreach (Element element7 in list11)
								{
									element = document.GetElement(element7.Id);
									element2 = document.GetElement(element7.GetTypeId());
									bool flag25 = element != null;
									if (flag25)
									{
										foreach (object obj6 in element.Parameters)
										{
											Parameter parameter4 = (Parameter)obj6;
											bool flag26 = parameter4.Definition.ParameterType == ParameterType.Length;
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
					}
					bool checked4 = this.checkBoxToMono.Checked;
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
						this.pBar1.Maximum = list11.Count;
						this.pBar1.Visible = true;
						bool flag29 = !flag7;
						if (flag29)
						{
							bool flag30 = view3D != null;
							if (flag30)
							{
								CheckExportContext checkExportContext = new CheckExportContext(document, this.m_AllViews);
								new CustomExporter(document, checkExportContext)
								{
									IncludeGeometricObjects = false,
									ShouldStopOnError = false
								}.Export(view3D);
								bool flag31 = checkExportContext.ListElementID01.Count != 0;
								if (flag31)
								{
									foreach (string item in checkExportContext.ListElementID01)
									{
										bool flag32 = !this.VerifyIdInstNameListfromView.Contains(item);
										if (flag32)
										{
											this.VerifyIdInstNameListfromView.Add(item);
										}
									}
								}
								bool flag33 = checkExportContext.ListLINKID01.Count != 0;
								if (flag33)
								{
									foreach (string item2 in checkExportContext.ListLINKID01)
									{
										bool flag34 = !this.VerifyIdLinkNameListfromView.Contains(item2);
										if (flag34)
										{
											this.VerifyIdLinkNameListfromView.Add(item2);
										}
									}
								}
							}
						}
						foreach (Element element8 in list11)
						{
							this.pBar1.Step = 1;
							this.pBar1.PerformStep();
							element = document.GetElement(element8.Id);
							element2 = document.GetElement(element8.GetTypeId());
							bool flag35 = element != null & element2 != null;
							if (flag35)
							{
								this.ParamInfos(element, element2);
							}
						}
						foreach (string value2 in this.VerifyIdLinkNameListfromView)
						{
							FilteredElementCollector filteredElementCollector6 = new FilteredElementCollector(document);
							// 这里将-2001352显示转换了
							IList<Element> list15 = filteredElementCollector6.OfCategory((BuiltInCategory)(-2001352)).OfClass(typeof(RevitLinkType)).ToElements();
							int num5 = Convert.ToInt32(value2);
							ElementId elementId = new ElementId(num5);
							RevitLinkType revitLinkType = document.GetElement(elementId) as RevitLinkType;
							string name = revitLinkType.Name;
							bool flag36 = list15 != null;
							if (flag36)
							{
								bool flag37 = list15.Count > 0 & revitLinkType != null;
								if (flag37)
								{
									foreach (object obj7 in document.Application.Documents)
									{
										Document document2 = (Document)obj7;
										bool flag38 = name.Contains(document2.Title);
										if (flag38)
										{
											FilteredElementCollector filteredElementCollector7 = new FilteredElementCollector(document2).WhereElementIsNotElementType();
											foreach (Element element9 in filteredElementCollector7)
											{
												element = document2.GetElement(element9.Id);
												element2 = document2.GetElement(element9.GetTypeId());
												bool flag39 = element != null & element2 != null;
												if (flag39)
												{
													this.ParamInfos(element, element2);
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
							using (StreamWriter streamWriter = new StreamWriter(text, true))
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
						foreach (Element element10 in list11)
						{
							bool flag41 = element10.Category.Name == "Generic Models";
							if (flag41)
							{
								FamilyInstance familyInstance = element10 as FamilyInstance;
								bool flag42 = familyInstance.Name == "GenBubbleDiagramRect01";
								if (flag42)
								{
									element = document.GetElement(element10.Id);
									element2 = document.GetElement(element10.GetTypeId());
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
											// 这里将ID进行了显式转换
											string text14 = Convert.ToString(element.get_Parameter((BuiltInParameter)(-1002051)).AsValueString()) + " " + Convert.ToString(element.get_Parameter((BuiltInParameter)(-1002050)).AsValueString());
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
												foreach (object obj8 in this.listBoxParameters.SelectedItems)
												{
													string text16 = (string)obj8;
													foreach (object obj9 in element.Parameters)
													{
														Parameter parameter5 = (Parameter)obj9;
														bool flag46 = parameter5.Definition.ParameterType == ParameterType.Text;
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
																			double value3 = 0.0;
																			bool flag50 = this.UniteSuffixe == " mm";
																			if (flag50)
																			{
																				value3 = parameter5.AsDouble() * 0.3048 * 1000.0;
																			}
																			bool flag51 = this.UniteSuffixe == " m";
																			if (flag51)
																			{
																				value3 = parameter5.AsDouble() * 0.3048;
																			}
																			bool flag52 = this.UniteSuffixe == " ft";
																			if (flag52)
																			{
																				value3 = parameter5.AsDouble();
																			}
																			bool flag53 = this.UniteSuffixe == " in";
																			if (flag53)
																			{
																				value3 = parameter5.AsDouble() * 12.0;
																			}
																			double value4 = Math.Round(value3, 2);
																			string text17 = text16.Replace("\"", "\\\"");
																			string text18 = Convert.ToString(value4);
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
														bool flag54 = parameter5.Definition.ParameterType == ParameterType.Area;
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
																			double value5 = 0.0;
																			bool flag58 = this.UniteSuffixe == " mm" | this.UniteSuffixe == " m";
																			if (flag58)
																			{
																				value5 = parameter5.AsDouble() * Math.Pow(0.3048, 2.0);
																			}
																			bool flag59 = this.UniteSuffixe == " ft" | this.UniteSuffixe == " in";
																			if (flag59)
																			{
																				value5 = parameter5.AsDouble();
																			}
																			double value6 = Math.Round(value5, 2);
																			string text20 = text16.Replace("\"", "\\\"");
																			string text21 = Convert.ToString(value6);
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
														bool flag60 = parameter5.Definition.ParameterType == ParameterType.Number;
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
																			double value7 = parameter5.AsDouble();
																			double value8 = Math.Round(value7, 2);
																			string text23 = text16.Replace("\"", "\\\"");
																			string text24 = Convert.ToString(value8);
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
														bool flag64 = parameter5.Definition.ParameterType == ParameterType.Text;
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
																		string value9 = parameter5.AsString();
																		string text26 = text16.Replace("\"", "\\\"");
																		string text27 = Convert.ToString(value9);
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
													foreach (object obj10 in element2.Parameters)
													{
														Parameter parameter6 = (Parameter)obj10;
														bool flag67 = parameter6.Definition.ParameterType == ParameterType.Length;
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
																			double value10 = 0.0;
																			bool flag71 = this.UniteSuffixe == " mm";
																			if (flag71)
																			{
																				value10 = parameter6.AsDouble() * 0.3048 * 1000.0;
																			}
																			bool flag72 = this.UniteSuffixe == " m";
																			if (flag72)
																			{
																				value10 = parameter6.AsDouble() * 0.3048;
																			}
																			bool flag73 = this.UniteSuffixe == " ft";
																			if (flag73)
																			{
																				value10 = parameter6.AsDouble();
																			}
																			bool flag74 = this.UniteSuffixe == " in";
																			if (flag74)
																			{
																				value10 = parameter6.AsDouble() * 12.0;
																			}
																			double value11 = Math.Round(value10, 2);
																			string text29 = text16.Replace("\"", "\\\"");
																			string text30 = Convert.ToString(value11);
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
														bool flag75 = parameter6.Definition.ParameterType == ParameterType.Area;
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
																			double value12 = 0.0;
																			bool flag79 = this.UniteSuffixe == " mm" | this.UniteSuffixe == " m";
																			if (flag79)
																			{
																				value12 = parameter6.AsDouble() * Math.Pow(0.3048, 2.0);
																			}
																			bool flag80 = this.UniteSuffixe == " ft" | this.UniteSuffixe == " in";
																			if (flag80)
																			{
																				value12 = parameter6.AsDouble();
																			}
																			double value13 = Math.Round(value12, 2);
																			string text32 = text16.Replace("\"", "\\\"");
																			string text33 = Convert.ToString(value13);
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
														bool flag81 = parameter6.Definition.ParameterType == ParameterType.Number;
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
																			double value14 = parameter6.AsDouble();
																			double value15 = Math.Round(value14, 2);
																			string text35 = text16.Replace("\"", "\\\"");
																			string text36 = Convert.ToString(value15);
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
														bool flag85 = parameter6.Definition.ParameterType == ParameterType.Text;
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
																		string value16 = parameter6.AsString();
																		string text38 = text16.Replace("\"", "\\\"");
																		string text39 = Convert.ToString(value16);
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
											using (StreamWriter streamWriter4 = new StreamWriter(text, true))
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
						foreach (Element element11 in list11)
						{
							element = document.GetElement(element11.Id);
							element2 = document.GetElement(element11.GetTypeId());
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
											foreach (object obj11 in element.Parameters)
											{
												Parameter parameter7 = (Parameter)obj11;
												bool flag92 = parameter7.Definition.Name == "Area";
												if (flag92)
												{
													foreach (object obj12 in element.Parameters)
													{
														Parameter parameter8 = (Parameter)obj12;
														bool flag93 = parameter8.Definition.Name == "Mur Orientation";
														if (flag93)
														{
															bool flag94 = parameter8.AsString() != null;
															if (flag94)
															{
																bool flag95 = list.Contains(parameter8.AsString());
																if (flag95)
																{
																	foreach (object obj13 in collection)
																	{
																		string text41 = (string)obj13;
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
											foreach (object obj14 in element.Parameters)
											{
												Parameter parameter9 = (Parameter)obj14;
												bool flag99 = parameter9.Definition.Name == "Area";
												if (flag99)
												{
													foreach (object obj15 in element.Parameters)
													{
														Parameter parameter10 = (Parameter)obj15;
														bool flag100 = parameter10.Definition.Name == "Mur Orientation";
														if (flag100)
														{
															bool flag101 = parameter10.AsString() != null;
															if (flag101)
															{
																bool flag102 = list10.Contains(parameter10.AsString());
																if (flag102)
																{
																	foreach (object obj16 in collection)
																	{
																		string text42 = (string)obj16;
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
									foreach (object obj17 in element.Parameters)
									{
										Parameter parameter11 = (Parameter)obj17;
										bool flag106 = parameter11.Definition.Name == "Area";
										if (flag106)
										{
											foreach (object obj18 in element.Parameters)
											{
												Parameter parameter12 = (Parameter)obj18;
												bool flag107 = parameter12.Definition.Name == "Mur Orientation";
												if (flag107)
												{
													bool flag108 = parameter12.AsString() != null;
													if (flag108)
													{
														bool flag109 = list10.Contains(parameter12.AsString());
														if (flag109)
														{
															foreach (object obj19 in collection)
															{
																string text43 = (string)obj19;
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
				Transaction transaction = new Transaction(document);
				transaction.Start("WriteToText");
				document.Regenerate();
				transaction.Commit();
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00008F30 File Offset: 0x00007130
		private void ParamInfos(Element element, Element elementType)
		{
			bool flag = element != null;
			if (flag)
			{
				bool flag2 = this.VerifyIdInstNameListfromView.Contains(element.Id.ToString());
				if (flag2)
				{
					bool flag3 = !this.VerifyIdInstNameList.Contains(element.Id.ToString());
					if (flag3)
					{
						this.VerifyIdInstNameList.Add(element.Id.ToString());
						List<string> list = new List<string>();
						// 这里将ID显式转换了
						string text = Convert.ToString(Convert.ToString(element.get_Parameter((BuiltInParameter)(-1002050)).AsValueString()));
						string text2 = text.Replace(" ", "_");
						string text3 = element.Id.ToString();
						string text4 = text2.Replace("\"", "\\\"");
						this.MESSsbuilder.Append(string.Concat(new string[]
						{
							"case \"",
							text4,
							"_[",
							element.Id.ToString(),
							"]\":\nd = "
						}));
						this.MESSsbuilder.Append("\"");
						bool @checked = this.checkBoxToVU.Checked;
						if (@checked)
						{
							this.VUsbuilder.Append(text2 + "_[" + text3 + "]\n");
						}
						bool flag4 = this.listBoxParameters.SelectedItems.Count > 0;
						if (flag4)
						{
							foreach (object obj in this.listBoxParameters.SelectedItems)
							{
								string text5 = (string)obj;
								foreach (object obj2 in element.Parameters)
								{
									Parameter parameter = (Parameter)obj2;
									bool flag5 = parameter != null;
									if (flag5)
									{
										bool flag6 = parameter.Definition.Name == text5;
										if (flag6)
										{
											bool flag7 = !list.Contains(text5);
											if (flag7)
											{
												bool flag8 = parameter.Definition.ParameterType == ParameterType.Length;
												if (flag8)
												{
													bool hasValue = parameter.HasValue;
													if (hasValue)
													{
														bool flag9 = parameter.AsDouble().ToString() != null;
														if (flag9)
														{
															double value = 0.0;
															bool flag10 = this.UniteSuffixe == " mm";
															if (flag10)
															{
																value = parameter.AsDouble() * 0.3048 * 1000.0;
															}
															bool flag11 = this.UniteSuffixe == " m";
															if (flag11)
															{
																value = parameter.AsDouble() * 0.3048;
															}
															bool flag12 = this.UniteSuffixe == " ft";
															if (flag12)
															{
																value = parameter.AsDouble();
															}
															bool flag13 = this.UniteSuffixe == " in";
															if (flag13)
															{
																value = parameter.AsDouble() * 12.0;
															}
															double value2 = Math.Round(value, 2);
															string text6 = text5.Replace("\"", "\\\"");
															string text7 = Convert.ToString(value2);
															string text8 = text7.Replace("\"", "\\\"");
															this.MESSsbuilder.Append(string.Concat(new string[]
															{
																text6,
																" : ",
																text8,
																this.UniteSuffixe,
																" \\n"
															}));
															bool checked2 = this.checkBoxToVU.Checked;
															if (checked2)
															{
																this.VUsbuilder.Append(string.Concat(new string[]
																{
																	text3,
																	" ",
																	text5,
																	" : ",
																	text7,
																	this.UniteSuffixe,
																	"\n"
																}));
															}
															list.Add(text5);
														}
													}
												}
												bool flag14 = parameter.Definition.ParameterType == ParameterType.Angle;
												if (flag14)
												{
													bool hasValue2 = parameter.HasValue;
													if (hasValue2)
													{
														bool flag15 = parameter.AsDouble().ToString() != null;
														if (flag15)
														{
															double value3 = 0.0;
															bool flag16 = this.UniteSuffixe == " mm";
															if (flag16)
															{
																value3 = ExpDataForm.RadianToDegree(parameter.AsDouble());
															}
															bool flag17 = this.UniteSuffixe == " m";
															if (flag17)
															{
																value3 = ExpDataForm.RadianToDegree(parameter.AsDouble());
															}
															bool flag18 = this.UniteSuffixe == " ft";
															if (flag18)
															{
																value3 = ExpDataForm.RadianToDegree(parameter.AsDouble());
															}
															bool flag19 = this.UniteSuffixe == " in";
															if (flag19)
															{
																value3 = ExpDataForm.RadianToDegree(parameter.AsDouble());
															}
															double value4 = Math.Round(value3, 2);
															string str = text5.Replace("\"", "\\\"");
															string text9 = Convert.ToString(value4);
															string str2 = text9.Replace("\"", "\\\"");
															this.MESSsbuilder.Append(str + " : " + str2 + " deg \\n");
															bool checked3 = this.checkBoxToVU.Checked;
															if (checked3)
															{
																this.VUsbuilder.Append(string.Concat(new string[]
																{
																	text3,
																	" ",
																	text5,
																	" : ",
																	text9,
																	" deg\n"
																}));
															}
															list.Add(text5);
														}
													}
												}
												bool flag20 = parameter.Definition.ParameterType == ParameterType.Area;
												if (flag20)
												{
													bool hasValue3 = parameter.HasValue;
													if (hasValue3)
													{
														bool flag21 = parameter.AsDouble().ToString() != null;
														if (flag21)
														{
															double value5 = 0.0;
															bool flag22 = this.UniteSuffixe == " mm" | this.UniteSuffixe == " m";
															if (flag22)
															{
																value5 = parameter.AsDouble() * Math.Pow(0.3048, 2.0);
															}
															bool flag23 = this.UniteSuffixe == " ft" | this.UniteSuffixe == " in";
															if (flag23)
															{
																value5 = parameter.AsDouble();
															}
															double value6 = Math.Round(value5, 2);
															string text10 = text5.Replace("\"", "\\\"");
															string text11 = Convert.ToString(value6);
															string text12 = text11.Replace("\"", "\\\"");
															this.MESSsbuilder.Append(string.Concat(new string[]
															{
																text10,
																" : ",
																text12,
																this.AreaUniteSuffixe,
																" \\n"
															}));
															bool checked4 = this.checkBoxToVU.Checked;
															if (checked4)
															{
																this.VUsbuilder.Append(string.Concat(new string[]
																{
																	text3,
																	" ",
																	text5,
																	" : ",
																	text11,
																	this.AreaUniteSuffixe,
																	"\n"
																}));
															}
															list.Add(text5);
														}
													}
												}
												bool flag24 = parameter.Definition.ParameterType == ParameterType.Number;
												if (flag24)
												{
													bool hasValue4 = parameter.HasValue;
													if (hasValue4)
													{
														bool flag25 = parameter.AsDouble().ToString() != null;
														if (flag25)
														{
															double value7 = parameter.AsDouble();
															double value8 = Math.Round(value7, 2);
															string str3 = text5.Replace("\"", "\\\"");
															string text13 = Convert.ToString(value8);
															string str4 = text13.Replace("\"", "\\\"");
															this.MESSsbuilder.Append(str3 + " : " + str4 + " \\n");
															bool checked5 = this.checkBoxToVU.Checked;
															if (checked5)
															{
																this.VUsbuilder.Append(string.Concat(new string[]
																{
																	text3,
																	" ",
																	text5,
																	" : ",
																	text13,
																	"\n"
																}));
															}
															list.Add(text5);
														}
													}
												}
												bool flag26 = parameter.Definition.ParameterType == ParameterType.Text;
												if (flag26)
												{
													bool hasValue5 = parameter.HasValue;
													if (hasValue5)
													{
														bool flag27 = parameter.AsString() != null;
														if (flag27)
														{
															string value9 = parameter.AsString();
															string str5 = text5.Replace("\"", "\\\"");
															string text14 = Convert.ToString(value9);
															string str6 = text14.Replace("\"", "\\\"");
															this.MESSsbuilder.Append(str5 + " : " + str6 + " \\n");
															bool checked6 = this.checkBoxToVU.Checked;
															if (checked6)
															{
																this.VUsbuilder.Append(string.Concat(new string[]
																{
																	text3,
																	" ",
																	text5,
																	" : ",
																	text14,
																	"\n"
																}));
															}
														}
													}
												}
											}
										}
									}
								}
								foreach (object obj3 in elementType.Parameters)
								{
									Parameter parameter2 = (Parameter)obj3;
									bool flag28 = parameter2 != null;
									if (flag28)
									{
										bool flag29 = parameter2.Definition.Name == text5;
										if (flag29)
										{
											bool flag30 = !list.Contains(text5);
											if (flag30)
											{
												bool flag31 = parameter2.Definition.ParameterType == ParameterType.Length;
												if (flag31)
												{
													bool hasValue6 = parameter2.HasValue;
													if (hasValue6)
													{
														bool flag32 = parameter2.AsDouble().ToString() != null;
														if (flag32)
														{
															double value10 = 0.0;
															bool flag33 = this.UniteSuffixe == " mm";
															if (flag33)
															{
																value10 = parameter2.AsDouble() * 0.3048 * 1000.0;
															}
															bool flag34 = this.UniteSuffixe == " m";
															if (flag34)
															{
																value10 = parameter2.AsDouble() * 0.3048;
															}
															bool flag35 = this.UniteSuffixe == " ft";
															if (flag35)
															{
																value10 = parameter2.AsDouble();
															}
															bool flag36 = this.UniteSuffixe == " in";
															if (flag36)
															{
																value10 = parameter2.AsDouble() * 12.0;
															}
															double value11 = Math.Round(value10, 2);
															string text15 = text5.Replace("\"", "\\\"");
															string text16 = Convert.ToString(value11);
															string text17 = text16.Replace("\"", "\\\"");
															this.MESSsbuilder.Append(string.Concat(new string[]
															{
																text15,
																" : ",
																text17,
																this.UniteSuffixe,
																" \\n"
															}));
															bool checked7 = this.checkBoxToVU.Checked;
															if (checked7)
															{
																this.VUsbuilder.Append(string.Concat(new string[]
																{
																	text3,
																	" ",
																	text5,
																	" : ",
																	text16,
																	this.UniteSuffixe,
																	"\n"
																}));
															}
															list.Add(text5);
														}
													}
												}
											}
											bool flag37 = parameter2.Definition.ParameterType == ParameterType.Angle;
											if (flag37)
											{
												bool hasValue7 = parameter2.HasValue;
												if (hasValue7)
												{
													bool flag38 = parameter2.AsDouble().ToString() != null;
													if (flag38)
													{
														double value12 = 0.0;
														bool flag39 = this.UniteSuffixe == " mm";
														if (flag39)
														{
															value12 = ExpDataForm.RadianToDegree(parameter2.AsDouble());
														}
														bool flag40 = this.UniteSuffixe == " m";
														if (flag40)
														{
															value12 = ExpDataForm.RadianToDegree(parameter2.AsDouble());
														}
														bool flag41 = this.UniteSuffixe == " ft";
														if (flag41)
														{
															value12 = ExpDataForm.RadianToDegree(parameter2.AsDouble());
														}
														bool flag42 = this.UniteSuffixe == " in";
														if (flag42)
														{
															value12 = ExpDataForm.RadianToDegree(parameter2.AsDouble());
														}
														double value13 = Math.Round(value12, 2);
														string str7 = text5.Replace("\"", "\\\"");
														string text18 = Convert.ToString(value13);
														string str8 = text18.Replace("\"", "\\\"");
														this.MESSsbuilder.Append(str7 + " : " + str8 + " deg \\n");
														bool checked8 = this.checkBoxToVU.Checked;
														if (checked8)
														{
															this.VUsbuilder.Append(string.Concat(new string[]
															{
																text3,
																" ",
																text5,
																" : ",
																text18,
																" deg\n"
															}));
														}
														list.Add(text5);
													}
												}
											}
											bool flag43 = parameter2.Definition.ParameterType == ParameterType.Area;
											if (flag43)
											{
												bool hasValue8 = parameter2.HasValue;
												if (hasValue8)
												{
													bool flag44 = parameter2.AsDouble().ToString() != null;
													if (flag44)
													{
														double value14 = 0.0;
														bool flag45 = this.UniteSuffixe == " mm" | this.UniteSuffixe == " m";
														if (flag45)
														{
															value14 = parameter2.AsDouble() * Math.Pow(0.3048, 2.0);
														}
														bool flag46 = this.UniteSuffixe == " ft" | this.UniteSuffixe == " in";
														if (flag46)
														{
															value14 = parameter2.AsDouble();
														}
														double value15 = Math.Round(value14, 2);
														string text19 = text5.Replace("\"", "\\\"");
														string text20 = Convert.ToString(value15);
														string text21 = text20.Replace("\"", "\\\"");
														this.MESSsbuilder.Append(string.Concat(new string[]
														{
															text19,
															" : ",
															text21,
															this.AreaUniteSuffixe,
															" \\n"
														}));
														bool checked9 = this.checkBoxToVU.Checked;
														if (checked9)
														{
															this.VUsbuilder.Append(string.Concat(new string[]
															{
																text3,
																" ",
																text5,
																" : ",
																text20,
																this.AreaUniteSuffixe,
																"\n"
															}));
														}
														list.Add(text5);
													}
												}
											}
											bool flag47 = parameter2.Definition.ParameterType == ParameterType.Number;
											if (flag47)
											{
												bool hasValue9 = parameter2.HasValue;
												if (hasValue9)
												{
													bool flag48 = parameter2.AsDouble().ToString() != null;
													if (flag48)
													{
														double value16 = parameter2.AsDouble();
														double value17 = Math.Round(value16, 2);
														string str9 = text5.Replace("\"", "\\\"");
														string text22 = Convert.ToString(value17);
														string str10 = text22.Replace("\"", "\\\"");
														this.MESSsbuilder.Append(str9 + " : " + str10 + " \\n");
														bool checked10 = this.checkBoxToVU.Checked;
														if (checked10)
														{
															this.VUsbuilder.Append(string.Concat(new string[]
															{
																text3,
																" ",
																text5,
																" : ",
																text22,
																"\n"
															}));
														}
														list.Add(text5);
													}
												}
											}
											bool flag49 = parameter2.Definition.ParameterType == ParameterType.Text;
											if (flag49)
											{
												bool hasValue10 = parameter2.HasValue;
												if (hasValue10)
												{
													bool flag50 = parameter2.AsString() != null;
													if (flag50)
													{
														string value18 = parameter2.AsString();
														string str11 = text5.Replace("\"", "\\\"");
														string text23 = Convert.ToString(value18);
														string str12 = text23.Replace("\"", "\\\"");
														this.MESSsbuilder.Append(str11 + " : " + str12 + " \\n");
														bool checked11 = this.checkBoxToVU.Checked;
														if (checked11)
														{
															this.VUsbuilder.Append(string.Concat(new string[]
															{
																text3,
																" ",
																text5,
																" : ",
																text23,
																"\n"
															}));
														}
													}
												}
											}
										}
									}
								}
							}
						}
						this.MESSsbuilder.Append("\";\nbreak;\n");
					}
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000A0B4 File Offset: 0x000082B4
		private void ParamInfoList(Element element, Element elementType)
		{
			bool flag = element != null;
			if (flag)
			{
				bool flag2 = this.VerifyIdInstNameListfromView.Contains(element.Id.ToString());
				if (flag2)
				{
					foreach (object obj in element.Parameters)
					{
						Parameter parameter = (Parameter)obj;
						bool flag3 = parameter != null;
						if (flag3)
						{
							bool flag4 = parameter.Definition.ParameterType == ParameterType.Length;
							if (flag4)
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.Append(parameter.Definition.Name);
								bool flag5 = !this.ParameterList.Contains(stringBuilder.ToString());
								if (flag5)
								{
									this.ParameterList.Add(stringBuilder.ToString());
									this.listBoxParameters.Items.Add(stringBuilder.ToString());
								}
							}
							bool flag6 = parameter.Definition.ParameterType == ParameterType.Angle;
							if (flag6)
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								stringBuilder2.Append(parameter.Definition.Name);
								bool flag7 = !this.ParameterList.Contains(stringBuilder2.ToString());
								if (flag7)
								{
									this.ParameterList.Add(stringBuilder2.ToString());
									this.listBoxParameters.Items.Add(stringBuilder2.ToString());
								}
							}
							bool flag8 = parameter.Definition.ParameterType == ParameterType.Area;
							if (flag8)
							{
								StringBuilder stringBuilder3 = new StringBuilder();
								stringBuilder3.Append(parameter.Definition.Name);
								bool flag9 = !this.ParameterList.Contains(stringBuilder3.ToString());
								if (flag9)
								{
									this.ParameterList.Add(stringBuilder3.ToString());
									this.listBoxParameters.Items.Add(stringBuilder3.ToString());
								}
							}
							bool flag10 = parameter.Definition.ParameterType == ParameterType.Number;
							if (flag10)
							{
								StringBuilder stringBuilder4 = new StringBuilder();
								stringBuilder4.Append(parameter.Definition.Name);
								bool flag11 = !this.ParameterList.Contains(stringBuilder4.ToString());
								if (flag11)
								{
									this.ParameterList.Add(stringBuilder4.ToString());
									this.listBoxParameters.Items.Add(stringBuilder4.ToString());
								}
							}
							bool flag12 = parameter.Definition.ParameterType == ParameterType.Text;
							if (flag12)
							{
								StringBuilder stringBuilder5 = new StringBuilder();
								stringBuilder5.Append(parameter.Definition.Name);
								bool flag13 = !this.ParameterList.Contains(stringBuilder5.ToString());
								if (flag13)
								{
									this.ParameterList.Add(stringBuilder5.ToString());
									this.listBoxParameters.Items.Add(stringBuilder5.ToString());
								}
							}
						}
					}
					foreach (object obj2 in elementType.Parameters)
					{
						Parameter parameter2 = (Parameter)obj2;
						bool flag14 = parameter2 != null;
						if (flag14)
						{
							bool flag15 = parameter2.Definition.ParameterType == ParameterType.Length;
							if (flag15)
							{
								StringBuilder stringBuilder6 = new StringBuilder();
								stringBuilder6.Append(parameter2.Definition.Name);
								bool flag16 = !this.ParameterList.Contains(stringBuilder6.ToString());
								if (flag16)
								{
									this.ParameterList.Add(stringBuilder6.ToString());
									this.listBoxParameters.Items.Add(stringBuilder6.ToString());
								}
							}
							bool flag17 = parameter2.Definition.ParameterType == ParameterType.Angle;
							if (flag17)
							{
								StringBuilder stringBuilder7 = new StringBuilder();
								stringBuilder7.Append(parameter2.Definition.Name);
								bool flag18 = !this.ParameterList.Contains(stringBuilder7.ToString());
								if (flag18)
								{
									this.ParameterList.Add(stringBuilder7.ToString());
									this.listBoxParameters.Items.Add(stringBuilder7.ToString());
								}
							}
							bool flag19 = parameter2.Definition.ParameterType == ParameterType.Area;
							if (flag19)
							{
								StringBuilder stringBuilder8 = new StringBuilder();
								stringBuilder8.Append(parameter2.Definition.Name);
								bool flag20 = !this.ParameterList.Contains(stringBuilder8.ToString());
								if (flag20)
								{
									this.ParameterList.Add(stringBuilder8.ToString());
									this.listBoxParameters.Items.Add(stringBuilder8.ToString());
								}
							}
							bool flag21 = parameter2.Definition.ParameterType == ParameterType.Number;
							if (flag21)
							{
								StringBuilder stringBuilder9 = new StringBuilder();
								stringBuilder9.Append(parameter2.Definition.Name);
								bool flag22 = !this.ParameterList.Contains(stringBuilder9.ToString());
								if (flag22)
								{
									this.ParameterList.Add(stringBuilder9.ToString());
									this.listBoxParameters.Items.Add(stringBuilder9.ToString());
								}
							}
							bool flag23 = parameter2.Definition.ParameterType == ParameterType.Text;
							if (flag23)
							{
								StringBuilder stringBuilder10 = new StringBuilder();
								stringBuilder10.Append(parameter2.Definition.Name);
								bool flag24 = !this.ParameterList.Contains(stringBuilder10.ToString());
								if (flag24)
								{
									this.ParameterList.Add(stringBuilder10.ToString());
									this.listBoxParameters.Items.Add(stringBuilder10.ToString());
								}
							}
						}
					}
				}
				this.listBoxParameters.Sorted = true;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000A694 File Offset: 0x00008894
		private void listBoxParameters_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.listBoxParameters.SelectionMode = SelectionMode.MultiExtended;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000A6A4 File Offset: 0x000088A4
		private void buttonParamList_Click(object sender, EventArgs e)
		{
			Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
			Document document = this.p_commandData.Application.ActiveUIDocument.Document;
			List<Element> list = new List<Element>();
			FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
			ICollection<Element> collection = filteredElementCollector.OfClass(typeof(View3D)).ToElements();
			View3D view3D = null;
			bool flag = false;
			this.m_AllViews.ExportProperties = true;
			bool flag2 = document.ActiveView.ViewType != ViewType.ThreeD;
			if (flag2)
			{
				MessageBox.Show("The active view must be a 3D view type.");
				base.Close();
			}
			bool flag3 = document.ActiveView.ViewType == ViewType.ThreeD & document.ActiveView.IsTemplate;
			if (flag3)
			{
				MessageBox.Show("The active view is a template view and is not exportable.");
				flag = true;
				base.Close();
			}
			bool flag4 = document.ActiveView.ViewType == ViewType.ThreeD & !document.ActiveView.IsTemplate;
			if (flag4)
			{
				view3D = (document.ActiveView as View3D);
			}
			bool flag5 = !flag;
			if (flag5)
			{
				bool flag6 = view3D != null;
				if (flag6)
				{
					CheckExportContext checkExportContext = new CheckExportContext(document, this.m_AllViews);
					new CustomExporter(document, checkExportContext)
					{
						IncludeGeometricObjects = false,
						ShouldStopOnError = false
					}.Export(view3D);
					bool flag7 = checkExportContext.ListElementID01.Count != 0;
					if (flag7)
					{
						foreach (string item in checkExportContext.ListElementID01)
						{
							bool flag8 = !this.VerifyIdInstNameListfromView.Contains(item);
							if (flag8)
							{
								this.VerifyIdInstNameListfromView.Add(item);
							}
						}
					}
					bool flag9 = checkExportContext.ListLINKID01.Count != 0;
					if (flag9)
					{
						foreach (string item2 in checkExportContext.ListLINKID01)
						{
							bool flag10 = !this.VerifyIdLinkNameListfromViewForListBox.Contains(item2);
							if (flag10)
							{
								this.VerifyIdLinkNameListfromViewForListBox.Add(item2);
							}
						}
					}
				}
			}
			FilteredElementCollector filteredElementCollector2 = new FilteredElementCollector(document, view3D.Id);
			ICollection<ElementId> collection2 = filteredElementCollector2.ToElementIds();
			foreach (ElementId elementId in collection2)
			{
				Element element = document.GetElement(elementId);
				bool flag11 = element.Category != null && element.Category.HasMaterialQuantities;
				if (flag11)
				{
					list.Add(element);
				}
				bool flag12 = element.Category != null && element.Category.Id.IntegerValue == -2000171;
				if (flag12)
				{
					list.Add(element);
				}
			}
			foreach (Element element2 in list)
			{
				Element element3 = document.GetElement(element2.Id);
				Element element4 = document.GetElement(element2.GetTypeId());
				bool flag13 = element3 != null & element4 != null;
				if (flag13)
				{
					this.ParamInfoList(element3, element4);
				}
			}
			foreach (string value in this.VerifyIdLinkNameListfromViewForListBox)
			{
				FilteredElementCollector filteredElementCollector3 = new FilteredElementCollector(document);
				IList<Element> list2 = filteredElementCollector3.OfCategory((BuiltInCategory)(-2001352)).OfClass(typeof(RevitLinkType)).ToElements();
				int num = Convert.ToInt32(value);
				ElementId elementId2 = new ElementId(num);
				RevitLinkType revitLinkType = document.GetElement(elementId2) as RevitLinkType;
				string name = revitLinkType.Name;
				bool flag14 = list2 != null;
				if (flag14)
				{
					bool flag15 = list2.Count > 0 & revitLinkType != null;
					if (flag15)
					{
						foreach (Element element5 in list2)
						{
							RevitLinkType revitLinkType2 = element5 as RevitLinkType;
							foreach (object obj in document.Application.Documents)
							{
								Document document2 = (Document)obj;
								bool flag16 = name.Contains(document2.Title);
								if (flag16)
								{
									FilteredElementCollector filteredElementCollector4 = new FilteredElementCollector(document2).WhereElementIsNotElementType();
									foreach (Element element6 in filteredElementCollector4)
									{
										Element element3 = document2.GetElement(element6.Id);
										Element element4 = document2.GetElement(element6.GetTypeId());
										bool flag17 = element3 != null & element4 != null;
										if (flag17)
										{
											this.ParamInfoList(element3, element4);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000AC90 File Offset: 0x00008E90
		private void ExpDataForm_Load(object sender, EventArgs e)
		{
			this.m_AllViews.ExportProperties = false;
			this.m_AllViews.ExportProperties = false;
			bool flag = this.ProjectUnits(this.units) == this.mm;
			if (flag)
			{
				this.mmButton.Checked = true;
			}
			bool flag2 = this.ProjectUnits(this.units) == this.m;
			if (flag2)
			{
				this.mButton.Checked = true;
			}
			bool flag3 = this.ProjectUnits(this.units) == this.inches;
			if (flag3)
			{
				this.inchesButton.Checked = true;
			}
			bool flag4 = this.ProjectUnits(this.units) == this.ft;
			if (flag4)
			{
				this.feetButton.Checked = true;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000AD60 File Offset: 0x00008F60
		private static double DegreeToRadian(double angle)
		{
			return 3.1415926535897931 * angle / 180.0;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000AD88 File Offset: 0x00008F88
		private static double RadianToDegree(double angle)
		{
			return angle * 57.295779513082323;
		}

		// Token: 0x0400006C RID: 108
		private ExternalCommandData p_commandData;

		// Token: 0x0400006D RID: 109
		private AllViews m_AllViews;

		// Token: 0x0400006E RID: 110
		private const double MM = 0.0032808;

		// Token: 0x0400006F RID: 111
		private const double Metre = 3.2808398;

		// Token: 0x04000070 RID: 112
		private const double Inches = 0.0833333;

		// Token: 0x04000071 RID: 113
		private const double I = 1.0;

		// Token: 0x04000072 RID: 114
		private double unite = 1.0;

		// Token: 0x04000073 RID: 115
		private string UniteSuffixe = " ft";

		// Token: 0x04000074 RID: 116
		private string AreaUniteSuffixe = " ft2";

		// Token: 0x04000075 RID: 117
		private const double METERS_IN_FEET = 0.3048;

		// Token: 0x04000076 RID: 118
		private const double _eps = 1E-09;

		// Token: 0x04000077 RID: 119
		private const double _feet_to_mm = 304.79999999999995;

		// Token: 0x04000078 RID: 120
		private string m = "m";

		// Token: 0x04000079 RID: 121
		private string mm = "mm";

		// Token: 0x0400007A RID: 122
		private string ft = "ft";

		// Token: 0x0400007B RID: 123
		private string inches = "in";

		// Token: 0x0400007C RID: 124
		private string units = null;

		// Token: 0x0400007D RID: 125
		private List<string> VerifyIdInstNameListfromView = new List<string>();

		// Token: 0x0400007E RID: 126
		private List<string> VerifyIdLinkNameListfromView = new List<string>();

		// Token: 0x0400007F RID: 127
		private List<string> VerifyIdLinkNameListfromViewForListBox = new List<string>();

		// Token: 0x04000080 RID: 128
		private List<string> VerifyIdInstNameList = new List<string>();

		// Token: 0x04000081 RID: 129
		private StringBuilder MESSsbuilder = new StringBuilder();

		// Token: 0x04000082 RID: 130
		private StringBuilder INFOsbuilder = new StringBuilder();

		// Token: 0x04000083 RID: 131
		private StringBuilder VUsbuilder = new StringBuilder();

		// Token: 0x04000084 RID: 132
		private List<string> ParameterList = new List<string>();
	}
}
