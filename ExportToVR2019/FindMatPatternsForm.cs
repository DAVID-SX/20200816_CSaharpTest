using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;

namespace ExportToVR
{
	// Token: 0x0200000D RID: 13
	public partial class FindMatPatternsForm : System.Windows.Forms.Form
    {
		// Token: 0x060000AC RID: 172 RVA: 0x0000BD34 File Offset: 0x00009F34
		public FindMatPatternsForm(ExternalCommandData commandData, AllViews families)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = families;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000BDA6 File Offset: 0x00009FA6
		private void ViewForm_Load(object sender, EventArgs e)
		{
			this.m_AllViews.VerticeNb = 0;
			this.m_AllViews.VerticeNb = 2000000;
			this.m_AllViews.MaxVerticesPerObj = false;
			this.m_AllViews.MaxVerticesPerObj = false;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000BDE4 File Offset: 0x00009FE4
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

		// Token: 0x060000AF RID: 175 RVA: 0x0000BE58 File Offset: 0x0000A058
		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.79999999999995;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000BE78 File Offset: 0x0000A078
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
				result = this.ft;
			}
			return result;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		private void buttonOK_Click(object sender, EventArgs e)
		{
			try
			{
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				UIDocument activeUIDocument = this.p_commandData.Application.ActiveUIDocument;
				ICollection<ElementId> elementIds = activeUIDocument.Selection.GetElementIds();
				bool flag = elementIds != null & elementIds.Count > 0;
				if (flag)
				{
					elementIds.Clear();
				}
				bool flag2 = this.listBoxElements.SelectedItems.Count != 0;
				if (flag2)
				{
					foreach (object obj in this.listBoxElements.SelectedItems)
					{
						string text = (string)obj;
						string[] array = text.Split(new char[]
						{
							'_'
						});
						string text2 = array[0].ToString();
						string value = array[1].ToString();
						int num = Convert.ToInt32(value);
						ElementId elementId = new ElementId(num);
						Element element = document.GetElement(elementId);
						bool flag3 = elementId != null;
						if (flag3)
						{
							elementIds.Add(elementId);
						}
					}
				}
				bool flag4 = elementIds != null;
				if (flag4)
				{
					bool flag5 = elementIds.Count > 0;
					if (flag5)
					{
						activeUIDocument.Selection.SetElementIds(elementIds);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000C090 File Offset: 0x0000A290
		public bool IsProcessOpen(string name)
		{
			foreach (Process process in Process.GetProcesses())
			{
				bool flag = process.ProcessName.Contains(name);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		private void OpenVUIfClosed()
		{
			string text = "C:\\Citron\\VU\\OpenLive\\VU.exe";
			bool flag = File.Exists(text);
			if (flag)
			{
				Process process = new Process();
				process.StartInfo.FileName = text;
				bool flag2 = !this.IsProcessOpen(text);
				if (flag2)
				{
					process.Start();
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000C124 File Offset: 0x0000A324
		public void ZipTheFile()
		{
			string text = Path.Combine(Path.GetDirectoryName(this._path), Path.GetFileNameWithoutExtension(this._path));
			string path = this._path;
			string text2 = Path.GetDirectoryName(this._path) + ".zip";
			bool flag = File.Exists(text2);
			if (flag)
			{
				File.Delete(text2);
			}
			ZipFile.CreateFromDirectory(path, text2);
			this._path = text2;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000C190 File Offset: 0x0000A390
		private void checkBoxLinkTransp_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBoxLinkTransp.Checked;
			if (@checked)
			{
				this.m_AllViews.LinkTransparent = true;
			}
			bool flag = !this.checkBoxLinkTransp.Checked;
			if (flag)
			{
				this.m_AllViews.LinkTransparent = false;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		private void buttonShowPatterns_Click(object sender, EventArgs e)
		{
			try
			{
				this.m_AllViews.FindPatterns = true;
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
				ICollection<Element> collection = filteredElementCollector.OfClass(typeof(View3D)).ToElements();
				View3D view3D = null;
				FilteredElementCollector filteredElementCollector2 = new FilteredElementCollector(document);
				ICollection<Element> collection2 = filteredElementCollector2.OfClass(typeof(View3D)).ToElements();
				FilteredElementCollector filteredElementCollector3 = new FilteredElementCollector(document);
				ICollection<Element> collection3 = filteredElementCollector3.OfClass(typeof(FamilySymbol)).ToElements();
				FilteredElementCollector filteredElementCollector4 = new FilteredElementCollector(document);
				ICollection<Element> collection4 = filteredElementCollector4.OfClass(typeof(FamilySymbol)).ToElements();
				FilteredElementCollector filteredElementCollector5 = new FilteredElementCollector(document);
				ICollection<Element> collection5 = filteredElementCollector5.OfClass(typeof(FamilyInstance)).ToElements();
				List<string> list = new List<string>();
				MessageBoxIcon icon = MessageBoxIcon.Exclamation;
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				string caption = "ef | Export To Unity";
				int num = 1;
				bool @checked = this.radioButtonSingleObject.Checked;
				if (@checked)
				{
					num = 1;
				}
				bool checked2 = this.radioButtonByTypes.Checked;
				if (checked2)
				{
					num = 5;
				}
				bool checked3 = this.radioButtonMaterialsFast.Checked;
				if (checked3)
				{
					num = 6;
				}
				bool flag = false;
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
				bool checked4 = this.checkBoxStartVU.Checked;
				if (checked4)
				{
				}
				Transaction transaction = new Transaction(document);
				transaction.Start("HideAnnotations");
				foreach (object obj in document.Settings.Categories)
				{
					Category category = (Category)obj;
					bool flag5 = category.get_AllowsVisibilityControl(view3D);
					if (flag5)
					{
						bool flag6 = category.CategoryType != CategoryType.Model;
						if (flag6)
						{
							view3D.SetCategoryHidden(category.Id, true);
						}
					}
				}
				transaction.Commit();
				int num2 = 500000;
				int num3 = 2000000;
				int num4 = num3 * 2;
				bool flag7 = !this.checkBoxMaxVertices.Checked;
				if (flag7)
				{
					bool flag8 = !flag;
					if (flag8)
					{
						bool flag9 = view3D != null;
						if (flag9)
						{
							CheckExportContext checkExportContext = new CheckExportContext(document, this.m_AllViews);
							new CustomExporter(document, checkExportContext)
							{
								IncludeGeometricObjects = false,
								ShouldStopOnError = false
							}.Export(view3D);
							this.m_AllViews.GroupingOptions = 0;
							bool checked5 = this.radioButtonSingleObject.Checked;
							if (checked5)
							{
								this.m_AllViews.GroupingOptions = 3;
							}
							bool checked6 = this.radioButtonByTypes.Checked;
							if (checked6)
							{
								this.m_AllViews.GroupingOptions = 5;
							}
							bool checked7 = this.radioButtonMaterialsFast.Checked;
							if (checked7)
							{
								this.m_AllViews.GroupingOptions = 6;
							}
							bool flag10 = checkExportContext.TotalNBofPoints > num2;
							if (flag10)
							{
								DialogResult dialogResult = MessageBox.Show("Cette vue contient " + checkExportContext.TotalNBofPoints.ToString() + " vertices.\nVoulez-vous vraiment continuer?", "Avertissement", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
								bool flag11 = dialogResult == DialogResult.No;
								if (flag11)
								{
									flag = true;
								}
							}
							bool flag12 = checkExportContext.TotalNBofPoints > num4;
							if (flag12)
							{
								MessageBox.Show(string.Concat(new string[]
								{
									"This 3D View contains ",
									checkExportContext.TotalNBofPoints.ToString(),
									" Vertices.\nMax Vertices per Export: ",
									num4.ToString(),
									"."
								}), caption, buttons, icon);
								flag = true;
							}
						}
					}
				}
				bool flag13 = flag;
				if (flag13)
				{
					base.Close();
				}
				bool flag14 = application.VersionNumber.Contains("2019") | application.VersionNumber.Contains("2020");
				if (flag14)
				{
					bool flag15 = !this.checkBoxMaxVertices.Checked;
					if (flag15)
					{
						bool flag16 = view3D != null & !flag;
						if (flag16)
						{
							this.context = new CheckExportContext(document, this.m_AllViews);
							CustomExporter customExporter = new CustomExporter(document, this.context);
							customExporter.IncludeGeometricObjects = false;
							customExporter.ShouldStopOnError = false;
							try
							{
								this.m_AllViews.ExportSubCategories = false;
								bool flag17 = num == 1 | num == 5 | num == 6;
								if (flag17)
								{
									int num5 = 1000;
									FilteredElementCollector filteredElementCollector6 = new FilteredElementCollector(document, view3D.Id);
									ICollection<ElementId> collection6 = filteredElementCollector6.ToElementIds();
									ICollection<ElementId> collection7 = filteredElementCollector6.ToElementIds();
									collection7.Clear();
									List<int> list2 = new List<int>();
									List<int> list3 = new List<int>();
									List<int> list4 = new List<int>();
									bool flag18 = false;
									foreach (ElementId elementId in collection6)
									{
										bool flag19 = false;
										Element element = document.GetElement(elementId);
										bool flag20 = element != null;
										if (flag20)
										{
											bool flag21 = element.Category != null;
											if (flag21)
											{
												bool flag22 = element.Category.CategoryType == CategoryType.Model;
												if (flag22)
												{
													flag19 = true;
												}
												bool flag23 = element.Category.Id.IntegerValue == -2001340;
												if (flag23)
												{
													flag18 = true;
												}
												bool flag24 = element.Category.Id.IntegerValue == -2001352;
												if (flag24)
												{
													flag18 = true;
												}
											}
											bool flag25 = element.GetTypeId() != null;
											if (flag25)
											{
												int integerValue = element.GetTypeId().IntegerValue;
												bool flag26 = flag19 & !flag18;
												if (flag26)
												{
													GeometryElement geometryElement = element.get_Geometry(new Options
													{
														ComputeReferences = true
													});
													bool flag27 = geometryElement != null;
													if (flag27)
													{
														foreach (GeometryObject geometryObject in geometryElement)
														{
															bool flag28 = geometryObject is Solid;
															if (flag28)
															{
																Solid solid = geometryObject as Solid;
																bool flag29 = null != solid;
																if (flag29)
																{
																	bool flag30 = solid.Faces.Size > 0;
																	if (flag30)
																	{
																		flag18 = true;
																		break;
																	}
																}
															}
															GeometryInstance geometryInstance = geometryObject as GeometryInstance;
															bool flag31 = null != geometryInstance;
															if (flag31)
															{
																foreach (GeometryObject geometryObject2 in geometryInstance.SymbolGeometry)
																{
																	Solid solid2 = geometryObject2 as Solid;
																	bool flag32 = null != solid2;
																	if (flag32)
																	{
																		bool flag33 = solid2.Faces.Size > 0;
																		if (flag33)
																		{
																			flag18 = true;
																			break;
																		}
																	}
																}
															}
														}
													}
												}
												bool flag34 = !list2.Contains(integerValue) && flag18;
												if (flag34)
												{
													list2.Add(integerValue);
												}
											}
										}
										flag18 = false;
									}
									for (int i = 0; i < list2.Count; i++)
									{
										int item = list2[i];
										int num6 = 0;
										bool flag35 = num6 <= num5;
										if (flag35)
										{
											list4.Add(item);
										}
										bool flag36 = num6 > num5;
										if (flag36)
										{
											list3.Add(item);
										}
									}
									bool flag37 = list4.Count > 0;
									if (flag37)
									{
										bool flag38 = false;
										foreach (ElementId elementId2 in collection6)
										{
											Element element2 = document.GetElement(elementId2);
											bool flag39 = element2 != null;
											if (flag39)
											{
												int integerValue2 = element2.GetTypeId().IntegerValue;
												bool flag40 = !list4.Contains(integerValue2);
												if (flag40)
												{
													bool flag41 = element2.Category != null;
													if (flag41)
													{
														bool flag42 = element2.Category.Id.IntegerValue == -2001340;
														if (flag42)
														{
															flag38 = true;
														}
													}
													bool flag43 = !flag18;
													if (flag43)
													{
														GeometryElement geometryElement2 = element2.get_Geometry(new Options
														{
															ComputeReferences = true
														});
														bool flag44 = geometryElement2 != null;
														if (flag44)
														{
															foreach (GeometryObject geometryObject3 in geometryElement2)
															{
																bool flag45 = geometryObject3 is Solid;
																if (flag45)
																{
																	Solid solid3 = geometryObject3 as Solid;
																	bool flag46 = null != solid3;
																	if (flag46)
																	{
																		bool flag47 = solid3.Faces.Size > 0;
																		if (flag47)
																		{
																			flag38 = true;
																			break;
																		}
																	}
																}
																GeometryInstance geometryInstance2 = geometryObject3 as GeometryInstance;
																bool flag48 = null != geometryInstance2;
																if (flag48)
																{
																	foreach (GeometryObject geometryObject4 in geometryInstance2.SymbolGeometry)
																	{
																		Solid solid4 = geometryObject4 as Solid;
																		bool flag49 = null != solid4;
																		if (flag49)
																		{
																			bool flag50 = solid4.Faces.Size > 0;
																			if (flag50)
																			{
																				flag38 = true;
																				break;
																			}
																		}
																	}
																}
															}
														}
													}
													bool flag51 = flag38;
													if (flag51)
													{
														bool flag52 = element2.CanBeHidden(view3D);
														if (flag52)
														{
															collection7.Add(elementId2);
														}
													}
												}
											}
											flag38 = false;
										}
										Transaction transaction2 = new Transaction(document);
										transaction2.Start("TempHideType");
										bool flag53 = collection7.Count > 0;
										if (flag53)
										{
											view3D.HideElements(collection7);
										}
										transaction2.Commit();
										customExporter.Export(view3D);
										Transaction transaction3 = new Transaction(document);
										transaction3.Start("TempUnhideType");
										bool flag54 = collection7.Count > 0;
										if (flag54)
										{
											view3D.UnhideElements(collection7);
										}
										transaction3.Commit();
										collection7.Clear();
									}
									bool flag55 = list3.Count > 0;
									if (flag55)
									{
										foreach (int num7 in list3)
										{
											bool flag56 = false;
											bool flag57 = num7 != -1;
											if (flag57)
											{
												foreach (ElementId elementId3 in collection6)
												{
													Element element3 = document.GetElement(elementId3);
													bool flag58 = element3 != null;
													if (flag58)
													{
														int integerValue3 = element3.GetTypeId().IntegerValue;
														bool flag59 = num7 != integerValue3;
														if (flag59)
														{
															bool flag60 = element3.Category != null;
															if (flag60)
															{
																bool flag61 = element3.Category.Id.IntegerValue == -2001340;
																if (flag61)
																{
																	flag56 = true;
																}
															}
															bool flag62 = !flag56;
															if (flag62)
															{
																GeometryElement geometryElement3 = element3.get_Geometry(new Options
																{
																	ComputeReferences = true
																});
																bool flag63 = geometryElement3 != null;
																if (flag63)
																{
																	foreach (GeometryObject geometryObject5 in geometryElement3)
																	{
																		bool flag64 = geometryObject5 is Solid;
																		if (flag64)
																		{
																			Solid solid5 = geometryObject5 as Solid;
																			bool flag65 = null != solid5;
																			if (flag65)
																			{
																				bool flag66 = solid5.Faces.Size > 0;
																				if (flag66)
																				{
																					flag56 = true;
																					break;
																				}
																			}
																		}
																		GeometryInstance geometryInstance3 = geometryObject5 as GeometryInstance;
																		bool flag67 = null != geometryInstance3;
																		if (flag67)
																		{
																			foreach (GeometryObject geometryObject6 in geometryInstance3.SymbolGeometry)
																			{
																				Solid solid6 = geometryObject6 as Solid;
																				bool flag68 = null != solid6;
																				if (flag68)
																				{
																					bool flag69 = solid6.Faces.Size > 0;
																					if (flag69)
																					{
																						flag56 = true;
																						break;
																					}
																				}
																			}
																		}
																	}
																}
															}
															bool flag70 = flag56;
															if (flag70)
															{
																bool flag71 = element3.CanBeHidden(view3D);
																if (flag71)
																{
																	collection7.Add(elementId3);
																}
															}
														}
													}
													flag56 = false;
												}
												Transaction transaction4 = new Transaction(document);
												transaction4.Start("TempHideType");
												bool flag72 = collection7.Count > 0;
												if (flag72)
												{
													view3D.HideElements(collection7);
												}
												transaction4.Commit();
												customExporter.Export(view3D);
												Transaction transaction5 = new Transaction(document);
												transaction5.Start("TempUnhideType");
												bool flag73 = collection7.Count > 0;
												if (flag73)
												{
													view3D.UnhideElements(collection7);
												}
												transaction5.Commit();
												collection7.Clear();
											}
										}
									}
								}
							}
							catch (ExternalApplicationException ex)
							{
								Debug.Print("ExternalApplicationException " + ex.Message);
							}
							bool flag74 = !flag;
							if (flag74)
							{
								foreach (int num8 in this.context.ListMaterialID)
								{
									bool flag75 = num8 != ElementId.InvalidElementId.IntegerValue & !num8.ToString().Contains("-") & num8.ToString() != "-1";
									if (flag75)
									{
										ElementId elementId4 = new ElementId(num8);
										Material material = document.GetElement(elementId4) as Material;
										bool flag76 = material != null;
										if (flag76)
										{
											FillPatternElement fillPatternElement = document.GetElement(material.SurfaceForegroundPatternId) as FillPatternElement;
											bool flag77 = fillPatternElement != null;
											if (flag77)
											{
												bool flag78 = !list.Contains(fillPatternElement.Name);
												if (flag78)
												{
													list.Add(fillPatternElement.Name);
													this.h_MatIDPatID.Add(material.Id.IntegerValue, fillPatternElement.Id.IntegerValue);
													this.key_MatIDPatID = this.h_MatIDPatID.Keys;
												}
											}
										}
									}
								}
								bool flag79 = list != null;
								if (flag79)
								{
									bool flag80 = list.Count > 0;
									if (flag80)
									{
										this.listBoxPatterns.DataSource = list;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000D25C File Offset: 0x0000B45C
		private void buttonShowElements_Click(object sender, EventArgs e)
		{
			try
			{
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
				FilteredElementCollector filteredElementCollector2 = filteredElementCollector.WherePasses(new ElementClassFilter(typeof(Material)));
				FilteredElementCollector filteredElementCollector3 = new FilteredElementCollector(document);
				FilteredElementCollector filteredElementCollector4 = filteredElementCollector3.WherePasses(new ElementClassFilter(typeof(FillPatternElement)));
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				FillPatternElement fillPatternElement = null;
				foreach (object obj in this.listBoxPatterns.SelectedItems)
				{
					string b = (string)obj;
					foreach (Element element in filteredElementCollector4)
					{
						FillPatternElement fillPatternElement2 = (FillPatternElement)element;
						fillPatternElement = fillPatternElement2;
						bool flag = fillPatternElement != null;
						if (flag)
						{
							bool flag2 = fillPatternElement.Name == b;
							if (flag2)
							{
								break;
							}
						}
					}
					foreach (object obj2 in this.key_MatIDPatID)
					{
						int num = (int)obj2;
						bool flag3 = fillPatternElement.Id.IntegerValue == (int)this.h_MatIDPatID[num];
						if (flag3)
						{
							ElementId elementId = new ElementId(num);
							Material material = document.GetElement(elementId) as Material;
							bool flag4 = material != null;
							if (flag4)
							{
								foreach (object obj3 in this.context.key_ElementIDListMatID)
								{
									int num2 = (int)obj3;
									foreach (int num3 in ((List<int>)this.context.h_ElementIDListMatID[num2]))
									{
										bool flag5 = num3 == num;
										if (flag5)
										{
											bool flag6 = !list.Contains(num2.ToString());
											if (flag6)
											{
												list.Add(num2.ToString());
											}
										}
									}
								}
							}
						}
					}
				}
				bool flag7 = list != null;
				if (flag7)
				{
					bool flag8 = list.Count > 0;
					if (flag8)
					{
						foreach (string text in list)
						{
							int num4 = Convert.ToInt32(text);
							ElementId elementId2 = new ElementId(num4);
							Element element2 = document.GetElement(elementId2);
							bool flag9 = element2 != null;
							if (flag9)
							{
								string item = element2.Category.Name + "_" + text;
								bool flag10 = !list2.Contains(item);
								if (flag10)
								{
									list2.Add(item);
								}
							}
						}
					}
				}
				bool flag11 = list2 != null;
				if (flag11)
				{
					bool flag12 = list2.Count > 0;
					if (flag12)
					{
						list2.Sort();
						this.listBoxElements.DataSource = list2;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
		private void listBoxElements_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.listBoxElements.SelectionMode = SelectionMode.MultiExtended;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000D6B4 File Offset: 0x0000B8B4
		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
		private void listBoxPatterns_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x04000099 RID: 153
		private ExternalCommandData p_commandData;

		// Token: 0x0400009A RID: 154
		private AllViews m_AllViews;

		// Token: 0x0400009B RID: 155
		private CheckExportContext context;

		// Token: 0x0400009C RID: 156
		private Hashtable h_ElementIDMatNamesList = new Hashtable();

		// Token: 0x0400009D RID: 157
		private ICollection key_ElementIDMatNamesList = null;

		// Token: 0x0400009E RID: 158
		private Hashtable h_MatIDPatID = new Hashtable();

		// Token: 0x0400009F RID: 159
		private ICollection key_MatIDPatID = null;

		// Token: 0x040000A0 RID: 160
		private const double _eps = 1E-09;

		// Token: 0x040000A1 RID: 161
		private const double _feet_to_mm = 304.79999999999995;

		// Token: 0x040000A2 RID: 162
		private string mm = "mm";

		// Token: 0x040000A3 RID: 163
		private string ft = "ft";

		// Token: 0x040000A4 RID: 164
		private string units = null;

		// Token: 0x040000A5 RID: 165
		private static string _export_folder_name = null;

		// Token: 0x040000A6 RID: 166
		public int TotalElementInView;

		// Token: 0x040000A7 RID: 167
		private string _path;
	}
}
