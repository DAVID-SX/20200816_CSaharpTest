using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using System.IO;
using System.Threading.Tasks;

namespace ForTestWinform
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            // 调出存储文件的对话框
            string text = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = null;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
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

            TaskDialog.Show("text", text);

            return Result.Succeeded;
        }
    }
}
