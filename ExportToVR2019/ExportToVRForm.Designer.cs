namespace ExportToVR
{
	// Token: 0x02000015 RID: 21
	public partial class ExportToVRForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000139 RID: 313 RVA: 0x000273E4 File Offset: 0x000255E4
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0002741C File Offset: 0x0002561C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::ExportToVR.ExportToVRForm));
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.okButton = new global::System.Windows.Forms.Button();
			this.pBar1 = new global::System.Windows.Forms.ProgressBar();
			this.checkBoxTrialVersion = new global::System.Windows.Forms.CheckBox();
			this.radioButtonMaterials = new global::System.Windows.Forms.RadioButton();
			this.radioButtonEntities = new global::System.Windows.Forms.RadioButton();
			this.radioButtonSingleObject = new global::System.Windows.Forms.RadioButton();
			this.label1 = new global::System.Windows.Forms.Label();
			this.checkBoxMaxVertices = new global::System.Windows.Forms.CheckBox();
			this.trackBar1 = new global::System.Windows.Forms.TrackBar();
			this.labelVertices = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.radioButtonSubcategories = new global::System.Windows.Forms.RadioButton();
			this.radioButtonByTypes = new global::System.Windows.Forms.RadioButton();
			this.radioButtonMaterialsFast = new global::System.Windows.Forms.RadioButton();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			((global::System.ComponentModel.ISupportInitialize)this.trackBar1).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.cancelButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new global::System.Drawing.Point(486, 452);
			this.cancelButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(183, 83);
			this.cancelButton.TabIndex = 22;
			this.cancelButton.Text = "&CANCEL";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.okButton.Location = new global::System.Drawing.Point(235, 452);
			this.okButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(238, 83);
			this.okButton.TabIndex = 21;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new global::System.EventHandler(this.buttonOK_Click);
			this.pBar1.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.pBar1.ForeColor = global::System.Drawing.Color.Blue;
			this.pBar1.Location = new global::System.Drawing.Point(26, 537);
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
			this.radioButtonMaterials.AutoSize = true;
			this.radioButtonMaterials.Enabled = false;
			this.radioButtonMaterials.Location = new global::System.Drawing.Point(518, 296);
			this.radioButtonMaterials.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonMaterials.Name = "radioButtonMaterials";
			this.radioButtonMaterials.Size = new global::System.Drawing.Size(124, 29);
			this.radioButtonMaterials.TabIndex = 221;
			this.radioButtonMaterials.Text = "ByMatOld";
			this.radioButtonMaterials.UseVisualStyleBackColor = true;
			this.radioButtonMaterials.Visible = false;
			this.radioButtonEntities.AutoSize = true;
			this.radioButtonEntities.Enabled = false;
			this.radioButtonEntities.Location = new global::System.Drawing.Point(518, 338);
			this.radioButtonEntities.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonEntities.Name = "radioButtonEntities";
			this.radioButtonEntities.Size = new global::System.Drawing.Size(120, 29);
			this.radioButtonEntities.TabIndex = 222;
			this.radioButtonEntities.Text = "ByEntOld";
			this.radioButtonEntities.UseVisualStyleBackColor = true;
			this.radioButtonEntities.Visible = false;
			this.radioButtonSingleObject.AutoSize = true;
			this.radioButtonSingleObject.Location = new global::System.Drawing.Point(74, 254);
			this.radioButtonSingleObject.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonSingleObject.Name = "radioButtonSingleObject";
			this.radioButtonSingleObject.Size = new global::System.Drawing.Size(197, 29);
			this.radioButtonSingleObject.TabIndex = 223;
			this.radioButtonSingleObject.Text = "One Single Object";
			this.radioButtonSingleObject.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(55, 113);
			this.label1.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(171, 25);
			this.label1.TabIndex = 224;
			this.label1.Text = "Grouping Options:";
			this.checkBoxMaxVertices.AutoSize = true;
			this.checkBoxMaxVertices.Enabled = false;
			this.checkBoxMaxVertices.Location = new global::System.Drawing.Point(60, 379);
			this.checkBoxMaxVertices.Margin = new global::System.Windows.Forms.Padding(6);
			this.checkBoxMaxVertices.Name = "checkBoxMaxVertices";
			this.checkBoxMaxVertices.Size = new global::System.Drawing.Size(142, 29);
			this.checkBoxMaxVertices.TabIndex = 225;
			this.checkBoxMaxVertices.Text = "MxVrt/OBJ:";
			this.checkBoxMaxVertices.UseVisualStyleBackColor = true;
			this.checkBoxMaxVertices.Visible = false;
			this.checkBoxMaxVertices.CheckedChanged += new global::System.EventHandler(this.checkBoxMaxVertices_CheckedChanged);
			this.trackBar1.Enabled = false;
			this.trackBar1.LargeChange = 5000;
			this.trackBar1.Location = new global::System.Drawing.Point(60, 445);
			this.trackBar1.Margin = new global::System.Windows.Forms.Padding(6);
			this.trackBar1.Maximum = 2000000;
			this.trackBar1.Minimum = 200000;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new global::System.Drawing.Size(133, 80);
			this.trackBar1.SmallChange = 1000;
			this.trackBar1.TabIndex = 228;
			this.trackBar1.Value = 1000000;
			this.trackBar1.Visible = false;
			this.trackBar1.Scroll += new global::System.EventHandler(this.trackBar1_Scroll);
			this.labelVertices.AutoSize = true;
			this.labelVertices.Enabled = false;
			this.labelVertices.Location = new global::System.Drawing.Point(83, 414);
			this.labelVertices.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.labelVertices.Name = "labelVertices";
			this.labelVertices.Size = new global::System.Drawing.Size(89, 25);
			this.labelVertices.TabIndex = 229;
			this.labelVertices.Text = "1000000";
			this.labelVertices.Visible = false;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(202, 215);
			this.label2.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(204, 25);
			this.label2.TabIndex = 230;
			this.label2.Text = "(To Export Properties)";
			this.radioButtonSubcategories.AutoSize = true;
			this.radioButtonSubcategories.Enabled = false;
			this.radioButtonSubcategories.Location = new global::System.Drawing.Point(518, 379);
			this.radioButtonSubcategories.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonSubcategories.Name = "radioButtonSubcategories";
			this.radioButtonSubcategories.Size = new global::System.Drawing.Size(156, 29);
			this.radioButtonSubcategories.TabIndex = 231;
			this.radioButtonSubcategories.Text = "ByEntSubOld";
			this.radioButtonSubcategories.UseVisualStyleBackColor = true;
			this.radioButtonSubcategories.Visible = false;
			this.radioButtonByTypes.AutoSize = true;
			this.radioButtonByTypes.Location = new global::System.Drawing.Point(74, 213);
			this.radioButtonByTypes.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonByTypes.Name = "radioButtonByTypes";
			this.radioButtonByTypes.Size = new global::System.Drawing.Size(128, 29);
			this.radioButtonByTypes.TabIndex = 232;
			this.radioButtonByTypes.Text = "By Entities";
			this.radioButtonByTypes.UseVisualStyleBackColor = true;
			this.radioButtonMaterialsFast.AutoSize = true;
			this.radioButtonMaterialsFast.Checked = true;
			this.radioButtonMaterialsFast.Location = new global::System.Drawing.Point(74, 172);
			this.radioButtonMaterialsFast.Margin = new global::System.Windows.Forms.Padding(6);
			this.radioButtonMaterialsFast.Name = "radioButtonMaterialsFast";
			this.radioButtonMaterialsFast.Size = new global::System.Drawing.Size(144, 29);
			this.radioButtonMaterialsFast.TabIndex = 233;
			this.radioButtonMaterialsFast.TabStop = true;
			this.radioButtonMaterialsFast.Text = "By Materials";
			this.radioButtonMaterialsFast.UseVisualStyleBackColor = true;
			this.pictureBox1.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.pictureBox1.Image = global::ExportToVR.Properties.Resources.ExportToVR_Gris_36X36;
			this.pictureBox1.Location = new global::System.Drawing.Point(603, 22);
			this.pictureBox1.Margin = new global::System.Windows.Forms.Padding(6);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(66, 66);
			this.pictureBox1.TabIndex = 220;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(11f, 24f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(689, 558);
			base.Controls.Add(this.radioButtonMaterialsFast);
			base.Controls.Add(this.radioButtonByTypes);
			base.Controls.Add(this.radioButtonSubcategories);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.labelVertices);
			base.Controls.Add(this.trackBar1);
			base.Controls.Add(this.checkBoxMaxVertices);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.radioButtonSingleObject);
			base.Controls.Add(this.radioButtonMaterials);
			base.Controls.Add(this.radioButtonEntities);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.pBar1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.okButton);
			base.Controls.Add(this.checkBoxTrialVersion);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new global::System.Windows.Forms.Padding(6);
			this.MaximumSize = new global::System.Drawing.Size(713, 869);
			this.MinimumSize = new global::System.Drawing.Size(713, 574);
			base.Name = "ExportToVRForm";
			this.Text = "ef | Export To Unity";
			base.Load += new global::System.EventHandler(this.ViewForm_Load);
			((global::System.ComponentModel.ISupportInitialize)this.trackBar1).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000180 RID: 384
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000181 RID: 385
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x04000182 RID: 386
		private global::System.Windows.Forms.Button okButton;

		// Token: 0x04000183 RID: 387
		private global::System.Windows.Forms.ProgressBar pBar1;

		// Token: 0x04000184 RID: 388
		private global::System.Windows.Forms.CheckBox checkBoxTrialVersion;

		// Token: 0x04000185 RID: 389
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000186 RID: 390
		private global::System.Windows.Forms.RadioButton radioButtonMaterials;

		// Token: 0x04000187 RID: 391
		private global::System.Windows.Forms.RadioButton radioButtonEntities;

		// Token: 0x04000188 RID: 392
		private global::System.Windows.Forms.RadioButton radioButtonSingleObject;

		// Token: 0x04000189 RID: 393
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400018A RID: 394
		private global::System.Windows.Forms.CheckBox checkBoxMaxVertices;

		// Token: 0x0400018B RID: 395
		private global::System.Windows.Forms.TrackBar trackBar1;

		// Token: 0x0400018C RID: 396
		private global::System.Windows.Forms.Label labelVertices;

		// Token: 0x0400018D RID: 397
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400018E RID: 398
		private global::System.Windows.Forms.RadioButton radioButtonSubcategories;

		// Token: 0x0400018F RID: 399
		private global::System.Windows.Forms.RadioButton radioButtonByTypes;

		// Token: 0x04000190 RID: 400
		private global::System.Windows.Forms.RadioButton radioButtonMaterialsFast;
	}
}
