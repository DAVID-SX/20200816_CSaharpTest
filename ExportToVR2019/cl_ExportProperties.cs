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
	// Token: 0x02000008 RID: 8
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class cl_ExportProperties : IExternalCommand
	{
		// Token: 0x06000085 RID: 133 RVA: 0x0000376C File Offset: 0x0000196C
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
			Result result;
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
						using (ExpDataForm expDataForm = new ExpDataForm(commandData, allViews))
						{
							bool flag6 = expDataForm.ShowDialog() == DialogResult.OK;
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
			return result;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000390C File Offset: 0x00001B0C
		private bool verifyEntitlementEF(string userId, string userPassword, string secret)
		{
			RestClient restClient = new RestClient("https://www.emanuelfavreau.com/");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			restClient.Authenticator = new HttpBasicAuthenticator(userId, userPassword);
			RestRequest restRequest = new RestRequest("nina/users/ClashResults.json", Method.GET);
			restRequest.RequestFormat = DataFormat.Json;
			restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
			IRestResponse<List<cl_ExportProperties.GetJsonResult>> restResponse = restClient.Execute<List<cl_ExportProperties.GetJsonResult>>(restRequest);
			bool result = false;
			bool flag = restResponse.Data != null;
			if (flag)
			{
				foreach (cl_ExportProperties.GetJsonResult getJsonResult in restResponse.Data)
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

		// Token: 0x04000042 RID: 66
		public const string _baseApiUrl = "https://apps.exchange.autodesk.com/";

		// Token: 0x04000043 RID: 67
		public const string _appId = "3759955758891315427";

		// Token: 0x0200001D RID: 29
		public class GetJsonResult
		{
			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000178 RID: 376 RVA: 0x00028486 File Offset: 0x00026686
			// (set) Token: 0x06000179 RID: 377 RVA: 0x0002848E File Offset: 0x0002668E
			public string client_secret { get; set; }

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x0600017A RID: 378 RVA: 0x00028497 File Offset: 0x00026697
			// (set) Token: 0x0600017B RID: 379 RVA: 0x0002849F File Offset: 0x0002669F
			public string email { get; set; }

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x0600017C RID: 380 RVA: 0x000284A8 File Offset: 0x000266A8
			// (set) Token: 0x0600017D RID: 381 RVA: 0x000284B0 File Offset: 0x000266B0
			public string link { get; set; }
		}
	}
}
