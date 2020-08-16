using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ExportToVR
{
	public class AllViews
	{
		public Hashtable h_MaterialIdAppearanceAssetId
		{
			get
			{
				return this.m_h_MaterialIdAppearanceAssetId;
			}
			set
			{
				this.m_h_MaterialIdAppearanceAssetId = value;
			}
		}

		public ICollection key_MaterialIdAppearanceAssetId
		{
			get
			{
				return this.m_key_MaterialIdAppearanceAssetId;
			}
			set
			{
				this.m_key_MaterialIdAppearanceAssetId = value;
			}
		}

        public List<string> fsymbolListName
		{
			get 
			{
				return this.m_fsymbolListName;
			}
			set
			{
				this.m_fsymbolListName = value;
			}
		}

		public List<string> ListByMaterialName
		{
			get
			{
				return this.m_ListByMaterialName;
			}
			set
			{
				this.m_ListByMaterialName = value;
			}
		}

		public List<string> ListByElementName
		{
			get
			{
				return this.m_ListByElementName;
			}
			set
			{
				this.m_ListByElementName = value;
			}
		}

		public List<string> ListMaterialNotVisible
		{
			get
			{
				return this.m_ListMaterialNotVisible;
			}
			set
			{
				this.m_ListMaterialNotVisible = value;
			}
		}

		public List<string> ListByElementSubCat
		{
			get
			{
				return this.m_ListByElementSubCat;
			}
			set
			{
				this.m_ListByElementSubCat = value;
			}
		}

		public List<string> ListMaterialName
		{
			get
			{
				return this.m_ListMaterialName;
			}
			set
			{
				this.m_ListMaterialName = value;
			}
		}

		public List<ElementId> ListAppearanceAssetID
		{
			get
			{
				return this.m_ListAppearanceAssetID;
			}
			set
			{
				this.m_ListAppearanceAssetID = value;
			}
		}

		public int TotalfacNB
		{
			get
			{
				return this.m_TotalfacNB;
			}
			set
			{
				this.m_TotalfacNB = value;
			}
		}

		public int TotalNbofPoints
		{
			get
			{
				return this.m_TotalNbofPoints;
			}
			set
			{
				this.m_TotalNbofPoints = value;
			}
		}

		public StringBuilder XYZsbuilder
		{
			get
			{
				return this.m_XYZsbuilder;
			}
			set
			{
				this.m_XYZsbuilder = value;
			}
		}

		public StringBuilder NORMALsbuilder
		{
			get
			{
				return this.m_NORMALsbuilder;
			}
			set
			{
				this.m_NORMALsbuilder = value;
			}
		}

		public List<string> XYZsbuilderLIST
		{
			get
			{
				return this.m_XYZsbuilderLIST;
			}
			set
			{
				this.m_XYZsbuilderLIST = value;
			}
		}

		public List<string> NORMALsbuilderLIST
		{
			get
			{
				return this.m_NORMALsbuilderLIST;
			}
			set
			{
				this.m_NORMALsbuilderLIST = value;
			}
		}

		public List<string> UVsbuilderLIST
		{
			get
			{
				return this.m_UVsbuilderLIST;
			}
			set
			{
				this.m_UVsbuilderLIST = value;
			}
		}

		public List<string> FCTbySUBCATsbuilderLIST
		{
			get
			{
				return this.m_FCTbySUBCATsbuilderLIST;
			}
			set
			{
				this.m_FCTbySUBCATsbuilderLIST = value;
			}
		}

		public List<string> FCTbyENTsbuilderLIST
		{
			get
			{
				return this.m_FCTbyENTsbuilderLIST;
			}
			set
			{
				this.m_FCTbyENTsbuilderLIST = value;
			}
		}

		public List<string> FCTbyMATsbuilderLIST
		{
			get
			{
				return this.m_FCTbyMATsbuilderLIST;
			}
			set
			{
				this.m_FCTbyMATsbuilderLIST = value;
			}
		}

		public List<string> FCTsbuilderLIST
		{
			get
			{
				return this.m_FCTsbuilderLIST;
			}
			set
			{
				this.m_FCTsbuilderLIST = value;
			}
		}

		public List<string> MATERIALsbuilderLIST
		{
			get
			{
				return this.m_MATERIALsbuilderLIST;
			}
			set
			{
				this.m_MATERIALsbuilderLIST = value;
			}
		}

		public StringBuilder FCTbySUBCATsbuilder
		{
			get
			{
				return this.m_FCTbySUBCATsbuilder;
			}
			set
			{
				this.m_FCTbySUBCATsbuilder = value;
			}
		}

		public StringBuilder FCTbyENTsbuilder
		{
			get
			{
				return this.m_FCTbyENTsbuilder;
			}
			set
			{
				this.m_FCTbyENTsbuilder = value;
			}
		}

		public StringBuilder FCTbyMATsbuilder
		{
			get
			{
				return this.m_FCTbyMATsbuilder;
			}
			set
			{
				this.m_FCTbyMATsbuilder = value;
			}
		}

		public StringBuilder FCTsbuilder
		{
			get
			{
				return this.m_FCTsbuilder;
			}
			set
			{
				this.m_FCTsbuilder = value;
			}
		}

		public StringBuilder UVsbuilder
		{
			get
			{
				return this.m_UVsbuilder;
			}
			set
			{
				this.m_UVsbuilder = value;
			}
		}

		public StringBuilder MATERIALsbuilder
		{
			get
			{
				return this.m_MATERIALsbuilder;
			}
			set
			{
				this.m_MATERIALsbuilder = value;
			}
		}

		public List<string> ViewListName
		{
			get
			{
				return this.m_ViewListName;
			}
			set
			{
				this.m_ViewListName = value;
			}
		}

		public bool MaxVerticesPerObj
		{
			get
			{
				return this.m_MaxVerticesPerObj;
			}
			set
			{
				this.m_MaxVerticesPerObj = value;
			}
		}

		public bool ExportProperties
		{
			get
			{
				return this.m_ExportProperties;
			}
			set
			{
				this.m_ExportProperties = value;
			}
		}

		public bool ExportSubCategories
		{
			get
			{
				return this.m_ExportSubCategories;
			}
			set
			{
				this.m_ExportSubCategories = value;
			}
		}

		public bool FindPatterns
		{
			get
			{
				return this.m_FindPatterns;
			}
			set
			{
				this.m_FindPatterns = value;
			}
		}

		public bool ChangeObjectID
		{
			get
			{
				return this.m_ChangeObjectID;
			}
			set
			{
				this.m_ChangeObjectID = value;
			}
		}

		public bool ImageExist
		{
			get
			{
				return this.m_ImageExist;
			}
			set
			{
				this.m_ImageExist = value;
			}
		}

		public bool LinkTransparent
		{
			get
			{
				return this.m_LinkTransparent;
			}
			set
			{
				this.m_LinkTransparent = value;
			}
		}

		public bool StartVUoption
		{
			get
			{
				return this.m_StartVUoption;
			}
			set
			{
				this.m_StartVUoption = value;
			}
		}

		public bool StandAloneVersion
		{
			get
			{
				return this.m_StandAloneVersion;
			}
			set
			{
				this.m_StandAloneVersion = value;
			}
		}

		public List<string> IDListName01
		{
			get
			{
				return this.m_IDListName01;
			}
			set
			{
				this.m_IDListName01 = value;
			}
		}

		public List<string> IDListName02
		{
			get
			{
				return this.m_IDListName02;
			}
			set
			{
				this.m_IDListName02 = value;
			}
		}

		public List<string> IDListName03
		{
			get
			{
				return this.m_IDListName03;
			}
			set
			{
				this.m_IDListName03 = value;
			}
		}

		public List<string> IDListName04
		{
			get
			{
				return this.m_IDListName04;
			}
			set
			{
				this.m_IDListName04 = value;
			}
		}

		public List<string> IDListName05
		{
			get
			{
				return this.m_IDListName05;
			}
			set
			{
				this.m_IDListName05 = value;
			}
		}

		public List<string> IDListName06
		{
			get
			{
				return this.m_IDListName06;
			}
			set
			{
				this.m_IDListName06 = value;
			}
		}

		public List<string> IDListName07
		{
			get
			{
				return this.m_IDListName07;
			}
			set
			{
				this.m_IDListName07 = value;
			}
		}

		public List<string> IDListName08
		{
			get
			{
				return this.m_IDListName08;
			}
			set
			{
				this.m_IDListName08 = value;
			}
		}

		public List<string> IDListName09
		{
			get
			{
				return this.m_IDListName09;
			}
			set
			{
				this.m_IDListName09 = value;
			}
		}

		public List<string> IDListName10
		{
			get
			{
				return this.m_IDListName10;
			}
			set
			{
				this.m_IDListName10 = value;
			}
		}

		public List<string> IDListNameToExport
		{
			get
			{
				return this.m_IDListNameToExport;
			}
			set
			{
				this.m_IDListNameToExport = value;
			}
		}

		public int GroupingOptions
		{
			get
			{
				return this.m_GroupingOptions;
			}
			set
			{
				this.m_GroupingOptions = value;
			}
		}

		public int ExportNB
		{
			get
			{
				return this.m_ExportNB;
			}
			set
			{
				this.m_ExportNB = value;
			}
		}

		public int ExportOrder
		{
			get
			{
				return this.m_ExportOrder;
			}
			set
			{
				this.m_ExportOrder = value;
			}
		}

		public Hashtable h_ListElementID
		{
			get
			{
				return this.m_h_ListElementID;
			}
			set
			{
				this.m_h_ListElementID = value;
			}
		}

		public int VerticeNb
		{
			get
			{
				return this.m_VerticeNb;
			}
			set
			{
				this.m_VerticeNb = value;
			}
		}

		public int NodeNb
		{
			get
			{
				return this.m_NodeNb;
			}
			set
			{
				this.m_NodeNb = value;
			}
		}

		public ICollection key_ListElementID
		{
			get
			{
				return this.m_key_ListElementID;
			}
			set
			{
				this.m_key_ListElementID = value;
			}
		}

		private void ObtaineFSymbolList(FilteredElementIterator elements)
		{
			this.fsymbolListName = new List<string>();
			elements.Reset();
			while (elements.MoveNext())
			{
				Element element = elements.Current;
				FamilySymbol familySymbol = element as FamilySymbol;
				bool flag = familySymbol == null || familySymbol.Category == null;
				if (!flag)
				{
					this.fsymbolListName.Add(string.Concat(new string[]
					{
						familySymbol.Category.Name,
						" : ",
						familySymbol.Family.Name,
						" : ",
						familySymbol.Name
					}));
					this.fsymbolListName.Sort();
				}
			}
		}

		public void ObtainAllfamilies(ExternalCommandData commandData)
		{
			bool flag = commandData == null;
			if (flag)
			{
				throw new ArgumentNullException("commandData");
			}
			UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
			FilteredElementIterator elementIterator = new FilteredElementCollector(activeUIDocument.Document).OfClass(typeof(FamilySymbol)).GetElementIterator();
			this.ObtaineFSymbolList(elementIterator);
		}

		private void ObtaineViewList(FilteredElementIterator elements)
		{
			this.ViewListName = new List<string>();
			elements.Reset();
			while (elements.MoveNext())
			{
				Element element = elements.Current;
				View3D view3D = element as View3D;
				if (view3D != null)
				{
					if (view3D.ViewType == ViewType.ThreeD && view3D.IsTemplate == false)
					{
						this.ViewListName.Add(view3D.Name);
						this.ViewListName.Sort();
					}
				}
			}
		}

		// 
		public void ObtainAllViews(ExternalCommandData commandData)
		{
			if (commandData == null)
			{
				throw new ArgumentNullException("commandData");
			}
			UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
			FilteredElementIterator elementIterator = new FilteredElementCollector(activeUIDocument.Document).OfClass(typeof(View)).GetElementIterator();
			this.ObtaineViewList(elementIterator);
		}

		private static int CompareString(string x, string y)
		{
			int num = y.Length.CompareTo(x.Length);
			bool flag = num != 0;
			int result;
			if (flag)
			{
				result = num;
			}
			else
			{
				result = x.CompareTo(y);
			}
			return result;
		}

		private List<string> m_ViewListName;

		private List<string> m_IDListName01;

		private List<string> m_IDListName02;

		private List<string> m_IDListName03;

		private List<string> m_IDListName04;

		private List<string> m_IDListName05;

		private List<string> m_IDListName06;

		private List<string> m_IDListName07;

		private List<string> m_IDListName08;

		private List<string> m_IDListName09;

		private List<string> m_IDListName10;

		private Hashtable m_h_ListElementID = new Hashtable();

		private ICollection m_key_ListElementID = null;

		private List<string> m_IDListNameToExport;

		private int m_ExportNB;

		private int m_ExportOrder;

		private int m_VerticeNb;

		private int m_NodeNb;

		private int m_GroupingOptions;

		private bool m_MaxVerticesPerObj = false;

		private bool m_ExportProperties = false;

		private bool m_ExportSubCategories = false;

		private bool m_FindPatterns = false;

		private bool m_ChangeObjectID = false;

		private StringBuilder m_XYZsbuilder = new StringBuilder();

		private StringBuilder m_NORMALsbuilder = new StringBuilder();

		private StringBuilder m_FCTbyENTsbuilder = new StringBuilder();

		private StringBuilder m_FCTbyMATsbuilder = new StringBuilder();

		private StringBuilder m_FCTsbuilder = new StringBuilder();

		private StringBuilder m_FCTbySUBCATsbuilder = new StringBuilder();

		private StringBuilder m_UVsbuilder = new StringBuilder();

		private StringBuilder m_MATERIALsbuilder = new StringBuilder();

		private List<string> m_XYZsbuilderLIST = new List<string>();

		private List<string> m_NORMALsbuilderLIST = new List<string>();

		private List<string> m_UVsbuilderLIST = new List<string>();

		private List<string> m_FCTbyENTsbuilderLIST = new List<string>();

		private List<string> m_FCTbyMATsbuilderLIST = new List<string>();

		private List<string> m_FCTsbuilderLIST = new List<string>();

		private List<string> m_FCTbySUBCATsbuilderLIST = new List<string>();

		private List<string> m_MATERIALsbuilderLIST = new List<string>();

		private List<string> m_ListByMaterialName = new List<string>();

		private List<ElementId> m_ListAppearanceAssetID = new List<ElementId>();

		private List<string> m_ListMaterialNotVisible = new List<string>();

		private List<string> m_ListByElementName = new List<string>();

		private List<string> m_ListByElementSubCat = new List<string>();

		private List<string> m_ListMaterialName = new List<string>();

		private int m_TotalfacNB;

		private int m_TotalNbofPoints;

		private List<string> m_fsymbolListName;

		private Hashtable m_h_MaterialIdAppearanceAssetId = new Hashtable();

		private ICollection m_key_MaterialIdAppearanceAssetId = null;

		private bool m_ImageExist = false;

		private bool m_LinkTransparent = false;

		private bool m_StartVUoption = false;

		private bool m_StandAloneVersion = false;
	}
}
