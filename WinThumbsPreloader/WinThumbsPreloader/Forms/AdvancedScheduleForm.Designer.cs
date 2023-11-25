namespace WinThumbsPreloader.Forms
{
    partial class AdvancedScheduleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new System.Windows.Forms.GroupBox();
            DefaultButton = new System.Windows.Forms.Button();
            DetailedTimeModeCheckBox = new System.Windows.Forms.CheckBox();
            TwentyFourHourCheckBox = new System.Windows.Forms.CheckBox();
            DriveSortLabel = new System.Windows.Forms.Label();
            DriveSortComboBox = new System.Windows.Forms.ComboBox();
            DriveSelectionCheckBox = new System.Windows.Forms.CheckBox();
            CloseButton = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            GeneralDescriptionLabel = new System.Windows.Forms.Label();
            GeneralDescriptionTextBox = new System.Windows.Forms.TextBox();
            GeneralHiddenCheckBox = new System.Windows.Forms.CheckBox();
            GeneralSecurityOptionsGroupBox = new System.Windows.Forms.GroupBox();
            checkBox2 = new System.Windows.Forms.CheckBox();
            radioButton1 = new System.Windows.Forms.RadioButton();
            radioButton2 = new System.Windows.Forms.RadioButton();
            tabPage2 = new System.Windows.Forms.TabPage();
            tabPage3 = new System.Windows.Forms.TabPage();
            groupBox4 = new System.Windows.Forms.GroupBox();
            comboBox2 = new System.Windows.Forms.ComboBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            checkBox6 = new System.Windows.Forms.CheckBox();
            checkBox5 = new System.Windows.Forms.CheckBox();
            checkBox4 = new System.Windows.Forms.CheckBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            checkBox7 = new System.Windows.Forms.CheckBox();
            checkBox8 = new System.Windows.Forms.CheckBox();
            checkBox9 = new System.Windows.Forms.CheckBox();
            tabPage4 = new System.Windows.Forms.TabPage();
            comboBox5 = new System.Windows.Forms.ComboBox();
            label4 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            comboBox4 = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            comboBox3 = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            checkBox14 = new System.Windows.Forms.CheckBox();
            checkBox13 = new System.Windows.Forms.CheckBox();
            checkBox12 = new System.Windows.Forms.CheckBox();
            checkBox11 = new System.Windows.Forms.CheckBox();
            checkBox10 = new System.Windows.Forms.CheckBox();
            OpenTaskSchedulerButton = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            GeneralSecurityOptionsGroupBox.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.Window;
            groupBox1.Controls.Add(DefaultButton);
            groupBox1.Controls.Add(DetailedTimeModeCheckBox);
            groupBox1.Controls.Add(TwentyFourHourCheckBox);
            groupBox1.Controls.Add(DriveSortLabel);
            groupBox1.Controls.Add(DriveSortComboBox);
            groupBox1.Controls.Add(DriveSelectionCheckBox);
            groupBox1.Location = new System.Drawing.Point(12, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(409, 108);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Advanced Options";
            // 
            // DefaultButton
            // 
            DefaultButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            DefaultButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            DefaultButton.Location = new System.Drawing.Point(314, 72);
            DefaultButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DefaultButton.Name = "DefaultButton";
            DefaultButton.Size = new System.Drawing.Size(88, 28);
            DefaultButton.TabIndex = 15;
            DefaultButton.Text = "Default";
            DefaultButton.UseVisualStyleBackColor = true;
            DefaultButton.Click += DefaultButton_Click;
            // 
            // DetailedTimeModeCheckBox
            // 
            DetailedTimeModeCheckBox.AutoSize = true;
            DetailedTimeModeCheckBox.Location = new System.Drawing.Point(8, 48);
            DetailedTimeModeCheckBox.Name = "DetailedTimeModeCheckBox";
            DetailedTimeModeCheckBox.Size = new System.Drawing.Size(132, 19);
            DetailedTimeModeCheckBox.TabIndex = 4;
            DetailedTimeModeCheckBox.Text = "Detailed Time Mode";
            DetailedTimeModeCheckBox.UseVisualStyleBackColor = true;
            DetailedTimeModeCheckBox.CheckedChanged += DetailedTimeModeCheckBox_CheckedChanged;
            // 
            // TwentyFourHourCheckBox
            // 
            TwentyFourHourCheckBox.AutoSize = true;
            TwentyFourHourCheckBox.Location = new System.Drawing.Point(8, 73);
            TwentyFourHourCheckBox.Name = "TwentyFourHourCheckBox";
            TwentyFourHourCheckBox.Size = new System.Drawing.Size(121, 19);
            TwentyFourHourCheckBox.TabIndex = 3;
            TwentyFourHourCheckBox.Text = "Use 24-Hour Time";
            TwentyFourHourCheckBox.UseVisualStyleBackColor = true;
            TwentyFourHourCheckBox.CheckedChanged += TwentyFourHourCheckBox_CheckedChanged;
            // 
            // DriveSortLabel
            // 
            DriveSortLabel.AutoSize = true;
            DriveSortLabel.Location = new System.Drawing.Point(199, 23);
            DriveSortLabel.Name = "DriveSortLabel";
            DriveSortLabel.Size = new System.Drawing.Size(61, 15);
            DriveSortLabel.TabIndex = 2;
            DriveSortLabel.Text = "Drive Sort:";
            // 
            // DriveSortComboBox
            // 
            DriveSortComboBox.FormattingEnabled = true;
            DriveSortComboBox.Items.AddRange(new object[] { "Vertical", "Horizontal" });
            DriveSortComboBox.Location = new System.Drawing.Point(266, 18);
            DriveSortComboBox.Name = "DriveSortComboBox";
            DriveSortComboBox.Size = new System.Drawing.Size(121, 23);
            DriveSortComboBox.TabIndex = 1;
            DriveSortComboBox.SelectedIndexChanged += DriveSortComboBox_SelectedIndexChanged;
            // 
            // DriveSelectionCheckBox
            // 
            DriveSelectionCheckBox.AutoSize = true;
            DriveSelectionCheckBox.Location = new System.Drawing.Point(8, 22);
            DriveSelectionCheckBox.Name = "DriveSelectionCheckBox";
            DriveSelectionCheckBox.Size = new System.Drawing.Size(142, 19);
            DriveSelectionCheckBox.TabIndex = 0;
            DriveSelectionCheckBox.Text = "Enable Drive Selection";
            DriveSelectionCheckBox.UseVisualStyleBackColor = true;
            DriveSelectionCheckBox.CheckedChanged += DriveSelectionCheckBox_CheckedChanged;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CloseButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            CloseButton.Location = new System.Drawing.Point(333, 421);
            CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(88, 28);
            CloseButton.TabIndex = 7;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = System.Drawing.SystemColors.Window;
            groupBox2.Controls.Add(tabControl1);
            groupBox2.Location = new System.Drawing.Point(12, 125);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(409, 290);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Task Scheduler";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Location = new System.Drawing.Point(8, 23);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(395, 262);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(GeneralDescriptionLabel);
            tabPage1.Controls.Add(GeneralDescriptionTextBox);
            tabPage1.Controls.Add(GeneralHiddenCheckBox);
            tabPage1.Controls.Add(GeneralSecurityOptionsGroupBox);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(387, 234);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "General";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // GeneralDescriptionLabel
            // 
            GeneralDescriptionLabel.AutoSize = true;
            GeneralDescriptionLabel.Location = new System.Drawing.Point(12, 9);
            GeneralDescriptionLabel.Name = "GeneralDescriptionLabel";
            GeneralDescriptionLabel.Size = new System.Drawing.Size(70, 15);
            GeneralDescriptionLabel.TabIndex = 6;
            GeneralDescriptionLabel.Text = "Description:";
            // 
            // GeneralDescriptionTextBox
            // 
            GeneralDescriptionTextBox.Location = new System.Drawing.Point(88, 6);
            GeneralDescriptionTextBox.Multiline = true;
            GeneralDescriptionTextBox.Name = "GeneralDescriptionTextBox";
            GeneralDescriptionTextBox.Size = new System.Drawing.Size(293, 88);
            GeneralDescriptionTextBox.TabIndex = 5;
            GeneralDescriptionTextBox.Text = "This section is unfinished and non-functional.\r\n\r\nFeel free to suggest changes to this section.\r\n\r\nRecreating Task Scheduler settings to modify here.";
            // 
            // GeneralHiddenCheckBox
            // 
            GeneralHiddenCheckBox.AutoSize = true;
            GeneralHiddenCheckBox.Location = new System.Drawing.Point(6, 210);
            GeneralHiddenCheckBox.Name = "GeneralHiddenCheckBox";
            GeneralHiddenCheckBox.Size = new System.Drawing.Size(65, 19);
            GeneralHiddenCheckBox.TabIndex = 3;
            GeneralHiddenCheckBox.Text = "Hidden";
            GeneralHiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // GeneralSecurityOptionsGroupBox
            // 
            GeneralSecurityOptionsGroupBox.Controls.Add(checkBox2);
            GeneralSecurityOptionsGroupBox.Controls.Add(radioButton1);
            GeneralSecurityOptionsGroupBox.Controls.Add(radioButton2);
            GeneralSecurityOptionsGroupBox.Location = new System.Drawing.Point(6, 100);
            GeneralSecurityOptionsGroupBox.Name = "GeneralSecurityOptionsGroupBox";
            GeneralSecurityOptionsGroupBox.Size = new System.Drawing.Size(375, 104);
            GeneralSecurityOptionsGroupBox.TabIndex = 4;
            GeneralSecurityOptionsGroupBox.TabStop = false;
            GeneralSecurityOptionsGroupBox.Text = "Security options";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new System.Drawing.Point(6, 72);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(168, 19);
            checkBox2.TabIndex = 2;
            checkBox2.Text = "Run with highest privileges";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(6, 22);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(197, 19);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "Run only when user is logged on";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(6, 47);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(220, 19);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "Run whether user is logged on or not";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(387, 234);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Triggers";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox4);
            tabPage3.Controls.Add(groupBox5);
            tabPage3.Location = new System.Drawing.Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(387, 234);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Conditions";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(comboBox2);
            groupBox4.Controls.Add(comboBox1);
            groupBox4.Controls.Add(label1);
            groupBox4.Controls.Add(checkBox6);
            groupBox4.Controls.Add(checkBox5);
            groupBox4.Controls.Add(checkBox4);
            groupBox4.Location = new System.Drawing.Point(6, 6);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(375, 125);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Idle";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "1 minute", "5 minutes", "10 minutes", "15 minutes", "30 minutes", "1 hour" });
            comboBox2.Location = new System.Drawing.Point(266, 15);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(103, 23);
            comboBox2.TabIndex = 5;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Do not wait", "1 minute", "5 minutes", "10 minutes", "15 minutes", "30 minutes", "1 hour", "2 hours" });
            comboBox1.Location = new System.Drawing.Point(128, 44);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(121, 23);
            comboBox1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(30, 47);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(92, 15);
            label1.TabIndex = 3;
            label1.Text = "Wait for idle for:";
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new System.Drawing.Point(41, 98);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new System.Drawing.Size(189, 19);
            checkBox6.TabIndex = 2;
            checkBox6.Text = "Restart if the idle state resumes";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new System.Drawing.Point(25, 73);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new System.Drawing.Size(224, 19);
            checkBox5.TabIndex = 1;
            checkBox5.Text = "Stop if the computer ceases to be idle";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new System.Drawing.Point(6, 19);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new System.Drawing.Size(259, 19);
            checkBox4.TabIndex = 0;
            checkBox4.Text = "Start the task only if the computer is idle for:";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(checkBox7);
            groupBox5.Controls.Add(checkBox8);
            groupBox5.Controls.Add(checkBox9);
            groupBox5.Location = new System.Drawing.Point(6, 137);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(375, 94);
            groupBox5.TabIndex = 7;
            groupBox5.TabStop = false;
            groupBox5.Text = "Power";
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Location = new System.Drawing.Point(6, 19);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new System.Drawing.Size(288, 19);
            checkBox7.TabIndex = 3;
            checkBox7.Text = "Start the task only if the computer is on AC power";
            checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            checkBox8.AutoSize = true;
            checkBox8.Location = new System.Drawing.Point(21, 44);
            checkBox8.Name = "checkBox8";
            checkBox8.Size = new System.Drawing.Size(273, 19);
            checkBox8.TabIndex = 4;
            checkBox8.Text = "Stop if the computer switches to battery power";
            checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            checkBox9.AutoSize = true;
            checkBox9.Location = new System.Drawing.Point(6, 69);
            checkBox9.Name = "checkBox9";
            checkBox9.Size = new System.Drawing.Size(211, 19);
            checkBox9.TabIndex = 5;
            checkBox9.Text = "Wake the computer to run this task";
            checkBox9.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(comboBox5);
            tabPage4.Controls.Add(label4);
            tabPage4.Controls.Add(textBox1);
            tabPage4.Controls.Add(comboBox4);
            tabPage4.Controls.Add(label3);
            tabPage4.Controls.Add(comboBox3);
            tabPage4.Controls.Add(label2);
            tabPage4.Controls.Add(checkBox14);
            tabPage4.Controls.Add(checkBox13);
            tabPage4.Controls.Add(checkBox12);
            tabPage4.Controls.Add(checkBox11);
            tabPage4.Controls.Add(checkBox10);
            tabPage4.Location = new System.Drawing.Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3);
            tabPage4.Size = new System.Drawing.Size(387, 234);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Settings";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // comboBox5
            // 
            comboBox5.FormattingEnabled = true;
            comboBox5.Items.AddRange(new object[] { "1 hour", "2 hours", "4 hours", "8 hours", "12 hours", "1 day", "3 days" });
            comboBox5.Location = new System.Drawing.Point(270, 113);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new System.Drawing.Size(93, 23);
            comboBox5.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(306, 91);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(36, 15);
            label4.TabIndex = 10;
            label4.Text = "times";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(270, 83);
            textBox1.MaxLength = 3;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(30, 23);
            textBox1.TabIndex = 9;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "1 minute", "5 minutes", "10 minutes", "15 minutes", "30 minutes", "1 hour", "2 hours" });
            comboBox4.Location = new System.Drawing.Point(270, 54);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new System.Drawing.Size(93, 23);
            comboBox4.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(22, 87);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(135, 15);
            label3.TabIndex = 7;
            label3.Text = "Attempt to restart up to:";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "Do not start a new instance", "Run a new instance in parallel", "Queue a new instance", "Stop the existing instance" });
            comboBox3.Location = new System.Drawing.Point(6, 205);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(229, 23);
            comboBox3.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 178);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(324, 15);
            label2.TabIndex = 5;
            label2.Text = "If the task is already running, then the following rule applies:";
            // 
            // checkBox14
            // 
            checkBox14.AutoSize = true;
            checkBox14.Location = new System.Drawing.Point(6, 142);
            checkBox14.Name = "checkBox14";
            checkBox14.Size = new System.Drawing.Size(364, 19);
            checkBox14.TabIndex = 4;
            checkBox14.Text = "If the running task does not end when requested, force it to stop";
            checkBox14.UseVisualStyleBackColor = true;
            // 
            // checkBox13
            // 
            checkBox13.AutoSize = true;
            checkBox13.Location = new System.Drawing.Point(6, 117);
            checkBox13.Name = "checkBox13";
            checkBox13.Size = new System.Drawing.Size(207, 19);
            checkBox13.TabIndex = 3;
            checkBox13.Text = "Stop the task if it runs longer than:";
            checkBox13.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            checkBox12.AutoSize = true;
            checkBox12.Location = new System.Drawing.Point(6, 56);
            checkBox12.Name = "checkBox12";
            checkBox12.Size = new System.Drawing.Size(174, 19);
            checkBox12.TabIndex = 2;
            checkBox12.Text = "If the task fails, restart every:";
            checkBox12.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            checkBox11.AutoSize = true;
            checkBox11.Location = new System.Drawing.Point(6, 31);
            checkBox11.Name = "checkBox11";
            checkBox11.Size = new System.Drawing.Size(344, 19);
            checkBox11.TabIndex = 1;
            checkBox11.Text = "Run task as soon as possible after a scheduled start is missed";
            checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            checkBox10.AutoSize = true;
            checkBox10.Location = new System.Drawing.Point(6, 6);
            checkBox10.Name = "checkBox10";
            checkBox10.Size = new System.Drawing.Size(195, 19);
            checkBox10.TabIndex = 0;
            checkBox10.Text = "Allow task to be run on demand";
            checkBox10.UseVisualStyleBackColor = true;
            // 
            // OpenTaskSchedulerButton
            // 
            OpenTaskSchedulerButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OpenTaskSchedulerButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            OpenTaskSchedulerButton.Location = new System.Drawing.Point(12, 421);
            OpenTaskSchedulerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            OpenTaskSchedulerButton.Name = "OpenTaskSchedulerButton";
            OpenTaskSchedulerButton.Size = new System.Drawing.Size(100, 28);
            OpenTaskSchedulerButton.TabIndex = 14;
            OpenTaskSchedulerButton.Text = "Task Scheduler";
            OpenTaskSchedulerButton.UseVisualStyleBackColor = true;
            OpenTaskSchedulerButton.Click += OpenTaskSchedulerButton_Click;
            // 
            // AdvancedScheduleForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(434, 461);
            Controls.Add(OpenTaskSchedulerButton);
            Controls.Add(groupBox2);
            Controls.Add(CloseButton);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AdvancedScheduleForm";
            Text = "WinThumbsPreloader - Advanced Schedule";
            Load += ScheduleOptionsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            GeneralSecurityOptionsGroupBox.ResumeLayout(false);
            GeneralSecurityOptionsGroupBox.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox DriveSelectionCheckBox;
        private System.Windows.Forms.Label DriveSortLabel;
        private System.Windows.Forms.ComboBox DriveSortComboBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox TwentyFourHourCheckBox;
        private System.Windows.Forms.CheckBox DetailedTimeModeCheckBox;
        private System.Windows.Forms.Button OpenTaskSchedulerButton;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox GeneralHiddenCheckBox;
        private System.Windows.Forms.GroupBox GeneralSecurityOptionsGroupBox;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label GeneralDescriptionLabel;
        private System.Windows.Forms.TextBox GeneralDescriptionTextBox;
        private System.Windows.Forms.Button DefaultButton;
    }
}