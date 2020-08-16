using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using ExportToVR.Properties;

namespace ExportToVR
{
	// Token: 0x02000010 RID: 16
	public partial class ExportToVUForm : System.Windows.Forms.Form
    {
		// Token: 0x060000DA RID: 218 RVA: 0x00017C5C File Offset: 0x00015E5C
		public ExportToVUForm(ExternalCommandData commandData, AllViews families)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = families;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00017CD6 File Offset: 0x00015ED6
		private void ViewForm_Load(object sender, EventArgs e)
		{
			this.m_AllViews.VerticeNb = 0;
			this.m_AllViews.MaxVerticesPerObj = false;
			this.m_AllViews.MaxVerticesPerObj = false;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00017D00 File Offset: 0x00015F00
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

		// Token: 0x060000DD RID: 221 RVA: 0x00017D74 File Offset: 0x00015F74
		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.79999999999995;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00017D94 File Offset: 0x00015F94
		private string ProjectUnits(string s)
		{
			Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
			Document document = this.p_commandData.Application.ActiveUIDocument.Document;
			FormatOptions formatOptions = document.GetUnits().GetFormatOptions(0);
			bool flag = formatOptions.DisplayUnits == DisplayUnitType.DUT_MILLIMETERS ;
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

		// Token: 0x060000DF RID: 223 RVA: 0x00017E04 File Offset: 0x00016004
		private static double DegreeToRadian(double angle)
		{
			return 3.1415926535897931 * angle / 180.0;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00017E2C File Offset: 0x0001602C
		private static double RadianToDegree(double angle)
		{
			return angle * 57.295779513082323;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00017E4C File Offset: 0x0001604C
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
				this.m_AllViews.StandAloneVersion = true;
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
				FilteredElementCollector filteredElementCollector6 = new FilteredElementCollector(document);
				ICollection<Element> collection6 = filteredElementCollector6.OfClass(typeof(FamilyInstance)).ToElements();
				string text = null;
				string text2 = null;
				List<string> list = new List<string>();
				IList<Schema> list2 = Schema.ListSchemas();
				bool flag5 = list2 != null;
				if (flag5)
				{
					foreach (Schema schema in list2)
					{
						bool flag6 = schema.SchemaName.ToString() == "FlipbookFrames";
						if (flag6)
						{
							Guid guid = new Guid(schema.GUID.ToString());
							Schema schema2 = Schema.Lookup(guid);
							IList<Field> list3 = schema2.ListFields();
							List<ElementId> list4 = new List<ElementId>();
							FilteredElementCollector filteredElementCollector7 = new FilteredElementCollector(document);
							filteredElementCollector7.WherePasses(new ExtensibleStorageFilter(schema2.GUID));
							list4.AddRange(filteredElementCollector7.ToElementIds());
							StringBuilder stringBuilder = new StringBuilder();
							StringBuilder stringBuilder2 = new StringBuilder();
							foreach (ElementId elementId in list4)
							{
								bool flag7 = elementId != null;
								if (flag7)
								{
									Element element = document.GetElement(elementId);
									bool flag8 = element.Category.Id.IntegerValue == -2003101;
									if (flag8)
									{
										Entity entity = element.GetEntity(schema2);
										string str = "Fields in the Schema:\n";
										str += "StringParamMap:\n";
										IDictionary<string, string> dictionary = entity.Get<IDictionary<string, string>>("FieldFlipFrames");
										foreach (KeyValuePair<string, string> keyValuePair in dictionary)
										{
											bool flag9 = !keyValuePair.Key.Contains("_p_") && !keyValuePair.Value.Contains("_c_");
											if (flag9)
											{
												string[] array = keyValuePair.Key.Split(new char[]
												{
													'_'
												});
												string item = array[1].ToString();
												bool flag10 = !list.Contains(item);
												if (flag10)
												{
													list.Add(item);
												}
												text = string.Concat(new string[]
												{
													text,
													keyValuePair.Key.ToString(),
													"_",
													keyValuePair.Value.ToString(),
													"\n"
												});
											}
										}
										foreach (string text3 in list)
										{
											string value = text3;
											int num2 = Convert.ToInt32(value);
											ElementId elementId2 = new ElementId(num2);
											Element element2 = document.GetElement(elementId2);
											FamilyInstance familyInstance = element2 as FamilyInstance;
											Location location = familyInstance.Location;
											LocationPoint locationPoint = location as LocationPoint;
											XYZ point = locationPoint.Point;
											double angle = 270.0;
											double num3 = ExportToVUForm.DegreeToRadian(angle);
											Transform transform = Transform.CreateRotation(XYZ.BasisX, num3);
											XYZ xyz = new XYZ(point.X, point.Y, point.Z);
											xyz = transform.OfPoint(xyz);
											XYZ xyz2 = new XYZ(0.0, 0.0, 0.0);
											XYZ xyz3 = new XYZ(xyz2.X, xyz2.Y + 10.0, xyz2.Z);
											Line line = Line.CreateBound(xyz2, xyz3);
											Plane plane = Plane.CreateByNormalAndOrigin(XYZ.BasisX, XYZ.Zero);
											Transform transform2 = Transform.CreateReflection(plane);
											xyz = transform2.OfPoint(xyz);
											string text4 = xyz.X.ToString();
											string text5 = text4.Replace(",", ".");
											string text6 = xyz.Y.ToString();
											string text7 = text6.Replace(",", ".");
											string text8 = xyz.Z.ToString();
											string text9 = text8.Replace(",", ".");
											text2 = string.Concat(new string[]
											{
												text2,
												"Location_",
												text3,
												"_",
												text4,
												"_",
												text6,
												"_",
												text8,
												"\n"
											});
											int num4 = 0;
											int num5 = 0;
											foreach (KeyValuePair<string, string> keyValuePair2 in dictionary)
											{
												bool flag11 = !keyValuePair2.Key.Contains("_p_") && keyValuePair2.Key.Contains(text3) && !keyValuePair2.Value.Contains("_c_");
												if (flag11)
												{
													num4++;
												}
											}
											Hashtable hashtable = new Hashtable();
											ICollection collection7 = null;
											Hashtable hashtable2 = new Hashtable();
											Hashtable hashtable3 = new Hashtable();
											Hashtable hashtable4 = new Hashtable();
											Hashtable hashtable5 = new Hashtable();
											foreach (KeyValuePair<string, string> keyValuePair3 in dictionary)
											{
												bool flag12 = !keyValuePair3.Key.Contains("_p_") && keyValuePair3.Key.Contains(text3) && !keyValuePair3.Value.Contains("_c_");
												if (flag12)
												{
													string[] array2 = keyValuePair3.Key.Split(new char[]
													{
														'_'
													});
													string b = array2[1].ToString();
													string value2 = array2[2].ToString();
													string text10 = array2[3].ToString();
													string[] array3 = keyValuePair3.Value.Split(new char[]
													{
														'_'
													});
													string value3 = array3[0].ToString();
													string value4 = array3[1].ToString();
													string value5 = array3[2].ToString();
													string value6 = array3[3].ToString();
													double num6 = Convert.ToDouble(value3);
													double num7 = Convert.ToDouble(value4);
													double num8 = Convert.ToDouble(value5);
													foreach (Element element3 in collection6)
													{
														FamilyInstance familyInstance2 = element3 as FamilyInstance;
														bool flag13 = familyInstance2.Id.IntegerValue.ToString() == b;
														if (flag13)
														{
															Transform transform3 = familyInstance2.GetTransform();
														}
													}
													double angle2 = 270.0;
													double num9 = ExportToVUForm.DegreeToRadian(angle2);
													Transform transform4 = Transform.CreateRotation(XYZ.BasisX, num9);
													XYZ xyz4 = new XYZ(num6, num7, num8);
													xyz4 = transform4.OfPoint(xyz4);
													XYZ xyz5 = new XYZ(0.0, 0.0, 0.0);
													XYZ xyz6 = new XYZ(xyz5.X, xyz5.Y + 10.0, xyz5.Z);
													Line line2 = Line.CreateBound(xyz5, xyz6);
													Plane plane2 = Plane.CreateByNormalAndOrigin(XYZ.BasisX, XYZ.Zero);
													Transform transform5 = Transform.CreateReflection(plane2);
													xyz4 = transform5.OfPoint(xyz4);
													double num10 = 15.0;
													string text11 = (Convert.ToDouble(value2) / num10).ToString();
													string value7 = text11.Replace(",", ".");
													string text12 = xyz4.X.ToString();
													string text13 = text12.Replace(",", ".");
													string text14 = xyz4.Y.ToString();
													string text15 = text14.Replace(",", ".");
													string text16 = xyz4.Z.ToString();
													string text17 = text16.Replace(",", ".");
													double num11 = ExportToVUForm.RadianToDegree(Convert.ToDouble(value6));
													hashtable.Add(num5, value7);
													collection7 = hashtable.Keys;
													hashtable2.Add(num5, xyz4.X);
													hashtable3.Add(num5, xyz4.Y);
													hashtable4.Add(num5, xyz4.Z);
													hashtable5.Add(num5, num11);
													num5++;
												}
											}
											double num12 = 0.0;
											double num13 = 0.0;
											double num14 = 0.0;
											double num15 = 0.0;
											for (int i = 0; i < collection7.Count; i++)
											{
												bool flag14 = i < collection7.Count;
												if (flag14)
												{
													bool flag15 = i == 0;
													if (flag15)
													{
														string text18 = hashtable[i].ToString();
														string text19 = "0";
														string text20 = "0";
														string text21 = "0";
														string text22 = "0";
														string text23 = text18.ToString();
														string text24 = text23.Replace(".", ",");
														text2 = string.Concat(new string[]
														{
															text2,
															"Unity_",
															text3,
															"_",
															text24,
															"_0_",
															text19,
															"_",
															text20,
															"_",
															text21,
															"_",
															text22,
															"\n"
														});
													}
													bool flag16 = i == 1;
													if (flag16)
													{
														string text25 = hashtable[i].ToString();
														string value8 = hashtable2[i - 1].ToString();
														string value9 = hashtable2[i].ToString();
														string value10 = hashtable3[i - 1].ToString();
														string value11 = hashtable3[i].ToString();
														string value12 = hashtable4[i - 1].ToString();
														string value13 = hashtable4[i].ToString();
														string value14 = hashtable5[i - 1].ToString();
														string value15 = hashtable5[i].ToString();
														double num16 = Convert.ToDouble(value9) - Convert.ToDouble(value8);
														double num17 = Convert.ToDouble(value11) - Convert.ToDouble(value10);
														double num18 = Convert.ToDouble(value13) - Convert.ToDouble(value12);
														double num19 = Convert.ToDouble(value15) - Convert.ToDouble(value14);
														double num20 = num19;
														bool flag17 = num19 > 180.0 & num19 < 360.0;
														if (flag17)
														{
															num20 = -(360.0 - num19);
														}
														bool flag18 = num19 < 0.0 & num19 > -180.0;
														if (flag18)
														{
															num20 = -(180.0 + num19);
														}
														bool flag19 = num19 <= -180.0 & num19 > -360.0;
														if (flag19)
														{
															num20 = -(360.0 + num19);
														}
														num12 = num16;
														num13 = num17;
														num14 = num18;
														num15 = num20;
														string text26 = num16.ToString();
														string text27 = text26.Replace(",", ".");
														string text28 = num17.ToString();
														string text29 = text28.Replace(",", ".");
														string text30 = num18.ToString();
														string text31 = text30.Replace(",", ".");
														string text32 = num20.ToString();
														string text33 = text32;
														bool flag20 = text32.Contains("-");
														if (flag20)
														{
															text33 = text32.Replace("-", "");
														}
														bool flag21 = !text32.Contains("-");
														if (flag21)
														{
															text33 = "-" + text32;
														}
														text32 = text33;
														string text34 = text25.ToString();
														string text35 = text34.Replace(".", ",");
														text2 = string.Concat(new string[]
														{
															text2,
															"Unity_",
															text3,
															"_",
															text35,
															"_0_",
															text26,
															"_",
															text28,
															"_",
															text30,
															"_",
															text32,
															"\n"
														});
													}
													bool flag22 = i > 1;
													if (flag22)
													{
														string text36 = hashtable[i].ToString();
														string value16 = hashtable2[i - 1].ToString();
														string value17 = hashtable2[i].ToString();
														string value18 = hashtable3[i - 1].ToString();
														string value19 = hashtable3[i].ToString();
														string value20 = hashtable4[i - 1].ToString();
														string value21 = hashtable4[i].ToString();
														string value22 = hashtable5[i - 1].ToString();
														string value23 = hashtable5[i].ToString();
														double num21 = num12 + (Convert.ToDouble(value17) - Convert.ToDouble(value16));
														double num22 = num13 + (Convert.ToDouble(value19) - Convert.ToDouble(value18));
														double num23 = num14 + (Convert.ToDouble(value21) - Convert.ToDouble(value20));
														double num24 = num15 + Convert.ToDouble(value23) - Convert.ToDouble(value22);
														double num25 = num24;
														bool flag23 = num24 > 180.0 & num24 < 360.0;
														if (flag23)
														{
															num25 = -(360.0 - num24);
														}
														bool flag24 = num24 < 0.0 & num24 > -180.0;
														if (flag24)
														{
															num25 = -(180.0 + num24);
														}
														bool flag25 = num24 <= -180.0 & num24 > -360.0;
														if (flag25)
														{
															num25 = -(360.0 + num24);
														}
														num12 = num21;
														num13 = num22;
														num14 = num23;
														num15 = num25;
														string text37 = num21.ToString();
														string text38 = text37.Replace(",", ".");
														string text39 = num22.ToString();
														string text40 = text39.Replace(",", ".");
														string text41 = num23.ToString();
														string text42 = text41.Replace(",", ".");
														string text43 = num25.ToString();
														string text44 = text43;
														bool flag26 = text43.Contains("-");
														if (flag26)
														{
															text44 = text43.Replace("-", "");
														}
														bool flag27 = !text43.Contains("-");
														if (flag27)
														{
															text44 = "-" + text43;
														}
														text43 = text44;
														string text45 = text36.ToString();
														string text46 = text45.Replace(".", ",");
														text2 = string.Concat(new string[]
														{
															text2,
															"Unity_",
															text3,
															"_",
															text46,
															"_0_",
															text37,
															"_",
															text39,
															"_",
															text41,
															"_",
															text43,
															"\n"
														});
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
				string path = this.pathStart + "\\FlipbookSchema.txt";
				bool flag28 = File.Exists(path);
				if (flag28)
				{
					File.Delete(path);
				}
				using (StreamWriter streamWriter = new StreamWriter(path, true))
				{
					streamWriter.WriteLine(text2);
				}
				bool flag29 = false;
				bool checked4 = this.checkBoxStartVU.Checked;
				if (checked4)
				{
					flag29 = true;
					this.m_AllViews.StartVUoption = true;
				}
				Transaction transaction = new Transaction(document);
				transaction.Start("HideAnnotations");
				foreach (object obj in document.Settings.Categories)
				{
					Category category = (Category)obj;
					bool flag30 = category.get_AllowsVisibilityControl(view3D);
					if (flag30)
					{
						bool flag31 = category.CategoryType != CategoryType.Model;
						if (flag31)
						{
							view3D.SetCategoryHidden(category.Id, true);
						}
					}
				}
				transaction.Commit();
				int num26 = 20000000;
				int num27 = num26 * 2;
				bool flag32 = !this.checkBoxTrialVersion.Checked;
				if (flag32)
				{
					bool flag33 = !flag;
					if (flag33)
					{
						bool flag34 = view3D != null;
						if (flag34)
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
						}
					}
				}
				bool flag35 = flag;
				if (flag35)
				{
					base.Close();
				}
				bool flag36 = application.VersionNumber.Contains("2019") | application.VersionNumber.Contains("2020");
				if (flag36)
				{
					bool flag37 = !this.checkBoxTrialVersion.Checked;
					if (flag37)
					{
						bool flag38 = view3D != null & !flag;
						if (flag38)
						{
							string text47 = null;
							text47 = this.pathStart + this.pathEnd;
							ExportToVRContext exportToVRContext = new ExportToVRContext(document, this.m_AllViews);
							CustomExporter customExporter = new CustomExporter(document, exportToVRContext);
							customExporter.IncludeGeometricObjects = false;
							customExporter.ShouldStopOnError = false;
							try
							{
								this.m_AllViews.ExportSubCategories = false;
								bool flag39 = num == 2 | num == 3;
								if (flag39)
								{
									customExporter.Export(view3D);
								}
								bool flag40 = num == 4;
								if (flag40)
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
											bool flag41 = category6.Id.IntegerValue == -2000025;
											if (flag41)
											{
												category3 = category6;
											}
											bool flag42 = category6.Id.IntegerValue == -2000031;
											if (flag42)
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
										bool flag43 = category7.get_AllowsVisibilityControl(view3D);
										if (flag43)
										{
											bool flag44 = category7.Id.IntegerValue != -2000023;
											if (flag44)
											{
												view3D.SetCategoryHidden(category7.Id, true);
											}
										}
										foreach (object obj5 in category7.SubCategories)
										{
											Category category8 = (Category)obj5;
											bool flag45 = category8.get_AllowsVisibilityControl(view3D);
											if (flag45)
											{
												bool flag46 = category8.Id.IntegerValue != -2000025 && category8.Id.IntegerValue != -2000031;
												if (flag46)
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
								bool flag47 = num == 1 | num == 5 | num == 6;
								if (flag47)
								{
									int num28 = 1000;
									FilteredElementCollector filteredElementCollector8 = new FilteredElementCollector(document, view3D.Id);
									ICollection<ElementId> collection8 = filteredElementCollector8.ToElementIds();
									ICollection<ElementId> collection9 = filteredElementCollector8.ToElementIds();
									collection9.Clear();
									List<int> list5 = new List<int>();
									List<int> list6 = new List<int>();
									List<int> list7 = new List<int>();
									foreach (string value24 in list)
									{
										int num29 = Convert.ToInt32(value24);
										ElementId elementId3 = new ElementId(num29);
										Element element4 = document.GetElement(elementId3);
										bool flag48 = element4.Id.IntegerValue != -1;
										if (flag48)
										{
											bool flag49 = element4.CanBeHidden(view3D);
											if (flag49)
											{
												collection9.Add(elementId3);
											}
										}
									}
									bool flag50 = false;
									foreach (ElementId elementId4 in collection8)
									{
										bool flag51 = false;
										Element element5 = document.GetElement(elementId4);
										bool flag52 = element5 != null;
										if (flag52)
										{
											bool flag53 = element5.Category != null;
											if (flag53)
											{
												bool flag54 = element5.Category.CategoryType == CategoryType.Model;
												if (flag54)
												{
													flag51 = true;
												}
												bool flag55 = element5.Category.Id.IntegerValue == -2001340;
												if (flag55)
												{
													flag50 = true;
												}
												bool flag56 = element5.Category.Id.IntegerValue == -2001352;
												if (flag56)
												{
													flag50 = true;
												}
											}
											bool flag57 = element5.GetTypeId() != null;
											if (flag57)
											{
												int integerValue = element5.GetTypeId().IntegerValue;
												bool flag58 = flag51 & !flag50;
												if (flag58)
												{
													GeometryElement geometryElement = element5.get_Geometry(new Options
													{
														ComputeReferences = true
													});
													bool flag59 = geometryElement != null;
													if (flag59)
													{
														foreach (GeometryObject geometryObject in geometryElement)
														{
															bool flag60 = geometryObject is Solid;
															if (flag60)
															{
																Solid solid = geometryObject as Solid;
																bool flag61 = null != solid;
																if (flag61)
																{
																	bool flag62 = solid.Faces.Size > 0;
																	if (flag62)
																	{
																		flag50 = true;
																		break;
																	}
																}
															}
															GeometryInstance geometryInstance = geometryObject as GeometryInstance;
															bool flag63 = null != geometryInstance;
															if (flag63)
															{
																foreach (GeometryObject geometryObject2 in geometryInstance.SymbolGeometry)
																{
																	Solid solid2 = geometryObject2 as Solid;
																	bool flag64 = null != solid2;
																	if (flag64)
																	{
																		bool flag65 = solid2.Faces.Size > 0;
																		if (flag65)
																		{
																			flag50 = true;
																			break;
																		}
																	}
																}
															}
														}
													}
												}
												bool flag66 = !list5.Contains(integerValue) && flag50;
												if (flag66)
												{
													list5.Add(integerValue);
												}
											}
										}
										flag50 = false;
									}
									for (int j = 0; j < list5.Count; j++)
									{
										int item2 = list5[j];
										int num30 = 0;
										bool flag67 = num30 <= num28;
										if (flag67)
										{
											list7.Add(item2);
										}
										bool flag68 = num30 > num28;
										if (flag68)
										{
											list6.Add(item2);
										}
									}
									bool flag69 = list7.Count > 0;
									if (flag69)
									{
										bool flag70 = false;
										foreach (ElementId elementId5 in collection8)
										{
											Element element6 = document.GetElement(elementId5);
											bool flag71 = element6 != null;
											if (flag71)
											{
												int integerValue2 = element6.GetTypeId().IntegerValue;
												bool flag72 = !list7.Contains(integerValue2);
												if (flag72)
												{
													bool flag73 = element6.Category != null;
													if (flag73)
													{
														bool flag74 = element6.Category.Id.IntegerValue == -2001340;
														if (flag74)
														{
															flag70 = true;
														}
													}
													bool flag75 = !flag50;
													if (flag75)
													{
														GeometryElement geometryElement2 = element6.get_Geometry(new Options
														{
															ComputeReferences = true
														});
														bool flag76 = geometryElement2 != null;
														if (flag76)
														{
															foreach (GeometryObject geometryObject3 in geometryElement2)
															{
																bool flag77 = geometryObject3 is Solid;
																if (flag77)
																{
																	Solid solid3 = geometryObject3 as Solid;
																	bool flag78 = null != solid3;
																	if (flag78)
																	{
																		bool flag79 = solid3.Faces.Size > 0;
																		if (flag79)
																		{
																			flag70 = true;
																			break;
																		}
																	}
																}
																GeometryInstance geometryInstance2 = geometryObject3 as GeometryInstance;
																bool flag80 = null != geometryInstance2;
																if (flag80)
																{
																	foreach (GeometryObject geometryObject4 in geometryInstance2.SymbolGeometry)
																	{
																		Solid solid4 = geometryObject4 as Solid;
																		bool flag81 = null != solid4;
																		if (flag81)
																		{
																			bool flag82 = solid4.Faces.Size > 0;
																			if (flag82)
																			{
																				flag70 = true;
																				break;
																			}
																		}
																	}
																}
															}
														}
													}
													bool flag83 = flag70;
													if (flag83)
													{
														bool flag84 = element6.CanBeHidden(view3D);
														if (flag84)
														{
															collection9.Add(elementId5);
														}
													}
												}
											}
											flag70 = false;
										}
										Transaction transaction4 = new Transaction(document);
										transaction4.Start("TempHideType");
										bool flag85 = collection9.Count > 0;
										if (flag85)
										{
											view3D.HideElements(collection9);
										}
										transaction4.Commit();
										customExporter.Export(view3D);
										Transaction transaction5 = new Transaction(document);
										transaction5.Start("TempUnhideType");
										bool flag86 = collection9.Count > 0;
										if (flag86)
										{
											view3D.UnhideElements(collection9);
										}
										transaction5.Commit();
										collection9.Clear();
									}
									bool flag87 = list6.Count > 0;
									if (flag87)
									{
										foreach (int num31 in list6)
										{
											bool flag88 = false;
											bool flag89 = num31 != -1;
											if (flag89)
											{
												foreach (ElementId elementId6 in collection8)
												{
													Element element7 = document.GetElement(elementId6);
													bool flag90 = element7 != null;
													if (flag90)
													{
														int integerValue3 = element7.GetTypeId().IntegerValue;
														bool flag91 = num31 != integerValue3;
														if (flag91)
														{
															bool flag92 = element7.Category != null;
															if (flag92)
															{
																bool flag93 = element7.Category.Id.IntegerValue == -2001340;
																if (flag93)
																{
																	flag88 = true;
																}
															}
															bool flag94 = !flag88;
															if (flag94)
															{
																GeometryElement geometryElement3 = element7.get_Geometry(new Options
																{
																	ComputeReferences = true
																});
																bool flag95 = geometryElement3 != null;
																if (flag95)
																{
																	foreach (GeometryObject geometryObject5 in geometryElement3)
																	{
																		bool flag96 = geometryObject5 is Solid;
																		if (flag96)
																		{
																			Solid solid5 = geometryObject5 as Solid;
																			bool flag97 = null != solid5;
																			if (flag97)
																			{
																				bool flag98 = solid5.Faces.Size > 0;
																				if (flag98)
																				{
																					flag88 = true;
																					break;
																				}
																			}
																		}
																		GeometryInstance geometryInstance3 = geometryObject5 as GeometryInstance;
																		bool flag99 = null != geometryInstance3;
																		if (flag99)
																		{
																			foreach (GeometryObject geometryObject6 in geometryInstance3.SymbolGeometry)
																			{
																				Solid solid6 = geometryObject6 as Solid;
																				bool flag100 = null != solid6;
																				if (flag100)
																				{
																					bool flag101 = solid6.Faces.Size > 0;
																					if (flag101)
																					{
																						flag88 = true;
																						break;
																					}
																				}
																			}
																		}
																	}
																}
															}
															bool flag102 = flag88;
															if (flag102)
															{
																bool flag103 = element7.CanBeHidden(view3D);
																if (flag103)
																{
																	collection9.Add(elementId6);
																}
															}
														}
													}
													flag88 = false;
												}
												Transaction transaction6 = new Transaction(document);
												transaction6.Start("TempHideType");
												bool flag104 = collection9.Count > 0;
												if (flag104)
												{
													view3D.HideElements(collection9);
												}
												transaction6.Commit();
												customExporter.Export(view3D);
												Transaction transaction7 = new Transaction(document);
												transaction7.Start("TempUnhideType");
												bool flag105 = collection9.Count > 0;
												if (flag105)
												{
													view3D.UnhideElements(collection9);
												}
												transaction7.Commit();
												collection9.Clear();
											}
										}
									}
								}
							}
							catch (ExternalApplicationException ex)
							{
								Debug.Print("ExternalApplicationException " + ex.Message);
							}
							bool flag106 = !flag;
							if (flag106)
							{
								bool checked8 = this.checkBoxDeleteOBJ.Checked;
								if (checked8)
								{
									DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(text47));
									bool flag107 = Directory.Exists(Path.GetDirectoryName(text47));
									if (flag107)
									{
										FileInfo[] files = directoryInfo.GetFiles("*.obj");
										foreach (FileInfo fileInfo in files)
										{
											fileInfo.Delete();
										}
									}
								}
								bool flag108 = File.Exists(text47);
								if (flag108)
								{
									File.Delete(text47);
								}
								StringBuilder stringBuilder3 = new StringBuilder();
								StringBuilder stringBuilder4 = new StringBuilder();
								bool checked9 = this.checkBoxOverride.Checked;
								if (checked9)
								{
									stringBuilder3.Append(this.m_AllViews.FCTbySUBCATsbuilder.ToString());
									using (StringReader stringReader = new StringReader(stringBuilder3.ToString()))
									{
										for (string text48 = stringReader.ReadLine(); text48 != null; text48 = stringReader.ReadLine())
										{
											bool flag109 = text48.Contains("usemtl") & !text48.Contains("Glass");
											if (flag109)
											{
												stringBuilder3.Replace(text48, "usemtl VuGrey");
											}
										}
									}
									stringBuilder4.Append(this.m_AllViews.MATERIALsbuilder.ToString());
									stringBuilder4.Append("newmtl VuGrey\n");
									stringBuilder4.Append("Ka 0.7 0.7 0.7\n");
									stringBuilder4.Append("Kd 0.7 0.7 0.7\n");
									stringBuilder4.Append("d 1\n");
								}
								bool flag110 = Directory.Exists(Path.GetDirectoryName(text47));
								if (flag110)
								{
									using (StreamWriter streamWriter2 = new StreamWriter(text47, true))
									{
										bool flag111 = num == 1;
										if (flag111)
										{
											streamWriter2.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text47),
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
										bool flag112 = num == 2;
										if (flag112)
										{
											streamWriter2.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text47),
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
										bool flag113 = num == 6;
										if (flag113)
										{
											streamWriter2.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text47),
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
										bool flag114 = num == 3;
										if (flag114)
										{
											streamWriter2.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text47),
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
										bool flag115 = num == 4;
										if (flag115)
										{
											streamWriter2.WriteLine(string.Concat(new object[]
											{
												"mtllib ",
												Path.GetFileNameWithoutExtension(text47),
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
										bool flag116 = num == 5;
										if (flag116)
										{
											bool checked10 = this.checkBoxOverride.Checked;
											if (checked10)
											{
												streamWriter2.WriteLine(string.Concat(new object[]
												{
													"mtllib ",
													Path.GetFileNameWithoutExtension(text47),
													".mtl\n",
													this.m_AllViews.XYZsbuilder,
													"\n",
													this.m_AllViews.UVsbuilder,
													"\n",
													this.m_AllViews.NORMALsbuilder,
													"\n",
													stringBuilder3
												}));
											}
											bool flag117 = !this.checkBoxOverride.Checked;
											if (flag117)
											{
												streamWriter2.WriteLine(string.Concat(new object[]
												{
													"mtllib ",
													Path.GetFileNameWithoutExtension(text47),
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
										}
										string path2 = text47;
										text47 = Path.ChangeExtension(path2, "mtl");
										streamWriter2.Close();
									}
									using (StreamWriter streamWriter3 = new StreamWriter(text47))
									{
										bool flag118 = num == 2 | num == 3 | num == 4;
										if (flag118)
										{
											bool flag119 = exportToVRContext.MATERIALsbuilder != null;
											if (flag119)
											{
												streamWriter3.WriteLine(exportToVRContext.MATERIALsbuilder);
											}
										}
										bool flag120 = num == 1 | num == 5 | num == 6;
										if (flag120)
										{
											bool flag121 = this.m_AllViews.MATERIALsbuilder != null;
											if (flag121)
											{
												bool flag122 = !this.checkBoxOverride.Checked;
												if (flag122)
												{
													streamWriter3.WriteLine(this.m_AllViews.MATERIALsbuilder);
												}
												bool checked11 = this.checkBoxOverride.Checked;
												if (checked11)
												{
													streamWriter3.WriteLine(stringBuilder4);
												}
											}
										}
										streamWriter3.Close();
									}
									ExportToVUForm._export_folder_name = Path.GetDirectoryName(text47);
								}
								bool flag123 = Directory.Exists(Path.GetDirectoryName(text47));
								if (flag123)
								{
									bool textureExist = exportToVRContext.textureExist;
									if (textureExist)
									{
										bool flag124 = exportToVRContext.key_Materials != null;
										if (flag124)
										{
											bool flag125 = exportToVRContext.key_Materials.Count > 0;
											if (flag125)
											{
												foreach (object obj6 in exportToVRContext.key_Materials)
												{
													int num32 = (int)obj6;
													string text49 = Convert.ToString(exportToVRContext.h_Materials[num32]);
													bool flag126 = File.Exists(text49);
													if (flag126)
													{
														bool flag127 = text49 != null & text49 != "";
														if (flag127)
														{
															string fileName = Path.GetFileName(text49);
															string text50 = fileName.Replace(" ", "_");
															string str2 = text50;
															string text51 = ExportToVUForm._export_folder_name + "\\" + str2;
															bool flag128 = !File.Exists(text51);
															if (flag128)
															{
																File.Copy(text49, text51);
															}
														}
													}
												}
											}
										}
									}
								}
							}
							bool checked12 = this.checkBoxStartVU.Checked;
							if (checked12)
							{
								AllViews allViews = new AllViews();
								allViews.GroupingOptions = 0;
								bool checked13 = this.checkBoxStartVU.Checked;
								if (checked13)
								{
									num = 5;
									allViews.GroupingOptions = 5;
								}
								ExportToVRContext exportToVRContext2 = new ExportToVRContext(document, allViews);
								CustomExporter customExporter2 = new CustomExporter(document, exportToVRContext2);
								customExporter2.IncludeGeometricObjects = false;
								customExporter2.ShouldStopOnError = false;
								try
								{
									allViews.ExportSubCategories = false;
									bool flag129 = num == 1 | num == 5 | num == 6;
									if (flag129)
									{
										FilteredElementCollector filteredElementCollector9 = new FilteredElementCollector(document, view3D.Id);
										ICollection<ElementId> collection10 = filteredElementCollector9.ToElementIds();
										ICollection<ElementId> collection11 = filteredElementCollector9.ToElementIds();
										collection11.Clear();
										List<int> list8 = new List<int>();
										List<int> list9 = new List<int>();
										List<int> list10 = new List<int>();
										bool flag130 = list.Count != 0;
										if (flag130)
										{
											foreach (ElementId elementId7 in collection10)
											{
												Element element8 = document.GetElement(elementId7);
												bool flag131 = element8.Id.IntegerValue != -1;
												if (flag131)
												{
													bool flag132 = !list.Contains(elementId7.IntegerValue.ToString());
													if (flag132)
													{
														bool flag133 = element8.CanBeHidden(view3D);
														if (flag133)
														{
															collection11.Add(element8.Id);
														}
													}
												}
											}
											Transaction transaction8 = new Transaction(document);
											transaction8.Start("TempHideType");
											bool flag134 = collection11.Count > 0;
											if (flag134)
											{
												view3D.HideElements(collection11);
											}
											transaction8.Commit();
											customExporter2.Export(view3D);
											Transaction transaction9 = new Transaction(document);
											transaction9.Start("TempUnhideType");
											bool flag135 = collection11.Count > 0;
											if (flag135)
											{
												view3D.UnhideElements(collection11);
											}
											transaction9.Commit();
											collection11.Clear();
										}
									}
								}
								catch (ExternalApplicationException ex2)
								{
									Debug.Print("ExternalApplicationException " + ex2.Message);
								}
								bool flag136 = !flag;
								if (flag136)
								{
									StringBuilder stringBuilder5 = new StringBuilder();
									StringBuilder stringBuilder6 = new StringBuilder();
									string text52 = this.pathStart + this.pathEndGrey;
									bool flag137 = File.Exists(text52);
									if (flag137)
									{
										File.Delete(text52);
									}
									bool flag138 = num == 5;
									if (flag138)
									{
										bool flag139 = Directory.Exists(Path.GetDirectoryName(text52));
										if (flag139)
										{
											using (StreamWriter streamWriter4 = new StreamWriter(text52, true))
											{
												streamWriter4.WriteLine(string.Concat(new object[]
												{
													"mtllib ",
													Path.GetFileNameWithoutExtension(text52),
													".mtl\n",
													allViews.XYZsbuilder,
													"\n",
													allViews.UVsbuilder,
													"\n",
													allViews.NORMALsbuilder,
													"\n",
													allViews.FCTbySUBCATsbuilder
												}));
												string path3 = text52;
												text52 = Path.ChangeExtension(path3, "mtl");
												streamWriter4.Close();
											}
											using (StreamWriter streamWriter5 = new StreamWriter(text52))
											{
												bool flag140 = num == 1 | num == 5 | num == 6;
												if (flag140)
												{
													bool flag141 = allViews.MATERIALsbuilder != null;
													if (flag141)
													{
														streamWriter5.WriteLine(allViews.MATERIALsbuilder);
													}
												}
												streamWriter5.Close();
											}
											ExportToVUForm._export_folder_name = Path.GetDirectoryName(text52);
											bool flag142 = Directory.Exists(Path.GetDirectoryName(text52));
											if (flag142)
											{
												bool textureExist2 = exportToVRContext.textureExist;
												if (textureExist2)
												{
													bool flag143 = exportToVRContext.key_Materials != null;
													if (flag143)
													{
														bool flag144 = exportToVRContext.key_Materials.Count > 0;
														if (flag144)
														{
															foreach (object obj7 in exportToVRContext.key_Materials)
															{
																int num33 = (int)obj7;
																string text53 = Convert.ToString(exportToVRContext.h_Materials[num33]);
																bool flag145 = File.Exists(text53);
																if (flag145)
																{
																	bool flag146 = text53 != null & text53 != "";
																	if (flag146)
																	{
																		string fileName2 = Path.GetFileName(text53);
																		string text54 = fileName2.Replace(" ", "_");
																		string str3 = text54;
																		string text55 = ExportToVUForm._export_folder_name + "\\" + str3;
																		bool flag147 = !File.Exists(text55);
																		if (flag147)
																		{
																			File.Copy(text53, text55);
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
						}
					}
				}
				bool flag148 = flag29;
				if (flag148)
				{
					this.OpenVUIfClosed();
				}
				base.Close();
			}
			catch (Exception ex3)
			{
				MessageBox.Show(ex3.Message);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0001B9A4 File Offset: 0x00019BA4
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

		// Token: 0x060000E3 RID: 227 RVA: 0x0001B9EC File Offset: 0x00019BEC
		private void OpenVUIfClosed()
		{
			bool flag = File.Exists(this.VUPath);
			if (flag)
			{
				Process process = new Process();
				process.StartInfo.FileName = this.VUPath;
				bool flag2 = !this.IsProcessOpen(this.VUPath);
				if (flag2)
				{
					process.Start();
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0001BA40 File Offset: 0x00019C40
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

		// Token: 0x060000E5 RID: 229 RVA: 0x0001BAAC File Offset: 0x00019CAC
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

		// Token: 0x040000F6 RID: 246
		private ExternalCommandData p_commandData;

		// Token: 0x040000F7 RID: 247
		private AllViews m_AllViews;

		// Token: 0x040000F8 RID: 248
		private const double _eps = 1E-09;

		// Token: 0x040000F9 RID: 249
		private const double _feet_to_mm = 304.79999999999995;

		// Token: 0x040000FA RID: 250
		private string mm = "mm";

		// Token: 0x040000FB RID: 251
		private string ft = "ft";

		// Token: 0x040000FC RID: 252
		private string units = null;

		// Token: 0x040000FD RID: 253
		private static string _export_folder_name = null;

		// Token: 0x040000FE RID: 254
		public int TotalElementInView;

		// Token: 0x040000FF RID: 255
		private string _path;

		// Token: 0x04000100 RID: 256
		private string pathStart = "C:\\MSDLBIM\\VU\\3D\\";

		// Token: 0x04000101 RID: 257
		private string pathEnd = "a.obj";

		// Token: 0x04000102 RID: 258
		private string pathEndGrey = "b.obj";

		// Token: 0x04000103 RID: 259
		private string VUPath = "C:\\MSDLBIM\\VU\\GO\\GO.exe";

		// Token: 0x04000104 RID: 260
		public static string ProjectInfos = "Informations sur le projet";

		// Token: 0x04000105 RID: 261
		public static int ProjInfoID = 49504;
	}
}
