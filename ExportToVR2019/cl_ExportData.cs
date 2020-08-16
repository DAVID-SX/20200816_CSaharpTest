using System;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RestSharp;

namespace ExportToVR
{
    // Token: 0x02000005 RID: 5
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class cl_ExportData : IExternalCommand
    {
        // Token: 0x0600007B RID: 123 RVA: 0x00002FA4 File Offset: 0x000011A4
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application application = commandData.Application.Application;
            Document document = commandData.Application.ActiveUIDocument.Document;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            string caption = "ef | Export To Unity";
            string versionNumber = application.VersionNumber;
            string subVersionNumber = application.SubVersionNumber;
            if (versionNumber != "2019" & versionNumber != "2020")
            {
                MessageBox.Show("This version only works with Revit 2019 or 2020.", caption, buttons, icon);
                return Result.Failed;
            }
            else
            {
                try
                {
                    AllViews allViews = new AllViews();
                    allViews.ObtainAllViews(commandData);
                    using (ExpDataForm expDataForm = new ExpDataForm(commandData, allViews))
                    {
                        bool flag5 = expDataForm.ShowDialog() == DialogResult.OK;
                        if (flag5)
                        {
                            return Result.Cancelled;
                        }
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

        // Token: 0x0600007C RID: 124 RVA: 0x000030FC File Offset: 0x000012FC
        private bool verifyEntitlement(string appId, string userId)
        {
            RestClient restClient = new RestClient();
            restClient.BaseUrl = new Uri("https://apps.exchange.autodesk.com");
            RestRequest restRequest = new RestRequest();
            restRequest.Resource = "webservices/checkentitlement";
            restRequest.Method = Method.GET;
            restRequest.AddParameter("userid", userId);
            restRequest.AddParameter("appid", appId);
            IRestResponse<cl_ExportData.EntitlementResult> restResponse = restClient.Execute<cl_ExportData.EntitlementResult>(restRequest);
            bool result = false;
            bool flag = restResponse.Data != null && restResponse.Data.IsValid;
            if (flag)
            {
                result = true;
            }
            return result;
        }

        // Token: 0x0400003C RID: 60
        public const string _baseApiUrl = "https://apps.exchange.autodesk.com/";

        // Token: 0x0400003D RID: 61
        public const string _appId = "3759955758891315427";

        // Token: 0x02000019 RID: 25
        [Serializable]
        public class EntitlementResult
        {
            // Token: 0x1700004F RID: 79
            // (get) Token: 0x06000158 RID: 344 RVA: 0x00028398 File Offset: 0x00026598
            // (set) Token: 0x06000159 RID: 345 RVA: 0x000283A0 File Offset: 0x000265A0
            public string UserId { get; set; }

            // Token: 0x17000050 RID: 80
            // (get) Token: 0x0600015A RID: 346 RVA: 0x000283A9 File Offset: 0x000265A9
            // (set) Token: 0x0600015B RID: 347 RVA: 0x000283B1 File Offset: 0x000265B1
            public string AppId { get; set; }

            // Token: 0x17000051 RID: 81
            // (get) Token: 0x0600015C RID: 348 RVA: 0x000283BA File Offset: 0x000265BA
            // (set) Token: 0x0600015D RID: 349 RVA: 0x000283C2 File Offset: 0x000265C2
            public bool IsValid { get; set; }

            // Token: 0x17000052 RID: 82
            // (get) Token: 0x0600015E RID: 350 RVA: 0x000283CB File Offset: 0x000265CB
            // (set) Token: 0x0600015F RID: 351 RVA: 0x000283D3 File Offset: 0x000265D3
            public string Message { get; set; }
        }
    }
}
