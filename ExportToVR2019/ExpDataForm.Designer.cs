namespace ExportToVR
{
	// Token: 0x0200000C RID: 12
	public partial class ExpDataForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::ExportToVR.ExpDataForm));
			this.okButton = new global::System.Windows.Forms.Button();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.pBar1 = new global::System.Windows.Forms.ProgressBar();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cBoxOuvertures = new global::System.Windows.Forms.CheckBox();
			this.checkBoxId = new global::System.Windows.Forms.CheckBox();
			this.cBoxAssemCode = new global::System.Windows.Forms.CheckBox();
			this.checkBoxList = new global::System.Windows.Forms.CheckBox();
			this.feetButton = new global::System.Windows.Forms.RadioButton();
			this.mmButton = new global::System.Windows.Forms.RadioButton();
			this.inchesButton = new global::System.Windows.Forms.RadioButton();
			this.mButton = new global::System.Windows.Forms.RadioButton();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.listBoxParameters = new global::System.Windows.Forms.ListBox();
			this.buttonParamList = new global::System.Windows.Forms.Button();
			this.checkBoxToMono = new global::System.Windows.Forms.CheckBox();
			this.checkBoxRoomToMono = new global::System.Windows.Forms.CheckBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.checkBoxToVU = new global::System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.okButton.Location = new global::System.Drawing.Point(363, 864);
			this.okButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(253, 85);
			this.okButton.TabIndex = 7;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new global::System.EventHandler(this.okButton_Click);
			this.cancelButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new global::System.Drawing.Point(627, 864);
			this.cancelButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(211, 85);
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "&CANCEL";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.pBar1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.pBar1.Location = new global::System.Drawing.Point(64, 962);
			this.pBar1.Margin = new global::System.Windows.Forms.Padding(6);
			this.pBar1.Name = "pBar1";
			this.pBar1.Size = new global::System.Drawing.Size(774, 28);
			this.pBar1.TabIndex = 14;
			this.pBar1.Visible = false;
			this.label1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.label1.AutoSize = true;
			this.label1.ForeColor = global::System.Drawing.SystemColors.ControlDarkDark;
			this.label1.Location = new global::System.Drawing.Point(64, 930);
			this.label1.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(173, 25);
			this.label1.TabIndex = 15;
			this.label1.Text = "Properties Read = ";
			this.label1.Visible = false;
			this.cBoxOuvertures.AutoSize = true;
			this.cBoxOuvertures.Enabled = false;
			this.cBoxOuvertures.Location = new global::System.Drawing.Point(68, 476);
			this.cBoxOuvertures.Margin = new global::System.Windows.Forms.Padding(6);
			this.cBoxOuvertures.Name = "cBoxOuvertures";
			this.cBoxOuvertures.Size = new global::System.Drawing.Size(135, 29);
			this.cBoxOuvertures.TabIndex = 0;
			this.cBoxOuvertures.Text = "Ouvertures";
			this.cBoxOuvertures.UseVisualStyleBackColor = true;
			this.cBoxOuvertures.Visible = false;
			this.checkBoxId.AutoSize = true;
			this.checkBoxId.Enabled = false;
			this.checkBoxId.Location = new global::System.Drawing.Point(68, 519);
			this.checkBoxId.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxId.Name = "checkBoxId";
			this.checkBoxId.Size = new global::System.Drawing.Size(54, 29);
			this.checkBoxId.TabIndex = 21;
			this.checkBoxId.Text = "Id";
			this.checkBoxId.UseVisualStyleBackColor = true;
			this.checkBoxId.Visible = false;
			this.cBoxAssemCode.AutoSize = true;
			this.cBoxAssemCode.Enabled = false;
			this.cBoxAssemCode.Location = new global::System.Drawing.Point(64, 438);
			this.cBoxAssemCode.Margin = new global::System.Windows.Forms.Padding(6);
			this.cBoxAssemCode.Name = "cBoxAssemCode";
			this.cBoxAssemCode.Size = new global::System.Drawing.Size(187, 29);
			this.cBoxAssemCode.TabIndex = 22;
			this.cBoxAssemCode.Text = "Assembly Codes";
			this.cBoxAssemCode.UseVisualStyleBackColor = true;
			this.cBoxAssemCode.Visible = false;
			this.checkBoxList.AutoSize = true;
			this.checkBoxList.Enabled = false;
			this.checkBoxList.Location = new global::System.Drawing.Point(68, 561);
			this.checkBoxList.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxList.Name = "checkBoxList";
			this.checkBoxList.Size = new global::System.Drawing.Size(68, 29);
			this.checkBoxList.TabIndex = 23;
			this.checkBoxList.Text = "List";
			this.checkBoxList.UseVisualStyleBackColor = true;
			this.checkBoxList.Visible = false;
			this.feetButton.AutoSize = true;
			this.feetButton.Checked = true;
			this.feetButton.Location = new global::System.Drawing.Point(83, 41);
			this.feetButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.feetButton.Name = "feetButton";
			this.feetButton.Size = new global::System.Drawing.Size(76, 29);
			this.feetButton.TabIndex = 11;
			this.feetButton.TabStop = true;
			this.feetButton.Text = "Feet";
			this.feetButton.UseVisualStyleBackColor = true;
			this.feetButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
			this.mmButton.AutoSize = true;
			this.mmButton.Location = new global::System.Drawing.Point(83, 168);
			this.mmButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.mmButton.Name = "mmButton";
			this.mmButton.Size = new global::System.Drawing.Size(129, 29);
			this.mmButton.TabIndex = 10;
			this.mmButton.Text = "Millimeters";
			this.mmButton.UseVisualStyleBackColor = true;
			this.mmButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
			this.inchesButton.AutoSize = true;
			this.inchesButton.Location = new global::System.Drawing.Point(83, 83);
			this.inchesButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.inchesButton.Name = "inchesButton";
			this.inchesButton.Size = new global::System.Drawing.Size(95, 29);
			this.inchesButton.TabIndex = 12;
			this.inchesButton.Text = "Inches";
			this.inchesButton.UseVisualStyleBackColor = true;
			this.inchesButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
			this.mButton.AutoSize = true;
			this.mButton.Location = new global::System.Drawing.Point(83, 126);
			this.mButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.mButton.Name = "mButton";
			this.mButton.Size = new global::System.Drawing.Size(97, 29);
			this.mButton.TabIndex = 13;
			this.mButton.Text = "Meters";
			this.mButton.UseVisualStyleBackColor = true;
			this.mButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
			this.groupBox1.Controls.Add(this.mButton);
			this.groupBox1.Controls.Add(this.inchesButton);
			this.groupBox1.Controls.Add(this.mmButton);
			this.groupBox1.Controls.Add(this.feetButton);
			this.groupBox1.Location = new global::System.Drawing.Point(64, 118);
			this.groupBox1.Margin = new global::System.Windows.Forms.Padding(6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new global::System.Windows.Forms.Padding(6);
			this.groupBox1.Size = new global::System.Drawing.Size(255, 233);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Units";
			this.listBoxParameters.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.listBoxParameters.FormattingEnabled = true;
			this.listBoxParameters.HorizontalScrollbar = true;
			this.listBoxParameters.ItemHeight = 24;
			this.listBoxParameters.Location = new global::System.Drawing.Point(363, 129);
			this.listBoxParameters.Margin = new global::System.Windows.Forms.Padding(6);
			this.listBoxParameters.Name = "listBoxParameters";
			this.listBoxParameters.Size = new global::System.Drawing.Size(472, 676);
			this.listBoxParameters.TabIndex = 242;
			this.listBoxParameters.SelectedIndexChanged += new global::System.EventHandler(this.listBoxParameters_SelectedIndexChanged);
			this.buttonParamList.Location = new global::System.Drawing.Point(363, 70);
			this.buttonParamList.Margin = new global::System.Windows.Forms.Padding(6);
			this.buttonParamList.Name = "buttonParamList";
			this.buttonParamList.Size = new global::System.Drawing.Size(253, 42);
			this.buttonParamList.TabIndex = 243;
			this.buttonParamList.Text = "Show Parameters";
			this.buttonParamList.UseVisualStyleBackColor = true;
			this.buttonParamList.Click += new global::System.EventHandler(this.buttonParamList_Click);
			this.checkBoxToMono.AutoSize = true;
			this.checkBoxToMono.Checked = true;
			this.checkBoxToMono.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBoxToMono.Enabled = false;
			this.checkBoxToMono.Location = new global::System.Drawing.Point(64, 604);
			this.checkBoxToMono.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxToMono.Name = "checkBoxToMono";
			this.checkBoxToMono.Size = new global::System.Drawing.Size(112, 29);
			this.checkBoxToMono.TabIndex = 244;
			this.checkBoxToMono.Text = "ToMono";
			this.checkBoxToMono.UseVisualStyleBackColor = true;
			this.checkBoxToMono.Visible = false;
			this.checkBoxRoomToMono.AutoSize = true;
			this.checkBoxRoomToMono.Enabled = false;
			this.checkBoxRoomToMono.Location = new global::System.Drawing.Point(64, 646);
			this.checkBoxRoomToMono.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxRoomToMono.Name = "checkBoxRoomToMono";
			this.checkBoxRoomToMono.Size = new global::System.Drawing.Size(173, 29);
			this.checkBoxRoomToMono.TabIndex = 245;
			this.checkBoxRoomToMono.Text = "RoomsToMono";
			this.checkBoxRoomToMono.UseVisualStyleBackColor = true;
			this.checkBoxRoomToMono.Visible = false;
			this.label2.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(363, 814);
			this.label2.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(254, 25);
			this.label2.TabIndex = 246;
			this.label2.Text = "Select Parameters to Export";
			this.checkBoxToVU.AutoSize = true;
			this.checkBoxToVU.Enabled = false;
			this.checkBoxToVU.Location = new global::System.Drawing.Point(147, 397);
			this.checkBoxToVU.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxToVU.Name = "checkBoxToVU";
			this.checkBoxToVU.Size = new global::System.Drawing.Size(90, 29);
			this.checkBoxToVU.TabIndex = 247;
			this.checkBoxToVU.Text = "ToVU";
			this.checkBoxToVU.UseVisualStyleBackColor = true;
			this.checkBoxToVU.Visible = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(11f, 24f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(893, 1002);
			base.Controls.Add(this.checkBoxToVU);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.checkBoxRoomToMono);
			base.Controls.Add(this.checkBoxToMono);
			base.Controls.Add(this.buttonParamList);
			base.Controls.Add(this.listBoxParameters);
			base.Controls.Add(this.checkBoxList);
			base.Controls.Add(this.cBoxAssemCode);
			base.Controls.Add(this.checkBoxId);
			base.Controls.Add(this.cBoxOuvertures);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.pBar1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.groupBox1);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new global::System.Windows.Forms.Padding(6);
			this.MinimumSize = new global::System.Drawing.Size(897, 673);
			base.Name = "ExpDataForm";
			this.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.Text = "ef | Export Properties To Unity";
			base.Load += new global::System.EventHandler(this.ExpDataForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000085 RID: 133
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000086 RID: 134
		private global::System.Windows.Forms.Button okButton;

		// Token: 0x04000087 RID: 135
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x04000088 RID: 136
		private global::System.Windows.Forms.ProgressBar pBar1;

		// Token: 0x04000089 RID: 137
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400008A RID: 138
		private global::System.Windows.Forms.CheckBox cBoxOuvertures;

		// Token: 0x0400008B RID: 139
		private global::System.Windows.Forms.CheckBox checkBoxId;

		// Token: 0x0400008C RID: 140
		private global::System.Windows.Forms.CheckBox cBoxAssemCode;

		// Token: 0x0400008D RID: 141
		private global::System.Windows.Forms.CheckBox checkBoxList;

		// Token: 0x0400008E RID: 142
		private global::System.Windows.Forms.RadioButton feetButton;

		// Token: 0x0400008F RID: 143
		private global::System.Windows.Forms.RadioButton mmButton;

		// Token: 0x04000090 RID: 144
		private global::System.Windows.Forms.RadioButton inchesButton;

		// Token: 0x04000091 RID: 145
		private global::System.Windows.Forms.RadioButton mButton;

		// Token: 0x04000092 RID: 146
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000093 RID: 147
		private global::System.Windows.Forms.ListBox listBoxParameters;

		// Token: 0x04000094 RID: 148
		private global::System.Windows.Forms.Button buttonParamList;

		// Token: 0x04000095 RID: 149
		private global::System.Windows.Forms.CheckBox checkBoxToMono;

		// Token: 0x04000096 RID: 150
		private global::System.Windows.Forms.CheckBox checkBoxRoomToMono;

		// Token: 0x04000097 RID: 151
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000098 RID: 152
		private global::System.Windows.Forms.CheckBox checkBoxToVU;
	}
}
