using System;
using System.Collections;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ExportToVR
{
	internal class CheckExportContext : IExportContext
	{
		public CheckExportContext(Document document, AllViews allViews)
		{
			this.m_document = document;
			this.m_TransformationStack.Push(Transform.Identity);
			this.m_AllViews = allViews;
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

		public void OnPolymesh(PolymeshTopology node)
		{
			Transform transform = this.m_TransformationStack.Peek();
			int numberOfFacets = node.NumberOfFacets;
			int numberOfPoints = node.NumberOfPoints;
			int numberOfUVs = node.NumberOfUVs;
			int numberOfNormals = node.NumberOfNormals;
			this.TotalNBofPoints += numberOfPoints;
			this.TotalNBofFacets += numberOfFacets;
			int num = this.m_AllViews.VerticeNb / 4;
			bool exportProperties = this.m_AllViews.ExportProperties;
			if (exportProperties)
			{
				bool flag = !this.ListElementID01.Contains(this.CurrentElementId.IntegerValue.ToString());
				if (flag)
				{
					this.ListElementID01.Add(this.CurrentElementId.IntegerValue.ToString());
				}
			}
			bool findPatterns = this.m_AllViews.FindPatterns;
			if (findPatterns)
			{
				List<int> list = new List<int>();
				bool flag2 = !list.Contains(this.MaterialFaceID);
				if (flag2)
				{
					list.Add(this.MaterialFaceID);
				}
				bool flag3 = this.key_ElementIDListMatID == null;
				if (flag3)
				{
					bool flag4 = !this.h_ElementIDListMatID.ContainsKey(this.CurrentElementId.IntegerValue);
					if (flag4)
					{
						this.h_ElementIDListMatID.Add(this.CurrentElementId.IntegerValue, list);
						this.key_ElementIDListMatID = this.h_ElementIDListMatID.Keys;
					}
				}
				bool flag5 = false;
				bool flag6 = this.key_ElementIDListMatID != null;
				if (flag6)
				{
					bool flag7 = this.key_ElementIDListMatID.Count > 0;
					if (flag7)
					{
						foreach (object obj in this.key_ElementIDListMatID)
						{
							int num2 = (int)obj;
							bool flag8 = num2 == this.CurrentElementId.IntegerValue;
							if (flag8)
							{
								flag5 = true;
							}
						}
						bool flag9 = flag5;
						if (flag9)
						{
							foreach (object obj2 in this.key_ElementIDListMatID)
							{
								int num3 = (int)obj2;
								bool flag10 = num3 == this.CurrentElementId.IntegerValue;
								if (flag10)
								{
									List<int> list2 = (List<int>)this.h_ElementIDListMatID[num3];
									bool flag11 = !list2.Contains(this.MaterialFaceID);
									if (flag11)
									{
										list2.Add(this.MaterialFaceID);
									}
									this.h_ElementIDListMatID[num3] = list2;
									break;
								}
							}
						}
						else
						{
							bool flag12 = !flag5;
							if (flag12)
							{
								bool flag13 = !this.h_ElementIDListMatID.ContainsKey(this.CurrentElementId.IntegerValue);
								if (flag13)
								{
									this.h_ElementIDListMatID.Add(this.CurrentElementId.IntegerValue, list);
									this.key_ElementIDListMatID = this.h_ElementIDListMatID.Keys;
								}
							}
						}
					}
				}
			}
			bool maxVerticesPerObj = this.m_AllViews.MaxVerticesPerObj;
			if (maxVerticesPerObj)
			{
				bool flag14 = this.TotalNBofPoints <= num * 4;
				if (flag14)
				{
					bool flag15 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID01.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag15)
					{
						this.ListElementID01.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag16 = this.TotalNBofPoints > num * 4 & this.TotalNBofPoints <= num * 8;
				if (flag16)
				{
					bool flag17 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID02.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag17)
					{
						this.ListElementID02.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag18 = this.TotalNBofPoints > num * 8 & this.TotalNBofPoints <= num * 12;
				if (flag18)
				{
					bool flag19 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID03.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag19)
					{
						this.ListElementID03.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag20 = this.TotalNBofPoints > num * 12 & this.TotalNBofPoints <= num * 16;
				if (flag20)
				{
					bool flag21 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID04.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag21)
					{
						this.ListElementID04.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag22 = this.TotalNBofPoints > num * 16 & this.TotalNBofPoints <= num * 20;
				if (flag22)
				{
					bool flag23 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID05.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag23)
					{
						this.ListElementID05.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag24 = this.TotalNBofPoints > num * 20 & this.TotalNBofPoints <= num * 24;
				if (flag24)
				{
					bool flag25 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID06.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag25)
					{
						this.ListElementID06.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag26 = this.TotalNBofPoints > num * 24 & this.TotalNBofPoints <= num * 28;
				if (flag26)
				{
					bool flag27 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID07.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag27)
					{
						this.ListElementID07.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag28 = this.TotalNBofPoints > num * 28 & this.TotalNBofPoints <= num * 32;
				if (flag28)
				{
					bool flag29 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID08.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag29)
					{
						this.ListElementID08.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag30 = this.TotalNBofPoints > num * 32 & this.TotalNBofPoints <= num * 36;
				if (flag30)
				{
					bool flag31 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID09.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag31)
					{
						this.ListElementID09.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
				bool flag32 = this.TotalNBofPoints > num * 36 & this.TotalNBofPoints <= num * 40;
				if (flag32)
				{
					bool flag33 = !this.ListElementID_ALL.Contains(this.CurrentElementId.IntegerValue.ToString()) & !this.ListElementID10.Contains(this.CurrentElementId.IntegerValue.ToString());
					if (flag33)
					{
						this.ListElementID10.Add(this.CurrentElementId.IntegerValue.ToString());
						this.ListElementID_ALL.Add(this.CurrentElementId.IntegerValue.ToString());
					}
				}
			}
		}

		private void ExportMeshPoints(IList<XYZ> points, Transform trf, IList<XYZ> normals)
		{
		}

		private void ExportMeshPoints(IList<XYZ> points, Transform trf, XYZ normal)
		{
		}

		private void ExportMeshPoints(IList<XYZ> points, Transform trf)
		{
		}

		private void ExportMeshFacets(IList<PolymeshFacet> facets, IList<XYZ> normals)
		{
			bool flag = normals == null;
			if (flag)
			{
			}
		}

		private void ExportMeshUVs(IList<UV> UVs)
		{
		}

		public void OnRPC(RPCNode node)
		{
		}

		public RenderNodeAction OnViewBegin(ViewNode node)
		{
			return 0;
		}

		public void OnViewEnd(ElementId elementId)
		{
		}

		public RenderNodeAction OnElementBegin(ElementId elementId)
		{
			this.elementStack.Push(elementId);
			return 0;
		}

		public void OnElementEnd(ElementId elementId)
		{
			this.elementStack.Pop();
		}

		public RenderNodeAction OnFaceBegin(FaceNode node)
		{
			return 0;
		}

		public void OnFaceEnd(FaceNode node)
		{
		}

		public RenderNodeAction OnInstanceBegin(InstanceNode node)
		{
			this.m_TransformationStack.Push(this.m_TransformationStack.Peek().Multiply(node.GetTransform()));
			return 0;
		}

		public void OnInstanceEnd(InstanceNode node)
		{
			this.m_TransformationStack.Pop();
		}

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
			bool flag3 = symbolId != null;
			if (flag3)
			{
				bool flag4 = !this.ListLINKID01.Contains(symbolId.IntegerValue.ToString());
				if (flag4)
				{
					this.ListLINKID01.Add(symbolId.IntegerValue.ToString());
				}
			}
			this.m_TransformationStack.Push(this.m_TransformationStack.Peek().Multiply(node.GetTransform()));
			return 0;
		}

		public void OnLinkEnd(LinkNode node)
		{
			this.m_TransformationStack.Pop();
		}

		public void OnLight(LightNode node)
		{
		}

		public void OnMaterial(MaterialNode node)
		{
			this.currentMaterialId = node.MaterialId;
			if (this.currentMaterialId != ElementId.InvalidElementId & !this.currentMaterialId.IntegerValue.ToString().Contains("-"))
			{
				Material material = this.m_document.GetElement(this.currentMaterialId) as Material;
				this.MaterialFaceID = this.currentMaterialId.IntegerValue;
				if (!this.ListMaterialID.Contains(this.currentMaterialId.IntegerValue))
				{
					this.ListMaterialID.Add(this.currentMaterialId.IntegerValue);
					this.MaterialFaceID = this.currentMaterialId.IntegerValue;
				}
			}
		}

		private static double DegreeToRadian(double angle)
		{
			return Math.PI * angle / 180.0;
		}

		private static double RadianToDegree(double angle)
		{
			return angle * 57.295779513082323;
		}

		private Document m_document = null;

		private bool m_cancelled = false;

		private Stack<Transform> m_TransformationStack = new Stack<Transform>();

		private AllViews m_AllViews;

		public int TotalNBofPoints;

		public int TotalNBofFacets;

		public int TotalNBofNodes;

		public int OrderPoints = 1;

		public int OrderFacets = 1;

		public int OrderNormals = 1;

		public int OrderUVs = 1;

		public List<string> ListElementID01 = new List<string>();

		public List<string> ListElementID02 = new List<string>();

		public List<string> ListElementID03 = new List<string>();

		public List<string> ListElementID04 = new List<string>();

		public List<string> ListElementID05 = new List<string>();

		public List<string> ListElementID06 = new List<string>();

		public List<string> ListElementID07 = new List<string>();

		public List<string> ListElementID08 = new List<string>();

		public List<string> ListElementID09 = new List<string>();

		public List<string> ListElementID10 = new List<string>();

		public List<string> ListLINKID01 = new List<string>();

		public List<string> ListElementID_ALL = new List<string>();

		private int MaterialFaceID;

		private int memeElement = 0;

		private List<int> ListMaterialFaceID = new List<int>();

		private List<int> ListOneElementID = new List<int>();

		public List<int> ListMaterialID = new List<int>();

		public Hashtable h_ElementIDListMatID = new Hashtable();

		public ICollection key_ElementIDListMatID = null;

		private Stack<ElementId> elementStack = new Stack<ElementId>();

		private bool isLink = false;

		private Document ZeLinkDoc = null;

		private ElementId currentMaterialId = ElementId.InvalidElementId;
	}
}
