using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;

namespace ExportToVR
{
	internal class ExportToVRContext : IExportContext
	{
		public ExportToVRContext(Document document, AllViews families)
		{
			this.m_document = document;
			this.m_TransformationStack.Push(Transform.Identity);
			this.m_AllViews = families;
		}

		public bool Start()
		{
			return true;
		}

		public void Finish()
		{
		}

		public bool IsCanceled()
		{
			// 字段m_cancelled声明时，初始值为false
			return this.m_cancelled;
		}

		private ElementId CurrentElementId
		{
			get
			{
				return (this.elementStack.Count > 0) ? this.elementStack.Peek() : ElementId.InvalidElementId;
			}
		}

		private Element CurrentElement
		{
			get
			{
				return this.m_document.GetElement(this.CurrentElementId);
			}
		}

		public string GetCurrentElementName()
		{
			Element currentElement = this.CurrentElement;
			bool flag = currentElement != null;
			string result;
			if (flag)
			{
				result = currentElement.Name;
			}
			else
			{
				result = "";
			}
			return result;
		}

		public bool VUsol()
		{
			bool result = false;
			Reference reference = new Reference(this.CurrentElement);
			Element element = this.m_document.GetElement(reference);
			bool flag = element.Category.Id.IntegerValue == -2000032 | element.Category.Id.IntegerValue == -2000120 | element.Category.Id.IntegerValue == -2001340;
			if (flag)
			{
				result = true;
			}
			return result;
		}

