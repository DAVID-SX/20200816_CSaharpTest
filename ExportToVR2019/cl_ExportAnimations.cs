using System;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RestSharp;

namespace ExportToVR
{
	// Token: 0x02000003 RID: 3
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class cl_ExportAnimations : IExternalCommand
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00002B94 File Offset: 0x00000D94
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			Autodesk.Revit.ApplicationServices.Application application = commandData.Application.Application;
			Document document = commandData.Application.ActiveUIDocument.Document;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			string caption = "ef | Export To Unity";
			string versionNumber = application.VersionNumber;
			string subVersionNumber = application.SubVersionNumber;
			bool flag = versionNumber != "2018";
			Result result;
			if (flag)
			{
				MessageBox.Show("This version only works on Revit 2018.", caption, buttons, icon);
				return Result.Succeeded;
			}
			else
			{
				bool flag2 = subVersionNumber != "2018.1" & subVersionNumber != "2018.2" & subVersionNumber != "2018.3";
				if (flag2)
				{
					MessageBox.Show("This version only works on Revit 2018.1 or more.", caption, buttons, icon);
					return Result.Succeeded;
				}
				else
				{
					bool flag3 = document.ActiveView.ViewType != ViewType.ThreeD;
					if (flag3)
					{
						MessageBox.Show("The active view must be a 3D view type.");
						return Result.Succeeded;
					}
					else
					{
						bool flag4 = document.ActiveView.ViewType == ViewType.ThreeD & document.ActiveView.IsTemplate;
						if (flag4)
						{
							MessageBox.Show("The active view is a template view and is not exportable.");
							return Result.Succeeded;
						}
						else
						{
							try
							{
								AllViews allViews = new AllViews();
								allViews.ObtainAllViews(commandData);
								using (ExpAnimForm expAnimForm = new ExpAnimForm(commandData, allViews))
								{
									bool flag5 = expAnimForm.ShowDialog() == DialogResult.OK;
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
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002D20 File Offset: 0x00000F20
		private bool verifyEntitlement(string appId, string userId)
		{
			RestClient restClient = new RestClient();
			restClient.BaseUrl = new Uri("https://apps.exchange.autodesk.com");
			RestRequest restRequest = new RestRequest();
			restRequest.Resource = "webservices/checkentitlement";
			restRequest.Method = Method.GET;
			restRequest.AddParameter("userid", userId);
			restRequest.AddParameter("appid", appId);
			IRestResponse<cl_ExportAnimations.EntitlementResult> restResponse = restClient.Execute<cl_ExportAnimations.EntitlementResult>(restRequest);
			bool result = false;
			bool flag = restResponse.Data != null && restResponse.Data.IsValid;
			if (flag)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x04000038 RID: 56
		public const string _baseApiUrl = "https://apps.exchange.autodesk.com/";

		// Token: 0x04000039 RID: 57
		public const string _appId = "3759955758891315427";

		// Token: 0x02000017 RID: 23
		[Serializable]
		public class EntitlementResult
		{
			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000148 RID: 328 RVA: 0x00028321 File Offset: 0x00026521
			// (set) Token: 0x06000149 RID: 329 RVA: 0x00028329 File Offset: 0x00026529
			public string UserId { get; set; }

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x0600014A RID: 330 RVA: 0x00028332 File Offset: 0x00026532
			// (set) Token: 0x0600014B RID: 331 RVA: 0x0002833A File Offset: 0x0002653A
			public string AppId { get; set; }

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x0600014C RID: 332 RVA: 0x00028343 File Offset: 0x00026543
			// (set) Token: 0x0600014D RID: 333 RVA: 0x0002834B File Offset: 0x0002654B
			public bool IsValid { get; set; }

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x0600014E RID: 334 RVA: 0x00028354 File Offset: 0x00026554
			// (set) Token: 0x0600014F RID: 335 RVA: 0x0002835C File Offset: 0x0002655C
			public string Message { get; set; }
		}
	}
}
