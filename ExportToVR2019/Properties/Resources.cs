using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ExportToVR.Properties
{
	// Token: 0x02000016 RID: 22
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00028131 File Offset: 0x00026331
		internal Resources()
		{
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0002813C File Offset: 0x0002633C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("ExportToVR.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00028184 File Offset: 0x00026384
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0002819B File Offset: 0x0002639B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000281A4 File Offset: 0x000263A4
		internal static Bitmap ef32_RGris_96
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ef32_RGris_96", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000281D4 File Offset: 0x000263D4
		internal static Bitmap ExportProperties_Button
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ExportProperties_Button", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00028204 File Offset: 0x00026404
		internal static Bitmap ExportToVR_Button
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ExportToVR_Button", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00028234 File Offset: 0x00026434
		internal static Bitmap ExportToVR_Gris
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ExportToVR_Gris", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00028264 File Offset: 0x00026464
		internal static Bitmap ExportToVR_Gris_36X36
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ExportToVR_Gris_36X36", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00028294 File Offset: 0x00026494
		internal static Bitmap ExportToVU_Button
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ExportToVU_Button", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000282C4 File Offset: 0x000264C4
		internal static Bitmap GreenCircle
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("GreenCircle", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000282F4 File Offset: 0x000264F4
		internal static Bitmap TFParameters_Tooltip_140X340
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("TFParameters_Tooltip_140X340", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000191 RID: 401
		private static ResourceManager resourceMan;

		// Token: 0x04000192 RID: 402
		private static CultureInfo resourceCulture;
	}
}
