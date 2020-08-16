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
	// Token: 0x02000004 RID: 4
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class cl_ExportPAT : IExternalCommand
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			Autodesk.Revit.ApplicationServices.Application application = commandData.Application.Application;
			Document document = commandData.Application.ActiveUIDocument.Document;
			string userId = "manu";
			string userPassword = "protected";
			string secret = "msdlGeekNerdzA";
			bool flag = this.verifyEntitlementEF(userId, userPassword, secret);
			bool flag2 = flag;
			if (flag2)
			{
				try
				{
					AllViews allViews = new AllViews();
					allViews.ObtainAllViews(commandData);
					using (FindMatPatternsForm findMatPatternsForm = new FindMatPatternsForm(commandData, allViews))
					{
						bool flag3 = findMatPatternsForm.ShowDialog() == DialogResult.OK;
						if (flag3)
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

		// Token: 0x06000079 RID: 121 RVA: 0x00002E98 File Offset: 0x00001098
		private bool verifyEntitlementEF(string userId, string userPassword, string secret)
		{
			RestClient restClient = new RestClient("https://www.emanuelfavreau.com/");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			restClient.Authenticator = new HttpBasicAuthenticator(userId, userPassword);
			RestRequest restRequest = new RestRequest("nina/users/ClashResults.json", Method.GET);
			restRequest.RequestFormat = DataFormat.Json;
			restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
			IRestResponse<List<cl_ExportPAT.GetJsonResult>> restResponse = restClient.Execute<List<cl_ExportPAT.GetJsonResult>>(restRequest);
			bool result = false;
			bool flag = restResponse.Data != null;
			if (flag)
			{
				foreach (cl_ExportPAT.GetJsonResult getJsonResult in restResponse.Data)
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

		// Token: 0x0400003A RID: 58
		public const string _baseApiUrl = "https://apps.exchange.autodesk.com/";

		// Token: 0x0400003B RID: 59
		public const string _appId = "3759955758891315427";

		// Token: 0x02000018 RID: 24
		public class GetJsonResult
		{
			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000151 RID: 337 RVA: 0x00028365 File Offset: 0x00026565
			// (set) Token: 0x06000152 RID: 338 RVA: 0x0002836D File Offset: 0x0002656D
			public string client_secret { get; set; }

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000153 RID: 339 RVA: 0x00028376 File Offset: 0x00026576
			// (set) Token: 0x06000154 RID: 340 RVA: 0x0002837E File Offset: 0x0002657E
			public string email { get; set; }

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x06000155 RID: 341 RVA: 0x00028387 File Offset: 0x00026587
			// (set) Token: 0x06000156 RID: 342 RVA: 0x0002838F File Offset: 0x0002658F
			public string link { get; set; }
		}
	}
}