		public bool CatIsFurnitureAndMore()
		{
			bool result = false;
			bool flag = this.CurrentElement != null;
			if (flag)
			{
				Reference reference = new Reference(this.CurrentElement);
				bool flag2 = reference != null;
				if (flag2)
				{
					Element element = this.m_document.GetElement(reference);
					bool isForVU = this.IsForVU;
					if (isForVU)
					{
						bool flag3 = element.Category.Id.IntegerValue == -2000080 | element.Category.Id.IntegerValue == -2001100 | element.Category.Id.IntegerValue == -2001000 | element.Category.Id.IntegerValue == -2001370;
						if (flag3)
						{
							result = true;
						}
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		public string CurrentGraphicStyleCat()
		{
			string result = "";
			Reference reference = new Reference(this.CurrentElement);
			Element element = this.m_document.GetElement(reference);
			bool flag = element != null;
			if (flag)
			{
				GeometryElement geometryElement = element.get_Geometry(new Options
				{
					ComputeReferences = true,
					View = this.m_document.ActiveView
				});
				foreach (GeometryObject geometryObject in geometryElement)
				{
					bool flag2 = geometryObject is GeometryInstance;
					if (flag2)
					{
						GeometryInstance geometryInstance = geometryObject as GeometryInstance;
						GeometryElement symbolGeometry = geometryInstance.GetSymbolGeometry();
						foreach (GeometryObject geometryObject2 in symbolGeometry)
						{
							bool flag3 = geometryObject2 is Solid;
							if (flag3)
							{
								Solid solid = geometryObject2 as Solid;
								bool flag4 = null != solid;
								if (flag4)
								{
									ElementId graphicsStyleId = solid.GraphicsStyleId;
									GraphicsStyle graphicsStyle = this.m_document.GetElement(graphicsStyleId) as GraphicsStyle;
									bool flag5 = graphicsStyle != null;
									if (flag5)
									{
										result = graphicsStyle.Name;
										break;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		private static double DegreeToRadian(double angle)
		{
			return 3.1415926535897931 * angle / 180.0;
		}

		private static double RadianToDegree(double angle)
		{
			return angle * 57.295779513082323;
		}

		public void OnPolymesh(PolymeshTopology node)
		{
			Transform transform = this.m_TransformationStack.Peek();
			string text = "Anim";
			double num = 270.0;
			double num2 = ExportToVRContext.DegreeToRadian(num);
			Transform transform2 = Transform.CreateRotation(XYZ.BasisX, num2);
			XYZ xyz = new XYZ(0.0, 0.0, 0.0);
			Transform transform3 = Transform.CreateRotationAtPoint(XYZ.BasisX, num2, xyz);
			this.nbofPoints = 0;
			this.nboffacets = 0;
			int numberOfFacets = node.NumberOfFacets;
			int numberOfPoints = node.NumberOfPoints;
			int numberOfUVs = node.NumberOfUVs;
			int numberOfNormals = node.NumberOfNormals;
			this.nbofPoints = numberOfPoints;
			this.nboffacets = numberOfFacets;
			this.TotalFacNB = numberOfFacets;
			bool flag = this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
			if (flag)
			{
				this.m_AllViews.TotalNbofPoints = this.m_AllViews.TotalNbofPoints + numberOfPoints;
			}
			bool flag2 = false;
			bool maxVerticesPerObj = this.m_AllViews.MaxVerticesPerObj;
			if (maxVerticesPerObj)
			{
				bool flag3 = this.m_AllViews.ExportOrder == 1;
				if (flag3)
				{
					bool flag4 = this.m_AllViews.IDListName01.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag4)
					{
						flag2 = true;
					}
				}
				bool flag5 = this.m_AllViews.ExportOrder == 2;
				if (flag5)
				{
					bool flag6 = this.m_AllViews.IDListName02.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag6)
					{
						flag2 = true;
					}
				}
				bool flag7 = this.m_AllViews.ExportOrder == 3;
				if (flag7)
				{
					bool flag8 = this.m_AllViews.IDListName03.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag8)
					{
						flag2 = true;
					}
				}
				bool flag9 = this.m_AllViews.ExportOrder == 4;
				if (flag9)
				{
					bool flag10 = this.m_AllViews.IDListName04.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag10)
					{
						flag2 = true;
					}
				}
				bool flag11 = this.m_AllViews.ExportOrder == 5;
				if (flag11)
				{
					bool flag12 = this.m_AllViews.IDListName05.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag12)
					{
						flag2 = true;
					}
				}
				bool flag13 = this.m_AllViews.ExportOrder == 6;
				if (flag13)
				{
					bool flag14 = this.m_AllViews.IDListName06.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag14)
					{
						flag2 = true;
					}
				}
				bool flag15 = this.m_AllViews.ExportOrder == 7;
				if (flag15)
				{
					bool flag16 = this.m_AllViews.IDListName07.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag16)
					{
						flag2 = true;
					}
				}
				bool flag17 = this.m_AllViews.ExportOrder == 8;
				if (flag17)
				{
					bool flag18 = this.m_AllViews.IDListName08.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag18)
					{
						flag2 = true;
					}
				}
				bool flag19 = this.m_AllViews.ExportOrder == 9;
				if (flag19)
				{
					bool flag20 = this.m_AllViews.IDListName09.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag20)
					{
						flag2 = true;
					}
				}
				bool flag21 = this.m_AllViews.ExportOrder == 10;
				if (flag21)
				{
					bool flag22 = this.m_AllViews.IDListName10.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag22)
					{
						flag2 = true;
					}
				}
			}
			bool flag23 = !this.m_AllViews.MaxVerticesPerObj;
			if (flag23)
			{
				flag2 = true;
			}
			bool flag24 = flag2;
			if (flag24)
			{
				bool flag25 = node.DistributionOfNormals == 0;
				if (flag25)
				{
					for (int i = 0; i < node.GetPoints().Count; i++)
					{
						XYZ point = node.GetPoint(i);
						XYZ xyz2 = transform.OfPoint(point);
						xyz2 = transform2.OfPoint(xyz2);
						string text2 = xyz2.X.ToString();
						string text3 = text2.Replace(",", ".");
						string text4 = xyz2.Y.ToString();
						string text5 = text4.Replace(",", ".");
						string text6 = xyz2.Z.ToString();
						string text7 = text6.Replace(",", ".");
						bool flag26 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
						if (flag26)
						{
							this.m_AllViews.XYZsbuilder.Append(string.Concat(new string[]
							{
								"\nv ",
								text3,
								" ",
								text5,
								" ",
								text7
							}));
						}
					}
					for (int j = 0; j < node.GetNormals().Count; j++)
					{
						XYZ normal = node.GetNormal(j);
						XYZ xyz3 = transform.OfPoint(normal);
						xyz3 = transform2.OfPoint(xyz3);
						string text8 = xyz3.X.ToString();
						string text9 = text8.Replace(",", ".");
						string text10 = xyz3.Y.ToString();
						string text11 = text10.Replace(",", ".");
						string text12 = xyz3.Z.ToString();
						string text13 = text12.Replace(",", ".");
						bool flag27 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
						if (flag27)
						{
							this.m_AllViews.NORMALsbuilder.Append(string.Concat(new string[]
							{
								"\nvn ",
								text9,
								" ",
								text11,
								" ",
								text13
							}));
						}
					}
				}
				else
				{
					bool flag28 = node.DistributionOfNormals == DistributionOfNormals.OnePerFace;
					if (flag28)
					{
						for (int k = 0; k < node.GetPoints().Count; k++)
						{
							XYZ point2 = node.GetPoint(k);
							XYZ xyz4 = transform.OfPoint(point2);
							xyz4 = transform2.OfPoint(xyz4);
							string text14 = xyz4.X.ToString();
							string text15 = text14.Replace(",", ".");
							string text16 = xyz4.Y.ToString();
							string text17 = text16.Replace(",", ".");
							string text18 = xyz4.Z.ToString();
							string text19 = text18.Replace(",", ".");
							bool flag29 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
							if (flag29)
							{
								this.m_AllViews.XYZsbuilder.Append(string.Concat(new string[]
								{
									"\nv ",
									text15,
									" ",
									text17,
									" ",
									text19
								}));
							}
						}
						XYZ xyz5 = transform.OfPoint(node.GetNormal(0));
						xyz5 = transform2.OfPoint(xyz5);
						string text20 = xyz5.X.ToString();
						string text21 = text20.Replace(",", ".");
						string text22 = xyz5.Y.ToString();
						string text23 = text22.Replace(",", ".");
						string text24 = xyz5.Z.ToString();
						string text25 = text24.Replace(",", ".");
						bool flag30 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
						if (flag30)
						{
							this.m_AllViews.NORMALsbuilder.Append(string.Concat(new string[]
							{
								"\nvn ",
								text21,
								" ",
								text23,
								" ",
								text25
							}));
						}
					}
					else
					{
						for (int l = 0; l < node.GetPoints().Count; l++)
						{
							XYZ point3 = node.GetPoint(l);
							XYZ xyz6 = transform.OfPoint(point3);
							xyz6 = transform2.OfPoint(xyz6);
							string text26 = xyz6.X.ToString();
							string text27 = text26.Replace(",", ".");
							string text28 = xyz6.Y.ToString();
							string text29 = text28.Replace(",", ".");
							string text30 = xyz6.Z.ToString();
							string text31 = text30.Replace(",", ".");
							bool flag31 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
							if (flag31)
							{
								this.m_AllViews.XYZsbuilder.Append(string.Concat(new string[]
								{
									"\nv ",
									text27,
									" ",
									text29,
									" ",
									text31
								}));
							}
						}
					}
				}
				List<string> list = new List<string>();
				bool flag32 = node.DistributionOfNormals == DistributionOfNormals.OnEachFacet;
				
				if (flag32)
				{
					this.ElementName = null;
					this.ElementName = this.GetCurrentElementName();
					string elementName = this.ElementName;
					string text32 = elementName.Replace(" ", "_");
					bool flag33 = this.m_AllViews.GroupingOptions == 6;
					if (flag33)
					{
						this.m_AllViews.FCTbyMATsbuilder.Append("\ng " + this.MaterialFaceName + "\nusemtl " + this.MaterialFaceName);
					}
					bool flag34 = this.m_AllViews.GroupingOptions == 3;
					if (flag34)
					{
						this.m_AllViews.FCTsbuilder.Append("\nusemtl " + this.MaterialFaceName);
					}
					bool flag35 = (this.m_AllViews.GroupingOptions == 4 & !this.m_AllViews.ExportSubCategories) | (this.m_AllViews.GroupingOptions == 5 & !this.m_AllViews.ExportSubCategories & (this.CatIsFurnitureAndMore() || this.m_AllViews.StandAloneVersion));
					if (flag35)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text32,
							"_[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					bool flag36 = (this.m_AllViews.GroupingOptions == 4 & !this.m_AllViews.ExportSubCategories) | (this.m_AllViews.GroupingOptions == 5 & !this.m_AllViews.ExportSubCategories & this.CatIsFurnitureAndMore() & !this.m_AllViews.StandAloneVersion);
					if (flag36)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text32,
							"_[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					bool flag37 = (this.m_AllViews.GroupingOptions == 4 & !this.m_AllViews.ExportSubCategories) | (this.m_AllViews.GroupingOptions == 5 & !this.m_AllViews.ExportSubCategories & !this.CatIsFurnitureAndMore() & !this.m_AllViews.StandAloneVersion);
					if (flag37)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text32,
							"_[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					bool flag38 = this.m_AllViews.GroupingOptions == 4 & this.m_AllViews.ExportSubCategories;
					if (flag38)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text32,
							"_",
							text,
							"[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					for (int m = 0; m < node.GetFacets().Count; m++)
					{
						PolymeshFacet facet = node.GetFacet(m);
						string text33 = (this.facNB + 1 + facet.V1).ToString();
						string text34 = (this.facNB + 1 + facet.V2).ToString();
						string text35 = (this.facNB + 1 + facet.V3).ToString();
						bool flag39 = this.m_AllViews.GroupingOptions == 6;
						if (flag39)
						{
							this.m_AllViews.FCTbyMATsbuilder.Append(string.Concat(new string[]
							{
								"\nf ",
								text33,
								"/",
								text33,
								" ",
								text34,
								"/",
								text34,
								" ",
								text35,
								"/",
								text35
							}));
						}
						bool flag40 = this.m_AllViews.GroupingOptions == 3;
						if (flag40)
						{
							text33 = (this.facNB + 1 + facet.V1).ToString();
							text34 = (this.facNB + 1 + facet.V2).ToString();
							text35 = (this.facNB + 1 + facet.V3).ToString();
							this.m_AllViews.FCTsbuilder.Append(string.Concat(new string[]
							{
								"\nf ",
								text33,
								"/",
								text33,
								" ",
								text34,
								"/",
								text34,
								" ",
								text35,
								"/",
								text35
							}));
						}
						bool flag41 = this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5;
						if (flag41)
						{
							text33 = (this.facNB + 1 + facet.V1).ToString();
							text34 = (this.facNB + 1 + facet.V2).ToString();
							text35 = (this.facNB + 1 + facet.V3).ToString();
							this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
							{
								"\nf ",
								text33,
								"/",
								text33,
								" ",
								text34,
								"/",
								text34,
								" ",
								text35,
								"/",
								text35
							}));
						}
					}
					bool flag42 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
					if (flag42)
					{
						this.m_AllViews.TotalfacNB = this.m_AllViews.TotalfacNB + this.m_AllViews.TotalNbofPoints;
					}
					this.facNB += this.nbofPoints;
					for (int n = 0; n < node.GetNormals().Count; n++)
					{
						XYZ xyz7 = transform.OfPoint(node.GetNormal(n));
						string text36 = xyz7.X.ToString();
						string text37 = text36.Replace(",", ".");
						string text38 = xyz7.Y.ToString();
						string text39 = text38.Replace(",", ".");
						string text40 = xyz7.Z.ToString();
						string text41 = text40.Replace(",", ".");
						bool flag43 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
						if (flag43)
						{
							this.m_AllViews.NORMALsbuilder.Append(string.Concat(new string[]
							{
								"\nvn ",
								text37,
								" ",
								text39,
								" ",
								text41
							}));
						}
					}
				}
				else
				{
					this.ElementName = null;
					this.ElementName = this.GetCurrentElementName();
					string elementName2 = this.ElementName;
					string text42 = elementName2.Replace(" ", "_");
					bool flag44 = this.m_AllViews.GroupingOptions == 6;
					if (flag44)
					{
						this.m_AllViews.FCTbyMATsbuilder.Append("\ng " + this.MaterialFaceName + "\nusemtl " + this.MaterialFaceName);
					}
					bool flag45 = this.m_AllViews.GroupingOptions == 3;
					if (flag45)
					{
						this.m_AllViews.FCTsbuilder.Append("\nusemtl " + this.MaterialFaceName);
					}
					bool flag46 = (this.m_AllViews.GroupingOptions == 4 & !this.m_AllViews.ExportSubCategories) | (this.m_AllViews.GroupingOptions == 5 & !this.m_AllViews.ExportSubCategories & (this.CatIsFurnitureAndMore() || this.m_AllViews.StandAloneVersion));
					if (flag46)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text42,
							"_[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					bool flag47 = (this.m_AllViews.GroupingOptions == 4 & !this.m_AllViews.ExportSubCategories) | (this.m_AllViews.GroupingOptions == 5 & !this.m_AllViews.ExportSubCategories & this.CatIsFurnitureAndMore() & !this.m_AllViews.StandAloneVersion);
					if (flag47)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text42,
							"_[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					bool flag48 = (this.m_AllViews.GroupingOptions == 4 & !this.m_AllViews.ExportSubCategories) | (this.m_AllViews.GroupingOptions == 5 & !this.m_AllViews.ExportSubCategories & !this.CatIsFurnitureAndMore() & !this.m_AllViews.StandAloneVersion);
					if (flag48)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text42,
							"_[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					bool flag49 = this.m_AllViews.GroupingOptions == 4 & this.m_AllViews.ExportSubCategories;
					if (flag49)
					{
						this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
						{
							"\ng ",
							text42,
							"_",
							text,
							"[",
							this.CurrentElementId.ToString(),
							"]\nusemtl ",
							this.MaterialFaceName
						}));
					}
					for (int num3 = 0; num3 < node.GetFacets().Count; num3++)
					{
						PolymeshFacet facet2 = node.GetFacet(num3);
						string text43 = (this.facNB + 1 + facet2.V1).ToString();
						string text44 = (this.facNB + 1 + facet2.V2).ToString();
						string text45 = (this.facNB + 1 + facet2.V3).ToString();
						bool flag50 = this.m_AllViews.GroupingOptions == 6;
						if (flag50)
						{
							this.m_AllViews.FCTbyMATsbuilder.Append(string.Concat(new string[]
							{
								"\nf ",
								text43,
								"/",
								text43,
								" ",
								text44,
								"/",
								text44,
								" ",
								text45,
								"/",
								text45
							}));
						}
						bool flag51 = this.m_AllViews.GroupingOptions == 3;
						if (flag51)
						{
							text43 = (this.facNB + 1 + facet2.V1).ToString();
							text44 = (this.facNB + 1 + facet2.V2).ToString();
							text45 = (this.facNB + 1 + facet2.V3).ToString();
							this.m_AllViews.FCTsbuilder.Append(string.Concat(new string[]
							{
								"\nf ",
								text43,
								"/",
								text43,
								" ",
								text44,
								"/",
								text44,
								" ",
								text45,
								"/",
								text45
							}));
						}
						bool flag52 = this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5;
						if (flag52)
						{
							text43 = (this.facNB + 1 + facet2.V1).ToString();
							text44 = (this.facNB + 1 + facet2.V2).ToString();
							text45 = (this.facNB + 1 + facet2.V3).ToString();
							this.m_AllViews.FCTbySUBCATsbuilder.Append(string.Concat(new string[]
							{
								"\nf ",
								text43,
								"/",
								text43,
								" ",
								text44,
								"/",
								text44,
								" ",
								text45,
								"/",
								text45
							}));
						}
					}
					bool flag53 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
					if (flag53)
					{
						this.m_AllViews.TotalfacNB = this.m_AllViews.TotalfacNB + this.m_AllViews.TotalNbofPoints;
					}
					this.facNB += this.nbofPoints;
				}
				bool flag54 = node.NumberOfUVs > 0;
				if (flag54)
				{
					double num4 = 1.0;
					double num5 = 1.0;
					bool flag55 = this.key_Materials != null;
					if (flag55)
					{
						bool flag56 = this.key_Materials.Count > 0;
						if (flag56)
						{
							foreach (object obj in this.key_Materials)
							{
								int num6 = (int)obj;
								string b = Convert.ToString(this.h_MaterialNames[num6]);
								bool flag57 = this.MaterialFaceName == b;
								if (flag57)
								{
									num4 = Convert.ToDouble(this.h_modfU[num6]);
									num5 = Convert.ToDouble(this.h_modfV[num6]);
								}
							}
						}
					}
					for (int num7 = 0; num7 < node.GetUVs().Count; num7++)
					{
						UV uv = node.GetUV(num7);
						double num8 = uv.U / num4;
						double num9 = uv.V / num5;
						string text46 = num8.ToString();
						string str = text46.Replace(",", ".");
						string text47 = num9.ToString();
						string str2 = text47.Replace(",", ".");
						bool flag58 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
						if (flag58)
						{
							this.m_AllViews.UVsbuilder.Append("\nvt " + str + " " + str2);
						}
					}
				}
			}
		}

		private void ExportMeshPoints(IList<XYZ> points, Transform trf, IList<XYZ> normals)
		{
			for (int i = 0; i < points.Count; i++)
			{
				XYZ xyz = points.ElementAt(i);
				XYZ xyz2 = trf.OfPoint(xyz);
				double x = xyz2.X;
				double y = xyz2.Y;
				double z = xyz2.Z;
				double num = Math.Round(x, this.arrond);
				double num2 = Math.Round(y, this.arrond);
				double num3 = Math.Round(z, this.arrond);
				this.sommets = string.Concat(new string[]
				{
					this.sommets,
					"\nv ",
					num.ToString(),
					" ",
					num2.ToString(),
					" ",
					num3.ToString()
				});
			}
			for (int j = 0; j < normals.Count; j++)
			{
				XYZ xyz3 = normals.ElementAt(j);
				XYZ xyz2 = trf.OfPoint(xyz3);
				double x2 = xyz2.X;
				double y2 = xyz2.Y;
				double z2 = xyz2.Z;
				double num4 = Math.Round(x2, this.arrond);
				double num5 = Math.Round(y2, this.arrond);
				double num6 = Math.Round(z2, this.arrond);
				this.nrmals = string.Concat(new string[]
				{
					this.nrmals,
					"\nvn ",
					num4.ToString(),
					" ",
					num5.ToString(),
					" ",
					num6.ToString()
				});
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0001E928 File Offset: 0x0001CB28
		private void ExportMeshPoints(IList<XYZ> points, Transform trf, XYZ normal)
		{
			for (int i = 0; i < points.Count; i++)
			{
				XYZ xyz = points.ElementAt(i);
				XYZ xyz2 = trf.OfPoint(xyz);
				double x = xyz2.X;
				double y = xyz2.Y;
				double z = xyz2.Z;
				double num = Math.Round(x, this.arrond);
				double num2 = Math.Round(y, this.arrond);
				double num3 = Math.Round(z, this.arrond);
				this.sommets = string.Concat(new string[]
				{
					this.sommets,
					"\nv ",
					num.ToString(),
					" ",
					num2.ToString(),
					" ",
					num3.ToString()
				});
			}
			double x2 = trf.OfPoint(normal).X;
			double y2 = trf.OfPoint(normal).Y;
			double z2 = trf.OfPoint(normal).Z;
			double num4 = Math.Round(x2, this.arrond);
			double num5 = Math.Round(y2, this.arrond);
			double num6 = Math.Round(z2, this.arrond);
			this.nrmals = string.Concat(new string[]
			{
				this.nrmals,
				"\nvn ",
				num4.ToString(),
				" ",
				num5.ToString(),
				" ",
				num6.ToString()
			});
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
		private void ExportMeshPoints(IList<XYZ> points, Transform trf)
		{
			for (int i = 0; i < points.Count; i++)
			{
				XYZ xyz = points.ElementAt(i);
				XYZ xyz2 = trf.OfPoint(xyz);
				double x = xyz2.X;
				double y = xyz2.Y;
				double z = xyz2.Z;
				double num = Math.Round(x, this.arrond);
				double num2 = Math.Round(y, this.arrond);
				double num3 = Math.Round(z, this.arrond);
				this.sommets = string.Concat(new string[]
				{
					this.sommets,
					"\nv ",
					num.ToString(),
					" ",
					num2.ToString(),
					" ",
					num3.ToString()
				});
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0001EB74 File Offset: 0x0001CD74
		private void ExportMeshFacets(IList<PolymeshFacet> facets, IList<XYZ> normals)
		{
			this.ElementName = null;
			this.ElementName = this.GetCurrentElementName();
			string elementName = this.ElementName;
			string text = elementName.Replace(" ", "_");
			List<string> list = new List<string>();
			bool flag = normals == null;
			if (flag)
			{
				bool flag2 = this.m_AllViews.GroupingOptions == 1;
				if (flag2)
				{
					bool flag3 = list.Contains(this.MaterialFaceName);
					if (flag3)
					{
						this.fctsByMaterials = this.fctsByMaterials + "\nusemtl " + this.MaterialFaceName;
					}
					bool flag4 = !list.Contains(this.MaterialFaceName);
					if (flag4)
					{
						list.Add(this.MaterialFaceName);
						this.fctsByMaterials = string.Concat(new string[]
						{
							this.fctsByMaterials,
							"\ng ",
							this.MaterialFaceName,
							"\nusemtl ",
							this.MaterialFaceName
						});
					}
				}
				bool flag5 = this.m_AllViews.GroupingOptions == 2;
				if (flag5)
				{
					bool flag6 = this.ListByElementName.Contains(text + "_" + this.CurrentElementId.ToString());
					if (flag6)
					{
						this.fctsByEntities = this.fctsByEntities + "\nusemtl " + this.MaterialFaceName;
					}
					bool flag7 = !this.ListByElementName.Contains(text + "_" + this.CurrentElementId.ToString());
					if (flag7)
					{
						this.ListByElementName.Add(text + "_" + this.CurrentElementId.ToString());
						this.fctsByEntities = string.Concat(new string[]
						{
							this.fctsByEntities,
							"\ng ",
							text,
							"_",
							this.CurrentElementId.ToString(),
							"\nusemtl ",
							this.MaterialFaceName
						});
					}
				}
				bool flag8 = this.m_AllViews.GroupingOptions == 3;
				if (flag8)
				{
					this.fcts = this.fcts + "\nusemtl " + this.MaterialFaceName;
				}
				for (int i = 0; i < facets.Count; i++)
				{
					PolymeshFacet polymeshFacet = facets.ElementAt(i);
					string text2 = (this.facNB + 1 + polymeshFacet.V1).ToString();
					string text3 = (this.facNB + 1 + polymeshFacet.V2).ToString();
					string text4 = (this.facNB + 1 + polymeshFacet.V3).ToString();
					bool flag9 = this.m_AllViews.GroupingOptions == 1;
					if (flag9)
					{
						this.fctsByMaterials = string.Concat(new string[]
						{
							this.fctsByMaterials,
							"\nf ",
							text2,
							"/",
							text2,
							" ",
							text3,
							"/",
							text3,
							" ",
							text4,
							"/",
							text4
						});
					}
					bool flag10 = this.m_AllViews.GroupingOptions == 2;
					if (flag10)
					{
						this.fctsByEntities = string.Concat(new string[]
						{
							this.fctsByEntities,
							"\nf ",
							text2,
							"/",
							text2,
							" ",
							text3,
							"/",
							text3,
							" ",
							text4,
							"/",
							text4
						});
					}
					bool flag11 = this.m_AllViews.GroupingOptions == 3;
					if (flag11)
					{
						this.fcts = string.Concat(new string[]
						{
							this.fcts,
							"\nf ",
							text2,
							"/",
							text2,
							" ",
							text3,
							"/",
							text3,
							" ",
							text4,
							"/",
							text4
						});
					}
				}
				this.facNB += this.nbofPoints;
			}
			else
			{
				bool flag12 = this.m_AllViews.GroupingOptions == 1;
				if (flag12)
				{
					bool flag13 = list.Contains(this.MaterialFaceName);
					if (flag13)
					{
						this.fctsByMaterials = this.fctsByMaterials + "\nusemtl " + this.MaterialFaceName;
					}
					bool flag14 = !list.Contains(this.MaterialFaceName);
					if (flag14)
					{
						list.Add(this.MaterialFaceName);
						this.fctsByMaterials = string.Concat(new string[]
						{
							this.fctsByMaterials,
							"\ng ",
							this.MaterialFaceName,
							"\nusemtl ",
							this.MaterialFaceName
						});
					}
				}
				bool flag15 = this.m_AllViews.GroupingOptions == 2;
				if (flag15)
				{
					bool flag16 = this.ListByElementName.Contains(text + "_" + this.CurrentElementId.ToString());
					if (flag16)
					{
						this.fctsByEntities = this.fctsByEntities + "\nusemtl " + this.MaterialFaceName;
					}
					bool flag17 = !this.ListByElementName.Contains(text + "_" + this.CurrentElementId.ToString());
					if (flag17)
					{
						this.ListByElementName.Add(text + "_" + this.CurrentElementId.ToString());
						this.fctsByEntities = string.Concat(new string[]
						{
							this.fctsByEntities,
							"\ng ",
							text,
							"_",
							this.CurrentElementId.ToString(),
							"\nusemtl ",
							this.MaterialFaceName
						});
					}
				}
				bool flag18 = this.m_AllViews.GroupingOptions == 3;
				if (flag18)
				{
					this.fcts = this.fcts + "\nusemtl " + this.MaterialFaceName;
				}
				for (int j = 0; j < facets.Count; j++)
				{
					PolymeshFacet polymeshFacet2 = facets.ElementAt(j);
					string text5 = (this.facNB + 1 + polymeshFacet2.V1).ToString();
					string text6 = (this.facNB + 1 + polymeshFacet2.V2).ToString();
					string text7 = (this.facNB + 1 + polymeshFacet2.V3).ToString();
					bool flag19 = this.m_AllViews.GroupingOptions == 1;
					if (flag19)
					{
						this.fctsByMaterials = string.Concat(new string[]
						{
							this.fctsByMaterials,
							"\nf ",
							text5,
							"/",
							text5,
							" ",
							text6,
							"/",
							text6,
							" ",
							text7,
							"/",
							text7
						});
					}
					bool flag20 = this.m_AllViews.GroupingOptions == 2;
					if (flag20)
					{
						this.fctsByEntities = string.Concat(new string[]
						{
							this.fctsByEntities,
							"\nf ",
							text5,
							"/",
							text5,
							" ",
							text6,
							"/",
							text6,
							" ",
							text7,
							"/",
							text7
						});
					}
					bool flag21 = this.m_AllViews.GroupingOptions == 3;
					if (flag21)
					{
						this.fcts = string.Concat(new string[]
						{
							this.fcts,
							"\nf ",
							text5,
							"/",
							text5,
							" ",
							text6,
							"/",
							text6,
							" ",
							text7,
							"/",
							text7
						});
					}
				}
				this.facNB += this.nbofPoints;
				for (int k = 0; k < normals.Count; k++)
				{
					XYZ xyz = normals.ElementAt(k);
					double x = xyz.X;
					double y = xyz.Y;
					double z = xyz.Z;
					double num = Math.Round(x, this.arrond);
					double num2 = Math.Round(y, this.arrond);
					double num3 = Math.Round(z, this.arrond);
					this.nrmals = string.Concat(new string[]
					{
						this.nrmals,
						"\nvn ",
						num.ToString(),
						" ",
						num2.ToString(),
						" ",
						num3.ToString()
					});
				}
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0001F468 File Offset: 0x0001D668
		private void ExportMeshUVs(IList<UV> UVs)
		{
			for (int i = 0; i < UVs.Count; i++)
			{
				UV uv = UVs.ElementAt(i);
				double num = 1.0;
				double num2 = 1.0;
				bool flag = this.key_Materials != null;
				if (flag)
				{
					bool flag2 = this.key_Materials.Count > 0;
					if (flag2)
					{
						foreach (object obj in this.key_Materials)
						{
							int num3 = (int)obj;
							string b = Convert.ToString(this.h_MaterialNames[num3]);
							bool flag3 = this.MaterialFaceName == b;
							if (flag3)
							{
								num = Convert.ToDouble(this.h_modfU[num3]);
								num2 = Convert.ToDouble(this.h_modfV[num3]);
							}
						}
					}
				}
				double value = uv.U / num;
				double value2 = uv.V / num2;
				double num4 = Math.Round(value, this.arrond);
				double num5 = Math.Round(value2, this.arrond);
				this.uvs = string.Concat(new string[]
				{
					this.uvs,
					"\nvt ",
					num4.ToString(),
					" ",
					num5.ToString()
				});
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
		public void OnRPC(RPCNode node)
		{
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0001F600 File Offset: 0x0001D800
		public RenderNodeAction OnViewBegin(ViewNode node)
		{
			return 0;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
		public void OnViewEnd(ElementId elementId)
		{
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0001F614 File Offset: 0x0001D814
		public RenderNodeAction OnElementBegin(ElementId elementId)
		{
			this.elementStack.Push(elementId);
			return 0;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0001F634 File Offset: 0x0001D834
		public void OnElementEnd(ElementId elementId)
		{
			this.elementStack.Pop();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0001F644 File Offset: 0x0001D844
		public RenderNodeAction OnFaceBegin(FaceNode node)
		{
			return 0;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
		public void OnFaceEnd(FaceNode node)
		{
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0001F658 File Offset: 0x0001D858
		public RenderNodeAction OnInstanceBegin(InstanceNode node)
		{
			this.m_TransformationStack.Push(this.m_TransformationStack.Peek().Multiply(node.GetTransform()));
			return 0;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0001F68D File Offset: 0x0001D88D
		public void OnInstanceEnd(InstanceNode node)
		{
			this.m_TransformationStack.Pop();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0001F69C File Offset: 0x0001D89C
		public RenderNodeAction OnLinkBegin(LinkNode node)
		{
			ElementId symbolId = node.GetSymbolId();
			RevitLinkType revitLinkType = this.m_document.GetElement(symbolId) as RevitLinkType;
			string name = revitLinkType.Name;
			foreach (object obj in this.m_document.Application.Documents)
			{
				Document document = (Document)obj;
				bool flag = document.Title.Equals(name);
				if (flag)
				{
					this.ZeLinkDoc = document;
				}
			}
			bool flag2 = name != null;
			if (flag2)
			{
				this.isLink = true;
			}
			this.m_TransformationStack.Push(this.m_TransformationStack.Peek().Multiply(node.GetTransform()));
			return 0;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0001F68D File Offset: 0x0001D88D
		public void OnLinkEnd(LinkNode node)
		{
			this.m_TransformationStack.Pop();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000D6F8 File Offset: 0x0000B8F8
		public void OnLight(LightNode node)
		{
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0001F77C File Offset: 0x0001D97C
		public void OnMaterial(MaterialNode node)
		{
			bool flag = false;
			bool flag2 = !this.m_AllViews.MaxVerticesPerObj;
			if (flag2)
			{
				flag = true;
			}
			bool maxVerticesPerObj = this.m_AllViews.MaxVerticesPerObj;
			if (maxVerticesPerObj)
			{
				bool flag3 = this.m_AllViews.ExportOrder == 1;
				if (flag3)
				{
					bool flag4 = this.m_AllViews.IDListName01.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag4)
					{
						flag = true;
					}
				}
				bool flag5 = this.m_AllViews.ExportOrder == 2;
				if (flag5)
				{
					bool flag6 = this.m_AllViews.IDListName02.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag6)
					{
						flag = true;
					}
				}
				bool flag7 = this.m_AllViews.ExportOrder == 3;
				if (flag7)
				{
					bool flag8 = this.m_AllViews.IDListName03.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag8)
					{
						flag = true;
					}
				}
				bool flag9 = this.m_AllViews.ExportOrder == 4;
				if (flag9)
				{
					bool flag10 = this.m_AllViews.IDListName04.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag10)
					{
						flag = true;
					}
				}
				bool flag11 = this.m_AllViews.ExportOrder == 5;
				if (flag11)
				{
					bool flag12 = this.m_AllViews.IDListName05.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag12)
					{
						flag = true;
					}
				}
				bool flag13 = this.m_AllViews.ExportOrder == 6;
				if (flag13)
				{
					bool flag14 = this.m_AllViews.IDListName06.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag14)
					{
						flag = true;
					}
				}
				bool flag15 = this.m_AllViews.ExportOrder == 7;
				if (flag15)
				{
					bool flag16 = this.m_AllViews.IDListName07.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag16)
					{
						flag = true;
					}
				}
				bool flag17 = this.m_AllViews.ExportOrder == 8;
				if (flag17)
				{
					bool flag18 = this.m_AllViews.IDListName08.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag18)
					{
						flag = true;
					}
				}
				bool flag19 = this.m_AllViews.ExportOrder == 9;
				if (flag19)
				{
					bool flag20 = this.m_AllViews.IDListName09.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag20)
					{
						flag = true;
					}
				}
				bool flag21 = this.m_AllViews.ExportOrder == 10;
				if (flag21)
				{
					bool flag22 = this.m_AllViews.IDListName10.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag22)
					{
						flag = true;
					}
				}
			}
			bool flag23 = this.isLink;
			if (flag23)
			{
				this.currentMaterialId = node.MaterialId;
				int num = 40;
				bool flag24 = node != null;
				if (flag24)
				{
					bool flag25 = node.MaterialId != null;
					if (flag25)
					{
						this.currentMaterialId = node.MaterialId;
						string text = this.currentMaterialId.ToString();
						string text2 = text;
						string text3 = text2.Replace(" ", "_");
						bool flag26 = text3.Length > num;
						if (flag26)
						{
							int num2 = text3.Length - num;
							text3 = text3.Remove(text3.Length - num2);
						}
						bool flag27 = text3 != null;
						if (flag27)
						{
							this.StartMatName = text3;
						}
						bool flag28 = this.m_AllViews.ListMaterialName.Contains(this.StartMatName);
						if (flag28)
						{
							this.MaterialFaceName = this.StartMatName;
						}
						bool flag29 = !this.m_AllViews.ListMaterialName.Contains(this.StartMatName);
						if (flag29)
						{
							string text4 = "0.5";
							string text5 = "0.5";
							string text6 = "0.5";
							double num3 = 1.0;
							bool flag30 = node.Color != null;
							if (flag30)
							{
								text4 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
								text5 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
								text6 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
							}
							bool flag31 = node.Transparency.ToString() != null;
							if (flag31)
							{
								num3 = 1.0 - Convert.ToDouble(node.Transparency);
							}
							bool linkTransparent = this.m_AllViews.LinkTransparent;
							if (linkTransparent)
							{
								num3 = 0.5;
							}
							string text7 = text4;
							string text8 = text7.Replace(",", ".");
							string text9 = text5;
							string text10 = text9.Replace(",", ".");
							string text11 = text6;
							string text12 = text11.Replace(",", ".");
							string text13 = num3.ToString();
							string text14 = text13.Replace(",", ".");
							this.MaterialFaceName = this.StartMatName;
							this.m_AllViews.MATERIALsbuilder.Append(string.Concat(new string[]
							{
								"newmtl ",
								this.StartMatName,
								"\nKa ",
								text8,
								" ",
								text10,
								" ",
								text12,
								"\nKd ",
								text8,
								" ",
								text10,
								" ",
								text12,
								"\nd ",
								text14,
								"\n"
							}));
							this.m_AllViews.ListMaterialName.Add(this.StartMatName);
						}
					}
				}
			}
			bool flag32 = flag & !this.isLink;
			if (flag32)
			{
				int num4 = 40;
				bool flag33 = node != null;
				if (flag33)
				{
					bool flag34 = node.MaterialId.IntegerValue.ToString() == "-1";
					if (flag34)
					{
						bool flag35 = this.CurrentElement.Category != null;
						if (flag35)
						{
							this.StartMatName = this.CurrentElement.Category.Name.ToString();
						}
						string startMatName = this.StartMatName;
						string text15 = startMatName.Replace(" ", "_");
						text15 = text15.Replace("\"", "");
						text15 = text15.Replace(":", "");
						text15 = text15.Replace(";", "");
						text15 = text15.Replace("'", "");
						this.StartMatName = text15;
						bool flag36 = text15.Length > num4;
						if (flag36)
						{
							int num5 = text15.Length - num4;
							this.StartMatName = text15.Remove(text15.Length - num5);
						}
						bool flag37 = this.m_AllViews.ListMaterialName != null;
						if (flag37)
						{
							bool flag38 = this.m_AllViews.ListMaterialName.Contains(this.StartMatName);
							if (flag38)
							{
								this.MaterialFaceName = this.StartMatName;
							}
						}
						bool flag39 = this.m_AllViews.ListMaterialName != null;
						if (flag39)
						{
							bool flag40 = !this.m_AllViews.ListMaterialName.Contains(this.StartMatName);
							if (flag40)
							{
								string text16 = "0.5";
								string text17 = "0.5";
								string text18 = "0.5";
								double num6 = 1.0;
								bool flag41 = node.Color != null;
								if (flag41)
								{
									text16 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
									text17 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
									text18 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
								}
								bool flag42 = node.Transparency.ToString() != null;
								if (flag42)
								{
									num6 = 1.0 - Convert.ToDouble(node.Transparency);
								}
								string text19 = text16;
								string text20 = text19.Replace(",", ".");
								string text21 = text17;
								string text22 = text21.Replace(",", ".");
								string text23 = text18;
								string text24 = text23.Replace(",", ".");
								string text25 = num6.ToString();
								string text26 = text25.Replace(",", ".");
								this.MaterialFaceName = text15;
								this.m_AllViews.MATERIALsbuilder.Append(string.Concat(new string[]
								{
									"newmtl ",
									text15,
									"\nKa ",
									text20,
									" ",
									text22,
									" ",
									text24,
									"\nKd ",
									text20,
									" ",
									text22,
									" ",
									text24,
									"\nd ",
									text26,
									"\n"
								}));
								this.m_AllViews.ListMaterialName.Add(text15);
							}
						}
					}
				}
				bool flag43 = node != null & node.MaterialId.IntegerValue.ToString() != "-1";
				if (flag43)
				{
					this.currentMaterialId = node.MaterialId;
					bool flag44 = this.currentMaterialId != ElementId.InvalidElementId & !this.currentMaterialId.IntegerValue.ToString().Contains("-");
					if (flag44)
					{
						Material material = this.m_document.GetElement(this.currentMaterialId) as Material;
						string name = material.Name;
						string text27 = name.Replace(" ", "_");
						bool flag45 = text27.Length > num4;
						if (flag45)
						{
							int num7 = text27.Length - num4;
							text27 = text27.Remove(text27.Length - num7);
						}
						bool flag46 = this.m_AllViews.GroupingOptions == 1 | this.m_AllViews.GroupingOptions == 2;
						if (flag46)
						{
							bool flag47 = this.ListMaterialName.Contains(text27);
							if (flag47)
							{
								this.MaterialFaceName = text27;
							}
							bool flag48 = !this.ListMaterialName.Contains(text27);
							if (flag48)
							{
								this.MaterialFaceName = text27;
								this.modfU = 1.0;
								this.modfV = 1.0;
								this.angle = 360.0;
								this.Otherpathvalue = null;
								ElementId appearanceAssetId = material.AppearanceAssetId;
								bool flag49 = appearanceAssetId.ToString() != "-1";
								if (flag49)
								{
									AppearanceAssetElement appearanceAssetElement = this.m_document.GetElement(appearanceAssetId) as AppearanceAssetElement;
									bool flag50 = false;
									string text28 = "Other";
									using (AppearanceAssetEditScope appearanceAssetEditScope = new AppearanceAssetEditScope(appearanceAssetElement.Document))
									{
										Asset asset = appearanceAssetEditScope.Start(appearanceAssetElement.Id);
										AssetProperty assetProperty = null;
										bool flag51 = assetProperty == null;
										if (flag51)
										{
											assetProperty = asset.FindByName(Generic.GenericDiffuse);
											bool flag52 = assetProperty != null;
											if (flag52)
											{
												AssetPropertyDoubleArray4d assetPropertyDoubleArray4d = asset.FindByName("common_Tint_color") as AssetPropertyDoubleArray4d;
											}
										}
										appearanceAssetEditScope.Commit(true);
									}
									using (AppearanceAssetEditScope appearanceAssetEditScope2 = new AppearanceAssetEditScope(appearanceAssetElement.Document))
									{
										Asset asset2 = appearanceAssetEditScope2.Start(appearanceAssetElement.Id);
										AssetProperty assetProperty2 = null;
										bool flag53 = assetProperty2 == null;
										if (flag53)
										{
											assetProperty2 = asset2.FindByName(Generic.GenericDiffuse);
											text28 = "GenericDiffuse";
											this.MaterialType = text28;
										}
										bool flag54 = assetProperty2 == null;
										if (flag54)
										{
											this.MaterialType = "Other";
											Asset renderingAsset = appearanceAssetElement.GetRenderingAsset();
											int size = renderingAsset.Size;
											for (int i = 0; i < size; i++)
											{
												AssetProperty assetProperty3 = renderingAsset.Get(i);
												bool flag55 = assetProperty3.NumberOfConnectedProperties < 1;
												if (!flag55)
												{
													bool flag56 = false;
													bool flag57 = assetProperty3.Name.Contains("bump_map") | assetProperty3.Name.Contains("pattern_map");
													if (flag57)
													{
														flag56 = true;
													}
													Asset asset3 = assetProperty3.GetConnectedProperty(0) as Asset;
													bool flag58 = asset3.Name == "UnifiedBitmapSchema" & !flag56;
													if (flag58)
													{
														AssetPropertyString assetPropertyString = asset3.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;
														bool flag59 = assetPropertyString != null;
														if (flag59)
														{
															bool flag60 = assetPropertyString.Value.ToString() != "";
															if (flag60)
															{
																this.Otherpathvalue = assetPropertyString.Value;
															}
														}
														AssetPropertyDistance assetPropertyDistance = asset3.FindByName(UnifiedBitmap.TextureRealWorldScaleX) as AssetPropertyDistance;
														bool flag61 = assetPropertyDistance != null;
														if (flag61)
														{
															this.modfU = UnitUtils.Convert(assetPropertyDistance.Value, assetPropertyDistance.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
														}
														AssetPropertyDistance assetPropertyDistance2 = asset3.FindByName(UnifiedBitmap.TextureRealWorldScaleY) as AssetPropertyDistance;
														bool flag62 = assetPropertyDistance2 != null;
														if (flag62)
														{
															this.modfV = UnitUtils.Convert(assetPropertyDistance2.Value, assetPropertyDistance2.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
														}
													}
												}
											}
										}
										bool flag63 = assetProperty2 != null & text28 == "GenericDiffuse";
										if (flag63)
										{
											Asset singleConnectedAsset = assetProperty2.GetSingleConnectedAsset();
											bool flag64 = singleConnectedAsset != null & text28 == "GenericDiffuse";
											if (flag64)
											{
												AssetPropertyString assetPropertyString2 = singleConnectedAsset.FindByName("unifiedbitmap_Bitmap") as AssetPropertyString;
												bool flag65 = assetPropertyString2 != null;
												if (flag65)
												{
													this.textureExist = true;
													flag50 = true;
												}
											}
											bool flag66 = singleConnectedAsset == null & text28 == "GenericDiffuse";
											if (flag66)
											{
												bool tintOrNot = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).TintOrNot;
												bool colorOrNot = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).ColorOrNot;
												string name2 = material.Name;
												string text29 = name2.Replace(" ", "_");
												bool flag67 = text29.Length > num4;
												if (flag67)
												{
													int num8 = text29.Length - num4;
													text29 = text29.Remove(text29.Length - num8);
												}
												bool flag68 = !this.ListMaterialName.Contains(text29);
												if (flag68)
												{
													this.ListMaterialName.Add(text29);
													string text30 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
													string text31 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
													string text32 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
													bool flag69 = colorOrNot;
													if (flag69)
													{
														text30 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Red) / 255.0).ToString();
														text31 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Green) / 255.0).ToString();
														text32 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Blue) / 255.0).ToString();
													}
													bool flag70 = tintOrNot;
													if (flag70)
													{
														text30 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Red) / 255.0).ToString();
														text31 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Green) / 255.0).ToString();
														text32 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Blue) / 255.0).ToString();
													}
													double num9 = 0.0;
													bool flag71 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).transparancy.ToString() != "0";
													if (flag71)
													{
														num9 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).transparancy;
													}
													double num10 = 1.0 - num9;
													double num11 = Math.Round(num10, 2);
													this.MATERIALsbuilder.Append(string.Concat(new object[]
													{
														"newmtl ",
														text29,
														"\nKa ",
														text30,
														" ",
														text31,
														" ",
														text32,
														"\nKd ",
														text30,
														" ",
														text31,
														" ",
														text32,
														"\nd ",
														num10,
														"\n"
													}));
												}
											}
											bool flag72 = singleConnectedAsset != null & text28 == "GenericDiffuse" & this.textureExist;
											if (flag72)
											{
												bool tintOrNot2 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).TintOrNot;
												bool colorOrNot2 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).ColorOrNot;
												string name3 = material.Name;
												string text33 = name3.Replace(" ", "_");
												bool flag73 = text33.Length > num4;
												if (flag73)
												{
													int num12 = text33.Length - num4;
													text33 = text33.Remove(text33.Length - num12);
												}
												bool flag74 = !this.ListMaterialName.Contains(text33);
												if (flag74)
												{
													this.ListMaterialName.Add(text33);
													AssetPropertyString assetPropertyString3 = singleConnectedAsset.FindByName("unifiedbitmap_Bitmap") as AssetPropertyString;
													string text34 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
													string text35 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
													string text36 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
													bool flag75 = colorOrNot2;
													if (flag75)
													{
														text34 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Red) / 255.0).ToString();
														text35 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Green) / 255.0).ToString();
														text36 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Blue) / 255.0).ToString();
													}
													bool flag76 = tintOrNot2;
													if (flag76)
													{
														text34 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Red) / 255.0).ToString();
														text35 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Green) / 255.0).ToString();
														text36 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Blue) / 255.0).ToString();
													}
													double value = 1.0 - this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).transparancy;
													double num13 = Math.Round(value, 2);
													bool flag77 = assetPropertyString3.Value != null & assetPropertyString3.Value.ToString() != "";
													if (flag77)
													{
														bool scaleOrNot = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).ScaleOrNot;
														if (scaleOrNot)
														{
															bool flag78 = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).sx.ToString() != null;
															if (flag78)
															{
																this.modfU = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).sx;
															}
															bool flag79 = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).sy.ToString() != null;
															if (flag79)
															{
																this.modfV = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).sy;
															}
															bool flag80 = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).angle.ToString() != null;
															if (flag80)
															{
																this.angle = this.getTextureYesImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).angle;
															}
														}
														this.newimagePath = assetPropertyString3.Value;
														bool flag81 = this.newimagePath.Contains("|");
														if (flag81)
														{
															this.newimagePath = this.newimagePath.Substring(0, this.newimagePath.IndexOf("|"));
														}
														bool flag82 = this.newimagePath.Contains("\\") & this.newimagePath.Contains("/");
														if (flag82)
														{
															this.newimagePath = this.newimagePath.Replace("/", "\\");
														}
														bool flag83 = this.newimagePath.Contains("\\\\");
														if (flag83)
														{
															this.newimagePath = this.newimagePath.Replace("\\\\", "\\");
														}
														bool flag84 = File.Exists(this.newimagePath);
														if (flag84)
														{
															this.h_Materials.Add(this.keyNB, this.newimagePath);
															this.key_Materials = this.h_Materials.Keys;
															this.h_MaterialNames.Add(this.keyNB, text33);
															this.h_modfU.Add(this.keyNB, this.modfU);
															this.h_modfV.Add(this.keyNB, this.modfV);
															this.keyNB++;
															string fileName = Path.GetFileName(this.newimagePath);
															string text37 = fileName.Replace(" ", "_");
															string text38 = text37;
															bool flag85 = assetPropertyString3.Value.ToString() != "";
															if (flag85)
															{
																this.MATERIALsbuilder.Append(string.Concat(new object[]
																{
																	"newmtl ",
																	text33,
																	"\nKa ",
																	text34,
																	" ",
																	text35,
																	" ",
																	text36,
																	"\nKd ",
																	text34,
																	" ",
																	text35,
																	" ",
																	text36,
																	"\nd ",
																	num13,
																	"\nmap_Ka ",
																	text38,
																	"\nmap_Kd ",
																	text38,
																	"\n"
																}));
															}
														}
														bool flag86 = !File.Exists(this.newimagePath);
														if (flag86)
														{
															bool flag87 = assetPropertyString3.Value.ToString() != "";
															if (flag87)
															{
																this.MATERIALsbuilder.Append(string.Concat(new object[]
																{
																	"newmtl ",
																	text33,
																	"\nKa ",
																	text34,
																	" ",
																	text35,
																	" ",
																	text36,
																	"\nKd ",
																	text34,
																	" ",
																	text35,
																	" ",
																	text36,
																	"\nd ",
																	num13,
																	"\n"
																}));
															}
														}
													}
													bool flag88 = assetPropertyString3.Value == null || assetPropertyString3.Value.ToString() == "";
													if (flag88)
													{
														bool flag89 = assetPropertyString3.Value.ToString() != "";
														if (flag89)
														{
															this.MATERIALsbuilder.Append(string.Concat(new object[]
															{
																"newmtl ",
																text33,
																"\nKa ",
																text34,
																" ",
																text35,
																" ",
																text36,
																"\nKd ",
																text34,
																" ",
																text35,
																" ",
																text36,
																"\nd ",
																num13,
																"\n"
															}));
														}
													}
												}
											}
											bool flag90 = text28 == "GenericDiffuse" & !flag50;
											if (flag90)
											{
												bool tintOrNot3 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).TintOrNot;
												bool colorOrNot3 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).ColorOrNot;
												string name4 = material.Name;
												string text39 = name4.Replace(" ", "_");
												bool flag91 = text39.Length > num4;
												if (flag91)
												{
													int num14 = text39.Length - num4;
													text39 = text39.Remove(text39.Length - num14);
												}
												bool flag92 = !this.ListMaterialName.Contains(text39);
												if (flag92)
												{
													this.ListMaterialName.Add(text39);
													string text40 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
													string text41 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
													string text42 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
													bool flag93 = colorOrNot3;
													if (flag93)
													{
														text40 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Red) / 255.0).ToString();
														text41 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Green) / 255.0).ToString();
														text42 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Blue) / 255.0).ToString();
													}
													bool flag94 = tintOrNot3;
													if (flag94)
													{
														text40 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Red) / 255.0).ToString();
														text41 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Green) / 255.0).ToString();
														text42 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Blue) / 255.0).ToString();
													}
													double num15 = 0.0;
													bool flag95 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).transparancy.ToString() != "0";
													if (flag95)
													{
														num15 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).transparancy;
													}
													double value2 = 1.0 - num15;
													double num16 = Math.Round(value2, 2);
													this.MATERIALsbuilder.Append(string.Concat(new object[]
													{
														"newmtl ",
														text39,
														"\nKa ",
														text40,
														" ",
														text41,
														" ",
														text42,
														"\nKd ",
														text40,
														" ",
														text41,
														" ",
														text42,
														"\nd ",
														num16,
														"\n"
													}));
												}
											}
										}
										bool flag96 = this.MaterialType == "Other";
										if (flag96)
										{
											bool tintOrNot4 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).TintOrNot;
											bool colorOrNot4 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).ColorOrNot;
											string name5 = material.Name;
											string text43 = name5.Replace(" ", "_");
											bool flag97 = text43.Length > num4;
											if (flag97)
											{
												int num17 = text43.Length - num4;
												text43 = text43.Remove(text43.Length - num17);
											}
											bool flag98 = !this.ListMaterialName.Contains(text43);
											if (flag98)
											{
												this.ListMaterialName.Add(text43);
												string text44 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
												string text45 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
												string text46 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
												double value3 = Convert.ToDouble(node.Transparency);
												double num18 = Math.Round(value3, 2);
												bool flag99 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).textureCategorie == "water";
												if (flag99)
												{
													num18 = 0.4;
												}
												bool flag100 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).textureCategorie == "plastic";
												if (flag100)
												{
													num18 = 0.5;
												}
												bool flag101 = this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).textureCategorie == "glazing";
												if (flag101)
												{
													num18 = 0.2;
												}
												bool flag102 = colorOrNot4;
												if (flag102)
												{
													text44 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Red) / 255.0).ToString();
													text45 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Green) / 255.0).ToString();
													text46 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Blue) / 255.0).ToString();
												}
												bool flag103 = tintOrNot4;
												if (flag103)
												{
													text44 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Red) / 255.0).ToString();
													text45 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Green) / 255.0).ToString();
													text46 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement, ExportToVRContext.TextureTypes.Diffuse).color.Blue) / 255.0).ToString();
												}
												bool flag104 = this.Otherpathvalue != null;
												if (flag104)
												{
													bool flag105 = this.Otherpathvalue.Contains("|");
													if (flag105)
													{
														this.Otherpathvalue = this.Otherpathvalue.Substring(0, this.Otherpathvalue.IndexOf("|"));
													}
													bool flag106 = this.Otherpathvalue.Contains("\\") & this.Otherpathvalue.Contains("/");
													if (flag106)
													{
														this.Otherpathvalue = this.Otherpathvalue.Replace("/", "\\");
													}
													bool flag107 = this.Otherpathvalue.Contains("\\\\");
													if (flag107)
													{
														this.Otherpathvalue = this.Otherpathvalue.Replace("\\\\", "\\");
													}
													bool flag108 = File.Exists(this.Otherpathvalue);
													if (flag108)
													{
														this.h_Materials.Add(this.keyNB, this.Otherpathvalue);
														this.key_Materials = this.h_Materials.Keys;
														this.h_MaterialNames.Add(this.keyNB, text43);
														this.h_modfU.Add(this.keyNB, this.modfU);
														this.h_modfV.Add(this.keyNB, this.modfV);
														this.keyNB++;
														this.textureExist = true;
														string fileName2 = Path.GetFileName(this.Otherpathvalue);
														string text47 = fileName2.Replace(" ", "_");
														string text48 = text47;
														this.MATERIALsbuilder.Append(string.Concat(new object[]
														{
															"newmtl ",
															text43,
															"\nKa ",
															text44,
															" ",
															text45,
															" ",
															text46,
															"\nKd ",
															text44,
															" ",
															text45,
															" ",
															text46,
															"\nd ",
															num18,
															"\nmap_Ka ",
															text48,
															"\nmap_Kd ",
															text48,
															"\n"
														}));
													}
													bool flag109 = !File.Exists(this.Otherpathvalue);
													if (flag109)
													{
														this.MATERIALsbuilder.Append(string.Concat(new object[]
														{
															"newmtl ",
															text43,
															"\nKa ",
															text44,
															" ",
															text45,
															" ",
															text46,
															"\nKd ",
															text44,
															" ",
															text45,
															" ",
															text46,
															"\nd ",
															num18,
															"\n"
														}));
													}
												}
												bool flag110 = this.Otherpathvalue == null;
												if (flag110)
												{
													this.MATERIALsbuilder.Append(string.Concat(new object[]
													{
														"newmtl ",
														text43,
														"\nKa ",
														text44,
														" ",
														text45,
														" ",
														text46,
														"\nKd ",
														text44,
														" ",
														text45,
														" ",
														text46,
														"\nd ",
														num18,
														"\n"
													}));
												}
											}
										}
										appearanceAssetEditScope2.Dispose();
									}
								}
								bool flag111 = appearanceAssetId.ToString() == "-1";
								if (flag111)
								{
									string name6 = material.Name;
									string text49 = name6.Replace(" ", "_");
									bool flag112 = text49.Length > num4;
									if (flag112)
									{
										int num19 = text49.Length - num4;
										text49 = text49.Remove(text49.Length - num19);
									}
									bool flag113 = !this.ListMaterialName.Contains(text49);
									if (flag113)
									{
										this.ListMaterialName.Add(text49);
										string text50 = "0.5";
										string text51 = "0.5";
										string text52 = "0.5";
										double num20 = 1.0;
										string value4 = string.Concat(new object[]
										{
											"newmtl ",
											text49,
											"\nKa ",
											text50,
											" ",
											text51,
											" ",
											text52,
											"\nKd ",
											text50,
											" ",
											text51,
											" ",
											text52,
											"\nd ",
											num20,
											"\n"
										});
										this.MATERIALsbuilder.Append(value4);
									}
								}
							}
						}
						bool flag114 = this.m_AllViews.GroupingOptions == 3 | this.m_AllViews.GroupingOptions == 4 | this.m_AllViews.GroupingOptions == 5 | this.m_AllViews.GroupingOptions == 6;
						if (flag114)
						{
							bool flag115 = this.m_AllViews.ListMaterialName.Contains(text27);
							if (flag115)
							{
								this.MaterialFaceName = text27;
							}
							bool flag116 = !this.m_AllViews.ListMaterialName.Contains(text27);
							if (flag116)
							{
								this.MaterialFaceName = text27;
								this.modfU = 1.0;
								this.modfV = 1.0;
								this.angle = 360.0;
								this.Otherpathvalue = null;
								ElementId appearanceAssetId2 = material.AppearanceAssetId;
								bool flag117 = appearanceAssetId2.ToString() != "-1";
								if (flag117)
								{
									this.m_AllViews.ImageExist = false;
									AppearanceAssetElement appearanceAssetElement2 = this.m_document.GetElement(appearanceAssetId2) as AppearanceAssetElement;
									this.MaterialType = "Other";
									bool flag118 = this.MaterialType == "Other";
									if (flag118)
									{
										try
										{
											this.GetTheBitmaps(appearanceAssetElement2);
										}
										catch (Exception ex)
										{
										}
										bool imageExist = this.m_AllViews.ImageExist;
										if (imageExist)
										{
											bool tintOrNot5 = this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).TintOrNot;
											bool colorOrNot5 = this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).ColorOrNot;
											string name7 = material.Name;
											string text53 = name7.Replace(" ", "_");
											bool flag119 = text53.Length > num4;
											if (flag119)
											{
												int num21 = text53.Length - num4;
												text53 = text53.Remove(text53.Length - num21);
											}
											bool flag120 = !this.m_AllViews.ListMaterialName.Contains(text53);
											if (flag120)
											{
												this.m_AllViews.ListMaterialName.Add(text53);
												string text54 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
												string text55 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
												string text56 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
												double value5 = 1.0 - Convert.ToDouble(node.Transparency);
												double num22 = Math.Round(value5, 2);
												bool flag121 = this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).textureCategorie == "water";
												if (flag121)
												{
													num22 = 0.4;
												}
												bool flag122 = this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).textureCategorie == "plastic";
												if (flag122)
												{
													num22 = 0.5;
												}
												bool flag123 = this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).textureCategorie == "glazing";
												if (flag123)
												{
													num22 = 0.2;
												}
												bool flag124 = colorOrNot5;
												if (flag124)
												{
													text54 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Red) / 255.0).ToString();
													text55 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Green) / 255.0).ToString();
													text56 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).GenericColor.Blue) / 255.0).ToString();
												}
												bool flag125 = tintOrNot5;
												if (flag125)
												{
													text54 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).color.Red) / 255.0).ToString();
													text55 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).color.Green) / 255.0).ToString();
													text56 = (Convert.ToDouble(this.getTextureNoImage(appearanceAssetElement2, ExportToVRContext.TextureTypes.Diffuse).color.Blue) / 255.0).ToString();
												}
												string text57 = text54;
												text54 = text57.Replace(",", ".");
												string text58 = text55;
												text55 = text58.Replace(",", ".");
												string text59 = text56;
												text56 = text59.Replace(",", ".");
												string text60 = num22.ToString();
												string text61 = text60.Replace(",", ".");
												bool flag126 = this.Otherpathvalue != null;
												if (flag126)
												{
													bool flag127 = this.Otherpathvalue.Contains("|");
													if (flag127)
													{
														this.Otherpathvalue = this.Otherpathvalue.Substring(0, this.Otherpathvalue.IndexOf("|"));
													}
													bool flag128 = this.Otherpathvalue.Contains("\\") & this.Otherpathvalue.Contains("/");
													if (flag128)
													{
														this.Otherpathvalue = this.Otherpathvalue.Replace("/", "\\");
													}
													bool flag129 = this.Otherpathvalue.Contains("\\\\");
													if (flag129)
													{
														this.Otherpathvalue = this.Otherpathvalue.Replace("\\\\", "\\");
													}
													bool flag130 = File.Exists(this.Otherpathvalue);
													if (flag130)
													{
														this.h_Materials.Add(this.keyNB, this.Otherpathvalue);
														this.key_Materials = this.h_Materials.Keys;
														this.h_MaterialNames.Add(this.keyNB, text53);
														this.h_modfU.Add(this.keyNB, this.modfU);
														this.h_modfV.Add(this.keyNB, this.modfV);
														this.keyNB++;
														this.textureExist = true;
														string fileName3 = Path.GetFileName(this.Otherpathvalue);
														string text62 = fileName3.Replace(" ", "_");
														string text63 = text62;
														this.m_AllViews.MATERIALsbuilder.Append(string.Concat(new string[]
														{
															"newmtl ",
															text53,
															"\nKa ",
															text54,
															" ",
															text55,
															" ",
															text56,
															"\nKd ",
															text54,
															" ",
															text55,
															" ",
															text56,
															"\nd ",
															text61,
															"\nmap_Ka ",
															text63,
															"\nmap_Kd ",
															text63,
															"\n"
														}));
													}
													bool flag131 = !File.Exists(this.Otherpathvalue);
													if (flag131)
													{
														this.m_AllViews.MATERIALsbuilder.Append(string.Concat(new string[]
														{
															"newmtl ",
															text53,
															"\nKa ",
															text54,
															" ",
															text55,
															" ",
															text56,
															"\nKd ",
															text54,
															" ",
															text55,
															" ",
															text56,
															"\nd ",
															text61,
															"\n"
														}));
													}
												}
												bool flag132 = this.Otherpathvalue == null;
												if (flag132)
												{
													this.m_AllViews.MATERIALsbuilder.Append(string.Concat(new string[]
													{
														"newmtl ",
														text53,
														"\nKa ",
														text54,
														" ",
														text55,
														" ",
														text56,
														"\nKd ",
														text54,
														" ",
														text55,
														" ",
														text56,
														"\nd ",
														text61,
														"\n"
													}));
												}
											}
										}
										bool flag133 = !this.m_AllViews.ImageExist;
										if (flag133)
										{
											string text64 = "0.5";
											string text65 = "0.5";
											string text66 = "0.5";
											double num23 = 1.0;
											bool flag134 = node.Color != null;
											if (flag134)
											{
												text64 = (Convert.ToDouble(node.Color.Red) / 255.0).ToString();
												text65 = (Convert.ToDouble(node.Color.Green) / 255.0).ToString();
												text66 = (Convert.ToDouble(node.Color.Blue) / 255.0).ToString();
											}
											bool flag135 = node.Transparency.ToString() != null;
											if (flag135)
											{
												num23 = 1.0 - Convert.ToDouble(node.Transparency);
											}
											string text67 = text64;
											string text68 = text67.Replace(",", ".");
											string text69 = text65;
											string text70 = text69.Replace(",", ".");
											string text71 = text66;
											string text72 = text71.Replace(",", ".");
											string text73 = num23.ToString();
											string text74 = text73.Replace(",", ".");
											this.MaterialFaceName = text27;
											this.m_AllViews.MATERIALsbuilder.Append(string.Concat(new string[]
											{
												"newmtl ",
												text27,
												"\nKa ",
												text68,
												" ",
												text70,
												" ",
												text72,
												"\nKd ",
												text68,
												" ",
												text70,
												" ",
												text72,
												"\nd ",
												text74,
												"\n"
											}));
											bool flag136 = !this.m_AllViews.ListMaterialName.Contains(text27);
											if (flag136)
											{
												this.m_AllViews.ListMaterialName.Add(text27);
											}
										}
									}
								}
								bool flag137 = appearanceAssetId2.ToString() == "-1";
								if (flag137)
								{
									string name8 = material.Name;
									string text75 = name8.Replace(" ", "_");
									bool flag138 = text75.Length > num4;
									if (flag138)
									{
										int num24 = text75.Length - num4;
										text75 = text75.Remove(text75.Length - num24);
									}
									bool flag139 = !this.m_AllViews.ListMaterialName.Contains(text75);
									if (flag139)
									{
										this.m_AllViews.ListMaterialName.Add(text75);
										string text76 = "0.5";
										string text77 = "0.5";
										string text78 = "0.5";
										double num25 = 1.0;
										string value6 = string.Concat(new object[]
										{
											"newmtl ",
											text75,
											"\nKa ",
											text76,
											" ",
											text77,
											" ",
											text78,
											"\nKd ",
											text76,
											" ",
											text77,
											" ",
											text78,
											"\nd ",
											num25,
											"\n"
										});
										this.m_AllViews.MATERIALsbuilder.Append(value6);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0002293C File Offset: 0x00020B3C
		private ExportToVRContext.TextureInfo getTextureNoImage(AppearanceAssetElement materialAsset, ExportToVRContext.TextureTypes tt)
		{
			ExportToVRContext.TextureInfo textureInfo = new ExportToVRContext.TextureInfo();
			bool flag = materialAsset != null;
			if (flag)
			{
				Asset renderingAsset = materialAsset.GetRenderingAsset();
				List<AssetProperty> list = new List<AssetProperty>();
				for (int i = 0; i < renderingAsset.Size; i++)
				{
					AssetProperty item = renderingAsset.Get(i);
					list.Add(item);
				}
				list = (from ap in list
				orderby ap.Name
				select ap).ToList<AssetProperty>();
				for (int j = 0; j < list.Count; j++)
				{
					AssetProperty assetProperty = list[j];
					bool flag2 = assetProperty.Name == "common_Tint_color";
					if (flag2)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d = assetProperty as AssetPropertyDoubleArray4d;
						bool flag3 = assetPropertyDoubleArray4d != null;
						if (flag3)
						{
							textureInfo.color = assetPropertyDoubleArray4d.GetValueAsColor();
						}
					}
					bool flag4 = assetProperty.Name == "common_Tint_toggle";
					if (flag4)
					{
						AssetPropertyBoolean assetPropertyBoolean = assetProperty as AssetPropertyBoolean;
						bool flag5 = assetPropertyBoolean != null;
						if (flag5)
						{
							textureInfo.TintOrNot = assetPropertyBoolean.Value;
						}
					}
					bool flag6 = assetProperty.Name == "ceramic_color";
					if (flag6)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d2 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag7 = assetPropertyDoubleArray4d2 != null;
						if (flag7)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d2.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag8 = assetProperty.Name == "concrete_color";
					if (flag8)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d3 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag9 = assetPropertyDoubleArray4d3 != null;
						if (flag9)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d3.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag10 = assetProperty.Name == "hardwood_tint_color";
					if (flag10)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d4 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag11 = assetPropertyDoubleArray4d4 != null;
						if (flag11)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d4.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag12 = assetProperty.Name == "masonrycmu_color";
					if (flag12)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d5 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag13 = assetPropertyDoubleArray4d5 != null;
						if (flag13)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d5.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag14 = assetProperty.Name == "metal_color";
					if (flag14)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d6 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag15 = assetPropertyDoubleArray4d6 != null;
						if (flag15)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d6.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag16 = assetProperty.Name == "metallicpaint_base_color";
					if (flag16)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d7 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag17 = assetPropertyDoubleArray4d7 != null;
						if (flag17)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d7.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag18 = assetProperty.Name == "mirror_tintcolor";
					if (flag18)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d8 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag19 = assetPropertyDoubleArray4d8 != null;
						if (flag19)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d8.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag20 = assetProperty.Name == "plasticvinyl_color";
					if (flag20)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d9 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag21 = assetPropertyDoubleArray4d9 != null;
						if (flag21)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d9.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag22 = assetProperty.Name == "plasticvinyl_type";
					if (flag22)
					{
						AssetPropertyInteger assetPropertyInteger = assetProperty as AssetPropertyInteger;
						bool flag23 = assetPropertyInteger != null;
						if (flag23)
						{
							textureInfo.SubType = assetPropertyInteger.Value;
							textureInfo.transparancy = 1.0;
							bool flag24 = textureInfo.SubType == 1;
							if (flag24)
							{
								textureInfo.textureCategorie = "plastic";
							}
						}
					}
					bool flag25 = assetProperty.Name == "glazing_transmittance_map";
					if (flag25)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d10 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag26 = assetPropertyDoubleArray4d10 != null;
						if (flag26)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d10.GetValueAsColor();
							textureInfo.ColorOrNot = true;
							textureInfo.textureCategorie = "glazing";
						}
					}
					bool flag27 = assetProperty.Name == "solidglass_transmittance_custom_color";
					if (flag27)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d11 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag28 = assetPropertyDoubleArray4d11 != null;
						if (flag28)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d11.GetValueAsColor();
							textureInfo.ColorOrNot = true;
							textureInfo.textureCategorie = "glazing";
						}
					}
					bool flag29 = assetProperty.Name == "wallpaint_color";
					if (flag29)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d12 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag30 = assetPropertyDoubleArray4d12 != null;
						if (flag30)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d12.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag31 = assetProperty.Name == "water_tint_color";
					if (flag31)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d13 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag32 = assetPropertyDoubleArray4d13 != null;
						if (flag32)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d13.GetValueAsColor();
							textureInfo.ColorOrNot = true;
							textureInfo.textureCategorie = "water";
						}
					}
					bool flag33 = assetProperty.Name == "generic_diffuse";
					if (flag33)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d14 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag34 = assetPropertyDoubleArray4d14 != null;
						if (flag34)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d14.GetValueAsColor();
							textureInfo.ColorOrNot = true;
						}
					}
					bool flag35 = assetProperty.Name == "generic_transparency";
					if (flag35)
					{
						AssetPropertyDouble assetPropertyDouble = assetProperty as AssetPropertyDouble;
						bool flag36 = assetPropertyDouble != null;
						if (flag36)
						{
							textureInfo.transparancy = assetPropertyDouble.Value;
						}
						bool flag37 = assetPropertyDouble.ToString() == "0";
						if (flag37)
						{
							textureInfo.transparancy = 0.0;
						}
					}
				}
			}
			return textureInfo;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00022EC4 File Offset: 0x000210C4
		private ExportToVRContext.TextureInfo getTextureYesImage(AppearanceAssetElement materialAsset, ExportToVRContext.TextureTypes tt)
		{
			ExportToVRContext.TextureInfo textureInfo = new ExportToVRContext.TextureInfo();
			bool flag = materialAsset != null;
			if (flag)
			{
				Asset renderingAsset = materialAsset.GetRenderingAsset();
				List<AssetProperty> list = new List<AssetProperty>();
				for (int i = 0; i < renderingAsset.Size; i++)
				{
					AssetProperty item = renderingAsset.Get(i);
					list.Add(item);
				}
				string text = "_diffuse";
				bool flag2 = tt == ExportToVRContext.TextureTypes.Bump;
				if (flag2)
				{
					text = "_bump_map";
				}
				list = (from ap in list
				orderby ap.Name
				select ap).ToList<AssetProperty>();
				for (int j = 0; j < list.Count; j++)
				{
					AssetProperty assetProperty = list[j];
					bool flag3 = assetProperty.Name == "common_Tint_color";
					if (flag3)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d = assetProperty as AssetPropertyDoubleArray4d;
						bool flag4 = assetPropertyDoubleArray4d != null;
						if (flag4)
						{
							textureInfo.color = assetPropertyDoubleArray4d.GetValueAsColor();
						}
					}
					bool flag5 = assetProperty.Name == "generic_diffuse";
					if (flag5)
					{
						AssetPropertyDoubleArray4d assetPropertyDoubleArray4d2 = assetProperty as AssetPropertyDoubleArray4d;
						bool flag6 = assetPropertyDoubleArray4d2 != null;
						if (flag6)
						{
							textureInfo.GenericColor = assetPropertyDoubleArray4d2.GetValueAsColor();
						}
					}
					bool flag7 = assetProperty.Name == "generic_transparency";
					if (flag7)
					{
						AssetPropertyDouble assetPropertyDouble = assetProperty as AssetPropertyDouble;
						bool flag8 = assetPropertyDouble != null;
						if (flag8)
						{
							textureInfo.transparancy = assetPropertyDouble.Value;
						}
						bool flag9 = assetPropertyDouble.ToString() == "0";
						if (flag9)
						{
							textureInfo.transparancy = 0.0;
						}
					}
					bool flag10 = assetProperty.Name.Length - 12 >= 0;
					if (flag10)
					{
						bool flag11 = assetProperty.Name.Substring(assetProperty.Name.Length - 12) == "_bump_amount";
						if (flag11)
						{
							AssetPropertyDouble assetPropertyDouble2 = assetProperty as AssetPropertyDouble;
							textureInfo.amount = assetPropertyDouble2.Value;
						}
					}
					bool flag12 = assetProperty.Name.Length - text.Length >= 0;
					if (flag12)
					{
						string a = assetProperty.Name.Substring(assetProperty.Name.Length - text.Length);
						bool flag13 = a == text;
						if (flag13)
						{
							bool flag14 = assetProperty.NumberOfConnectedProperties > 0;
							if (flag14)
							{
								IList<AssetProperty> allConnectedProperties = assetProperty.GetAllConnectedProperties();
								foreach (AssetProperty assetProperty2 in allConnectedProperties)
								{
									bool flag15 = assetProperty2 is Asset;
									if (flag15)
									{
										Asset asset = assetProperty2 as Asset;
										int size = asset.Size;
										for (int k = 0; k < size; k++)
										{
											AssetProperty assetProperty3 = asset.Get(k);
											bool flag16 = assetProperty3.Name == "unifiedbitmap_Bitmap";
											if (flag16)
											{
												AssetPropertyString assetPropertyString = assetProperty3 as AssetPropertyString;
												textureInfo.texturePath = assetPropertyString.Value;
											}
											else
											{
												bool flag17 = assetProperty3.Name == "texture_RealWorldScaleX";
												if (flag17)
												{
													AssetPropertyDistance assetPropertyDistance = assetProperty3 as AssetPropertyDistance;
													textureInfo.sx = UnitUtils.Convert(assetPropertyDistance.Value, assetPropertyDistance.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
													textureInfo.ScaleOrNot = true;
												}
												else
												{
													bool flag18 = assetProperty3.Name == "texture_RealWorldScaleY";
													if (flag18)
													{
														AssetPropertyDistance assetPropertyDistance2 = assetProperty3 as AssetPropertyDistance;
														textureInfo.sy = UnitUtils.Convert(assetPropertyDistance2.Value, assetPropertyDistance2.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
													}
													else
													{
														bool flag19 = assetProperty3.Name == "texture_RealWorldOffsetX";
														if (flag19)
														{
															AssetPropertyDistance assetPropertyDistance3 = assetProperty3 as AssetPropertyDistance;
															textureInfo.ox = UnitUtils.Convert(assetPropertyDistance3.Value, assetPropertyDistance3.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
														}
														else
														{
															bool flag20 = assetProperty3.Name == "texture_RealWorldOffsetY";
															if (flag20)
															{
																AssetPropertyDistance assetPropertyDistance4 = assetProperty3 as AssetPropertyDistance;
																textureInfo.oy = UnitUtils.Convert(assetPropertyDistance4.Value, assetPropertyDistance4.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
															}
															else
															{
																bool flag21 = assetProperty3.Name == "texture_WAngle";
																if (flag21)
																{
																	AssetPropertyDouble assetPropertyDouble3 = assetProperty3 as AssetPropertyDouble;
																	textureInfo.angle = assetPropertyDouble3.Value;
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
			return textureInfo;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00023358 File Offset: 0x00021558
		private void GetTheBitmaps(AppearanceAssetElement appearanceElem)
		{
			bool flag = appearanceElem != null;
			if (flag)
			{
				Asset renderingAsset = appearanceElem.GetRenderingAsset();
				int size = renderingAsset.Size;
				for (int i = 0; i < size; i++)
				{
					AssetProperty assetProperty = renderingAsset.Get(i);
					bool flag2 = assetProperty.NumberOfConnectedProperties < 1;
					if (!flag2)
					{
						bool flag3 = false;
						bool flag4 = assetProperty.Name.Contains("bump_map") | assetProperty.Name.Contains("pattern_map");
						if (flag4)
						{
							flag3 = true;
						}
						Asset asset = assetProperty.GetConnectedProperty(0) as Asset;
						bool flag5 = !flag3;
						if (flag5)
						{
							bool flag6 = asset.Name == "UnifiedBitmapSchema" || asset.Name == "unifiedbitmap_Bitmap";
							if (flag6)
							{
								AssetPropertyString assetPropertyString = asset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;
								bool flag7 = assetPropertyString != null;
								if (flag7)
								{
									bool flag8 = assetPropertyString.Value.ToString() != "";
									if (flag8)
									{
										this.Otherpathvalue = assetPropertyString.Value;
										bool flag9 = this.Otherpathvalue.Contains("|");
										if (flag9)
										{
											this.Otherpathvalue = this.Otherpathvalue.Substring(0, this.Otherpathvalue.IndexOf("|"));
										}
										bool flag10 = this.Otherpathvalue.Contains("\\") & this.Otherpathvalue.Contains("/");
										if (flag10)
										{
											this.Otherpathvalue = this.Otherpathvalue.Replace("/", "\\");
										}
										bool flag11 = this.Otherpathvalue.Contains("\\") & this.Otherpathvalue.Contains("//");
										if (flag11)
										{
											this.Otherpathvalue = this.Otherpathvalue.Replace("//", "\\");
										}
										bool flag12 = this.Otherpathvalue.Contains("\\\\");
										if (flag12)
										{
											this.Otherpathvalue = this.Otherpathvalue.Replace("\\\\", "\\");
										}
										bool flag13 = !File.Exists(this.Otherpathvalue);
										if (flag13)
										{
											string str = "C:\\Program Files (x86)\\Common Files\\Autodesk Shared\\Materials\\Textures";
											string text = str + "\\" + this.Otherpathvalue;
											bool flag14 = File.Exists(text);
											if (flag14)
											{
												this.Otherpathvalue = text;
												this.m_AllViews.ImageExist = true;
											}
										}
										bool flag15 = File.Exists(this.Otherpathvalue);
										if (flag15)
										{
											this.m_AllViews.ImageExist = true;
										}
									}
								}
								AssetPropertyDistance assetPropertyDistance = asset.FindByName(UnifiedBitmap.TextureRealWorldScaleX) as AssetPropertyDistance;
								bool flag16 = assetPropertyDistance != null;
								if (flag16)
								{
									this.modfU = UnitUtils.Convert(assetPropertyDistance.Value, assetPropertyDistance.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
								}
								AssetPropertyDistance assetPropertyDistance2 = asset.FindByName(UnifiedBitmap.TextureRealWorldScaleY) as AssetPropertyDistance;
								bool flag17 = assetPropertyDistance2 != null;
								if (flag17)
								{
									this.modfV = UnitUtils.Convert(assetPropertyDistance2.Value, assetPropertyDistance2.DisplayUnitType, DisplayUnitType.DUT_DECIMAL_FEET);
								}
							}
						}
					}
				}
			}
		}

		private Document m_document = null;

		private bool m_cancelled = false;

		private Stack<Transform> m_TransformationStack = new Stack<Transform>();

		private AllViews m_AllViews;

		public string sommets;

		public string nrmals;

		public string uvs;

		public string fcts;

		public string fctsByMaterials;

		public string fctsByEntities;

		public int facNB;

		public int xyzNB = 1;

		public int TotalFacNB;

		public int matNB;

		public int nb = 1;

		private int nbofPoints;

		private int nboffacets;

		public double modfU;

		public double modfV;

		public double angle;

		public bool textureExist = false;

		private int arrond = 5;

		public Hashtable h_modfU = new Hashtable();

		public Hashtable h_modfV = new Hashtable();

		public ICollection key_modfU = null;

		public Hashtable h_MaterialTexture = new Hashtable();

		public string newimagePath;

		public string Otherpathvalue;

		public string MaterialType;

		private string MaterialFaceName;

		private List<string> ListMaterialName = new List<string>();

		private List<string> ListMaterialGeneral = new List<string>();

		private List<string> ListMaterialNameGenTexture = new List<string>();

		private List<string> ListMaterialNameGenNoTexture = new List<string>();

		private List<string> ListMaterialNameOther = new List<string>();

		public string ElementName;

		private List<string> ListByElementName = new List<string>();

		private List<string> ListByElementEntSubCat = new List<string>();

		private List<string> ListByElementSubCat = new List<string>();

		public string GroupTypeValue = null;

		public bool radiobuttonSelect = false;

		public Hashtable h_Materials = new Hashtable();

		public Hashtable h_MaterialNames = new Hashtable();

		public ICollection key_Materials = null;

		private List<string> ListXYZ = new List<string>();

		private List<string> ListNormals = new List<string>();

		public StringBuilder XYZsbuilder = new StringBuilder();

		public StringBuilder NORMALsbuilder = new StringBuilder();

		public StringBuilder FCTsbuilder = new StringBuilder();

		public StringBuilder FCTbyMATsbuilder = new StringBuilder();

		public StringBuilder FCTbyENTsbuilder = new StringBuilder();

		public StringBuilder FCTbySUBCATsbuilder = new StringBuilder();

		public StringBuilder UVsbuilder = new StringBuilder();

		public StringBuilder MATERIALsbuilder = new StringBuilder();

		private string StartMatName = "RvtToUnityMat";

		private Stack<ElementId> elementStack = new Stack<ElementId>();

		private bool IsForVU = false;

		private bool isLink = false;

		private Document ZeLinkDoc = null;

		private ElementId currentMaterialId = ElementId.InvalidElementId;

		private int keyNB = 1;

		public class TextureInfo
		{
			// Token: 0x06000188 RID: 392 RVA: 0x00028500 File Offset: 0x00026700
			public TextureInfo()
			{
				this.texturePath = "";
				this.textureCategorie = "";
				this.sx = (this.sy = (this.ox = (this.oy = (this.angle = 0.0))));
				this.amount = 1.0;
				this.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				this.GenericColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				this.transparancy = 1.0;
				this.SubType = 0;
			}

			// Token: 0x040001AF RID: 431
			public string texturePath;

			// Token: 0x040001B0 RID: 432
			public string textureCategorie;

			// Token: 0x040001B1 RID: 433
			public double sx;

			// Token: 0x040001B2 RID: 434
			public double sy;

			// Token: 0x040001B3 RID: 435
			public double ox;

			// Token: 0x040001B4 RID: 436
			public double oy;

			// Token: 0x040001B5 RID: 437
			public double angle;

			// Token: 0x040001B6 RID: 438
			public Color color;

			// Token: 0x040001B7 RID: 439
			public bool TintOrNot;

			// Token: 0x040001B8 RID: 440
			public bool ColorOrNot;

			// Token: 0x040001B9 RID: 441
			public bool ScaleOrNot;

			// Token: 0x040001BA RID: 442
			public Color GenericColor;

			// Token: 0x040001BB RID: 443
			public double transparancy;

			// Token: 0x040001BC RID: 444
			public double amount;

			// Token: 0x040001BD RID: 445
			public int SubType;
		}

		// Token: 0x02000020 RID: 32
		public enum TextureTypes
		{
			// Token: 0x040001BF RID: 447
			Diffuse = 1,
			// Token: 0x040001C0 RID: 448
			Bump
		}
	}
}
