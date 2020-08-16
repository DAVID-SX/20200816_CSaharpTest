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
	public partial class ExportViewsToVRForm : System.Windows.Forms.Form
    {
		public ExportViewsToVRForm(ExternalCommandData commandData, AllViews allViews)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = allViews;
		}

		private void ViewForm_Load(object sender, EventArgs e)
		{
			this.listBoxViews.DataSource = this.m_AllViews.ViewListName; 
			this.m_AllViews.VerticeNb = 0;
			this.m_AllViews.MaxVerticesPerObj = false;
			// this.m_AllViews.MaxVerticesPerObj = false;
		}

		private static long ConvertFeetToMillimetres(double d)
		{
			bool flag = 0.0 < d;
			long result;
			if (flag)
			{
				result = ((1E-09 > d) ? 0L : ((long)(304.8 * d + 0.5)));
			}
			else
			{
				result = ((1E-09 > -d) ? 0L : ((long)(304.8 * d - 0.5)));
			}
			return result;
		}

		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.8;
		}

		private string ProjectUnits(string s)
		{
			Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
			Document document = this.p_commandData.Application.ActiveUIDocument.Document;
			FormatOptions formatOptions = document.GetUnits().GetFormatOptions(0);
			string result;
			if (formatOptions.DisplayUnits == DisplayUnitType.DUT_MILLIMETERS)
			{
				result = this.mm;
			}
			else
			{
				result = this.ft;
			}
			return result;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			try
			{
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				// 收集项目中所有的三维视图
				FilteredElementCollector view3dCollector1 = new FilteredElementCollector(document);
				ICollection<Element> view3dCollection1 = view3dCollector1.OfClass(typeof(View3D)).ToElements();
				FilteredElementCollector view3dCollector2 = new FilteredElementCollector(document);
				ICollection<Element> view3dCollection2 = view3dCollector2.OfClass(typeof(View3D)).ToElements();
				// 收集项目中所有的族类型
				FilteredElementCollector familySymbolCollector1 = new FilteredElementCollector(document);
				ICollection<Element> familySymbolCollection1 = familySymbolCollector1.OfClass(typeof(FamilySymbol)).ToElements();
				FilteredElementCollector familySymbolCollector2 = new FilteredElementCollector(document);
				ICollection<Element> familySymbolCollection2 = familySymbolCollector2.OfClass(typeof(FamilySymbol)).ToElements();
				// 收集项目中所有的族实例
				FilteredElementCollector familyInstanceCollector = new FilteredElementCollector(document);
				ICollection<Element> familyInstanceCollection = familyInstanceCollector.OfClass(typeof(FamilyInstance)).ToElements();
				// 声明对话框中需要的内容
				MessageBoxIcon icon = MessageBoxIcon.Exclamation;
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				string caption = "模型导出";
				// ?
				this.m_AllViews.StandAloneVersion = false;
				// 判断哪个单选按钮被选中
				int radioButtonCheckNum = 1;
				if (this.radioButtonMaterialsFast.Checked)
				{
					radioButtonCheckNum = 6;  // 
				}
				if (this.radioButtonByTypes.Checked)
				{
					radioButtonCheckNum = 5;
				}
				if (this.radioButtonSingleObject.Checked)
				{
					radioButtonCheckNum = 1;
				}

				// 未勾选导出其他视图时，判断当前视图类型，满足条件后赋值给view3D
				View3D view3D = null;
				bool isTemplateAndThreeDView = false;
				if (!this.checkBoxOtherView.Checked)
				{
					if (document.ActiveView.ViewType != ViewType.ThreeD)
					{
						MessageBox.Show("当前视图必须为三维视图！");
						base.Close();
					}
					if (document.ActiveView.ViewType == ViewType.ThreeD & document.ActiveView.IsTemplate)
					{
						MessageBox.Show("当前视图为样板视图，不可导出！");
						isTemplateAndThreeDView = true;
						base.Close();
					}
					if (document.ActiveView.ViewType == ViewType.ThreeD & !document.ActiveView.IsTemplate)
					{
						view3D = (document.ActiveView as View3D);
					}
				}


				// 隐藏视图中的标注图元
				Transaction transaction = new Transaction(document);
				transaction.Start("隐藏标注");
				// 未勾选导出其他视图时的隐藏方式
				if (!this.checkBoxOtherView.Checked)
				{
					foreach (object obj in document.Settings.Categories)
					{
						Category category = (Category)obj;
						if (category.get_AllowsVisibilityControl(view3D))
						{
							if (category.CategoryType != CategoryType.Model)
							{
								view3D.SetCategoryHidden(category.Id, true);
							}
						}
					}
				}
				// 勾选导出其他视图时的隐藏方式
				if (this.checkBoxOtherView.Checked)
				{
					foreach (Element view3d in view3dCollection1)
					{
						View3D view3D2 = (View3D)view3d;
						foreach (object obj2 in this.listBoxViews.SelectedItems)
						{
							if (view3D2.Name == (string)obj2)
							{
								view3D = view3D2;
								if (view3D != null & view3D.ViewType == ViewType.ThreeD & view3D.IsTemplate)
								{
									isTemplateAndThreeDView = true;
								}
								if (!isTemplateAndThreeDView)
								{
									foreach (object obj3 in document.Settings.Categories)
									{
										Category category2 = (Category)obj3;
										if (category2.get_AllowsVisibilityControl(view3D))
										{
											if (category2.CategoryType != CategoryType.Model)
											{
												view3D.SetCategoryHidden(category2.Id, true);
											}
										}
									}
								}
							}
						}
					}
				}
				transaction.Commit();


				int num2 = 5000000;
				int num3 = num2 * 2;
				// 未勾选导出其他视图时的导出方式
				if (!this.checkBoxOtherView.Checked)
				{
					if (!isTemplateAndThreeDView)
					{
						if (view3D != null)
						{
							// 导出数据
							CheckExportContext checkExportContext = new CheckExportContext(document, this.m_AllViews);
							new CustomExporter(document, checkExportContext)
							{
								IncludeGeometricObjects = false,
								ShouldStopOnError = false
							}.Export(view3D);


							this.m_AllViews.GroupingOptions = 0;
							if (this.radioButtonSingleObject.Checked)
							{
								this.m_AllViews.GroupingOptions = 3;
							}
							if (this.radioButtonByTypes.Checked)
							{
								this.m_AllViews.GroupingOptions = 5;
							}
							if (this.radioButtonMaterialsFast.Checked)
							{
								this.m_AllViews.GroupingOptions = 6;
							}

							if (checkExportContext.TotalNBofPoints > num3)
							{
								MessageBox.Show(string.Concat(new string[]
								{
									"This 3D View contains ",
									checkExportContext.TotalNBofPoints.ToString(),
									" Vertices.\nMax Vertices per Export: ",
									num3.ToString(),
									"."
								}), caption, buttons, icon);
								isTemplateAndThreeDView = true;
							}
						}
					}
					if (isTemplateAndThreeDView)
					{
						base.Close();
					}
					if (application.VersionNumber.Contains("2019") | application.VersionNumber.Contains("2020"))
					{
						if (view3D != null & !isTemplateAndThreeDView)
						{
							string text = null;
							SaveFileDialog saveFileDialog = new SaveFileDialog();
							saveFileDialog.InitialDirectory = "C:\\";
							saveFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
							saveFileDialog.FilterIndex = 1;
							saveFileDialog.RestoreDirectory = true;
							saveFileDialog.FileName = null;
							if (saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								try
								{
									text = saveFileDialog.FileName;
									mtlPath = saveFileDialog.FileName;
								}
								catch (Exception ex)
								{
									MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
								}
							}
							else
							{
								if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
								{
									isTemplateAndThreeDView = true;
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
								if (radioButtonCheckNum == 2 | radioButtonCheckNum == 3)
								{
									customExporter.Export(view3D);
								}
								if (radioButtonCheckNum == 4)
								{
									Categories categories = document.Settings.Categories;
									Category category3 = categories.get_Item((BuiltInCategory)(-2000025));
									Category category4 = null;
									Category category5 = null;
									foreach (object obj4 in document.Settings.Categories)
									{
										Category category6 = (Category)obj4;
										foreach (object obj5 in category6.SubCategories)
										{
											Category category7 = (Category)obj5;
											bool flag25 = category7.Id.IntegerValue == -2000025;
											if (flag25)
											{
												category4 = category7;
											}
											bool flag26 = category7.Id.IntegerValue == -2000031;
											if (flag26)
											{
												category5 = category7;
											}
										}
									}
									Transaction transaction2 = new Transaction(document);
									transaction2.Start("TempIsolatePanels");
									view3D.SetCategoryHidden(category4.Id, true);
									view3D.SetCategoryHidden(category5.Id, true);
									transaction2.Commit();
									customExporter.Export(view3D);
									Transaction transaction3 = new Transaction(document);
									transaction3.Start("TempHidePanels");
									view3D.SetCategoryHidden(category4.Id, false);
									view3D.SetCategoryHidden(category5.Id, false);
									foreach (object obj6 in document.Settings.Categories)
									{
										Category category8 = (Category)obj6;
										bool flag27 = category8.get_AllowsVisibilityControl(view3D);
										if (flag27)
										{
											bool flag28 = category8.Id.IntegerValue != -2000023;
											if (flag28)
											{
												view3D.SetCategoryHidden(category8.Id, true);
											}
										}
										foreach (object obj7 in category8.SubCategories)
										{
											Category category9 = (Category)obj7;
											bool flag29 = category9.get_AllowsVisibilityControl(view3D);
											if (flag29)
											{
												bool flag30 = category9.Id.IntegerValue != -2000025 && category9.Id.IntegerValue != -2000031;
												if (flag30)
												{
													view3D.SetCategoryHidden(category9.Id, true);
												}
											}
										}
									}
									transaction3.Commit();
									this.m_AllViews.ExportSubCategories = true;
									customExporter.Export(view3D);
								}
								if (radioButtonCheckNum == 1 | radioButtonCheckNum == 5 | radioButtonCheckNum == 6)
								{
									int num4 = 1000;
									FilteredElementCollector view3dCollector = new FilteredElementCollector(document, view3D.Id);
									ICollection<ElementId> view3dIdsCollection = view3dCollector.ToElementIds();
									ICollection<ElementId> collection7 = view3dCollector.ToElementIds();
									collection7.Clear();
									List<int> list = new List<int>();
									List<int> list2 = new List<int>();
									List<int> list3 = new List<int>();
									bool flag32 = false;
									foreach (ElementId elementId in view3dIdsCollection)
									{
										bool flag33 = false;
										Element element2 = document.GetElement(elementId);
										if (element2 != null)
										{
											if (element2.Category != null)
											{
												if (element2.Category.CategoryType == CategoryType.Model)
												{
													flag33 = true;
												}
												if (element2.Category.Id.IntegerValue == -2001340)
												{
													flag32 = true;
												}
												if (element2.Category.Id.IntegerValue == -2001352)
												{
													flag32 = true;
												}
											}
											if (element2.GetTypeId() != null)
											{
												int integerValue = element2.GetTypeId().IntegerValue;
												bool flag40 = flag33 & !flag32;
												if (flag40)
												{
													GeometryElement geometryElement = element2.get_Geometry(new Options
													{
														ComputeReferences = true
													});
													bool flag41 = geometryElement != null;
													if (flag41)
													{
														foreach (GeometryObject geometryObject in geometryElement)
														{
															if (geometryObject is Solid)
															{
																Solid solid = geometryObject as Solid;
																bool flag43 = null != solid;
																if (flag43)
																{
																	bool flag44 = solid.Faces.Size > 0;
																	if (flag44)
																	{
																		flag32 = true;
																		break;
																	}
																}
															}
															GeometryInstance geometryInstance = geometryObject as GeometryInstance;
															bool flag45 = null != geometryInstance;
															if (flag45)
															{
																foreach (GeometryObject geometryObject2 in geometryInstance.SymbolGeometry)
																{
																	Solid solid2 = geometryObject2 as Solid;
																	bool flag46 = null != solid2;
																	if (flag46)
																	{
																		bool flag47 = solid2.Faces.Size > 0;
																		if (flag47)
																		{
																			flag32 = true;
																			break;
																		}
																	}
																}
															}
														}
													}
												}
												if (!list.Contains(integerValue) && flag32)
												{
													list.Add(integerValue);
												}
											}
										}
										flag32 = false;
									}
									for (int i = 0; i < list.Count; i++)
									{
										int item = list[i];
										int num5 = 0;
										bool flag49 = num5 <= num4;
										if (flag49)
										{
											list3.Add(item);
										}
										bool flag50 = num5 > num4;
										if (flag50)
										{
											list2.Add(item);
										}
									}
									bool flag51 = list3.Count > 0;
									if (flag51)
									{
										bool flag52 = false;
										foreach (ElementId elementId2 in view3dIdsCollection)
										{
											Element element3 = document.GetElement(elementId2);
											bool flag53 = element3 != null;
											if (flag53)
											{
												int integerValue2 = element3.GetTypeId().IntegerValue;
												bool flag54 = !list3.Contains(integerValue2);
												if (flag54)
												{
													bool flag55 = element3.Category != null;
													if (flag55)
													{
														bool flag56 = element3.Category.Id.IntegerValue == -2001340;
														if (flag56)
														{
															flag52 = true;
														}
													}
													bool flag57 = !flag32;
													if (flag57)
													{
														GeometryElement geometryElement2 = element3.get_Geometry(new Options
														{
															ComputeReferences = true
														});
														bool flag58 = geometryElement2 != null;
														if (flag58)
														{
															foreach (GeometryObject geometryObject3 in geometryElement2)
															{
																bool flag59 = geometryObject3 is Solid;
																if (flag59)
																{
																	Solid solid3 = geometryObject3 as Solid;
																	bool flag60 = null != solid3;
																	if (flag60)
																	{
																		bool flag61 = solid3.Faces.Size > 0;
																		if (flag61)
																		{
																			flag52 = true;
																			break;
																		}
																	}
																}
																GeometryInstance geometryInstance2 = geometryObject3 as GeometryInstance;
																bool flag62 = null != geometryInstance2;
																if (flag62)
																{
																	foreach (GeometryObject geometryObject4 in geometryInstance2.SymbolGeometry)
																	{
																		Solid solid4 = geometryObject4 as Solid;
																		bool flag63 = null != solid4;
																		if (flag63)
																		{
																			bool flag64 = solid4.Faces.Size > 0;
																			if (flag64)
																			{
																				flag52 = true;
																				break;
																			}
																		}
																	}
																}
															}
														}
													}
													bool flag65 = flag52;
													if (flag65)
													{
														bool flag66 = element3.CanBeHidden(view3D);
														if (flag66)
														{
															collection7.Add(elementId2);
														}
													}
												}
											}
											flag52 = false;
										}
										Transaction transaction4 = new Transaction(document);
										transaction4.Start("TempHideType");
										bool flag67 = collection7.Count > 0;
										if (flag67)
										{
											view3D.HideElements(collection7);
										}
										transaction4.Commit();
										customExporter.Export(view3D);
										Transaction transaction5 = new Transaction(document);
										transaction5.Start("TempUnhideType");
										bool flag68 = collection7.Count > 0;
										if (flag68)
										{
											view3D.UnhideElements(collection7);
										}
										transaction5.Commit();
										collection7.Clear();
									}
									bool flag69 = list2.Count > 0;
									if (flag69)
									{
										foreach (int num6 in list2)
										{
											bool flag70 = false;
											bool flag71 = num6 != -1;
											if (flag71)
											{
												foreach (ElementId elementId3 in view3dIdsCollection)
												{
													Element element4 = document.GetElement(elementId3);
													bool flag72 = element4 != null;
													if (flag72)
													{
														int integerValue3 = element4.GetTypeId().IntegerValue;
														bool flag73 = num6 != integerValue3;
														if (flag73)
														{
															bool flag74 = element4.Category != null;
															if (flag74)
															{
																bool flag75 = element4.Category.Id.IntegerValue == -2001340;
																if (flag75)
																{
																	flag70 = true;
																}
															}
															bool flag76 = !flag70;
															if (flag76)
															{
																GeometryElement geometryElement3 = element4.get_Geometry(new Options
																{
																	ComputeReferences = true
																});
																bool flag77 = geometryElement3 != null;
																if (flag77)
																{
																	foreach (GeometryObject geometryObject5 in geometryElement3)
																	{
																		bool flag78 = geometryObject5 is Solid;
																		if (flag78)
																		{
																			Solid solid5 = geometryObject5 as Solid;
																			bool flag79 = null != solid5;
																			if (flag79)
																			{
																				bool flag80 = solid5.Faces.Size > 0;
																				if (flag80)
																				{
																					flag70 = true;
																					break;
																				}
																			}
																		}
																		GeometryInstance geometryInstance3 = geometryObject5 as GeometryInstance;
																		bool flag81 = null != geometryInstance3;
																		if (flag81)
																		{
																			foreach (GeometryObject geometryObject6 in geometryInstance3.SymbolGeometry)
																			{
																				Solid solid6 = geometryObject6 as Solid;
																				bool flag82 = null != solid6;
																				if (flag82)
																				{
																					bool flag83 = solid6.Faces.Size > 0;
																					if (flag83)
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
															bool flag84 = flag70;
															if (flag84)
															{
																bool flag85 = element4.CanBeHidden(view3D);
																if (flag85)
																{
																	collection7.Add(elementId3);
																}
															}
														}
													}
													flag70 = false;
												}
												Transaction transaction6 = new Transaction(document);
												transaction6.Start("TempHideType");
												bool flag86 = collection7.Count > 0;
												if (flag86)
												{
													view3D.HideElements(collection7);
												}
												transaction6.Commit();
												customExporter.Export(view3D);
												Transaction transaction7 = new Transaction(document);
												transaction7.Start("TempUnhideType");
												bool flag87 = collection7.Count > 0;
												if (flag87)
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
							bool flag88 = !isTemplateAndThreeDView;
							if (flag88)
							{
								bool flag89 = File.Exists(text);
								if (flag89)
								{
									File.Delete(text);
								}
								bool flag90 = Directory.Exists(Path.GetDirectoryName(text));
								if (flag90)
								{
									using (StreamWriter streamWriter = new StreamWriter(text, true))
									{
										bool flag91 = radioButtonCheckNum == 1;
										if (flag91)
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
										bool flag92 = radioButtonCheckNum == 2;
										if (flag92)
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
										bool flag93 = radioButtonCheckNum == 6;
										if (flag93)
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
										bool flag94 = radioButtonCheckNum == 3;
										if (flag94)
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
										bool flag95 = radioButtonCheckNum == 4;
										if (flag95)
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
										bool flag96 = radioButtonCheckNum == 5;
										if (flag96)
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
										bool flag97 = radioButtonCheckNum == 2 | radioButtonCheckNum == 3 | radioButtonCheckNum == 4;
										if (flag97)
										{
											bool flag98 = exportToVRContext.MATERIALsbuilder != null;
											if (flag98)
											{
												streamWriter2.WriteLine(exportToVRContext.MATERIALsbuilder);
											}
										}
										bool flag99 = radioButtonCheckNum == 1 | radioButtonCheckNum == 5 | radioButtonCheckNum == 6;
										if (flag99)
										{
											bool flag100 = this.m_AllViews.MATERIALsbuilder != null;
											if (flag100)
											{
												streamWriter2.WriteLine(this.m_AllViews.MATERIALsbuilder);
											}
										}
										streamWriter2.Close();
									}
									ExportViewsToVRForm._export_folder_name = Path.GetDirectoryName(text);
								}
								bool flag101 = Directory.Exists(Path.GetDirectoryName(text));
								if (flag101)
								{
									bool textureExist = exportToVRContext.textureExist;
									if (textureExist)
									{
										bool flag102 = exportToVRContext.key_Materials != null;
										if (flag102)
										{
											bool flag103 = exportToVRContext.key_Materials.Count > 0;
											if (flag103)
											{
												foreach (object obj8 in exportToVRContext.key_Materials)
												{
													int num7 = (int)obj8;
													string text2 = Convert.ToString(exportToVRContext.h_Materials[num7]);
													bool flag104 = File.Exists(text2);
													if (flag104)
													{
														bool flag105 = text2 != null & text2 != "";
														if (flag105)
														{
															string fileName = Path.GetFileName(text2);
															string text3 = fileName.Replace(" ", "_");
															string str = text3;
															string text4 = ExportViewsToVRForm._export_folder_name + "\\" + str;
															bool flag106 = !File.Exists(text4);
															if (flag106)
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
				if (this.checkBoxOtherView.Checked)
				{
					bool flag107 = isTemplateAndThreeDView;
					if (flag107)
					{
						MessageBox.Show("One or more selected views are not exportable.");
						base.Close();
					}
					string text5 = null;
					string text6 = null;
					SaveFileDialog saveFileDialog2 = new SaveFileDialog();
					saveFileDialog2.InitialDirectory = "C:\\";
					saveFileDialog2.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
					saveFileDialog2.FilterIndex = 1;
					saveFileDialog2.RestoreDirectory = true;
					saveFileDialog2.FileName = null;
					bool flag108 = saveFileDialog2.ShowDialog() == DialogResult.OK;
					if (flag108)
					{
						try
						{
							text5 = saveFileDialog2.FileName;
							mtlPath = saveFileDialog2.FileName;
							text6 = Path.GetFileNameWithoutExtension(text5);
						}
						catch (Exception ex3)
						{
							MessageBox.Show("Error: Could not read file. Original error: " + ex3.Message);
						}
					}
					else
					{
						bool flag109 = saveFileDialog2.ShowDialog() == DialogResult.Cancel;
						if (flag109)
						{
							isTemplateAndThreeDView = true;
							return;
						}
					}
					bool flag110 = !isTemplateAndThreeDView;
					if (flag110)
					{
						foreach (Element element5 in view3dCollection1)
						{
							View3D view3D3 = (View3D)element5;
							foreach (object obj9 in this.listBoxViews.SelectedItems)
							{
								string b2 = (string)obj9;
								bool flag111 = view3D3.Name == b2;
								if (flag111)
								{
									view3D = view3D3;
									bool flag112 = view3D != null;
									if (flag112)
									{
										this.m_AllViews = new AllViews();
										CheckExportContext checkExportContext2 = new CheckExportContext(document, this.m_AllViews);
										new CustomExporter(document, checkExportContext2)
										{
											IncludeGeometricObjects = false,
											ShouldStopOnError = false
										}.Export(view3D);
										this.m_AllViews.GroupingOptions = 0;
										bool checked9 = this.radioButtonSingleObject.Checked;
										if (checked9)
										{
											this.m_AllViews.GroupingOptions = 3;
										}
										bool checked10 = this.radioButtonByTypes.Checked;
										if (checked10)
										{
											this.m_AllViews.GroupingOptions = 5;
										}
										bool checked11 = this.radioButtonMaterialsFast.Checked;
										if (checked11)
										{
											this.m_AllViews.GroupingOptions = 6;
										}
										bool flag113 = checkExportContext2.TotalNBofPoints > num3;
										if (flag113)
										{
											MessageBox.Show(string.Concat(new string[]
											{
												"This 3D View contains ",
												checkExportContext2.TotalNBofPoints.ToString(),
												" Vertices.\nMax Vertices per Export: ",
												num3.ToString(),
												"."
											}), caption, buttons, icon);
											isTemplateAndThreeDView = true;
										}
										bool flag114 = application.VersionNumber.Contains("2019") | application.VersionNumber.Contains("2020");
										if (flag114)
										{
											bool flag115 = view3D != null & !isTemplateAndThreeDView;
											if (flag115)
											{
												ExportToVRContext exportToVRContext2 = new ExportToVRContext(document, this.m_AllViews);
												CustomExporter customExporter2 = new CustomExporter(document, exportToVRContext2);
												customExporter2.IncludeGeometricObjects = false;
												customExporter2.ShouldStopOnError = false;
												try
												{
													this.m_AllViews.ExportSubCategories = false;
													bool flag116 = radioButtonCheckNum == 2 | radioButtonCheckNum == 3;
													if (flag116)
													{
														customExporter2.Export(view3D);
													}
													bool flag117 = radioButtonCheckNum == 4;
													if (flag117)
													{
														Categories categories2 = document.Settings.Categories;
														Category category10 = categories2.get_Item((BuiltInCategory)(-2000025));
														Category category11 = null;
														Category category12 = null;
														foreach (object obj10 in document.Settings.Categories)
														{
															Category category13 = (Category)obj10;
															foreach (object obj11 in category13.SubCategories)
															{
																Category category14 = (Category)obj11;
																bool flag118 = category14.Id.IntegerValue == -2000025;
																if (flag118)
																{
																	category11 = category14;
																}
																bool flag119 = category14.Id.IntegerValue == -2000031;
																if (flag119)
																{
																	category12 = category14;
																}
															}
														}
														Transaction transaction8 = new Transaction(document);
														transaction8.Start("TempIsolatePanels");
														view3D.SetCategoryHidden(category11.Id, true);
														view3D.SetCategoryHidden(category12.Id, true);
														transaction8.Commit();
														customExporter2.Export(view3D);
														Transaction transaction9 = new Transaction(document);
														transaction9.Start("TempHidePanels");
														view3D.SetCategoryHidden(category11.Id, false);
														view3D.SetCategoryHidden(category12.Id, false);
														foreach (object obj12 in document.Settings.Categories)
														{
															Category category15 = (Category)obj12;
															bool flag120 = category15.get_AllowsVisibilityControl(view3D);
															if (flag120)
															{
																bool flag121 = category15.Id.IntegerValue != -2000023;
																if (flag121)
																{
																	view3D.SetCategoryHidden(category15.Id, true);
																}
															}
															foreach (object obj13 in category15.SubCategories)
															{
																Category category16 = (Category)obj13;
																bool flag122 = category16.get_AllowsVisibilityControl(view3D);
																if (flag122)
																{
																	bool flag123 = category16.Id.IntegerValue != -2000025 && category16.Id.IntegerValue != -2000031;
																	if (flag123)
																	{
																		view3D.SetCategoryHidden(category16.Id, true);
																	}
																}
															}
														}
														transaction9.Commit();
														this.m_AllViews.ExportSubCategories = true;
														customExporter2.Export(view3D);
													}
													bool flag124 = radioButtonCheckNum == 1 | radioButtonCheckNum == 5 | radioButtonCheckNum == 6;
													if (flag124)
													{
														int num8 = 1000;
														FilteredElementCollector filteredElementCollector7 = new FilteredElementCollector(document, view3D.Id);
														ICollection<ElementId> collection8 = filteredElementCollector7.ToElementIds();
														ICollection<ElementId> collection9 = filteredElementCollector7.ToElementIds();
														collection9.Clear();
														List<int> list4 = new List<int>();
														List<int> list5 = new List<int>();
														List<int> list6 = new List<int>();
														bool flag125 = false;
														foreach (ElementId elementId4 in collection8)
														{
															bool flag126 = false;
															Element element6 = document.GetElement(elementId4);
															bool flag127 = element6 != null;
															if (flag127)
															{
																bool flag128 = element6.Category != null;
																if (flag128)
																{
																	bool flag129 = element6.Category.CategoryType == CategoryType.Model;
																	if (flag129)
																	{
																		flag126 = true;
																	}
																	bool flag130 = element6.Category.Id.IntegerValue == -2001340;
																	if (flag130)
																	{
																		flag125 = true;
																	}
																	bool flag131 = element6.Category.Id.IntegerValue == -2001352;
																	if (flag131)
																	{
																		flag125 = true;
																	}
																}
																bool flag132 = element6.GetTypeId() != null;
																if (flag132)
																{
																	int integerValue4 = element6.GetTypeId().IntegerValue;
																	bool flag133 = flag126 & !flag125;
																	if (flag133)
																	{
																		GeometryElement geometryElement4 = element6.get_Geometry(new Options
																		{
																			ComputeReferences = true
																		});
																		bool flag134 = geometryElement4 != null;
																		if (flag134)
																		{
																			foreach (GeometryObject geometryObject7 in geometryElement4)
																			{
																				bool flag135 = geometryObject7 is Solid;
																				if (flag135)
																				{
																					Solid solid7 = geometryObject7 as Solid;
																					bool flag136 = null != solid7;
																					if (flag136)
																					{
																						bool flag137 = solid7.Faces.Size > 0;
																						if (flag137)
																						{
																							flag125 = true;
																							break;
																						}
																					}
																				}
																				GeometryInstance geometryInstance4 = geometryObject7 as GeometryInstance;
																				bool flag138 = null != geometryInstance4;
																				if (flag138)
																				{
																					foreach (GeometryObject geometryObject8 in geometryInstance4.SymbolGeometry)
																					{
																						Solid solid8 = geometryObject8 as Solid;
																						bool flag139 = null != solid8;
																						if (flag139)
																						{
																							bool flag140 = solid8.Faces.Size > 0;
																							if (flag140)
																							{
																								flag125 = true;
																								break;
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																	bool flag141 = !list4.Contains(integerValue4) && flag125;
																	if (flag141)
																	{
																		list4.Add(integerValue4);
																	}
																}
															}
															flag125 = false;
														}
														for (int j = 0; j < list4.Count; j++)
														{
															int item2 = list4[j];
															int num9 = 0;
															bool flag142 = num9 <= num8;
															if (flag142)
															{
																list6.Add(item2);
															}
															bool flag143 = num9 > num8;
															if (flag143)
															{
																list5.Add(item2);
															}
														}
														bool flag144 = list6.Count > 0;
														if (flag144)
														{
															bool flag145 = false;
															foreach (ElementId elementId5 in collection8)
															{
																Element element7 = document.GetElement(elementId5);
																bool flag146 = element7 != null;
																if (flag146)
																{
																	int integerValue5 = element7.GetTypeId().IntegerValue;
																	bool flag147 = !list6.Contains(integerValue5);
																	if (flag147)
																	{
																		bool flag148 = element7.Category != null;
																		if (flag148)
																		{
																			bool flag149 = element7.Category.Id.IntegerValue == -2001340;
																			if (flag149)
																			{
																				flag145 = true;
																			}
																		}
																		bool flag150 = !flag125;
																		if (flag150)
																		{
																			GeometryElement geometryElement5 = element7.get_Geometry(new Options
																			{
																				ComputeReferences = true
																			});
																			bool flag151 = geometryElement5 != null;
																			if (flag151)
																			{
																				foreach (GeometryObject geometryObject9 in geometryElement5)
																				{
																					bool flag152 = geometryObject9 is Solid;
																					if (flag152)
																					{
																						Solid solid9 = geometryObject9 as Solid;
																						bool flag153 = null != solid9;
																						if (flag153)
																						{
																							bool flag154 = solid9.Faces.Size > 0;
																							if (flag154)
																							{
																								flag145 = true;
																								break;
																							}
																						}
																					}
																					GeometryInstance geometryInstance5 = geometryObject9 as GeometryInstance;
																					bool flag155 = null != geometryInstance5;
																					if (flag155)
																					{
																						foreach (GeometryObject geometryObject10 in geometryInstance5.SymbolGeometry)
																						{
																							Solid solid10 = geometryObject10 as Solid;
																							bool flag156 = null != solid10;
																							if (flag156)
																							{
																								bool flag157 = solid10.Faces.Size > 0;
																								if (flag157)
																								{
																									flag145 = true;
																									break;
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																		bool flag158 = flag145;
																		if (flag158)
																		{
																			bool flag159 = element7.CanBeHidden(view3D);
																			if (flag159)
																			{
																				collection9.Add(elementId5);
																			}
																		}
																	}
																}
																flag145 = false;
															}
															Transaction transaction10 = new Transaction(document);
															transaction10.Start("TempHideType");
															bool flag160 = collection9.Count > 0;
															if (flag160)
															{
																view3D.HideElements(collection9);
															}
															transaction10.Commit();
															customExporter2.Export(view3D);
															Transaction transaction11 = new Transaction(document);
															transaction11.Start("TempUnhideType");
															bool flag161 = collection9.Count > 0;
															if (flag161)
															{
																view3D.UnhideElements(collection9);
															}
															transaction11.Commit();
															collection9.Clear();
														}
														bool flag162 = list5.Count > 0;
														if (flag162)
														{
															foreach (int num10 in list5)
															{
																bool flag163 = false;
																bool flag164 = num10 != -1;
																if (flag164)
																{
																	foreach (ElementId elementId6 in collection8)
																	{
																		Element element8 = document.GetElement(elementId6);
																		bool flag165 = element8 != null;
																		if (flag165)
																		{
																			int integerValue6 = element8.GetTypeId().IntegerValue;
																			bool flag166 = num10 != integerValue6;
																			if (flag166)
																			{
																				bool flag167 = element8.Category != null;
																				if (flag167)
																				{
																					bool flag168 = element8.Category.Id.IntegerValue == -2001340;
																					if (flag168)
																					{
																						flag163 = true;
																					}
																				}
																				bool flag169 = !flag163;
																				if (flag169)
																				{
																					GeometryElement geometryElement6 = element8.get_Geometry(new Options
																					{
																						ComputeReferences = true
																					});
																					bool flag170 = geometryElement6 != null;
																					if (flag170)
																					{
																						foreach (GeometryObject geometryObject11 in geometryElement6)
																						{
																							bool flag171 = geometryObject11 is Solid;
																							if (flag171)
																							{
																								Solid solid11 = geometryObject11 as Solid;
																								bool flag172 = null != solid11;
																								if (flag172)
																								{
																									bool flag173 = solid11.Faces.Size > 0;
																									if (flag173)
																									{
																										flag163 = true;
																										break;
																									}
																								}
																							}
																							GeometryInstance geometryInstance6 = geometryObject11 as GeometryInstance;
																							bool flag174 = null != geometryInstance6;
																							if (flag174)
																							{
																								foreach (GeometryObject geometryObject12 in geometryInstance6.SymbolGeometry)
																								{
																									Solid solid12 = geometryObject12 as Solid;
																									bool flag175 = null != solid12;
																									if (flag175)
																									{
																										bool flag176 = solid12.Faces.Size > 0;
																										if (flag176)
																										{
																											flag163 = true;
																											break;
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																				bool flag177 = flag163;
																				if (flag177)
																				{
																					bool flag178 = element8.CanBeHidden(view3D);
																					if (flag178)
																					{
																						collection9.Add(elementId6);
																					}
																				}
																			}
																		}
																		flag163 = false;
																	}
																	Transaction transaction12 = new Transaction(document);
																	transaction12.Start("TempHideType");
																	bool flag179 = collection9.Count > 0;
																	if (flag179)
																	{
																		view3D.HideElements(collection9);
																	}
																	transaction12.Commit();
																	customExporter2.Export(view3D);
																	Transaction transaction13 = new Transaction(document);
																	transaction13.Start("TempUnhideType");
																	bool flag180 = collection9.Count > 0;
																	if (flag180)
																	{
																		view3D.UnhideElements(collection9);
																	}
																	transaction13.Commit();
																	collection9.Clear();
																}
															}
														}
													}
												}
												catch (ExternalApplicationException ex4)
												{
													Debug.Print("ExternalApplicationException " + ex4.Message);
												}
												bool flag181 = !isTemplateAndThreeDView;
												if (flag181)
												{
													string directoryName = Path.GetDirectoryName(text5);
													string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text5);
													string name = view3D.Name;
													string text7 = ".obj";
													text5 = string.Concat(new string[]
													{
														directoryName,
														"\\",
														text6,
														name,
														text7
													});
													bool flag182 = File.Exists(text5);
													if (flag182)
													{
														File.Delete(text5);
													}
													bool flag183 = Directory.Exists(Path.GetDirectoryName(text5));
													if (flag183)
													{
														using (StreamWriter streamWriter3 = new StreamWriter(text5, true))
														{
															bool flag184 = radioButtonCheckNum == 1;
															if (flag184)
															{
																streamWriter3.WriteLine(string.Concat(new object[]
																{
																	"mtllib ",
																	Path.GetFileNameWithoutExtension(text5),
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
															bool flag185 = radioButtonCheckNum == 2;
															if (flag185)
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
															bool flag186 = radioButtonCheckNum == 6;
															if (flag186)
															{
																streamWriter3.WriteLine(string.Concat(new object[]
																{
																	"mtllib ",
																	Path.GetFileNameWithoutExtension(text5),
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
															bool flag187 = radioButtonCheckNum == 3;
															if (flag187)
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
															bool flag188 = radioButtonCheckNum == 4;
															if (flag188)
															{
																streamWriter3.WriteLine(string.Concat(new object[]
																{
																	"mtllib ",
																	Path.GetFileNameWithoutExtension(text5),
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
															bool flag189 = radioButtonCheckNum == 5;
															if (flag189)
															{
																streamWriter3.WriteLine(string.Concat(new object[]
																{
																	"mtllib ",
																	Path.GetFileNameWithoutExtension(text5),
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
															string path2 = text5;
															text5 = Path.ChangeExtension(path2, "mtl");
															streamWriter3.Close();
														}
														using (StreamWriter streamWriter4 = new StreamWriter(text5))
														{
															bool flag190 = radioButtonCheckNum == 2 | radioButtonCheckNum == 3 | radioButtonCheckNum == 4;
															if (flag190)
															{
																bool flag191 = exportToVRContext2.MATERIALsbuilder != null;
																if (flag191)
																{
																	streamWriter4.WriteLine(exportToVRContext2.MATERIALsbuilder);
																}
															}
															bool flag192 = radioButtonCheckNum == 1 | radioButtonCheckNum == 5 | radioButtonCheckNum == 6;
															if (flag192)
															{
																bool flag193 = this.m_AllViews.MATERIALsbuilder != null;
																if (flag193)
																{
																	streamWriter4.WriteLine(this.m_AllViews.MATERIALsbuilder);
																}
															}
															streamWriter4.Close();
														}
														ExportViewsToVRForm._export_folder_name = Path.GetDirectoryName(text5);
													}
													bool flag194 = Directory.Exists(Path.GetDirectoryName(text5));
													if (flag194)
													{
														bool textureExist2 = exportToVRContext2.textureExist;
														if (textureExist2)
														{
															bool flag195 = exportToVRContext2.key_Materials != null;
															if (flag195)
															{
																bool flag196 = exportToVRContext2.key_Materials.Count > 0;
																if (flag196)
																{
																	foreach (object obj14 in exportToVRContext2.key_Materials)
																	{
																		int num11 = (int)obj14;
																		string text8 = Convert.ToString(exportToVRContext2.h_Materials[num11]);
																		bool flag197 = File.Exists(text8);
																		if (flag197)
																		{
																			bool flag198 = text8 != null & text8 != "";
																			if (flag198)
																			{
																				string fileName2 = Path.GetFileName(text8);
																				string text9 = fileName2.Replace(" ", "_");
																				string str2 = text9;
																				string text10 = ExportViewsToVRForm._export_folder_name + "\\" + str2;
																				bool flag199 = !File.Exists(text10);
																				if (flag199)
																				{
																					File.Copy(text8, text10);
																				}
																			}
																		}
																	}
																}
															}
														}
													}
													text5 = directoryName + "\\" + fileNameWithoutExtension + text7;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				base.Close();
			}
			catch (Exception ex5)
			{
				MessageBox.Show(ex5.Message);
			}
		}

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

		private void checkBoxOtherView_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBoxOtherView.Checked;
			if (@checked)
			{
				this.listBoxViews.Enabled = true;
			}
			bool flag = !this.checkBoxOtherView.Checked;
			if (flag)
			{
				this.listBoxViews.Enabled = false;
			}
		}

		private void listBoxViews_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.listBoxViews.SelectionMode = SelectionMode.MultiExtended;
			this.listBoxViews.DataSource = this.m_AllViews.ViewListName;
		}

		private ExternalCommandData p_commandData;

		private AllViews m_AllViews;

		private const double _eps = 1E-09;

		private const double _feet_to_mm = 304.79999999999995;

		private string mm = "mm";

		private string ft = "ft";

		private string units = null;

		private static string _export_folder_name = null;

		public int TotalElementInView;

		private string _path;
		public string mtlPath { get; set; }




		private void checkBoxTrialVersion_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
	
}
