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
	// Token: 0x0200000B RID: 11
	public partial class ExpAnimForm : System.Windows.Forms.Form
    {
		// Token: 0x0600008E RID: 142 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public ExpAnimForm(ExternalCommandData commandData, AllViews families)
		{
			this.InitializeComponent();
			this.p_commandData = commandData;
			this.m_AllViews = families;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003EA0 File Offset: 0x000020A0
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

		// Token: 0x06000090 RID: 144 RVA: 0x00003F14 File Offset: 0x00002114
		private static double ConvertMillimetresToFeet(long d)
		{
			return (double)d / 304.79999999999995;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003F34 File Offset: 0x00002134
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

		// Token: 0x06000092 RID: 146 RVA: 0x00003FDC File Offset: 0x000021DC
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
			string text3 = fileNameWithoutExtension + text2;
			string path = null;
			path = string.Concat(new string[]
			{
				directoryName,
				"\\",
				fileNameWithoutExtension,
				text2,
				".cs"
			});
			string arg = "using UnityEngine;\nusing System.Collections.Generic;";
			string value = "";
			StringBuilder stringBuilder = new StringBuilder();
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
			List<string> list10 = new List<string>();
			List<string> list11 = new List<string>();
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			Hashtable hashtable3 = new Hashtable();
			Hashtable hashtable4 = new Hashtable();
			List<string> list12 = new List<string>();
			Hashtable hashtable5 = new Hashtable();
			try
			{
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
				ICollection<Element> collection = filteredElementCollector.OfClass(typeof(Wall)).ToElements();
				FilteredElementCollector filteredElementCollector2 = new FilteredElementCollector(document);
				ICollection<Element> collection2 = filteredElementCollector2.OfClass(typeof(View3D)).ToElements();
				View3D view3D = null;
				FilteredElementCollector filteredElementCollector3 = new FilteredElementCollector(document);
				ICollection<Element> collection3 = filteredElementCollector3.OfClass(typeof(FamilySymbol)).ToElements();
				FamilySymbol familySymbol = null;
				double angle = 270.0;
				double num = ExpAnimForm.DegreeToRadian(angle);
				Transform transform = Transform.CreateRotation(XYZ.BasisX, num);
				this.m_AllViews.ExportProperties = true;
				bool flag5 = document.ActiveView.ViewType == ViewType.ThreeD & !document.ActiveView.IsTemplate;
				if (flag5)
				{
					view3D = (document.ActiveView as View3D);
				}
				FilteredElementCollector filteredElementCollector4 = new FilteredElementCollector(document, view3D.Id);
				ICollection<Element> collection4 = filteredElementCollector4.OfClass(typeof(FamilyInstance)).ToElements();
				FamilyInstance familyInstance = null;
				bool flag6 = text != null;
				if (flag6)
				{
					foreach (Element element in collection3)
					{
						familySymbol = (element as FamilySymbol);
						bool flag7 = familySymbol.Category != null;
						if (flag7)
						{
							bool flag8 = string.Concat(new string[]
							{
								familySymbol.Category.Name,
								" : ",
								familySymbol.Family.Name,
								" : ",
								familySymbol.Name
							}) == this.FamilySymbolNames;
							if (flag8)
							{
								break;
							}
						}
					}
					foreach (Element element2 in collection4)
					{
						familyInstance = (element2 as FamilyInstance);
						bool flag9 = familyInstance.Symbol.Id == familySymbol.Id;
						if (flag9)
						{
							foreach (Element element3 in collection)
							{
								Wall wall = (Wall)element3;
								bool flag10 = wall.Id == familyInstance.Host.Id;
								if (flag10)
								{
									Wall wall2 = wall;
									this.ProjY = wall2.Width;
								}
							}
							bool flag11 = familySymbol != null;
							if (flag11)
							{
								Document document2 = document.EditFamily(familySymbol.Family);
								FilteredElementCollector filteredElementCollector5 = new FilteredElementCollector(document2);
								ICollection<Element> collection5 = filteredElementCollector5.OfClass(typeof(Dimension)).ToElements();
								FilteredElementCollector filteredElementCollector6 = new FilteredElementCollector(document2);
								ICollection<Element> collection6 = filteredElementCollector6.OfClass(typeof(FamilyInstance)).ToElements();
								FilteredElementCollector filteredElementCollector7 = new FilteredElementCollector(document2);
								ICollection<Element> collection7 = filteredElementCollector7.OfClass(typeof(Wall)).ToElements();
								List<ElementId> list13 = new List<ElementId>();
								XYZ xyz = null;
								XYZ xyz2 = null;
								bool flag12 = collection6 != null;
								if (flag12)
								{
									foreach (Element element4 in collection6)
									{
										bool flag13 = element4 != null;
										if (flag13)
										{
											FamilyInstance familyInstance2 = element4 as FamilyInstance;
											bool flag14 = familyInstance2.GroupId.IntegerValue.ToString() == "-1";
											if (flag14)
											{
												list13.Add(familyInstance2.Id);
											}
										}
									}
								}
								bool flag15 = document2 != null && document2.IsFamilyDocument;
								if (flag15)
								{
									int num2 = 0;
									bool flag16 = familyInstance.Category.Id.IntegerValue == -2000023;
									if (flag16)
									{
										foreach (Element element5 in collection7)
										{
											Wall wall3 = (Wall)element5;
											bool flag17 = wall3 != null;
											if (flag17)
											{
												bool flag18 = num2 < 2;
												if (flag18)
												{
													Wall wall4 = wall3;
													this.FamY = wall4.Width;
													num2++;
												}
											}
										}
										bool flag19 = document2.FamilyManager.Types.Size == 1;
										if (flag19)
										{
											Transaction transaction = new Transaction(document2);
											transaction.Start("Add Type");
											string text4 = familyInstance.Name.ToString() + "_Anim";
											document2.FamilyManager.NewType(text4);
											transaction.Commit();
										}
									}
									double num3 = (this.ProjY - this.FamY) / 2.0;
									Transaction transaction2 = new Transaction(document2);
									transaction2.Start("Anim Data");
									foreach (object obj in document2.FamilyManager.Types)
									{
										FamilyType familyType = (FamilyType)obj;
										bool flag20 = familyType.Name == familyInstance.Name;
										if (flag20)
										{
											document2.FamilyManager.CurrentType = familyType;
											foreach (object obj2 in document2.FamilyManager.Parameters)
											{
												FamilyParameter familyParameter = (FamilyParameter)obj2;
												bool flag21 = familyParameter != null && !familyParameter.IsReadOnly;
												if (flag21)
												{
													bool flag22 = familyParameter.StorageType != StorageType.ElementId && familyParameter.StorageType > 0;
													
													if (flag22)
													{
														bool flag23 = familyParameter.Definition.Name.ToString() == this.FamilyParameter;
														if (flag23)
														{
															foreach (Element element6 in collection5)
															{
																Dimension dimension = element6 as Dimension;
																bool flag24 = dimension != null & dimension.DimensionShape == DimensionShape.Angular;
																
																if (flag24)
																{
																	bool flag25 = dimension.get_Parameter((BuiltInParameter)(-1004510)) != null;
																	if (flag25)
																	{
																		string b = dimension.get_Parameter((BuiltInParameter)(-1004510)).AsValueString().ToString();
																		bool flag26 = this.FamilyParameter == b;
																		if (flag26)
																		{
																			Curve curve = dimension.Curve;
																			bool flag27 = curve is Arc;
																			if (flag27)
																			{
																				Arc arc = curve as Arc;
																				xyz = arc.Center;
																				break;
																			}
																		}
																	}
																}
															}
															bool flag28 = xyz != null;
															if (flag28)
															{
																Transform transform2 = familyInstance.GetTransform();
																bool flag29 = (familyInstance.FacingFlipped && !familyInstance.HandFlipped) || (!familyInstance.FacingFlipped && familyInstance.HandFlipped);
																if (flag29)
																{
																	transform2.BasisY = -transform2.BasisY;
																}
																XYZ xyz3 = transform2.OfPoint(xyz);
																XYZ xyz4 = new XYZ(xyz3.X, xyz3.Y, xyz3.Z);
																XYZ xyz5 = new XYZ(xyz3.X + 2.0, xyz3.Y, xyz3.Z);
																xyz3 = transform.OfPoint(xyz3);
																xyz3 = new XYZ(-xyz3.X, xyz3.Y, xyz3.Z);
																double value2 = xyz3.X;
																double value3 = xyz3.Y;
																double value4 = xyz3.Z;
																value2 = Math.Round(value2, this.rnd);
																value3 = Math.Round(value3, this.rnd);
																value4 = Math.Round(value4, this.rnd);
																string text5 = value2.ToString();
																string text6 = text5.Replace(",", ".");
																string text7 = value3.ToString();
																string text8 = text7.Replace(",", ".");
																string text9 = value4.ToString();
																string text10 = text9.Replace(",", ".");
																string str = string.Concat(new string[]
																{
																	text6,
																	"f, ",
																	text8,
																	"f, ",
																	text10,
																	"f"
																});
																value = "\n" + str + "\n";
																Transaction transaction3 = new Transaction(document);
																transaction3.Start("AddLine");
																Line line = Line.CreateBound(xyz4, xyz5);
																SketchPlane sketchPlane = SketchPlane.Create(document, Plane.CreateByOriginAndBasis(xyz4, XYZ.BasisX, XYZ.BasisY));
																ModelLine modelLine = document.Create.NewModelCurve(line, sketchPlane) as ModelLine;
																transaction3.Commit();
															}
															bool flag30 = xyz == null;
															if (flag30)
															{
																foreach (ElementId elementId in list13)
																{
																	bool flag31 = elementId != null;
																	if (flag31)
																	{
																		Element element7 = document2.GetElement(elementId);
																		FamilyInstance familyInstance3 = element7 as FamilyInstance;
																		foreach (object obj3 in familyInstance3.Parameters)
																		{
																			Parameter parameter = (Parameter)obj3;
																			bool hasValue = parameter.HasValue;
																			if (hasValue)
																			{
																				bool flag32 = parameter.Definition.Name == this.FamilyParameter;
																				if (flag32)
																				{
																					Location location = familyInstance3.Location;
																					LocationPoint locationPoint = location as LocationPoint;
																					xyz2 = locationPoint.Point;
																					XYZ xyz6 = new XYZ(xyz2.X, xyz2.Y + num3, xyz2.Z);
																					xyz2 = xyz6;
																					break;
																				}
																			}
																		}
																	}
																}
															}
															bool flag33 = xyz2 != null;
															if (flag33)
															{
																Transform transform3 = familyInstance.GetTransform();
																bool flag34 = (familyInstance.FacingFlipped && !familyInstance.HandFlipped) || (!familyInstance.FacingFlipped && familyInstance.HandFlipped);
																if (flag34)
																{
																	transform3.BasisY = -transform3.BasisY;
																}
																XYZ xyz7 = transform3.OfPoint(xyz2);
																XYZ xyz8 = new XYZ(xyz7.X, xyz7.Y, xyz7.Z);
																XYZ xyz9 = new XYZ(xyz7.X + 2.0, xyz7.Y, xyz7.Z);
																xyz7 = transform.OfPoint(xyz7);
																xyz7 = new XYZ(-xyz7.X, xyz7.Y, xyz7.Z);
																double value5 = xyz7.X;
																double value6 = xyz7.Y;
																double value7 = xyz7.Z;
																value5 = Math.Round(value5, this.rnd);
																value6 = Math.Round(value6, this.rnd);
																value7 = Math.Round(value7, this.rnd);
																string text11 = value5.ToString();
																string text12 = text11.Replace(",", ".");
																string text13 = value6.ToString();
																string text14 = text13.Replace(",", ".");
																string text15 = value7.ToString();
																string text16 = text15.Replace(",", ".");
																string str2 = string.Concat(new string[]
																{
																	text12,
																	"f, ",
																	text14,
																	"f, ",
																	text16,
																	"f"
																});
																value = "\n" + str2 + "\n\n";
																Transaction transaction4 = new Transaction(document);
																transaction4.Start("AddLine");
																Line line2 = Line.CreateBound(xyz8, xyz9);
																SketchPlane sketchPlane2 = SketchPlane.Create(document, Plane.CreateByOriginAndBasis(xyz8, XYZ.BasisX, XYZ.BasisY));
																ModelLine modelLine2 = document.Create.NewModelCurve(line2, sketchPlane2) as ModelLine;
																transaction4.Commit();
															}
															string text17 = Convert.ToString(Convert.ToString(familyInstance.get_Parameter((BuiltInParameter)(-1002050)).AsValueString()));
															string text18 = text17.Replace(" ", "_");
															string str3 = text18.Replace("\"", "\\\"");
															string value8 = str3 + "_Anim[" + familyInstance.Id.ToString() + "]";
															stringBuilder.Append(value8);
															stringBuilder.Append(value);
														}
													}
												}
											}
										}
									}
									transaction2.Commit();
									document2.Close(false);
								}
							}
						}
					}
					string value9 = arg + stringBuilder;
					using (StreamWriter streamWriter = new StreamWriter(path, true))
					{
						streamWriter.WriteLine(value9);
					}
				}
				Transaction transaction5 = new Transaction(document);
				transaction5.Start("WriteToText");
				document.Regenerate();
				transaction5.Commit();
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000503C File Offset: 0x0000323C
		private void listBoxParameters_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.FamilyParameter = this.listBoxParameters.SelectedItem.ToString();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005058 File Offset: 0x00003258
		private static double DegreeToRadian(double angle)
		{
			return 3.1415926535897931 * angle / 180.0;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005080 File Offset: 0x00003280
		private static double RadianToDegree(double angle)
		{
			return angle * 57.295779513082323;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000050A0 File Offset: 0x000032A0
		private double ConvertUnits(double u)
		{
			double value = 0.0;
			bool flag = this.UniteSuffixe == " mm";
			if (flag)
			{
				value = u * 0.3048 * 1000.0;
			}
			bool flag2 = this.UniteSuffixe == " m";
			if (flag2)
			{
				value = u * 0.3048;
			}
			bool flag3 = this.UniteSuffixe == " ft";
			if (flag3)
			{
				value = u;
			}
			bool flag4 = this.UniteSuffixe == " in";
			if (flag4)
			{
				value = u * 12.0;
			}
			return Math.Round(value, 2);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005158 File Offset: 0x00003358
		private void buttonParamList_Click(object sender, EventArgs e)
		{
			Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
			Document document = this.p_commandData.Application.ActiveUIDocument.Document;
			FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
			ICollection<Element> collection = filteredElementCollector.OfClass(typeof(FamilyInstance)).ToElements();
			List<string> list = new List<string>();
			int num = 1;
			bool flag = this.listBoxParameters != null;
			if (flag)
			{
				this.listBoxParameters.Items.Clear();
				num = 1;
			}
			foreach (Element element in collection)
			{
				FamilyInstance familyInstance = element as FamilyInstance;
				bool flag2 = familyInstance != null;
				if (flag2)
				{
					bool flag3 = string.Concat(new string[]
					{
						familyInstance.Symbol.Category.Name,
						" : ",
						familyInstance.Symbol.Family.Name,
						" : ",
						familyInstance.Symbol.Name
					}) == this.FamilySymbolNames;
					if (flag3)
					{
						bool flag4 = num < 2;
						if (flag4)
						{
							foreach (object obj in familyInstance.Parameters)
							{
								Parameter parameter = (Parameter)obj;
								bool flag5 = parameter.Definition.ParameterType == ParameterType.Angle;
								if (flag5)
								{
									StringBuilder stringBuilder = new StringBuilder();
									stringBuilder.Append(parameter.Definition.Name);
									bool flag6 = !list.Contains(stringBuilder.ToString());
									if (flag6)
									{
										list.Add(stringBuilder.ToString());
									}
								}
							}
							list.Sort();
							ListBox.ObjectCollection items = this.listBoxParameters.Items;
							object[] items2 = list.ToArray();
							items.AddRange(items2);
							this.listBoxParameters.SelectionMode = SelectionMode.One;
							num++;
						}
					}
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000053AC File Offset: 0x000035AC
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
			try
			{
				Autodesk.Revit.ApplicationServices.Application application = this.p_commandData.Application.Application;
				Document document = this.p_commandData.Application.ActiveUIDocument.Document;
				UIDocument activeUIDocument = this.p_commandData.Application.ActiveUIDocument;
				UIApplication application2 = this.p_commandData.Application;
				FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
				ICollection<Element> collection = filteredElementCollector.OfClass(typeof(View3D)).ToElements();
				View3D view3D = null;
				bool flag5 = document.ActiveView.ViewType == ViewType.ThreeD & !document.ActiveView.IsTemplate;
				if (flag5)
				{
					view3D = (document.ActiveView as View3D);
				}
				FilteredElementCollector filteredElementCollector2 = new FilteredElementCollector(document, view3D.Id);
				ICollection<Element> collection2 = filteredElementCollector2.OfClass(typeof(FamilyInstance)).ToElements();
				List<string> list = new List<string>();
				foreach (Element element in collection2)
				{
					FamilyInstance familyInstance = (FamilyInstance)element;
					FamilySymbol symbol = familyInstance.Symbol;
					bool flag6 = symbol != null;
					if (flag6)
					{
						bool flag7 = !list.Contains(string.Concat(new string[]
						{
							symbol.Category.Name,
							" : ",
							symbol.Family.Name,
							" : ",
							symbol.Name
						}));
						if (flag7)
						{
							list.Add(string.Concat(new string[]
							{
								symbol.Category.Name,
								" : ",
								symbol.Family.Name,
								" : ",
								symbol.Name
							}));
						}
					}
				}
				list.Sort();
				this.listBoxComponents.DataSource = list;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000056A4 File Offset: 0x000038A4
		private void listBoxComponents_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.FamilySymbolNames = this.listBoxComponents.SelectedItem.ToString();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000056C0 File Offset: 0x000038C0
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

		private ExternalCommandData p_commandData;

		private AllViews m_AllViews;

		private const double MM = 0.0032808;

		private const double Metre = 3.2808398;

		private const double Inches = 0.0833333;

		private const double I = 1.0;

		private double unite = 1.0;

		private string UniteSuffixe = " ft";

		private string AreaUniteSuffixe = " ft2";

		private const double METERS_IN_FEET = 0.3048;

		private string FamilySymbolNames;

		private string FamilyParameter;

		private double originAngle = 0.0;

		private double ProjY = 1.0;

		private double FamY = 1.0;

		private List<ElementId> ViewFamInstids = new List<ElementId>();

		private int rnd = 4;

		private const double _eps = 1E-09;

		private const double _feet_to_mm = 304.79999999999995;

		private string m = "m";

		private string mm = "mm";

		private string ft = "ft";

		private string inches = "in";

		private string units = null;
	}
}
