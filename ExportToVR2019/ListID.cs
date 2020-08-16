using System;
using System.Collections.Generic;

namespace ExportToVR
{
	// Token: 0x02000014 RID: 20
	public class ListID
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000245A8 File Offset: 0x000227A8
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000245C0 File Offset: 0x000227C0
		public List<string> IDListName
		{
			get
			{
				return this.m_IDListName;
			}
			set
			{
				this.m_IDListName = value;
			}
		}

		// Token: 0x04000174 RID: 372
		private List<string> m_IDListName;

		// Token: 0x04000175 RID: 373
		public List<string> PubViewListName;
	}
}
