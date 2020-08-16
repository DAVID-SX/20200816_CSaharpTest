using System;
using System.Windows.Forms;

namespace ExportToVR
{
	// Token: 0x02000011 RID: 17
	public class GroupType
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x0001C5FC File Offset: 0x0001A7FC
		// (set) Token: 0x060000EA RID: 234 RVA: 0x0001C614 File Offset: 0x0001A814
		public string TheGroupType
		{
			get
			{
				return this.m_GroupType;
			}
			set
			{
				this.m_GroupType = value;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0001C620 File Offset: 0x0001A820
		private void ObtaineGroupType()
		{
			RadioButton radioButton = null;
			RadioButton radioButton2 = null;
			RadioButton radioButton3 = null;
			foreach (object obj in Form.ActiveForm.Controls)
			{
				Control control = (Control)obj;
				bool flag = control.Name == "radioButtonEntities";
				if (flag)
				{
					radioButton = (control as RadioButton);
				}
			}
			foreach (object obj2 in Form.ActiveForm.Controls)
			{
				Control control2 = (Control)obj2;
				bool flag2 = control2.Name == "radioButtonMaterials";
				if (flag2)
				{
					radioButton2 = (control2 as RadioButton);
				}
			}
			foreach (object obj3 in Form.ActiveForm.Controls)
			{
				Control control3 = (Control)obj3;
				bool flag3 = control3.Name == "radioButtonSingleObject";
				if (flag3)
				{
					radioButton3 = (control3 as RadioButton);
				}
			}
			bool @checked = radioButton.Checked;
			if (@checked)
			{
				this.TheGroupType = "radioButtonEntities";
			}
			bool checked2 = radioButton2.Checked;
			if (checked2)
			{
				this.TheGroupType = "radioButtonMaterials";
			}
			bool checked3 = radioButton3.Checked;
			if (checked3)
			{
				this.TheGroupType = "radioButtonSingleObject";
			}
		}

		// Token: 0x04000114 RID: 276
		private string m_GroupType;
	}
}
