using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using ExportToVR.Properties;

namespace ExportToVR
{
	// Token: 0x02000009 RID: 9
	public class cl_ExportToVR_Button : IExternalApplication
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00003A18 File Offset: 0x00001C18
		public Result OnStartup(UIControlledApplication app)
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			RibbonPanel ribbonPanel = app.CreateRibbonPanel("To Unity");
			string text = "\nModel";
			string str = "ExportToVR2020.dll";
			PushButton pushButton = ribbonPanel.AddItem(new PushButtonData(text, text, directoryName + "\\" + str, "ExportToVR.cl_ExportToVR")) as PushButton;
			Bitmap exportToVR_Button = Resources.ExportToVR_Button;
			BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(exportToVR_Button.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			pushButton.LargeImage = bitmapSource;
			pushButton.Image = bitmapSource;
			pushButton.ToolTip = "Export Models directly to Unity with materials and textures.";
			ContextualHelp contextualHelp = new ContextualHelp(ContextualHelpType.Url, "http://www.emanuelfavreau.com/urinfos.php");
			pushButton.SetContextualHelp(contextualHelp);
			pushButton.LongDescription = "Open or select a 3D View from the list and send visible elements to the assets folder of a Unity Project. The tool will generate OBJ, MTL and image files format that can be read by Unity. ";
			Bitmap tfparameters_Tooltip_140X = Resources.TFParameters_Tooltip_140X340;
			BitmapSource toolTipImage = Imaging.CreateBitmapSourceFromHBitmap(tfparameters_Tooltip_140X.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			pushButton.ToolTipImage = toolTipImage;
			string text2 = "\nProperties";
			string str2 = "ExportToVR2020.dll";
			PushButton pushButton2 = ribbonPanel.AddItem(new PushButtonData(text2, text2, directoryName + "\\" + str2, "ExportToVR.cl_ExportData")) as PushButton;
			Bitmap exportProperties_Button = Resources.ExportProperties_Button;
			BitmapSource bitmapSource2 = Imaging.CreateBitmapSourceFromHBitmap(exportProperties_Button.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			pushButton2.LargeImage = bitmapSource2;
			pushButton2.Image = bitmapSource2;
			pushButton2.ToolTip = "Export Models directly to Unity with materials and textures.";
			ContextualHelp contextualHelp2 = new ContextualHelp(ContextualHelpType.Url, "http://www.emanuelfavreau.com/urinfos.php");
		
			pushButton2.SetContextualHelp(contextualHelp2);
			pushButton2.LongDescription = "Open or select a 3D View from the list and send visible elements to the assets folder of a Unity Project. The tool will generate OBJ, MTL and image files format that can be read by Unity. ";
			Bitmap tfparameters_Tooltip_140X2 = Resources.TFParameters_Tooltip_140X340;
			BitmapSource toolTipImage2 = Imaging.CreateBitmapSourceFromHBitmap(tfparameters_Tooltip_140X2.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			pushButton2.ToolTipImage = toolTipImage2;
			return Result.Cancelled;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public Result OnShutdown(UIControlledApplication app)
		{
			return Result.Cancelled;
		}
	}
}
