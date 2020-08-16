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
	// Token: 0x02000006 RID: 6
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class cl_ExportVUxyz : IExternalCommand
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00003188 File Offset: 0x00001388
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
				string secret = "VuNerdzA";
				bool flag4 = this.verifyEntitlementEF(userId, userPassword, secret);
				bool flag5 = flag4;
				if (flag5)
				{
					try
					{
						AllViews allViews = new AllViews();
						allViews.ObtainAllViews(commandData);
						using (ExportToVUxyzForm exportToVUxyzForm = new ExportToVUxyzForm(commandData, allViews))
						{
							bool flag6 = exportToVUxyzForm.ShowDialog() == DialogResult.OK;
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

		// Token: 0x0600007F RID: 127 RVA: 0x00003328 File Offset: 0x00001528
		private bool verifyEntitlement(string appId, string userId)
		{
			RestClient restClient = new RestClient();
			restClient.BaseUrl = new Uri("https://apps.exchange.autodesk.com");
			RestRequest restRequest = new RestRequest();
			restRequest.Resource = "webservices/checkentitlement";
			restRequest.Method = Method.GET;
			restRequest.AddParameter("userid", userId);
			restRequest.AddParameter("appid", appId);
			IRestResponse<cl_ExportVUxyz.EntitlementResult> restResponse = restClient.Execute<cl_ExportVUxyz.EntitlementResult>(restRequest);
			bool result = false;
			bool flag = restResponse.Data != null && restResponse.Data.IsValid;
			if (flag)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000033B4 File Offset: 0x000015B4
		private bool verifyEntitlementEF(string userId, string userPassword, string secret)
		{
			RestClient restClient = new RestClient("https://www.emanuelfavreau.com/");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			restClient.Authenticator = new HttpBasicAuthenticator(userId, userPassword);
			RestRequest restRequest = new RestRequest("nina/users/accessVU.json", Method.GET);
			restRequest.RequestFormat = DataFormat.Json;
			restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
			IRestResponse<List<cl_ExportVUxyz.GetJsonResult>> restResponse = restClient.Execute<List<cl_ExportVUxyz.GetJsonResult>>(restRequest);
			bool result = false;
			bool flag = restResponse.Data != null;
			if (flag)
			{
				foreach (cl_ExportVUxyz.GetJsonResult getJsonResult in restResponse.Data)
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

		// Token: 0x0400003E RID: 62
		public const string _baseApiUrl = "https://apps.exchange.autodesk.com/";

		// Token: 0x0400003F RID: 63
		public const string _appId = "3759955758891315427";

		// Token: 0x0200001A RID: 26
		[Serializable]
		public class EntitlementResult
		{
			// Token: 0x17000053 RID: 83
			// (get) Token: 0x06000161 RID: 353 RVA: 0x000283DC File Offset: 0x000265DC
			// (set) Token: 0x06000162 RID: 354 RVA: 0x000283E4 File Offset: 0x000265E4
			public string UserId { get; set; }

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x06000163 RID: 355 RVA: 0x000283ED File Offset: 0x000265ED
			// (set) Token: 0x06000164 RID: 356 RVA: 0x000283F5 File Offset: 0x000265F5
			public string AppId { get; set; }

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000165 RID: 357 RVA: 0x000283FE File Offset: 0x000265FE
			// (set) Token: 0x06000166 RID: 358 RVA: 0x00028406 File Offset: 0x00026606
			public bool IsValid { get; set; }

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000167 RID: 359 RVA: 0x0002840F File Offset: 0x0002660F
			// (set) Token: 0x06000168 RID: 360 RVA: 0x00028417 File Offset: 0x00026617
			public string Message { get; set; }
		}

		// Token: 0x0200001B RID: 27
		public class GetJsonResult
		{
			// Token: 0x17000057 RID: 87
			// (get) Token: 0x0600016A RID: 362 RVA: 0x00028420 File Offset: 0x00026620
			// (set) Token: 0x0600016B RID: 363 RVA: 0x00028428 File Offset: 0x00026628
			public string client_secret { get; set; }

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x0600016C RID: 364 RVA: 0x00028431 File Offset: 0x00026631
			// (set) Token: 0x0600016D RID: 365 RVA: 0x00028439 File Offset: 0x00026639
			public string email { get; set; }

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x0600016E RID: 366 RVA: 0x00028442 File Offset: 0x00026642
			// (set) Token: 0x0600016F RID: 367 RVA: 0x0002844A File Offset: 0x0002664A
			public string link { get; set; }
		}
	}
}
