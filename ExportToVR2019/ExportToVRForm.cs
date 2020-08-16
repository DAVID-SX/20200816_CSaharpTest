using System;
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
using ExportToVR.Properties;

namespace ExportToVR
{
	public partial class ExportToVRForm : System.Windows.Forms.Form
    {
		// Token: 0x0600012E RID: 302 RVA: 0x000245CC File Offset: 0x000227CC
		public ExportToVRForm(ExternalCommandData commandData, AllViews families)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = families;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0002461C File Offset: 0x0002281C
		private void ViewForm_Load(object sender, EventArgs e)
		{
			this.m_AllViews.VerticeNb = 0;
			this.m_AllViews.VerticeNb = this.trackBar1.Value;
			this.m_AllViews.MaxVerticesPerObj = false;
			this.m_AllViews.MaxVerticesPerObj = false;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00024668 File Offset: 0x00022868
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

		// Token: 0x06000131 RID: 305 RVA: 0x000246DC File Offset: 0x000228DC
		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.79999999999995;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000246FC File Offset: 0x000228FC
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

		// Token: 0x06000133 RID: 307 RVA: 0x0002476C File Offset: 0x0002296C
		private void buttonOK_Click(object sender, EventArgs e)
		{
			try
			{
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
				MessageBoxIcon icon = MessageBoxIcon.Exclamation;
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				string caption = "ef | Export To Unity";
				this.m_AllViews.StandAloneVersion = false;
				int num = 1;
				bool @checked = this.radioButtonSingleObject.Checked;
				if (@checked)
				{
					num = 1;
				}
				bool checked2 = this.radioButtonMaterials.Checked;
				if (checked2)
				{
					num = 2;
				}
				bool checked3 = this.radioButtonEntities.Checked;
				if (checked3)
				{
					num = 3;
				}
				bool checked4 = this.radioButtonSubcategories.Checked;
				if (checked4)
				{
					num = 4;
				}
				bool checked5 = this.radioButtonByTypes.Checked;
				if (checked5)
				{
					num = 5;
				}
				bool checked6 = this.radioButtonMaterialsFast.Checked;
				if (checked6)
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
				bool flag5 = false;
				Transaction transaction = new Transaction(document);
				transaction.Start("HideAnnotations");
				foreach (object obj in document.Settings.Categories)
				{
					Category category = (Category)obj;
					bool flag6 = category.get_AllowsVisibilityControl(view3D);
					if (flag6)
					{
						bool flag7 = category.CategoryType != CategoryType.Model;
						if (flag7)
						{
							view3D.SetCategoryHidden(category.Id, true);
						}
					}
				}
				transaction.Commit();
				int maximum = this.trackBar1.Maximum;
				int num2 = maximum * 2;
				bool flag8 = !this.checkBoxMaxVertices.Checked;
				if (flag8)
				{
					bool flag9 = !flag;
					if (flag9)
					{
						bool flag10 = view3D != null;
						if (flag10)
						{
							CheckExportContext checkExportContext = new CheckExportContext(document, this.m_AllViews);
							new CustomExporter(document, checkExportContext)
							{
								IncludeGeometricObjects = false,
								ShouldStopOnError = false
							}.Export(view3D);
							this.m_AllViews.GroupingOptions = 0;
							bool checked7 = this.radioButtonMaterials.Checked;
							if (checked7)
							{
								this.m_AllViews.GroupingOptions = 1;
							}
							bool checked8 = this.radioButtonEntities.Checked;
							if (checked8)
							{
								this.m_AllViews.GroupingOptions = 2;
							}
							bool checked9 = this.radioButtonSingleObject.Checked;
							if (checked9)
							{
								this.m_AllViews.GroupingOptions = 3;
							}
							bool checked10 = this.radioButtonSubcategories.Checked;
							if (checked10)
							{
								this.m_AllViews.GroupingOptions = 4;
							}
							bool checked11 = this.radioButtonByTypes.Checked;
							if (checked11)
							{
								this.m_AllViews.GroupingOptions = 5;
							}
							bool checked12 = this.radioButtonMaterialsFast.Checked;
							if (checked12)
							{
								this.m_AllViews.GroupingOptions = 6;
							}
							bool flag11 = checkExportContext.TotalNBofPoints > num2;
							if (flag11)
							{
								MessageBox.Show(string.Concat(new string[]
								{
									"This 3D View contains ",
									checkExportContext.TotalNBofPoints.ToString(),
									" Vertices.\nMax Vertices per Export: ",
									num2.ToString(),
									"."
								}), caption, buttons, icon);
								flag = true;
							}
						}
					}
				}
				bool checked13 = this.checkBoxMaxVertices.Checked;
				if (checked13)
				{
					bool flag12 = !flag;
					if (flag12)
					{
						bool flag13 = view3D != null;
						if (flag13)
						{
							CheckExportContext checkExportContext2 = new CheckExportContext(document, this.m_AllViews);
							new CustomExporter(document, checkExportContext2)
							{
								IncludeGeometricObjects = false,
								ShouldStopOnError = false
							}.Export(view3D);
							this.m_AllViews.IDListName01 = new List<string>();
							this.m_AllViews.IDListName02 = new List<string>();
							this.m_AllViews.IDListName03 = new List<string>();
							this.m_AllViews.IDListName04 = new List<string>();
							this.m_AllViews.IDListName05 = new List<string>();
							this.m_AllViews.IDListName06 = new List<string>();
							this.m_AllViews.IDListName07 = new List<string>();
							this.m_AllViews.IDListName08 = new List<string>();
							this.m_AllViews.IDListName09 = new List<string>();
							this.m_AllViews.IDListName10 = new List<string>();
							this.m_AllViews.ExportNB = 0;
							this.m_AllViews.GroupingOptions = 0;
							bool checked14 = this.radioButtonMaterials.Checked;
							if (checked14)
							{
								this.m_AllViews.GroupingOptions = 1;
							}
							bool checked15 = this.radioButtonEntities.Checked;
							if (checked15)
							{
								this.m_AllViews.GroupingOptions = 2;
							}
							bool checked16 = this.radioButtonSingleObject.Checked;
							if (checked16)
							{
								this.m_AllViews.GroupingOptions = 3;
							}
							bool flag14 = checkExportContext2.TotalNBofPoints <= maximum;
							if (flag14)
							{
								bool flag15 = checkExportContext2.ListElementID01.Count != 0;
								if (flag15)
								{
									foreach (string item in checkExportContext2.ListElementID01)
									{
										bool flag16 = !this.m_AllViews.IDListName01.Contains(item);
										if (flag16)
										{
											this.m_AllViews.IDListName01.Add(item);
										}
									}
									this.m_AllViews.ExportNB = 1;
								}
								bool flag17 = checkExportContext2.ListElementID02.Count != 0;
								if (flag17)
								{
									foreach (string item2 in checkExportContext2.ListElementID02)
									{
										bool flag18 = !this.m_AllViews.IDListName02.Contains(item2);
										if (flag18)
										{
											this.m_AllViews.IDListName02.Add(item2);
										}
									}
									this.m_AllViews.ExportNB = 2;
								}
								bool flag19 = checkExportContext2.ListElementID03.Count != 0;
								if (flag19)
								{
									foreach (string item3 in checkExportContext2.ListElementID03)
									{
										bool flag20 = !this.m_AllViews.IDListName03.Contains(item3);
										if (flag20)
										{
											this.m_AllViews.IDListName03.Add(item3);
										}
									}
									this.m_AllViews.ExportNB = 3;
								}
								bool flag21 = checkExportContext2.ListElementID04.Count != 0;
								if (flag21)
								{
									foreach (string item4 in checkExportContext2.ListElementID04)
									{
										bool flag22 = !this.m_AllViews.IDListName04.Contains(item4);
										if (flag22)
										{
											this.m_AllViews.IDListName04.Add(item4);
										}
									}
									this.m_AllViews.ExportNB = 4;
								}
								bool flag23 = checkExportContext2.ListElementID05.Count != 0;
								if (flag23)
								{
									foreach (string item5 in checkExportContext2.ListElementID05)
									{
										bool flag24 = !this.m_AllViews.IDListName05.Contains(item5);
										if (flag24)
										{
											this.m_AllViews.IDListName05.Add(item5);
										}
									}
									this.m_AllViews.ExportNB = 5;
								}
								bool flag25 = checkExportContext2.ListElementID06.Count != 0;
								if (flag25)
								{
									foreach (string item6 in checkExportContext2.ListElementID06)
									{
										bool flag26 = !this.m_AllViews.IDListName06.Contains(item6);
										if (flag26)
										{
											this.m_AllViews.IDListName06.Add(item6);
										}
									}
									this.m_AllViews.ExportNB = 6;
								}
								bool flag27 = checkExportContext2.ListElementID07.Count != 0;
								if (flag27)
								{
									foreach (string item7 in checkExportContext2.ListElementID07)
									{
										bool flag28 = !this.m_AllViews.IDListName07.Contains(item7);
										if (flag28)
										{
											this.m_AllViews.IDListName07.Add(item7);
										}
									}
									this.m_AllViews.ExportNB = 7;
								}
								bool flag29 = checkExportContext2.ListElementID08.Count != 0;
								if (flag29)
								{
									foreach (string item8 in checkExportContext2.ListElementID08)
									{
										bool flag30 = !this.m_AllViews.IDListName08.Contains(item8);
										if (flag30)
										{
											this.m_AllViews.IDListName08.Add(item8);
										}
									}
									this.m_AllViews.ExportNB = 8;
								}
								bool flag31 = checkExportContext2.ListElementID09.Count != 0;
								if (flag31)
								{
									foreach (string item9 in checkExportContext2.ListElementID09)
									{
										bool flag32 = !this.m_AllViews.IDListName09.Contains(item9);
										if (flag32)
										{
											this.m_AllViews.IDListName09.Add(item9);
										}
									}
									this.m_AllViews.ExportNB = 9;
								}
								bool flag33 = checkExportContext2.ListElementID10.Count != 0;
								if (flag33)
								{
									foreach (string item10 in checkExportContext2.ListElementID10)
									{
										bool flag34 = !this.m_AllViews.IDListName10.Contains(item10);
										if (flag34)
										{
											this.m_AllViews.IDListName10.Add(item10);
										}
									}
									this.m_AllViews.ExportNB = 10;
								}
							}
							bool flag35 = checkExportContext2.TotalNBofPoints > maximum;
							if (flag35)
							{
								MessageBox.Show(string.Concat(new string[]
								{
									"This 3D View contains ",
									checkExportContext2.TotalNBofPoints.ToString(),
									" Vertices.\nMax Vertices per Export: ",
									maximum.ToString(),
									"."
								}), caption, buttons, icon);
								flag = true;
							}
						}
					}
				}
				bool flag36 = flag;
				if (flag36)
				{
					base.Close();
				}
				bool flag37 = application.VersionNumber.Contains("2019") | application.VersionNumber.Contains("2020");
				if (flag37)
				{
					bool flag38 = !this.checkBoxMaxVertices.Checked;
					if (flag38)
					{
						bool flag39 = view3D != null & !flag;
						if (flag39)
						{
							string text = null;
							SaveFileDialog saveFileDialog = new SaveFileDialog();
							saveFileDialog.InitialDirectory = "C:\\";
							saveFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
							saveFileDialog.FilterIndex = 1;
							saveFileDialog.RestoreDirectory = true;
							saveFileDialog.FileName = null;
							bool flag40 = saveFileDialog.ShowDialog() == DialogResult.OK;
							if (flag40)
							{
								try
								{
									text = saveFileDialog.FileName;
								}
								catch (Exception ex)
								{
									MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
								}
							}
							else
							{
								bool flag41 = saveFileDialog.ShowDialog() == DialogResult.Cancel;
								if (flag41)
								{
									flag = true;
									return;
								}
							}
							ExportToVRContext exportToVRContext = new ExportToVRContext(document, this.m_AllViews);
							CustomExporter customExporter = new CustomExporter(document, exportToVRContext);
							customExporter.IncludeGeometricObjects = false;
							customExporter.ShouldStopOnError = false;
							try
							{
								this.m_AllViews.ExportSubCategories = false;
								bool flag42 = num == 2 | num == 3;
								if (flag42)
								{
									customExporter.Export(view3D);
								}
								bool flag43 = num == 4;
								if (flag43)
								{
									Categories categories = document.Settings.Categories;
									Category category2 = categories.get_Item((BuiltInCategory)(-2000025));
									Category category3 = null;
									Category category4 = null;
									foreach (object obj2 in document.Settings.Categories)
									{
										Category category5 = (Category)obj2;
										foreach (object obj3 in category5.SubCategories)
										{
											Category category6 = (Category)obj3;
											bool flag44 = category6.Id.IntegerValue == -2000025;
											if (flag44)
											{
												category3 = category6;
											}
											bool flag45 = category6.Id.IntegerValue == -2000031;
											if (flag45)
											{
												category4 = category6;
											}
										}
									}
									Transaction transaction2 = new Transaction(document);
									transaction2.Start("TempIsolatePanels");
									view3D.SetCategoryHidden(category3.Id, true);
									view3D.SetCategoryHidden(category4.Id, true);
									transaction2.Commit();
									customExporter.Export(view3D);
									Transaction transaction3 = new Transaction(document);
									transaction3.Start("TempHidePanels");
									view3D.SetCategoryHidden(category3.Id, false);
									view3D.SetCategoryHidden(category4.Id, false);
									foreach (object obj4 in document.Settings.Categories)
									{
										Category category7 = (Category)obj4;
										bool flag46 = category7.get_AllowsVisibilityControl(view3D);
										if (flag46)
										{
											bool flag47 = category7.Id.IntegerValue != -2000023;
											if (flag47)
											{
												view3D.SetCategoryHidden(category7.Id, true);
											}
										}
										foreach (object obj5 in category7.SubCategories)
										{
											Category category8 = (Category)obj5;
											bool flag48 = category8.get_AllowsVisibilityControl(view3D);
											if (flag48)
											{
												bool flag49 = category8.Id.IntegerValue != -2000025 && category8.Id.IntegerValue != -2000031;
												if (flag49)
												{
													view3D.SetCategoryHidden(category8.Id, true);
												}
											}
										}
									}
									transaction3.Commit();
									this.m_AllViews.ExportSubCategories = true;
									customExporter.Export(view3D);
								}
								bool flag50 = num == 1 | num == 5 | num == 6;
								if (flag50)
								{
									int num3 = 1000;
									FilteredElementCollector filteredElementCollector6 = new FilteredElementCollector(document, view3D.Id);
									ICollection<ElementId> collection6 = filteredElementCollector6.ToElementIds();
									ICollection<ElementId> collection7 = filteredElementCollector6.ToElementIds();
									collection7.Clear();
									List<int> list = new List<int>();
									List<int> list2 = new List<int>();
									List<int> list3 = new List<int>();
									bool flag51 = false;
									foreach (ElementId elementId in collection6)
									{
										bool flag52 = false;
										Element element = document.GetElement(elementId);
										bool flag53 = element != null;
										if (flag53)
										{
											bool flag54 = element.Category != null;
											if (flag54)
											{
												bool flag55 = element.Category.CategoryType == CategoryType.Model;
												
												if (flag55)
												{
													flag52 = true;
												}
												bool flag56 = element.Category.Id.IntegerValue == -2001340;
												if (flag56)
												{
													flag51 = true;
												}
												bool flag57 = element.Category.Id.IntegerValue == -2001352;
												if (flag57)
												{
													flag51 = true;
												}
											}
											bool flag58 = element.GetTypeId() != null;
											if (flag58)
											{
												int integerValue = element.GetTypeId().IntegerValue;
												bool flag59 = flag52 & !flag51;
												if (flag59)
												{
													GeometryElement geometryElement = element.get_Geometry(new Options
													{
														ComputeReferences = true
													});
													bool flag60 = geometryElement != null;
													if (flag60)
													{
														foreach (GeometryObject geometryObject in geometryElement)
														{
															bool flag61 = geometryObject is Solid;
															if (flag61)
															{
																Solid solid = geometryObject as Solid;
																bool flag62 = null != solid;
																if (flag62)
																{
																	bool flag63 = solid.Faces.Size > 0;
																	if (flag63)
																	{
																		flag51 = true;
																		break;
																	}
																}
															}
															GeometryInstance geometryInstance = geometryObject as GeometryInstance;
															bool flag64 = null != geometryInstance;
															if (flag64)
															{
																foreach (GeometryObject geometryObject2 in geometryInstance.SymbolGeometry)
																{
																	Solid solid2 = geometryObject2 as Solid;
																	bool flag65 = null != solid2;
																	if (flag65)
																	{
																		bool flag66 = solid2.Faces.Size > 0;
																		if (flag66)
																		{
																			flag51 = true;
																			break;
																		}
																	}
																}
															}
														}
													}
												}
												bool flag67 = !list.Contains(integerValue) && flag51;
												if (flag67)
												{
													list.Add(integerValue);
												}
											}
										}
										flag51 = false;
									}
									for (int i = 0; i < list.Count; i++)
									{
										int item11 = list[i];
										int num4 = 0;
										bool flag68 = num4 <= num3;
										if (flag68)
										{
											list3.Add(item11);
										}
										bool flag69 = num4 > num3;
										if (flag69)
										{
											list2.Add(item11);
										}
									}
									bool flag70 = list3.Count > 0;
									if (flag70)
									{
										bool flag71 = false;
										foreach (ElementId elementId2 in collection6)
										{
											Element element2 = document.GetElement(elementId2);
											bool flag72 = element2 != null;
											if (flag72)
											{
												int integerValue2 = element2.GetTypeId().IntegerValue;
												bool flag73 = !list3.Contains(integerValue2);
												if (flag73)
												{
													bool flag74 = element2.Category != null;
													if (flag74)
													{
														bool flag75 = element2.Category.Id.IntegerValue == -2001340;
														if (flag75)
														{
															flag71 = true;
														}
													}
													bool flag76 = !flag51;
													if (flag76)
													{
														GeometryElement geometryElement2 = element2.get_Geometry(new Options
														{
															ComputeReferences = true
														});
														bool flag77 = geometryElement2 != null;
														if (flag77)
														{
															foreach (GeometryObject geometryObject3 in geometryElement2)
															{
																bool flag78 = geometryObject3 is Solid;
																if (flag78)
																{
																	Solid solid3 = geometryObject3 as Solid;
																	bool flag79 = null != solid3;
																	if (flag79)
																	{
																		bool flag80 = solid3.Faces.Size > 0;
																		if (flag80)
																		{
																			flag71 = true;
																			break;
																		}
																	}
																}
																GeometryInstance geometryInstance2 = geometryObject3 as GeometryInstance;
																bool flag81 = null != geometryInstance2;
																if (flag81)
																{
																	foreach (GeometryObject geometryObject4 in geometryInstance2.SymbolGeometry)
																	{
																		Solid solid4 = geometryObject4 as Solid;
																		bool flag82 = null != solid4;
																		if (flag82)
																		{
																			bool flag83 = solid4.Faces.Size > 0;
																			if (flag83)
																			{
																				flag71 = true;
																				break;
																			}
																		}
																	}
																}
															}
														}
													}
													bool flag84 = flag71;
													if (flag84)
													{
														bool flag85 = element2.CanBeHidden(view3D);
														if (flag85)
														{
															collection7.Add(elementId2);
														}
													}
												}
											}
											flag71 = false;
										}
										Transaction transaction4 = new Transaction(document);
										transaction4.Start("TempHideType");
										bool flag86 = collection7.Count > 0;
										if (flag86)
										{
											view3D.HideElements(collection7);
										}
										transaction4.Commit();
										customExporter.Export(view3D);
										Transaction transaction5 = new Transaction(document);
										transaction5.Start("TempUnhideType");
										bool flag87 = collection7.Count > 0;
										if (flag87)
										{
											view3D.UnhideElements(collection7);
										}
										transaction5.Commit();
										collection7.Clear();
									}
									bool flag88 = list2.Count > 0;
									if (flag88)
									{
										foreach (int num5 in list2)
										{
											bool flag89 = false;
											bool flag90 = num5 != -1;
											if (flag90)
											{
												foreach (ElementId elementId3 in collection6)
												{
													Element element3 = document.GetElement(elementId3);
													bool flag91 = element3 != null;
													if (flag91)
													{
														int integerValue3 = element3.GetTypeId().IntegerValue;
														bool flag92 = num5 != integerValue3;
														if (flag92)
														{
															bool flag93 = element3.Category != null;
															if (flag93)
															{
																bool flag94 = element3.Category.Id.IntegerValue == -2001340;
																if (flag94)
																{
																	flag89 = true;
																}
															}
															bool flag95 = !flag89;
															if (flag95)
															{
																GeometryElement geometryElement3 = element3.get_Geometry(new Options
																{
																	ComputeReferences = true
																});
																bool flag96 = geometryElement3 != null;
																if (flag96)
																{
																	foreach (GeometryObject geometryObject5 in geometryElement3)
																	{
																		bool flag97 = geometryObject5 is Solid;
																		if (flag97)
																		{
																			Solid solid5 = geometryObject5 as Solid;
																			bool flag98 = null != solid5;
																			if (flag98)
																			{
																				bool flag99 = solid5.Faces.Size > 0;
																				if (flag99)
																				{
																					flag89 = true;
																					break;
																				}
																			}
																		}
																		GeometryInstance geometryInstance3 = geometryObject5 as GeometryInstance;
																		bool flag100 = null != geometryInstance3;
																		if (flag100)
																		{
																			foreach (GeometryObject geometryObject6 in geometryInstance3.SymbolGeometry)
																			{
																				Solid solid6 = geometryObject6 as Solid;
																				bool flag101 = null != solid6;
																				if (flag101)
																				{
																					bool flag102 = solid6.Faces.Size > 0;
																					if (flag102)
																					{
																						flag89 = true;
																						break;
																					}
																				}
																			}
																		}
																	}
																}
															}
															bool flag103 = flag89;
															if (flag103)
															{
																bool flag104 = element3.CanBeHidden(view3D);
																if (flag104)
																{
																	collection7.Add(elementId3);
																}
															}
														}
													}
													flag89 = false;
												}
												Transaction transaction6 = new Transaction(document);
												transaction6.Start("TempHideType");
												bool flag105 = collection7.Count > 0;
												if (flag105)
												{
													view3D.HideElements(collection7);
												}
												transaction6.Commit();
												customExporter.Export(view3D);
												Transaction transaction7 = new Transaction(document);
												transaction7.Start("TempUnhideType");
												bool flag106 = collection7.Count > 0;
												if (flag106)
												{
													view3D.UnhideElements(collection7);
												}
												transaction7.Commit();
												collection7.Clear();
											}
										}
									}
								}
							}
							catch (ExternalApplicationException ex2)
							{
								Debug.Print("ExternalApplicationException " + ex2.Message);
							}
							bool flag107 = !flag;
							if (flag107)
							{
								bool flag108 = File.Exists(text);
								if (flag108)
								{
									File.Delete(text);
								}
								bool flag109 = Directory.Exists(Path.GetDirectoryName(text));
								if (flag109)
								{
									using (StreamWriter streamWriter = new StreamWriter(text, true))
									{
										bool flag110 = num == 1;
										if (flag110)
										{
											streamWriter.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text),
												".mtl\n",
												this.m_AllViews.XYZsbuilder,
												"\n",
												this.m_AllViews.UVsbuilder,
												"\n",
												this.m_AllViews.NORMALsbuilder,
												"\n",
												this.m_AllViews.FCTsbuilder
											}));
										}
										bool flag111 = num == 2;
										if (flag111)
										{
											streamWriter.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text),
												".mtl\n",
												exportToVRContext.XYZsbuilder,
												"\n",
												exportToVRContext.UVsbuilder,
												"\n",
												exportToVRContext.NORMALsbuilder,
												"\n",
												exportToVRContext.FCTbyMATsbuilder
											}));
										}
										bool flag112 = num == 6;
										if (flag112)
										{
											streamWriter.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text),
												".mtl\n",
												this.m_AllViews.XYZsbuilder,
												"\n",
												this.m_AllViews.UVsbuilder,
												"\n",
												this.m_AllViews.NORMALsbuilder,
												"\n",
												this.m_AllViews.FCTbyMATsbuilder
											}));
										}
										bool flag113 = num == 3;
										if (flag113)
										{
											streamWriter.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text),
												".mtl\n",
												exportToVRContext.XYZsbuilder,
												"\n",
												exportToVRContext.UVsbuilder,
												"\n",
												exportToVRContext.NORMALsbuilder,
												"\n",
												exportToVRContext.FCTbyENTsbuilder
											}));
										}
										bool flag114 = num == 4;
										if (flag114)
										{
											streamWriter.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text),
												".mtl\n",
												this.m_AllViews.XYZsbuilder,
												"\n",
												this.m_AllViews.UVsbuilder,
												"\n",
												this.m_AllViews.NORMALsbuilder,
												"\n",
												this.m_AllViews.FCTbySUBCATsbuilder
											}));
										}
										bool flag115 = num == 5;
										if (flag115)
										{
											streamWriter.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text),
												".mtl\n",
												this.m_AllViews.XYZsbuilder,
												"\n",
												this.m_AllViews.UVsbuilder,
												"\n",
												this.m_AllViews.NORMALsbuilder,
												"\n",
												this.m_AllViews.FCTbySUBCATsbuilder
											}));
										}
										string path = text;
										text = Path.ChangeExtension(path, "mtl");
										streamWriter.Close();
									}
									using (StreamWriter streamWriter2 = new StreamWriter(text))
									{
										bool flag116 = num == 2 | num == 3 | num == 4;
										if (flag116)
										{
											bool flag117 = exportToVRContext.MATERIALsbuilder != null;
											if (flag117)
											{
												streamWriter2.WriteLine(exportToVRContext.MATERIALsbuilder);
											}
										}
										bool flag118 = num == 1 | num == 5 | num == 6;
										if (flag118)
										{
											bool flag119 = this.m_AllViews.MATERIALsbuilder != null;
											if (flag119)
											{
												streamWriter2.WriteLine(this.m_AllViews.MATERIALsbuilder);
											}
										}
										streamWriter2.Close();
									}
									ExportToVRForm._export_folder_name = Path.GetDirectoryName(text);
								}
								bool flag120 = Directory.Exists(Path.GetDirectoryName(text));
								if (flag120)
								{
									bool textureExist = exportToVRContext.textureExist;
									if (textureExist)
									{
										bool flag121 = exportToVRContext.key_Materials != null;
										if (flag121)
										{
											bool flag122 = exportToVRContext.key_Materials.Count > 0;
											if (flag122)
											{
												foreach (object obj6 in exportToVRContext.key_Materials)
												{
													int num6 = (int)obj6;
													string text2 = Convert.ToString(exportToVRContext.h_Materials[num6]);
													bool flag123 = File.Exists(text2);
													if (flag123)
													{
														bool flag124 = text2 != null & text2 != "";
														if (flag124)
														{
															string fileName = Path.GetFileName(text2);
															string text3 = fileName.Replace(" ", "_");
															string str = text3;
															string text4 = ExportToVRForm._export_folder_name + "\\" + str;
															bool flag125 = !File.Exists(text4);
															if (flag125)
															{
																File.Copy(text2, text4);
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
				}
				bool checked17 = this.checkBoxMaxVertices.Checked;
				if (checked17)
				{
					bool flag126 = view3D != null & !flag;
					if (flag126)
					{
						string text5 = null;
						string path2 = null;
						SaveFileDialog saveFileDialog2 = new SaveFileDialog();
						saveFileDialog2.InitialDirectory = "C:\\";
						saveFileDialog2.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
						saveFileDialog2.FilterIndex = 1;
						saveFileDialog2.RestoreDirectory = true;
						saveFileDialog2.FileName = null;
						bool flag127 = saveFileDialog2.ShowDialog() == DialogResult.OK;
						if (flag127)
						{
							try
							{
								text5 = saveFileDialog2.FileName;
							}
							catch (Exception ex3)
							{
								MessageBox.Show("Error: Could not read file. Original error: " + ex3.Message);
							}
						}
						else
						{
							bool flag128 = saveFileDialog2.ShowDialog() == DialogResult.Cancel;
							if (flag128)
							{
								flag = true;
								return;
							}
						}
						path2 = text5;
						for (int j = 1; j <= this.m_AllViews.ExportNB; j++)
						{
							this.m_AllViews.ExportOrder = j;
							string text6 = null;
							bool flag129 = j.ToString().Length == 1;
							if (flag129)
							{
								text6 = "0" + Convert.ToString(j);
							}
							bool flag130 = j.ToString().Length == 2;
							if (flag130)
							{
								text6 = Convert.ToString(j);
							}
							bool flag131 = this.m_AllViews.ExportNB > 1 & j == 1;
							if (flag131)
							{
								text5 = string.Concat(new string[]
								{
									Path.GetDirectoryName(path2),
									"\\",
									Path.GetFileNameWithoutExtension(path2),
									"_",
									text6,
									".obj"
								});
							}
							bool flag132 = j > 1;
							if (flag132)
							{
								text5 = string.Concat(new string[]
								{
									Path.GetDirectoryName(path2),
									"\\",
									Path.GetFileNameWithoutExtension(path2),
									"_",
									text6,
									".obj"
								});
							}
							ExportToVRContext exportToVRContext2 = new ExportToVRContext(document, this.m_AllViews);
							CustomExporter customExporter2 = new CustomExporter(document, exportToVRContext2);
							customExporter2.IncludeGeometricObjects = false;
							customExporter2.ShouldStopOnError = false;
							try
							{
								customExporter2.Export(view3D);
							}
							catch (ExternalApplicationException ex4)
							{
								Debug.Print("ExternalApplicationException " + ex4.Message);
							}
							bool flag133 = !flag;
							if (flag133)
							{
								bool flag134 = File.Exists(text5);
								if (flag134)
								{
									File.Delete(text5);
								}
								bool flag135 = Directory.Exists(Path.GetDirectoryName(text5));
								if (flag135)
								{
									using (StreamWriter streamWriter3 = new StreamWriter(text5, true))
									{
										bool flag136 = num == 1;
										if (flag136)
										{
											streamWriter3.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text5),
												".mtl\n",
												exportToVRContext2.XYZsbuilder,
												"\n",
												exportToVRContext2.UVsbuilder,
												"\n",
												exportToVRContext2.NORMALsbuilder,
												"\n",
												exportToVRContext2.FCTsbuilder
											}));
										}
										bool flag137 = num == 2;
										if (flag137)
										{
											streamWriter3.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text5),
												".mtl\n",
												exportToVRContext2.XYZsbuilder,
												"\n",
												exportToVRContext2.UVsbuilder,
												"\n",
												exportToVRContext2.NORMALsbuilder,
												"\n",
												exportToVRContext2.FCTbyMATsbuilder
											}));
										}
										bool flag138 = num == 3;
										if (flag138)
										{
											streamWriter3.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text5),
												".mtl\n",
												exportToVRContext2.XYZsbuilder,
												"\n",
												exportToVRContext2.UVsbuilder,
												"\n",
												exportToVRContext2.NORMALsbuilder,
												"\n",
												exportToVRContext2.FCTbyENTsbuilder
											}));
										}
										string path3 = text5;
										text5 = Path.ChangeExtension(path3, "mtl");
										streamWriter3.Close();
									}
									using (StreamWriter streamWriter4 = new StreamWriter(text5))
									{
										bool flag139 = exportToVRContext2.MATERIALsbuilder != null;
										if (flag139)
										{
											streamWriter4.WriteLine(exportToVRContext2.MATERIALsbuilder);
										}
										streamWriter4.Close();
									}
									ExportToVRForm._export_folder_name = Path.GetDirectoryName(text5);
								}
								bool textureExist2 = exportToVRContext2.textureExist;
								if (textureExist2)
								{
									bool flag140 = exportToVRContext2.key_Materials != null;
									if (flag140)
									{
										bool flag141 = exportToVRContext2.key_Materials.Count > 0;
										if (flag141)
										{
											foreach (object obj7 in exportToVRContext2.key_Materials)
											{
												int num7 = (int)obj7;
												string text7 = Convert.ToString(exportToVRContext2.h_Materials[num7]);
												bool flag142 = File.Exists(text7);
												if (flag142)
												{
													bool flag143 = text7 != null & text7 != "";
													if (flag143)
													{
														string fileName2 = Path.GetFileName(text7);
														string text8 = fileName2.Replace(" ", "_");
														string str2 = text8;
														string text9 = ExportToVRForm._export_folder_name + "\\" + str2;
														bool flag144 = !File.Exists(text9);
														if (flag144)
														{
															File.Copy(text7, text9);
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
				bool flag145 = flag5;
				if (flag145)
				{
					this.OpenVUIfClosed();
				}
				base.Close();
			}
			catch (Exception ex5)
			{
				MessageBox.Show(ex5.Message);
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00027238 File Offset: 0x00025438
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

		// Token: 0x06000135 RID: 309 RVA: 0x00027280 File Offset: 0x00025480
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

		// Token: 0x06000136 RID: 310 RVA: 0x000272CC File Offset: 0x000254CC
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

		// Token: 0x06000137 RID: 311 RVA: 0x00027338 File Offset: 0x00025538
		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			this.m_AllViews.VerticeNb = this.trackBar1.Value;
			this.labelVertices.Text = this.trackBar1.Value.ToString();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0002737C File Offset: 0x0002557C
		private void checkBoxMaxVertices_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBoxMaxVertices.Checked;
			if (@checked)
			{
				this.m_AllViews.MaxVerticesPerObj = true;
				this.trackBar1.Enabled = true;
			}
			bool flag = !this.checkBoxMaxVertices.Checked;
			if (flag)
			{
				this.m_AllViews.MaxVerticesPerObj = false;
				this.trackBar1.Enabled = false;
			}
		}

		// Token: 0x04000176 RID: 374
		private ExternalCommandData p_commandData;

		// Token: 0x04000177 RID: 375
		private AllViews m_AllViews;

		// Token: 0x04000178 RID: 376
		private const double _eps = 1E-09;

		// Token: 0x04000179 RID: 377
		private const double _feet_to_mm = 304.79999999999995;

		// Token: 0x0400017A RID: 378
		private string mm = "mm";

		// Token: 0x0400017B RID: 379
		private string ft = "ft";

		// Token: 0x0400017C RID: 380
		private string units = null;

		// Token: 0x0400017D RID: 381
		private static string _export_folder_name = null;

		// Token: 0x0400017E RID: 382
		public int TotalElementInView;

		// Token: 0x0400017F RID: 383
		private string _path;
	}
}
