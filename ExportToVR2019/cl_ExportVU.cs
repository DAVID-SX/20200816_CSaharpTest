using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RestSharp;
using RestSharp.Authenticators;

namespace ExportToVR
{
	// Token: 0x02000007 RID: 7
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class cl_ExportVU : IExternalCommand
	{
		// Token: 0x06000082 RID: 130 RVA: 0x000034C0 File Offset: 0x000016C0
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			Autodesk.Revit.ApplicationServices.Application application = commandData.Application.Application;
			Document document = commandData.Application.ActiveUIDocument.Document;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			string caption = "ef | Export To Unity";
			string versionNumber = application.VersionNumber;
			string subVersionNumber = application.SubVersionNumber;
			bool flag = versionNumber != "2018" & versionNumber != "2019" & versionNumber != "2020";
			if (flag)
			{
				MessageBox.Show("This version only works with Revit 2018 or 2019.", caption, buttons, icon);
				return Result.Succeeded;
			}
			else
			{
				bool flag2 = versionNumber.Contains("2018");
				if (flag2)
				{
					bool flag3 = subVersionNumber != "2018.1" & subVersionNumber != "2018.2" & subVersionNumber != "2018.3";
					if (flag3)
					{
						MessageBox.Show("This version only works with Revit 2018.1 or more.", caption, buttons, icon);
						return Result.Cancelled;
					}
				}
				string userId = "manu";
				string userPassword = "protected";
				string secret = "msdlGeekNerdzA";
				bool flag4 = this.verifyEntitlementEF(userId, userPassword, secret);
				bool flag5 = flag4;
				if (flag5)
				{
					try
					{
						AllViews allViews = new AllViews();
						allViews.ObtainAllViews(commandData);
						using (ExportToVUForm exportToVUForm = new ExportToVUForm(commandData, allViews))
						{
							bool flag6 = exportToVUForm.ShowDialog() == DialogResult.OK;
							if (flag6)
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
				}
				else
				{
					TaskDialog.Show("Entitlement API", "User do not have entitlement to use the App");
				}
				return Result.Succeeded;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003660 File Offset: 0x00001860
		private bool verifyEntitlementEF(string userId, string userPassword, string secret)
		{
			RestClient restClient = new RestClient("https://www.emanuelfavreau.com/");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			restClient.Authenticator = new HttpBasicAuthenticator(userId, userPassword);
			RestRequest restRequest = new RestRequest("nina/users/ClashResults.json", Method.GET);
			restRequest.RequestFormat = DataFormat.Json;
			restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
			IRestResponse<List<cl_ExportVU.GetJsonResult>> restResponse = restClient.Execute<List<cl_ExportVU.GetJsonResult>>(restRequest);
			bool result = false;
			bool flag = restResponse.Data != null;
			if (flag)
			{
				foreach (cl_ExportVU.GetJsonResult getJsonResult in restResponse.Data)
				{
					bool flag2 = getJsonResult.client_secret.ToString() == secret;
					if (flag2)
					{
						result = true;
					}
				}
			}
			bool flag3 = restResponse.ErrorException != null;
			if (flag3)
			{
				string message = "Error retrieving your access token. " + restResponse.ErrorException.Message;
				throw new Exception(message);
			}
			return result;
		}

		// Token: 0x04000040 RID: 64
		public const string _baseApiUrl = "https://apps.exchange.autodesk.com/";

		// Token: 0x04000041 RID: 65
		public const string _appId = "3759955758891315427";

		// Token: 0x0200001C RID: 28
		public class GetJsonResult
		{
			// Token: 0x1700005A RID: 90
			// (get) Token: 0x06000171 RID: 369 RVA: 0x00028453 File Offset: 0x00026653
			// (set) Token: 0x06000172 RID: 370 RVA: 0x0002845B File Offset: 0x0002665B
			public string client_secret { get; set; }

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x06000173 RID: 371 RVA: 0x00028464 File Offset: 0x00026664
			// (set) Token: 0x06000174 RID: 372 RVA: 0x0002846C File Offset: 0x0002666C
			public string email { get; set; }

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000175 RID: 373 RVA: 0x00028475 File Offset: 0x00026675
			// (set) Token: 0x06000176 RID: 374 RVA: 0x0002847D File Offset: 0x0002667D
			public string link { get; set; }
		}
	}
}
