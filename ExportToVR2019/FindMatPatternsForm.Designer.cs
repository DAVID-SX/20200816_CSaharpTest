namespace ExportToVR
{
	// Token: 0x0200000D RID: 13
	public partial class FindMatPatternsForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000D6FC File Offset: 0x0000B8FC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000D734 File Offset: 0x0000B934
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::ExportToVR.FindMatPatternsForm));
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.okButton = new global::System.Windows.Forms.Button();
			this.pBar1 = new global::System.Windows.Forms.ProgressBar();
			this.checkBoxTrialVersion = new global::System.Windows.Forms.CheckBox();
			this.radioButtonSingleObject = new global::System.Windows.Forms.RadioButton();
			this.labelVertices = new global::System.Windows.Forms.Label();
			this.radioButtonByTypes = new global::System.Windows.Forms.RadioButton();
			this.radioButtonMaterialsFast = new global::System.Windows.Forms.RadioButton();
			this.checkBoxStartVU = new global::System.Windows.Forms.CheckBox();
			this.checkBoxLinkTransp = new global::System.Windows.Forms.CheckBox();
			this.checkBoxMaxVertices = new global::System.Windows.Forms.CheckBox();
			this.listBoxPatterns = new global::System.Windows.Forms.ListBox();
			this.buttonShowPatterns = new global::System.Windows.Forms.Button();
			this.buttonShowElements = new global::System.Windows.Forms.Button();
			this.listBoxElements = new global::System.Windows.Forms.ListBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.cancelButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new global::System.Drawing.Point(835, 1009);
			this.cancelButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(153, 60);
			this.cancelButton.TabIndex = 22;
			this.cancelButton.Text = "&CANCEL";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.okButton.Location = new global::System.Drawing.Point(744, 887);
			this.okButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(244, 83);
			this.okButton.TabIndex = 21;
			this.okButton.Text = "SELECTIONNER";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new global::System.EventHandler(this.buttonOK_Click);
			this.pBar1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.pBar1.ForeColor = global::System.Drawing.Color.Blue;
			this.pBar1.Location = new global::System.Drawing.Point(26, 1054);
			this.pBar1.Margin = new global::System.Windows.Forms.Padding(6);
			this.pBar1.Name = "pBar1";
			this.pBar1.Size = new global::System.Drawing.Size(549, 15);
			this.pBar1.TabIndex = 31;
			this.pBar1.Visible = false;
			this.checkBoxTrialVersion.AutoSize = true;
			this.checkBoxTrialVersion.Enabled = false;
			this.checkBoxTrialVersion.ForeColor = global::System.Drawing.SystemColors.HotTrack;
			this.checkBoxTrialVersion.Location = new global::System.Drawing.Point(40, 28);
			this.checkBoxTrialVersion.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxTrialVersion.Name = "checkBoxTrialVersion";
			this.checkBoxTrialVersion.Size = new global::System.Drawing.Size(76, 29);
			this.checkBoxTrialVersion.TabIndex = 117;
			this.checkBoxTrialVersion.Text = "Trial";
			this.checkBoxTrialVersion.UseVisualStyleBackColor = true;
			this.checkBoxTrialVersion.Visible = false;
			this.radioButtonSingleObject.AutoSize = true;
			this.radioButtonSingleObject.Enabled = false;
			this.radioButtonSingleObject.Location = new global::System.Drawing.Point(10, 254);
			this.radioButtonSingleObject.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonSingleObject.Name = "radioButtonSingleObject";
			this.radioButtonSingleObject.Size = new global::System.Drawing.Size(75, 29);
			this.radioButtonSingleObject.TabIndex = 223;
			this.radioButtonSingleObject.Text = "One";
			this.radioButtonSingleObject.UseVisualStyleBackColor = true;
			this.radioButtonSingleObject.Visible = false;
			this.labelVertices.AutoSize = true;
			this.labelVertices.Enabled = false;
			this.labelVertices.Location = new global::System.Drawing.Point(33, 467);
			this.labelVertices.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.labelVertices.Name = "labelVertices";
			this.labelVertices.Size = new global::System.Drawing.Size(89, 25);
			this.labelVertices.TabIndex = 229;
			this.labelVertices.Text = "1000000";
			this.labelVertices.Visible = false;
			this.radioButtonByTypes.AutoSize = true;
			this.radioButtonByTypes.Enabled = false;
			this.radioButtonByTypes.Location = new global::System.Drawing.Point(10, 213);
			this.radioButtonByTypes.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonByTypes.Name = "radioButtonByTypes";
			this.radioButtonByTypes.Size = new global::System.Drawing.Size(128, 29);
			this.radioButtonByTypes.TabIndex = 232;
			this.radioButtonByTypes.Text = "By Entities";
			this.radioButtonByTypes.UseVisualStyleBackColor = true;
			this.radioButtonByTypes.Visible = false;
			this.radioButtonMaterialsFast.AutoSize = true;
			this.radioButtonMaterialsFast.Checked = true;
			this.radioButtonMaterialsFast.Enabled = false;
			this.radioButtonMaterialsFast.Location = new global::System.Drawing.Point(10, 172);
			this.radioButtonMaterialsFast.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonMaterialsFast.Name = "radioButtonMaterialsFast";
			this.radioButtonMaterialsFast.Size = new global::System.Drawing.Size(144, 29);
			this.radioButtonMaterialsFast.TabIndex = 233;
			this.radioButtonMaterialsFast.TabStop = true;
			this.radioButtonMaterialsFast.Text = "By Materials";
			this.radioButtonMaterialsFast.UseVisualStyleBackColor = true;
			this.radioButtonMaterialsFast.Visible = false;
			this.checkBoxStartVU.AutoSize = true;
			this.checkBoxStartVU.Enabled = false;
			this.checkBoxStartVU.Location = new global::System.Drawing.Point(10, 393);
			this.checkBoxStartVU.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxStartVU.Name = "checkBoxStartVU";
			this.checkBoxStartVU.Size = new global::System.Drawing.Size(112, 29);
			this.checkBoxStartVU.TabIndex = 234;
			this.checkBoxStartVU.Text = "Start VU";
			this.checkBoxStartVU.UseVisualStyleBackColor = true;
			this.checkBoxStartVU.Visible = false;
			this.checkBoxLinkTransp.AutoSize = true;
			this.checkBoxLinkTransp.Enabled = false;
			this.checkBoxLinkTransp.Location = new global::System.Drawing.Point(10, 432);
			this.checkBoxLinkTransp.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxLinkTransp.Name = "checkBoxLinkTransp";
			this.checkBoxLinkTransp.Size = new global::System.Drawing.Size(151, 29);
			this.checkBoxLinkTransp.TabIndex = 235;
			this.checkBoxLinkTransp.Text = "Links Transp";
			this.checkBoxLinkTransp.UseVisualStyleBackColor = true;
			this.checkBoxLinkTransp.Visible = false;
			this.checkBoxLinkTransp.CheckedChanged += new global::System.EventHandler(this.checkBoxLinkTransp_CheckedChanged);
			this.checkBoxMaxVertices.Enabled = false;
			this.checkBoxMaxVertices.Location = new global::System.Drawing.Point(10, 495);
			this.checkBoxMaxVertices.Name = "checkBoxMaxVertices";
			this.checkBoxMaxVertices.Size = new global::System.Drawing.Size(160, 32);
			this.checkBoxMaxVertices.TabIndex = 236;
			this.checkBoxMaxVertices.Text = "Max Vertices";
			this.checkBoxMaxVertices.Visible = false;
			this.listBoxPatterns.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.listBoxPatterns.FormattingEnabled = true;
			this.listBoxPatterns.ItemHeight = 24;
			this.listBoxPatterns.Location = new global::System.Drawing.Point(163, 124);
			this.listBoxPatterns.Name = "listBoxPatterns";
			this.listBoxPatterns.Size = new global::System.Drawing.Size(412, 748);
			this.listBoxPatterns.TabIndex = 237;
			this.listBoxPatterns.SelectedIndexChanged += new global::System.EventHandler(this.listBoxPatterns_SelectedIndexChanged);
			this.buttonShowPatterns.Location = new global::System.Drawing.Point(163, 59);
			this.buttonShowPatterns.Name = "buttonShowPatterns";
			this.buttonShowPatterns.Size = new global::System.Drawing.Size(181, 53);
			this.buttonShowPatterns.TabIndex = 238;
			this.buttonShowPatterns.Text = "PATTERNS";
			this.buttonShowPatterns.UseVisualStyleBackColor = true;
			this.buttonShowPatterns.Click += new global::System.EventHandler(this.buttonShowPatterns_Click);
			this.buttonShowElements.Location = new global::System.Drawing.Point(581, 59);
			this.buttonShowElements.Name = "buttonShowElements";
			this.buttonShowElements.Size = new global::System.Drawing.Size(181, 53);
			this.buttonShowElements.TabIndex = 240;
			this.buttonShowElements.Text = "ELEMENTS";
			this.buttonShowElements.UseVisualStyleBackColor = true;
			this.buttonShowElements.Click += new global::System.EventHandler(this.buttonShowElements_Click);
			this.listBoxElements.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.listBoxElements.FormattingEnabled = true;
			this.listBoxElements.ItemHeight = 24;
			this.listBoxElements.Location = new global::System.Drawing.Point(581, 124);
			this.listBoxElements.Name = "listBoxElements";
			this.listBoxElements.Size = new global::System.Drawing.Size(407, 748);
			this.listBoxElements.TabIndex = 239;
			this.listBoxElements.SelectedIndexChanged += new global::System.EventHandler(this.listBoxElements_SelectedIndexChanged);
			this.button1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.button1.Location = new global::System.Drawing.Point(744, 1009);
			this.button1.Margin = new global::System.Windows.Forms.Padding(6);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(85, 60);
			this.button1.TabIndex = 241;
			this.button1.Text = "&OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(129, 73);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(28, 25);
			this.label1.TabIndex = 242;
			this.label1.Text = "1.";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(545, 73);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(28, 25);
			this.label2.TabIndex = 243;
			this.label2.Text = "2.";
			this.label3.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(707, 916);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(28, 25);
			this.label3.TabIndex = 244;
			this.label3.Text = "3.";
			this.label4.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(707, 1027);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(28, 25);
			this.label4.TabIndex = 245;
			this.label4.Text = "4.";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(11f, 24f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1007, 1084);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.buttonShowElements);
			base.Controls.Add(this.listBoxElements);
			base.Controls.Add(this.buttonShowPatterns);
			base.Controls.Add(this.listBoxPatterns);
			base.Controls.Add(this.checkBoxLinkTransp);
			base.Controls.Add(this.checkBoxStartVU);
			base.Controls.Add(this.radioButtonMaterialsFast);
			base.Controls.Add(this.radioButtonByTypes);
			base.Controls.Add(this.labelVertices);
			base.Controls.Add(this.checkBoxMaxVertices);
			base.Controls.Add(this.radioButtonSingleObject);
			base.Controls.Add(this.pBar1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.checkBoxTrialVersion);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new global::System.Windows.Forms.Padding(6);
			this.MaximumSize = new global::System.Drawing.Size(2000, 2000);
			this.MinimumSize = new global::System.Drawing.Size(713, 574);
			base.Name = "FindMatPatternsForm";
			this.Text = "Patterns | Elements";
			base.Load += new global::System.EventHandler(this.ViewForm_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000A8 RID: 168
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000A9 RID: 169
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x040000AA RID: 170
		private global::System.Windows.Forms.Button okButton;

		// Token: 0x040000AB RID: 171
		private global::System.Windows.Forms.ProgressBar pBar1;

		// Token: 0x040000AC RID: 172
		private global::System.Windows.Forms.CheckBox checkBoxTrialVersion;

		// Token: 0x040000AD RID: 173
		private global::System.Windows.Forms.RadioButton radioButtonSingleObject;

		// Token: 0x040000AE RID: 174
		private global::System.Windows.Forms.Label labelVertices;

		// Token: 0x040000AF RID: 175
		private global::System.Windows.Forms.RadioButton radioButtonByTypes;

		// Token: 0x040000B0 RID: 176
		private global::System.Windows.Forms.RadioButton radioButtonMaterialsFast;

		// Token: 0x040000B1 RID: 177
		private global::System.Windows.Forms.CheckBox checkBoxStartVU;

		// Token: 0x040000B2 RID: 178
		private global::System.Windows.Forms.CheckBox checkBoxLinkTransp;

		// Token: 0x040000B3 RID: 179
		private global::System.Windows.Forms.CheckBox checkBoxMaxVertices;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.ListBox listBoxPatterns;

		// Token: 0x040000B5 RID: 181
		private global::System.Windows.Forms.Button buttonShowPatterns;

		// Token: 0x040000B6 RID: 182
		private global::System.Windows.Forms.Button buttonShowElements;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.ListBox listBoxElements;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.Label label4;
	}
}
