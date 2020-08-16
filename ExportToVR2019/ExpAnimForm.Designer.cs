namespace ExportToVR
{
	// Token: 0x0200000B RID: 11
	public partial class ExpAnimForm : global::System.Windows.Forms.Form
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000057DC File Offset: 0x000039DC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005814 File Offset: 0x00003A14
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::ExportToVR.ExpAnimForm));
			this.okButton = new global::System.Windows.Forms.Button();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.pBar1 = new global::System.Windows.Forms.ProgressBar();
			this.label1 = new global::System.Windows.Forms.Label();
			this.listBoxParameters = new global::System.Windows.Forms.ListBox();
			this.buttonParamList = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.listBoxComponents = new global::System.Windows.Forms.ListBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.mButton = new global::System.Windows.Forms.RadioButton();
			this.inchesButton = new global::System.Windows.Forms.RadioButton();
			this.mmButton = new global::System.Windows.Forms.RadioButton();
			this.feetButton = new global::System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.okButton.Location = new global::System.Drawing.Point(362, 864);
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
			this.listBoxParameters.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.listBoxParameters.FormattingEnabled = true;
			this.listBoxParameters.HorizontalScrollbar = true;
			this.listBoxParameters.ItemHeight = 24;
			this.listBoxParameters.Location = new global::System.Drawing.Point(524, 273);
			this.listBoxParameters.Margin = new global::System.Windows.Forms.Padding(6);
			this.listBoxParameters.Name = "listBoxParameters";
			this.listBoxParameters.Size = new global::System.Drawing.Size(314, 532);
			this.listBoxParameters.TabIndex = 242;
			this.listBoxParameters.SelectedIndexChanged += new global::System.EventHandler(this.listBoxParameters_SelectedIndexChanged);
			this.buttonParamList.Location = new global::System.Drawing.Point(524, 219);
			this.buttonParamList.Margin = new global::System.Windows.Forms.Padding(6);
			this.buttonParamList.Name = "buttonParamList";
			this.buttonParamList.Size = new global::System.Drawing.Size(211, 42);
			this.buttonParamList.TabIndex = 243;
			this.buttonParamList.Text = "Show Parameters";
			this.buttonParamList.UseVisualStyleBackColor = true;
			this.buttonParamList.Click += new global::System.EventHandler(this.buttonParamList_Click);
			this.label2.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(529, 822);
			this.label2.Margin = new global::System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(244, 25);
			this.label2.TabIndex = 246;
			this.label2.Text = "Select Parameter to Export";
			this.listBoxComponents.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.listBoxComponents.FormattingEnabled = true;
			this.listBoxComponents.ItemHeight = 24;
			this.listBoxComponents.Location = new global::System.Drawing.Point(29, 272);
			this.listBoxComponents.Margin = new global::System.Windows.Forms.Padding(6);
			this.listBoxComponents.Name = "listBoxComponents";
			this.listBoxComponents.Size = new global::System.Drawing.Size(483, 532);
			this.listBoxComponents.TabIndex = 247;
			this.listBoxComponents.SelectedIndexChanged += new global::System.EventHandler(this.listBoxComponents_SelectedIndexChanged);
			this.groupBox1.Controls.Add(this.mButton);
			this.groupBox1.Controls.Add(this.inchesButton);
			this.groupBox1.Controls.Add(this.mmButton);
			this.groupBox1.Controls.Add(this.feetButton);
			this.groupBox1.Location = new global::System.Drawing.Point(29, 28);
			this.groupBox1.Margin = new global::System.Windows.Forms.Padding(6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new global::System.Windows.Forms.Padding(6);
			this.groupBox1.Size = new global::System.Drawing.Size(255, 217);
			this.groupBox1.TabIndex = 248;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Units";
			this.mButton.AutoSize = true;
			this.mButton.Location = new global::System.Drawing.Point(83, 126);
			this.mButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.mButton.Name = "mButton";
			this.mButton.Size = new global::System.Drawing.Size(97, 29);
			this.mButton.TabIndex = 13;
			this.mButton.Text = "Meters";
			this.mButton.UseVisualStyleBackColor = true;
			this.mButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
			this.inchesButton.AutoSize = true;
			this.inchesButton.Location = new global::System.Drawing.Point(83, 83);
			this.inchesButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.inchesButton.Name = "inchesButton";
			this.inchesButton.Size = new global::System.Drawing.Size(95, 29);
			this.inchesButton.TabIndex = 12;
			this.inchesButton.Text = "Inches";
			this.inchesButton.UseVisualStyleBackColor = true;
			this.inchesButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
			this.mmButton.AutoSize = true;
			this.mmButton.Location = new global::System.Drawing.Point(83, 168);
			this.mmButton.Margin = new global::System.Windows.Forms.Padding(6);
			this.mmButton.Name = "mmButton";
			this.mmButton.Size = new global::System.Drawing.Size(129, 29);
			this.mmButton.TabIndex = 10;
			this.mmButton.Text = "Millimeters";
			this.mmButton.UseVisualStyleBackColor = true;
			this.mmButton.CheckedChanged += new global::System.EventHandler(this.UnitButton_Click_MM);
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
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(11f, 24f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(893, 1002);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.listBoxComponents);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.buttonParamList);
			base.Controls.Add(this.listBoxParameters);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.pBar1);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.okButton);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new global::System.Windows.Forms.Padding(6);
			this.MinimumSize = new global::System.Drawing.Size(897, 673);
			base.Name = "ExpAnimForm";
			this.RightToLeft = global::System.Windows.Forms.RightToLeft.No;
			this.Text = "ef | Export Properties To Unity";
			base.Load += new global::System.EventHandler(this.ExpDataForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400005E RID: 94
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400005F RID: 95
		private global::System.Windows.Forms.Button okButton;

		// Token: 0x04000060 RID: 96
		private global::System.Windows.Forms.Button cancelButton;

		// Token: 0x04000061 RID: 97
		private global::System.Windows.Forms.ProgressBar pBar1;

		// Token: 0x04000062 RID: 98
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000063 RID: 99
		private global::System.Windows.Forms.ListBox listBoxParameters;

		// Token: 0x04000064 RID: 100
		private global::System.Windows.Forms.Button buttonParamList;

		// Token: 0x04000065 RID: 101
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000066 RID: 102
		private global::System.Windows.Forms.ListBox listBoxComponents;

		// Token: 0x04000067 RID: 103
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x04000068 RID: 104
		private global::System.Windows.Forms.RadioButton mButton;

		// Token: 0x04000069 RID: 105
		private global::System.Windows.Forms.RadioButton inchesButton;

		// Token: 0x0400006A RID: 106
		private global::System.Windows.Forms.RadioButton mmButton;

		// Token: 0x0400006B RID: 107
		private global::System.Windows.Forms.RadioButton feetButton;
	}
}
