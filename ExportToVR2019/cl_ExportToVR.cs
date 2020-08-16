using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using RestSharp;

namespace ExportToVR
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class cl_ExportToVR : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 获取文档
            Autodesk.Revit.ApplicationServices.Application application = commandData.Application.Application;
            Document document = commandData.Application.ActiveUIDocument.Document;
            // 定义报警消息框的相关信息
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string caption = "模型数据导出";
            string versionNumber = application.VersionNumber;
            string subVersionNumber = application.SubVersionNumber;
            // 如果版本有问题就弹出对话框
            if (versionNumber != "2019" & versionNumber != "2020")
            {
                MessageBox.Show("该版本仅可在 Revit2019/2020 平台上运行！", caption, buttons, icon);
                return Result.Failed;
            }
            else
            {
                try
                {
                    AllViews allViews = new AllViews();
                    allViews.ObtainAllViews(commandData); //向AllViews的属性ViewListName中增加三维视图的名称
                    using (ExportViewsToVRForm exportViewsToVRForm = new ExportViewsToVRForm(commandData, allViews))
                    {
                        if (exportViewsToVRForm.ShowDialog() == DialogResult.OK)
                        {
                            return Result.Cancelled;
                        }
                        // 更改mtl文件
                        string folderPath = Path.GetDirectoryName(exportViewsToVRForm.mtlPath);
                        string fileName = Path.GetFileNameWithoutExtension(exportViewsToVRForm.mtlPath);
                        string fileContent = null;

                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }
                return Result.Succeeded;
            }
        }
        
        public void WriteMtlInfo(Document doc)
        {
            // 收集项目中的所有材质
            IList<Element> materialList = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Materials).ToElements();
            // 复制贴图

            // 写入mtl文件信息
            foreach (Material mat in materialList)
            {
                string newmtl = mat.Name + "\n";
                string Ka = mat.Color.Red.ToString() + mat.Color.Green.ToString() + mat.Color.Blue.ToString() + "\n";
                string Kb = mat.Color.Red.ToString() + mat.Color.Green.ToString() + mat.Color.Blue.ToString() + "\n";
                string d = (1 - mat.Transparency / 100).ToString();
                string map_Ka = Path.GetFileName();

                
                   


            }
        }
        // 定义获取贴图名称的方法
        public void GetMapname(Document doc, Material mat)
        {
            if (Convert.ToString(mat.AppearanceAssetId) != "-1")
            {
                AppearanceAssetElement appearanceAssetElement = doc.GetElement(mat.AppearanceAssetId) as AppearanceAssetElement;
                Asset asset = appearanceAssetElement.GetRenderingAsset();
                Asset asset2 = asset.FindByName("generic_diffuse").GetSingleConnectedAsset();
                string name = asset2.FindByName("unifiedbitmap_Bitmap").ToString();


            }

        }

        // 定义获取材质路径的方法
        public void GetRenderingTexturePath(Document doc, Material mat, List<Material> changePathMaterial, List<string> oldPath)
        {
            if (Convert.ToString(mat.AppearanceAssetId) != "-1")  // 如果材质没有AppearanceAssetId就跳过
            {
                Asset matAsset = (doc.GetElement(mat.AppearanceAssetId) as AppearanceAssetElement).GetRenderingAsset();
                if (matAsset.Size != 0)
                {
                    for (int i = 0; i < matAsset.Size; i++)
                    {
                        if (matAsset[i].Name == "generic_diffuse" || matAsset[i].Name == "masonrycmu_color")
                        {
                            Asset diffuseAsset = matAsset[i].GetSingleConnectedAsset();
                            if (diffuseAsset is null) continue;
                            else
                            {
                                for (int j = 0; j < diffuseAsset.Size; j++)
                                {
                                    if (diffuseAsset[j].Name == "unifiedbitmap_Bitmap")
                                    {
                                        AssetPropertyString path = diffuseAsset[j] as AssetPropertyString;
                                        string bitMapPath = path.Value;
                                        if (bitMapPath.StartsWith(@"C:\") | bitMapPath.StartsWith(@"D:\")
                                            | bitMapPath.StartsWith(@"E:\") | bitMapPath.StartsWith(@"F:\")
                                            | bitMapPath.StartsWith(@"G:\") | bitMapPath.StartsWith(@"H:\"))
                                        {
                                            changePathMaterial.Add(mat);
                                            oldPath.Add(path.Value);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

            }
        }

        // 定义复制文件的方法
        public static void CopyFile(string sourceFilePath, string targetFolderPath, List<string> newPath, Material mat)
        {
            try
            {
                // 若目录不存在，建立目录
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
                // 根据目标文件夹及源文件路径复制文件
                String targetFilePath = Path.Combine(targetFolderPath, Path.GetFileName(sourceFilePath));
                newPath.Add(targetFilePath);
                bool isrewrite = true;   // true=覆盖已存在的同名文件,false则反之
                File.Copy(sourceFilePath, targetFilePath, isrewrite);
            }
            catch (Exception)
            {
                TaskDialog.Show("失败提示！！！", "材质【" + mat.Name + "】的贴图复制失败，请手动更改材质贴图");
            }
        }

        // 定义修改贴图路径的方法
        public string ChangeRenderingTexturePath(Document doc, Material mat, string newPath)
        {
            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("更改贴图位置");

                    using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
                    {
                        Asset editableAsset = editScope.Start(mat.AppearanceAssetId);
                        // Getting the correct AssetProperty
                        AssetProperty assetProperty = editableAsset.FindByName("generic_diffuse");
                        if (assetProperty is null)
                        {
                            assetProperty = editableAsset.FindByName("masonrycmu_color");
                        }

                        Asset connectedAsset = assetProperty.GetConnectedProperty(0) as Asset;
                        // getting the right connected Asset
                        if (connectedAsset.Name == "UnifiedBitmapSchema")
                        {
                            AssetPropertyString path = connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;
                            if (path.IsValidValue(newPath)) path.Value = newPath;
                        }
                        editScope.Commit(true);
                    }
                    t.Commit();
                    t.Dispose();
                }
                return mat.Name;
            }
            catch (Exception)
            {

                TaskDialog.Show("错误提示！！！", "材质【" + mat.Name + "】的贴图更改失败，请手动更改材质贴图");
                return null;
            }
        }
    }
}
