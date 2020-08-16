using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using ExportToVR.Properties;
using RestSharp;

namespace ExportToVR
{
	// Token: 0x0200000E RID: 14
	public partial class ExportToVUxyzForm : System.Windows.Forms.Form
    {
		// Token: 0x060000BE RID: 190 RVA: 0x0000E634 File Offset: 0x0000C834
		public ExportToVUxyzForm(ExternalCommandData commandData, AllViews families)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = families;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000E6CF File Offset: 0x0000C8CF
		private void ViewForm_Load(object sender, EventArgs e)
		{
			this.m_AllViews.VerticeNb = 0;
			this.m_AllViews.MaxVerticesPerObj = false;
			this.m_AllViews.MaxVerticesPerObj = false;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000E6FC File Offset: 0x0000C8FC
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

		// Token: 0x060000C1 RID: 193 RVA: 0x0000E770 File Offset: 0x0000C970
		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.79999999999995;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000E790 File Offset: 0x0000C990
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

		// Token: 0x060000C3 RID: 195 RVA: 0x0000E800 File Offset: 0x0000CA00
		private static double DegreeToRadian(double angle)
		{
			return 3.1415926535897931 * angle / 180.0;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000E828 File Offset: 0x0000CA28
		private static double RadianToDegree(double angle)
		{
			return angle * 57.295779513082323;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000E848 File Offset: 0x0000CA48
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
											double num3 = ExportToVUxyzForm.DegreeToRadian(angle);
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
													double num9 = ExportToVUxyzForm.DegreeToRadian(angle2);
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
													double num11 = ExportToVUxyzForm.RadianToDegree(Convert.ToDouble(value6));
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
				string path = this.pathStart + this.pathSchema;
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
				bool flag30 = false;
				bool checked4 = this.checkBoxStartVU.Checked;
				if (checked4)
				{
					flag29 = true;
					this.m_AllViews.StartVUoption = true;
				}
				bool checked5 = this.checkBoxToWebGL.Checked;
				if (checked5)
				{
					flag30 = true;
				}
				Transaction transaction = new Transaction(document);
				transaction.Start("HideAnnotations");
				foreach (object obj in document.Settings.Categories)
				{
					Category category = (Category)obj;
					bool flag31 = category.get_AllowsVisibilityControl(view3D);
					if (flag31)
					{
						bool flag32 = category.CategoryType != CategoryType.Model;
						
						if (flag32)
						{
							view3D.SetCategoryHidden(category.Id, true);
						}
					}
				}
				transaction.Commit();
				int num26 = 20000000;
				int num27 = num26 * 2;
				bool flag33 = !this.checkBoxTrialVersion.Checked;
				if (flag33)
				{
					bool flag34 = !flag;
					if (flag34)
					{
						bool flag35 = view3D != null;
						if (flag35)
						{
							CheckExportContext checkExportContext = new CheckExportContext(document, this.m_AllViews);
							new CustomExporter(document, checkExportContext)
							{
								IncludeGeometricObjects = false,
								ShouldStopOnError = false
							}.Export(view3D);
							this.m_AllViews.GroupingOptions = 0;
							bool checked6 = this.radioButtonSingleObject.Checked;
							if (checked6)
							{
								this.m_AllViews.GroupingOptions = 3;
							}
							bool checked7 = this.radioButtonByTypes.Checked;
							if (checked7)
							{
								this.m_AllViews.GroupingOptions = 5;
							}
							bool checked8 = this.radioButtonMaterialsFast.Checked;
							if (checked8)
							{
								this.m_AllViews.GroupingOptions = 6;
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
					bool flag38 = !this.checkBoxTrialVersion.Checked;
					if (flag38)
					{
						bool flag39 = view3D != null & !flag;
						if (flag39)
						{
							string text47 = null;
							text47 = this.pathStart + this.pathEndObj;
							ExportToVRContext exportToVRContext = new ExportToVRContext(document, this.m_AllViews);
							CustomExporter customExporter = new CustomExporter(document, exportToVRContext);
							customExporter.IncludeGeometricObjects = false;
							customExporter.ShouldStopOnError = false;
							try
							{
								this.m_AllViews.ExportSubCategories = false;
								bool flag40 = num == 2 | num == 3;
								if (flag40)
								{
									customExporter.Export(view3D);
								}
								bool flag41 = num == 4;
								if (flag41)
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
											bool flag42 = category6.Id.IntegerValue == -2000025;
											if (flag42)
											{
												category3 = category6;
											}
											bool flag43 = category6.Id.IntegerValue == -2000031;
											if (flag43)
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
										bool flag44 = category7.get_AllowsVisibilityControl(view3D);
										if (flag44)
										{
											bool flag45 = category7.Id.IntegerValue != -2000023;
											if (flag45)
											{
												view3D.SetCategoryHidden(category7.Id, true);
											}
										}
										foreach (object obj5 in category7.SubCategories)
										{
											Category category8 = (Category)obj5;
											bool flag46 = category8.get_AllowsVisibilityControl(view3D);
											if (flag46)
											{
												bool flag47 = category8.Id.IntegerValue != -2000025 && category8.Id.IntegerValue != -2000031;
												if (flag47)
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
								bool flag48 = num == 1 | num == 5 | num == 6;
								if (flag48)
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
										bool flag49 = element4.Id.IntegerValue != -1;
										if (flag49)
										{
											bool flag50 = element4.CanBeHidden(view3D);
											if (flag50)
											{
												collection9.Add(elementId3);
											}
										}
									}
									bool flag51 = false;
									foreach (ElementId elementId4 in collection8)
									{
										bool flag52 = false;
										Element element5 = document.GetElement(elementId4);
										bool flag53 = element5 != null;
										if (flag53)
										{
											bool flag54 = element5.Category != null;
											if (flag54)
											{
												bool flag55 = element5.Category.CategoryType == CategoryType.Model;
												if (flag55)
												{
													flag52 = true;
												}
												bool flag56 = element5.Category.Id.IntegerValue == -2001340;
												if (flag56)
												{
													flag51 = true;
												}
												bool flag57 = element5.Category.Id.IntegerValue == -2001352;
												if (flag57)
												{
													flag51 = true;
												}
											}
											bool flag58 = element5.GetTypeId() != null;
											if (flag58)
											{
												int integerValue = element5.GetTypeId().IntegerValue;
												bool flag59 = flag52 & !flag51;
												if (flag59)
												{
													GeometryElement geometryElement = element5.get_Geometry(new Options
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
												bool flag67 = !list5.Contains(integerValue) && flag51;
												if (flag67)
												{
													list5.Add(integerValue);
												}
											}
										}
										flag51 = false;
									}
									for (int j = 0; j < list5.Count; j++)
									{
										int item2 = list5[j];
										int num30 = 0;
										bool flag68 = num30 <= num28;
										if (flag68)
										{
											list7.Add(item2);
										}
										bool flag69 = num30 > num28;
										if (flag69)
										{
											list6.Add(item2);
										}
									}
									bool flag70 = list7.Count > 0;
									if (flag70)
									{
										bool flag71 = false;
										foreach (ElementId elementId5 in collection8)
										{
											Element element6 = document.GetElement(elementId5);
											bool flag72 = element6 != null;
											if (flag72)
											{
												int integerValue2 = element6.GetTypeId().IntegerValue;
												bool flag73 = !list7.Contains(integerValue2);
												if (flag73)
												{
													bool flag74 = element6.Category != null;
													if (flag74)
													{
														bool flag75 = element6.Category.Id.IntegerValue == -2001340;
														if (flag75)
														{
															flag71 = true;
														}
													}
													bool flag76 = !flag51;
													if (flag76)
													{
														GeometryElement geometryElement2 = element6.get_Geometry(new Options
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
														bool flag85 = element6.CanBeHidden(view3D);
														if (flag85)
														{
															collection9.Add(elementId5);
														}
													}
												}
											}
											flag71 = false;
										}
										Transaction transaction4 = new Transaction(document);
										transaction4.Start("TempHideType");
										bool flag86 = collection9.Count > 0;
										if (flag86)
										{
											view3D.HideElements(collection9);
										}
										transaction4.Commit();
										customExporter.Export(view3D);
										Transaction transaction5 = new Transaction(document);
										transaction5.Start("TempUnhideType");
										bool flag87 = collection9.Count > 0;
										if (flag87)
										{
											view3D.UnhideElements(collection9);
										}
										transaction5.Commit();
										collection9.Clear();
									}
									bool flag88 = list6.Count > 0;
									if (flag88)
									{
										foreach (int num31 in list6)
										{
											bool flag89 = false;
											bool flag90 = num31 != -1;
											if (flag90)
											{
												foreach (ElementId elementId6 in collection8)
												{
													Element element7 = document.GetElement(elementId6);
													bool flag91 = element7 != null;
													if (flag91)
													{
														int integerValue3 = element7.GetTypeId().IntegerValue;
														bool flag92 = num31 != integerValue3;
														if (flag92)
														{
															bool flag93 = element7.Category != null;
															if (flag93)
															{
																bool flag94 = element7.Category.Id.IntegerValue == -2001340;
																if (flag94)
																{
																	flag89 = true;
																}
															}
															bool flag95 = !flag89;
															if (flag95)
															{
																GeometryElement geometryElement3 = element7.get_Geometry(new Options
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
																bool flag104 = element7.CanBeHidden(view3D);
																if (flag104)
																{
																	collection9.Add(elementId6);
																}
															}
														}
													}
													flag89 = false;
												}
												Transaction transaction6 = new Transaction(document);
												transaction6.Start("TempHideType");
												bool flag105 = collection9.Count > 0;
												if (flag105)
												{
													view3D.HideElements(collection9);
												}
												transaction6.Commit();
												customExporter.Export(view3D);
												Transaction transaction7 = new Transaction(document);
												transaction7.Start("TempUnhideType");
												bool flag106 = collection9.Count > 0;
												if (flag106)
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
							bool flag107 = !flag;
							if (flag107)
							{
								bool checked9 = this.checkBoxDeleteOBJ.Checked;
								if (checked9)
								{
									DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(text47));
									bool flag108 = Directory.Exists(Path.GetDirectoryName(text47));
									if (flag108)
									{
										FileInfo[] files = directoryInfo.GetFiles("*.obj");
										foreach (FileInfo fileInfo in files)
										{
											fileInfo.Delete();
										}
									}
								}
								bool flag109 = File.Exists(text47);
								if (flag109)
								{
									File.Delete(text47);
								}
								StringBuilder stringBuilder3 = new StringBuilder();
								StringBuilder stringBuilder4 = new StringBuilder();
								bool checked10 = this.checkBoxOverride.Checked;
								if (checked10)
								{
									stringBuilder3.Append(this.m_AllViews.FCTbySUBCATsbuilder.ToString());
									using (StringReader stringReader = new StringReader(stringBuilder3.ToString()))
									{
										for (string text48 = stringReader.ReadLine(); text48 != null; text48 = stringReader.ReadLine())
										{
											bool flag110 = text48.Contains("usemtl") & !text48.Contains("Glass");
											if (flag110)
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
								bool flag111 = Directory.Exists(Path.GetDirectoryName(text47));
								if (flag111)
								{
									using (StreamWriter streamWriter2 = new StreamWriter(text47, true))
									{
										bool flag112 = num == 1;
										if (flag112)
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
										bool flag113 = num == 2;
										if (flag113)
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
										bool flag114 = num == 6;
										if (flag114)
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
										bool flag115 = num == 3;
										if (flag115)
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
										bool flag116 = num == 4;
										if (flag116)
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
										bool flag117 = num == 5;
										if (flag117)
										{
											bool checked11 = this.checkBoxOverride.Checked;
											if (checked11)
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
											bool flag118 = !this.checkBoxOverride.Checked;
											if (flag118)
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
										bool flag119 = num == 2 | num == 3 | num == 4;
										if (flag119)
										{
											bool flag120 = exportToVRContext.MATERIALsbuilder != null;
											if (flag120)
											{
												streamWriter3.WriteLine(exportToVRContext.MATERIALsbuilder);
											}
										}
										bool flag121 = num == 1 | num == 5 | num == 6;
										if (flag121)
										{
											bool flag122 = this.m_AllViews.MATERIALsbuilder != null;
											if (flag122)
											{
												bool flag123 = !this.checkBoxOverride.Checked;
												if (flag123)
												{
													streamWriter3.WriteLine(this.m_AllViews.MATERIALsbuilder);
												}
												bool checked12 = this.checkBoxOverride.Checked;
												if (checked12)
												{
													streamWriter3.WriteLine(stringBuilder4);
												}
											}
										}
										streamWriter3.Close();
									}
									ExportToVUxyzForm._export_folder_name = Path.GetDirectoryName(text47);
								}
								bool flag124 = Directory.Exists(Path.GetDirectoryName(text47));
								if (flag124)
								{
									bool textureExist = exportToVRContext.textureExist;
									if (textureExist)
									{
										bool flag125 = exportToVRContext.key_Materials != null;
										if (flag125)
										{
											bool flag126 = exportToVRContext.key_Materials.Count > 0;
											if (flag126)
											{
												foreach (object obj6 in exportToVRContext.key_Materials)
												{
													int num32 = (int)obj6;
													string text49 = Convert.ToString(exportToVRContext.h_Materials[num32]);
													bool flag127 = File.Exists(text49);
													if (flag127)
													{
														bool flag128 = text49 != null & text49 != "";
														if (flag128)
														{
															string fileName = Path.GetFileName(text49);
															string text50 = fileName.Replace(" ", "_");
															string str2 = text50;
															string text51 = ExportToVUxyzForm._export_folder_name + "\\" + str2;
															bool flag129 = !File.Exists(text51);
															if (flag129)
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
							bool flag130 = this.checkBoxStartVU.Checked || this.checkBoxToWebGL.Checked;
							if (flag130)
							{
								AllViews allViews = new AllViews();
								allViews.GroupingOptions = 0;
								bool flag131 = this.checkBoxStartVU.Checked || this.checkBoxToWebGL.Checked;
								if (flag131)
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
									bool flag132 = num == 1 | num == 5 | num == 6;
									if (flag132)
									{
										FilteredElementCollector filteredElementCollector9 = new FilteredElementCollector(document, view3D.Id);
										ICollection<ElementId> collection10 = filteredElementCollector9.ToElementIds();
										ICollection<ElementId> collection11 = filteredElementCollector9.ToElementIds();
										collection11.Clear();
										List<int> list8 = new List<int>();
										List<int> list9 = new List<int>();
										List<int> list10 = new List<int>();
										bool flag133 = list.Count != 0;
										if (flag133)
										{
											foreach (ElementId elementId7 in collection10)
											{
												Element element8 = document.GetElement(elementId7);
												bool flag134 = element8.Id.IntegerValue != -1;
												if (flag134)
												{
													bool flag135 = !list.Contains(elementId7.IntegerValue.ToString());
													if (flag135)
													{
														bool flag136 = element8.CanBeHidden(view3D);
														if (flag136)
														{
															collection11.Add(element8.Id);
														}
													}
												}
											}
											Transaction transaction8 = new Transaction(document);
											transaction8.Start("TempHideType");
											bool flag137 = collection11.Count > 0;
											if (flag137)
											{
												view3D.HideElements(collection11);
											}
											transaction8.Commit();
											customExporter2.Export(view3D);
											Transaction transaction9 = new Transaction(document);
											transaction9.Start("TempUnhideType");
											bool flag138 = collection11.Count > 0;
											if (flag138)
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
								bool flag139 = !flag;
								if (flag139)
								{
									StringBuilder stringBuilder5 = new StringBuilder();
									StringBuilder stringBuilder6 = new StringBuilder();
									string text52 = this.pathStart + this.pathEndGrey;
									bool flag140 = File.Exists(text52);
									if (flag140)
									{
										File.Delete(text52);
									}
									bool flag141 = num == 5;
									if (flag141)
									{
										bool flag142 = Directory.Exists(Path.GetDirectoryName(text52));
										if (flag142)
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
												bool flag143 = num == 1 | num == 5 | num == 6;
												if (flag143)
												{
													bool flag144 = allViews.MATERIALsbuilder != null;
													if (flag144)
													{
														streamWriter5.WriteLine(allViews.MATERIALsbuilder);
													}
												}
												streamWriter5.Close();
											}
											ExportToVUxyzForm._export_folder_name = Path.GetDirectoryName(text52);
											bool flag145 = Directory.Exists(Path.GetDirectoryName(text52));
											if (flag145)
											{
												bool textureExist2 = exportToVRContext.textureExist;
												if (textureExist2)
												{
													bool flag146 = exportToVRContext.key_Materials != null;
													if (flag146)
													{
														bool flag147 = exportToVRContext.key_Materials.Count > 0;
														if (flag147)
														{
															foreach (object obj7 in exportToVRContext.key_Materials)
															{
																int num33 = (int)obj7;
																string text53 = Convert.ToString(exportToVRContext.h_Materials[num33]);
																bool flag148 = File.Exists(text53);
																if (flag148)
																{
																	bool flag149 = text53 != null & text53 != "";
																	if (flag149)
																	{
																		string fileName2 = Path.GetFileName(text53);
																		string text54 = fileName2.Replace(" ", "_");
																		string str3 = text54;
																		string text55 = ExportToVUxyzForm._export_folder_name + "\\" + str3;
																		bool flag150 = !File.Exists(text55);
																		if (flag150)
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
				bool flag151 = flag29;
				if (flag151)
				{
					this.OpenVUIfClosed();
				}
				bool flag152 = flag30;
				if (flag152)
				{
					this.UploadToWebGL();
				}
				base.Close();
			}
			catch (Exception ex3)
			{
				MessageBox.Show(ex3.Message);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000123FC File Offset: 0x000105FC
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

		// Token: 0x060000C7 RID: 199 RVA: 0x00012444 File Offset: 0x00010644
		private void OpenVUIfClosed()
		{
			bool flag = File.Exists(this.VUxyzPath);
			if (flag)
			{
				Process process = new Process();
				process.StartInfo.FileName = this.VUxyzPath;
				bool flag2 = !this.IsProcessOpen(this.VUxyzPath);
				if (flag2)
				{
					process.Start();
				}
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00012498 File Offset: 0x00010698
		private void UploadToWebGL()
		{
			string path = this.pathStart + this.pathEndObj;
			string text = this.pathStart + this.pathEndMtl;
			string text2 = this.pathStart + this.pathSchema;
			string text3 = this.pathStart + this.pathEndGrey;
			string text4 = this.pathStart + this.pathEndGreyMtl;
			bool flag = File.Exists(path);
			if (flag)
			{
				RestClient restClient = new RestClient("https://www.emanuelfavreau.com/");
				RestRequest restRequest = new RestRequest("nina/uploadVU.php", Method.POST);
				restRequest.RequestFormat = DataFormat.Json;
				restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
				this._path = path;
				restRequest.AddHeader("Content-Type", "multipart/form-data");
				byte[] bytes = File.ReadAllBytes(this._path);
				restRequest.AddParameter("fileToUpload", Path.GetFileName(this._path).ToString());
				restRequest.AddFile("fileToUpload", bytes, this._path, "multipart/form-data");
				IRestResponse restResponse = restClient.Execute(restRequest);
				bool flag2 = restResponse.StatusCode == HttpStatusCode.OK;
				if (!flag2)
				{
					MessageBox.Show(":-(\nFailed.\nObj Error: " + restResponse.ErrorMessage);
				}
			}
			bool flag3 = File.Exists(text);
			if (flag3)
			{
				RestClient restClient2 = new RestClient("https://www.emanuelfavreau.com/");
				RestRequest restRequest2 = new RestRequest("nina/uploadVU.php", Method.POST);
				restRequest2.RequestFormat = DataFormat.Json;
				restRequest2.AddHeader("X-Requested-With", "XMLHttpRequest");
				restRequest2.AddHeader("Content-Type", "multipart/form-data");
				byte[] bytes2 = File.ReadAllBytes(text);
				restRequest2.AddParameter("fileToUpload", Path.GetFileName(text).ToString());
				restRequest2.AddFile("fileToUpload", bytes2, text, "multipart/form-data");
				IRestResponse restResponse2 = restClient2.Execute(restRequest2);
				bool flag4 = restResponse2.StatusCode == HttpStatusCode.OK;
				if (!flag4)
				{
					MessageBox.Show(":-(\nFailed.\nMtl Error: " + restResponse2.ErrorMessage);
				}
			}
			bool flag5 = File.Exists(text2);
			if (flag5)
			{
				RestClient restClient3 = new RestClient("https://www.emanuelfavreau.com/");
				RestRequest restRequest3 = new RestRequest("nina/uploadVU.php", Method.POST);
				restRequest3.RequestFormat = DataFormat.Json;
				restRequest3.AddHeader("X-Requested-With", "XMLHttpRequest");
				restRequest3.AddHeader("Content-Type", "multipart/form-data");
				byte[] bytes3 = File.ReadAllBytes(text2);
				restRequest3.AddParameter("fileToUpload", Path.GetFileName(text2).ToString());
				restRequest3.AddFile("fileToUpload", bytes3, text2, "multipart/form-data");
				IRestResponse restResponse3 = restClient3.Execute(restRequest3);
				bool flag6 = restResponse3.StatusCode == HttpStatusCode.OK;
				if (!flag6)
				{
					MessageBox.Show(":-(\nFailed.\nMtl Error: " + restResponse3.ErrorMessage);
				}
			}
			bool flag7 = File.Exists(text3);
			if (flag7)
			{
				RestClient restClient4 = new RestClient("https://www.emanuelfavreau.com/");
				RestRequest restRequest4 = new RestRequest("nina/uploadVU.php", Method.POST);
				restRequest4.RequestFormat = DataFormat.Json;
				restRequest4.AddHeader("X-Requested-With", "XMLHttpRequest");
				restRequest4.AddHeader("Content-Type", "multipart/form-data");
				byte[] bytes4 = File.ReadAllBytes(text3);
				restRequest4.AddParameter("fileToUpload", Path.GetFileName(text3).ToString());
				restRequest4.AddFile("fileToUpload", bytes4, text3, "multipart/form-data");
				IRestResponse restResponse4 = restClient4.Execute(restRequest4);
				bool flag8 = restResponse4.StatusCode == HttpStatusCode.OK;
				if (!flag8)
				{
					MessageBox.Show(":-(\nFailed.\nMtl Error: " + restResponse4.ErrorMessage);
				}
			}
			bool flag9 = File.Exists(text4);
			if (flag9)
			{
				RestClient restClient5 = new RestClient("https://www.emanuelfavreau.com/");
				RestRequest restRequest5 = new RestRequest("nina/uploadVU.php", Method.POST);
				restRequest5.RequestFormat = DataFormat.Json;
				restRequest5.AddHeader("X-Requested-With", "XMLHttpRequest");
				restRequest5.AddHeader("Content-Type", "multipart/form-data");
				byte[] bytes5 = File.ReadAllBytes(text4);
				restRequest5.AddParameter("fileToUpload", Path.GetFileName(text4).ToString());
				restRequest5.AddFile("fileToUpload", bytes5, text4, "multipart/form-data");
				IRestResponse restResponse5 = restClient5.Execute(restRequest5);
				bool flag10 = restResponse5.StatusCode == HttpStatusCode.OK;
				if (!flag10)
				{
					MessageBox.Show(":-(\nFailed.\nMtl Error: " + restResponse5.ErrorMessage);
				}
			}
			List<string> list = new List<string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(this.pathStart);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				bool flag11 = fileInfo.Extension == ".gif" | fileInfo.Extension == ".jpg" | fileInfo.Extension == ".jpeg" | fileInfo.Extension == ".png";
				if (flag11)
				{
					bool flag12 = !list.Contains(fileInfo.FullName);
					if (flag12)
					{
						list.Add(fileInfo.FullName);
					}
				}
			}
			foreach (string text5 in list)
			{
				bool flag13 = File.Exists(text5);
				if (flag13)
				{
					RestClient restClient6 = new RestClient("https://www.emanuelfavreau.com/");
					RestRequest restRequest6 = new RestRequest("nina/uploadVU.php", Method.POST);
					restRequest6.RequestFormat = DataFormat.Json;
					restRequest6.AddHeader("X-Requested-With", "XMLHttpRequest");
					restRequest6.AddHeader("Content-Type", "multipart/form-data");
					byte[] bytes6 = File.ReadAllBytes(text5);
					restRequest6.AddParameter("fileToUpload", Path.GetFileName(text5).ToString());
					restRequest6.AddFile("fileToUpload", bytes6, text5, "multipart/form-data");
					IRestResponse restResponse6 = restClient6.Execute(restRequest6);
					bool flag14 = restResponse6.StatusCode == HttpStatusCode.OK;
					if (!flag14)
					{
						MessageBox.Show(":-(\nFailed.\nImage Error: " + restResponse6.ErrorMessage);
					}
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00012AD4 File Offset: 0x00010CD4
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

		// Token: 0x060000CA RID: 202 RVA: 0x00012B40 File Offset: 0x00010D40
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

		// Token: 0x040000BD RID: 189
		private ExternalCommandData p_commandData;

		// Token: 0x040000BE RID: 190
		private AllViews m_AllViews;

		// Token: 0x040000BF RID: 191
		private const double _eps = 1E-09;

		// Token: 0x040000C0 RID: 192
		private const double _feet_to_mm = 304.79999999999995;

		// Token: 0x040000C1 RID: 193
		private string mm = "mm";

		// Token: 0x040000C2 RID: 194
		private string ft = "ft";

		// Token: 0x040000C3 RID: 195
		private string units = null;

		// Token: 0x040000C4 RID: 196
		private static string _export_folder_name = null;

		// Token: 0x040000C5 RID: 197
		public int TotalElementInView;

		// Token: 0x040000C6 RID: 198
		private string _path;

		// Token: 0x040000C7 RID: 199
		private string pathStart = "C:\\MSDLBIM\\VU\\3D\\";

		// Token: 0x040000C8 RID: 200
		private string pathEndObj = "a.obj";

		// Token: 0x040000C9 RID: 201
		private string pathEndMtl = "a.mtl";

		// Token: 0x040000CA RID: 202
		private string pathEndGrey = "b.obj";

		// Token: 0x040000CB RID: 203
		private string pathEndGreyMtl = "b.mtl";

		// Token: 0x040000CC RID: 204
		private string pathSchema = "FlipbookSchema.txt";

		// Token: 0x040000CD RID: 205
		private string VUxyzPath = "C:\\MSDLBIM\\VU\\3D\\XYZ\\XYZ.exe";

		// Token: 0x040000CE RID: 206
		public static string ProjectInfos = "Informations sur le projet";

		// Token: 0x040000CF RID: 207
		public static int ProjInfoID = 49504;
	}
}
