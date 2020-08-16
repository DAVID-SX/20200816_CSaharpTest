namespace ExportToVR
{
	// Token: 0x02000010 RID: 16
	public partial class ExportToVUForm : global::System.Windows.Forms.Form
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x0001BAFC File Offset: 0x00019CFC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0001BB34 File Offset: 0x00019D34
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::ExportToVR.ExportToVUForm));
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.okButton = new global::System.Windows.Forms.Button();
			this.pBar1 = new global::System.Windows.Forms.ProgressBar();
			this.checkBoxTrialVersion = new global::System.Windows.Forms.CheckBox();
			this.radioButtonSingleObject = new global::System.Windows.Forms.RadioButton();
			this.label1 = new global::System.Windows.Forms.Label();
			this.radioButtonByTypes = new global::System.Windows.Forms.RadioButton();
			this.radioButtonMaterialsFast = new global::System.Windows.Forms.RadioButton();
			this.checkBoxStartVU = new global::System.Windows.Forms.CheckBox();
			this.checkBoxDeleteOBJ = new global::System.Windows.Forms.CheckBox();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.checkBoxLinkTransp = new global::System.Windows.Forms.CheckBox();
			this.checkBoxOverride = new global::System.Windows.Forms.CheckBox();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.cancelButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new global::System.Drawing.Point(486, 390);
			this.cancelButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(183, 83);
			this.cancelButton.TabIndex = 22;
			this.cancelButton.Text = "&CANCEL";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.okButton.Location = new global::System.Drawing.Point(235, 390);
			this.okButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(238, 83);
			this.okButton.TabIndex = 21;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new global::System.EventHandler(this.buttonOK_Click);
			this.pBar1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.pBar1.ForeColor = global::System.Drawing.Color.Blue;
			this.pBar1.Location = new global::System.Drawing.Point(26, 475);
			this.pBar1.Margin = new global::System.Windows.Forms.Padding(6);
			this.pBar1.Name = "pBar1";
			this.pBar1.Size = new global::System.Drawing.Size(644, 15);
			this.pBar1.TabIndex = 31;
			this.pBar1.Visible = false;
			this.checkBoxTrialVersion.AutoSize = true;
			this.checkBoxTrialVersion.Enabled = false;
			this.checkBoxTrialVersion.ForeColor = global::System.Drawing.SystemColors.HotTrack;
			this.checkBoxTrialVersion.Location = new global::System.Drawing.Point(326, 22);
			this.checkBoxTrialVersion.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxTrialVersion.Name = "checkBoxTrialVersion";
			this.checkBoxTrialVersion.Size = new global::System.Drawing.Size(76, 29);
			this.checkBoxTrialVersion.TabIndex = 117;
			this.checkBoxTrialVersion.Text = "Trial";
			this.checkBoxTrialVersion.UseVisualStyleBackColor = true;
			this.checkBoxTrialVersion.Visible = false;
			this.radioButtonSingleObject.AutoSize = true;
			this.radioButtonSingleObject.Enabled = false;
			this.radioButtonSingleObject.Location = new global::System.Drawing.Point(477, 111);
			this.radioButtonSingleObject.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonSingleObject.Name = "radioButtonSingleObject";
			this.radioButtonSingleObject.Size = new global::System.Drawing.Size(197, 29);
			this.radioButtonSingleObject.TabIndex = 223;
			this.radioButtonSingleObject.Text = "One Single Object";
			this.radioButtonSingleObject.UseVisualStyleBackColor = true;
			this.radioButtonSingleObject.Visible = false;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(69, 124);
			this.label1.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(138, 25);
			this.label1.TabIndex = 224;
			this.label1.Text = "BY OBJECTS";
			this.radioButtonByTypes.AutoSize = true;
			this.radioButtonByTypes.Location = new global::System.Drawing.Point(74, 213);
			this.radioButtonByTypes.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonByTypes.Name = "radioButtonByTypes";
			this.radioButtonByTypes.Size = new global::System.Drawing.Size(77, 29);
			this.radioButtonByTypes.TabIndex = 232;
			this.radioButtonByTypes.Text = "YES";
			this.radioButtonByTypes.UseVisualStyleBackColor = true;
			this.radioButtonMaterialsFast.AutoSize = true;
			this.radioButtonMaterialsFast.Checked = true;
			this.radioButtonMaterialsFast.Location = new global::System.Drawing.Point(74, 172);
			this.radioButtonMaterialsFast.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonMaterialsFast.Name = "radioButtonMaterialsFast";
			this.radioButtonMaterialsFast.Size = new global::System.Drawing.Size(67, 29);
			this.radioButtonMaterialsFast.TabIndex = 233;
			this.radioButtonMaterialsFast.TabStop = true;
			this.radioButtonMaterialsFast.Text = "NO";
			this.radioButtonMaterialsFast.UseVisualStyleBackColor = true;
			this.checkBoxStartVU.AutoSize = true;
			this.checkBoxStartVU.Checked = true;
			this.checkBoxStartVU.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBoxStartVU.Enabled = false;
			this.checkBoxStartVU.Location = new global::System.Drawing.Point(411, 295);
			this.checkBoxStartVU.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxStartVU.Name = "checkBoxStartVU";
			this.checkBoxStartVU.Size = new global::System.Drawing.Size(112, 29);
			this.checkBoxStartVU.TabIndex = 234;
			this.checkBoxStartVU.Text = "Start VU";
			this.checkBoxStartVU.UseVisualStyleBackColor = true;
			this.checkBoxStartVU.Visible = false;
			this.checkBoxDeleteOBJ.AutoSize = true;
			this.checkBoxDeleteOBJ.Checked = true;
			this.checkBoxDeleteOBJ.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.checkBoxDeleteOBJ.Enabled = false;
			this.checkBoxDeleteOBJ.Location = new global::System.Drawing.Point(411, 336);
			this.checkBoxDeleteOBJ.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxDeleteOBJ.Name = "checkBoxDeleteOBJ";
			this.checkBoxDeleteOBJ.Size = new global::System.Drawing.Size(228, 29);
			this.checkBoxDeleteOBJ.TabIndex = 235;
			this.checkBoxDeleteOBJ.Text = "Replace Existing Files";
			this.checkBoxDeleteOBJ.UseVisualStyleBackColor = true;
			this.checkBoxDeleteOBJ.Visible = false;
			this.pictureBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.pictureBox1.Image = global::ExportToVR.Properties.Resources.ExportToVR_Gris_36X36;
			this.pictureBox1.Location = new global::System.Drawing.Point(603, 22);
			this.pictureBox1.Margin = new global::System.Windows.Forms.Padding(6);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(66, 66);
			this.pictureBox1.TabIndex = 220;
			this.pictureBox1.TabStop = false;
			this.checkBoxLinkTransp.AutoSize = true;
			this.checkBoxLinkTransp.Location = new global::System.Drawing.Point(296, 213);
			this.checkBoxLinkTransp.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxLinkTransp.Name = "checkBoxLinkTransp";
			this.checkBoxLinkTransp.Size = new global::System.Drawing.Size(151, 29);
			this.checkBoxLinkTransp.TabIndex = 236;
			this.checkBoxLinkTransp.Text = "Links Transp";
			this.checkBoxLinkTransp.UseVisualStyleBackColor = true;
			this.checkBoxLinkTransp.CheckedChanged += new global::System.EventHandler(this.checkBoxLinkTransp_CheckedChanged);
			this.checkBoxOverride.AutoSize = true;
			this.checkBoxOverride.Location = new global::System.Drawing.Point(296, 254);
			this.checkBoxOverride.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxOverride.Name = "checkBoxOverride";
			this.checkBoxOverride.Size = new global::System.Drawing.Size(175, 29);
			this.checkBoxOverride.TabIndex = 237;
			this.checkBoxOverride.Text = "Override Colors";
			this.checkBoxOverride.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(11f, 24f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(689, 496);
			base.Controls.Add(this.checkBoxOverride);
			base.Controls.Add(this.checkBoxLinkTransp);
			base.Controls.Add(this.checkBoxDeleteOBJ);
			base.Controls.Add(this.checkBoxStartVU);
			base.Controls.Add(this.radioButtonMaterialsFast);
			base.Controls.Add(this.radioButtonByTypes);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.radioButtonSingleObject);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.pBar1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.checkBoxTrialVersion);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new global::System.Windows.Forms.Padding(6);
			this.MaximumSize = new global::System.Drawing.Size(713, 869);
			this.MinimumSize = new global::System.Drawing.Size(713, 560);
			base.Name = "ExportToVUForm";
			this.Text = "ef | Export To VU";
			base.Load += new global::System.EventHandler(this.ViewForm_Load);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000106 RID: 262
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000107 RID: 263
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x04000108 RID: 264
		private global::System.Windows.Forms.Button okButton;

		// Token: 0x04000109 RID: 265
		private global::System.Windows.Forms.ProgressBar pBar1;

		// Token: 0x0400010A RID: 266
		private global::System.Windows.Forms.CheckBox checkBoxTrialVersion;

		// Token: 0x0400010B RID: 267
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x0400010C RID: 268
		private global::System.Windows.Forms.RadioButton radioButtonSingleObject;

		// Token: 0x0400010D RID: 269
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400010E RID: 270
		private global::System.Windows.Forms.RadioButton radioButtonByTypes;

		// Token: 0x0400010F RID: 271
		private global::System.Windows.Forms.RadioButton radioButtonMaterialsFast;

		// Token: 0x04000110 RID: 272
		private global::System.Windows.Forms.CheckBox checkBoxStartVU;

		// Token: 0x04000111 RID: 273
		private global::System.Windows.Forms.CheckBox checkBoxDeleteOBJ;

		// Token: 0x04000112 RID: 274
		private global::System.Windows.Forms.CheckBox checkBoxLinkTransp;

		// Token: 0x04000113 RID: 275
		private global::System.Windows.Forms.CheckBox checkBoxOverride;
	}
}
