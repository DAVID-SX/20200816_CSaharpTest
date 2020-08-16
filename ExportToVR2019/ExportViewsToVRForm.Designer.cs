namespace ExportToVR
{
	public partial class ExportViewsToVRForm : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBoxTrialVersion = new System.Windows.Forms.CheckBox();
            this.radioButtonSingleObject = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonByTypes = new System.Windows.Forms.RadioButton();
            this.radioButtonMaterialsFast = new System.Windows.Forms.RadioButton();
            this.listBoxViews = new System.Windows.Forms.ListBox();
            this.checkBoxOtherView = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(313, 415);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 42);
            this.cancelButton.TabIndex = 22;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(176, 415);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(130, 42);
            this.okButton.TabIndex = 21;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // pBar1
            // 
            this.pBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBar1.ForeColor = System.Drawing.Color.Blue;
            this.pBar1.Location = new System.Drawing.Point(14, 458);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(399, 8);
            this.pBar1.TabIndex = 31;
            this.pBar1.Visible = false;
            // 
            // checkBoxTrialVersion
            // 
            this.checkBoxTrialVersion.AutoSize = true;
            this.checkBoxTrialVersion.Enabled = false;
            this.checkBoxTrialVersion.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.checkBoxTrialVersion.Location = new System.Drawing.Point(178, 11);
            this.checkBoxTrialVersion.Name = "checkBoxTrialVersion";
            this.checkBoxTrialVersion.Size = new System.Drawing.Size(54, 16);
            this.checkBoxTrialVersion.TabIndex = 117;
            this.checkBoxTrialVersion.Text = "Trial";
            this.checkBoxTrialVersion.UseVisualStyleBackColor = true;
            this.checkBoxTrialVersion.Visible = false;
            this.checkBoxTrialVersion.CheckedChanged += new System.EventHandler(this.checkBoxTrialVersion_CheckedChanged);
            // 
            // radioButtonSingleObject
            // 
            this.radioButtonSingleObject.AutoSize = true;
            this.radioButtonSingleObject.Location = new System.Drawing.Point(40, 127);
            this.radioButtonSingleObject.Name = "radioButtonSingleObject";
            this.radioButtonSingleObject.Size = new System.Drawing.Size(71, 16);
            this.radioButtonSingleObject.TabIndex = 223;
            this.radioButtonSingleObject.Text = "单个对象";
            this.radioButtonSingleObject.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 224;
            this.label1.Text = "分组选项:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 230;
            this.label2.Text = "(导出属性)";
            // 
            // radioButtonByTypes
            // 
            this.radioButtonByTypes.AutoSize = true;
            this.radioButtonByTypes.Location = new System.Drawing.Point(40, 106);
            this.radioButtonByTypes.Name = "radioButtonByTypes";
            this.radioButtonByTypes.Size = new System.Drawing.Size(59, 16);
            this.radioButtonByTypes.TabIndex = 232;
            this.radioButtonByTypes.Text = "按实体";
            this.radioButtonByTypes.UseVisualStyleBackColor = true;
            // 
            // radioButtonMaterialsFast
            // 
            this.radioButtonMaterialsFast.AutoSize = true;
            this.radioButtonMaterialsFast.Checked = true;
            this.radioButtonMaterialsFast.Location = new System.Drawing.Point(40, 86);
            this.radioButtonMaterialsFast.Name = "radioButtonMaterialsFast";
            this.radioButtonMaterialsFast.Size = new System.Drawing.Size(59, 16);
            this.radioButtonMaterialsFast.TabIndex = 233;
            this.radioButtonMaterialsFast.TabStop = true;
            this.radioButtonMaterialsFast.Text = "按材质";
            this.radioButtonMaterialsFast.UseVisualStyleBackColor = true;
            // 
            // listBoxViews
            // 
            this.listBoxViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxViews.Enabled = false;
            this.listBoxViews.FormattingEnabled = true;
            this.listBoxViews.ItemHeight = 12;
            this.listBoxViews.Location = new System.Drawing.Point(33, 201);
            this.listBoxViews.Name = "listBoxViews";
            this.listBoxViews.Size = new System.Drawing.Size(381, 196);
            this.listBoxViews.TabIndex = 236;
            this.listBoxViews.SelectedIndexChanged += new System.EventHandler(this.listBoxViews_SelectedIndexChanged);
            // 
            // checkBoxOtherView
            // 
            this.checkBoxOtherView.AutoSize = true;
            this.checkBoxOtherView.Location = new System.Drawing.Point(40, 176);
            this.checkBoxOtherView.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxOtherView.Name = "checkBoxOtherView";
            this.checkBoxOtherView.Size = new System.Drawing.Size(96, 16);
            this.checkBoxOtherView.TabIndex = 238;
            this.checkBoxOtherView.Text = "导出其他视图";
            this.checkBoxOtherView.UseVisualStyleBackColor = true;
            this.checkBoxOtherView.CheckedChanged += new System.EventHandler(this.checkBoxOtherView_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::ExportToVR.Properties.Resources.ExportToVR_Gris_36X36;
            this.pictureBox1.Location = new System.Drawing.Point(376, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(36, 33);
            this.pictureBox1.TabIndex = 220;
            this.pictureBox1.TabStop = false;
            // 
            // ExportViewsToVRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 468);
            this.Controls.Add(this.checkBoxOtherView);
            this.Controls.Add(this.listBoxViews);
            this.Controls.Add(this.radioButtonMaterialsFast);
            this.Controls.Add(this.radioButtonByTypes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonSingleObject);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.checkBoxTrialVersion);
            this.MaximumSize = new System.Drawing.Size(607, 1519);
            this.MinimumSize = new System.Drawing.Size(389, 395);
            this.Name = "ExportViewsToVRForm";
            this.Text = "模型数据导出";
            this.Load += new System.EventHandler(this.ViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private global::System.ComponentModel.IContainer components = null;

		private global::System.Windows.Forms.Button cancelButton;

		private global::System.Windows.Forms.Button okButton;

		private global::System.Windows.Forms.ProgressBar pBar1;

		private global::System.Windows.Forms.CheckBox checkBoxTrialVersion;

		private global::System.Windows.Forms.PictureBox pictureBox1;

		private global::System.Windows.Forms.RadioButton radioButtonSingleObject;

		private global::System.Windows.Forms.Label label1;

		private global::System.Windows.Forms.Label label2;

		private global::System.Windows.Forms.RadioButton radioButtonByTypes;

		private global::System.Windows.Forms.RadioButton radioButtonMaterialsFast;

		private global::System.Windows.Forms.ListBox listBoxViews;

		private global::System.Windows.Forms.CheckBox checkBoxOtherView;
	}
}
