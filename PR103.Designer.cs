namespace ПР103
{
    partial class PR103
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PR103));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtDeviceIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSlaveID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRegisters = new System.Windows.Forms.TextBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnClearLog = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBinary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLogFileName = new System.Windows.Forms.TextBox();
            this.lblLogPath = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPollCount = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnLogSettings = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.FunctionCode = new System.Windows.Forms.Label();
            this.cmbFunctionCode = new System.Windows.Forms.ComboBox();
            this.btnApplyFunction = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDataFormat = new System.Windows.Forms.Label();
            this.cmbDataFormat = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(16, 15);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 28);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(124, 15);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 28);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtDeviceIP
            // 
            this.txtDeviceIP.Location = new System.Drawing.Point(11, 39);
            this.txtDeviceIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeviceIP.Name = "txtDeviceIP";
            this.txtDeviceIP.Size = new System.Drawing.Size(116, 22);
            this.txtDeviceIP.TabIndex = 2;
            this.txtDeviceIP.Text = "10.2.11.122";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP адрес:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Порт:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(138, 39);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(65, 22);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "502";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Slave ID:";
            // 
            // txtSlaveID
            // 
            this.txtSlaveID.Location = new System.Drawing.Point(216, 39);
            this.txtSlaveID.Margin = new System.Windows.Forms.Padding(4);
            this.txtSlaveID.Name = "txtSlaveID";
            this.txtSlaveID.Size = new System.Drawing.Size(65, 22);
            this.txtSlaveID.TabIndex = 6;
            this.txtSlaveID.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Интервал (сек):";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(11, 39);
            this.txtInterval.Margin = new System.Windows.Forms.Padding(4);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(65, 22);
            this.txtInterval.TabIndex = 8;
            this.txtInterval.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(244, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Адреса (dec) (через , или -):";
            // 
            // txtRegisters
            // 
            this.txtRegisters.Location = new System.Drawing.Point(247, 39);
            this.txtRegisters.Margin = new System.Windows.Forms.Padding(4);
            this.txtRegisters.Name = "txtRegisters";
            this.txtRegisters.Size = new System.Drawing.Size(255, 22);
            this.txtRegisters.TabIndex = 10;
            this.txtRegisters.Text = "4000, 4002, 4010, 4015-4020";
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.SystemColors.Window;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(4, 19);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(525, 200);
            this.rtbLog.TabIndex = 12;
            this.rtbLog.Text = "";
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(232, 15);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(100, 28);
            this.btnClearLog.TabIndex = 13;
            this.btnClearLog.Text = "Очистить";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAddress,
            this.colDec,
            this.colHex,
            this.colBinary,
            this.colTimestamp});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.Location = new System.Drawing.Point(3, 16);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersWidth = 51;
            this.dgvResults.Size = new System.Drawing.Size(394, 162);
            this.dgvResults.TabIndex = 14;
            // 
            // colAddress
            // 
            this.colAddress.HeaderText = "Адрес";
            this.colAddress.MinimumWidth = 6;
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.Width = 70;
            // 
            // colDec
            // 
            this.colDec.HeaderText = "Dec";
            this.colDec.MinimumWidth = 6;
            this.colDec.Name = "colDec";
            this.colDec.ReadOnly = true;
            this.colDec.Width = 70;
            // 
            // colHex
            // 
            this.colHex.HeaderText = "Hex";
            this.colHex.MinimumWidth = 6;
            this.colHex.Name = "colHex";
            this.colHex.ReadOnly = true;
            this.colHex.Width = 70;
            // 
            // colBinary
            // 
            this.colBinary.HeaderText = "Binary";
            this.colBinary.MinimumWidth = 6;
            this.colBinary.Name = "colBinary";
            this.colBinary.ReadOnly = true;
            this.colBinary.Width = 120;
            // 
            // colTimestamp
            // 
            this.colTimestamp.HeaderText = "Время";
            this.colTimestamp.MinimumWidth = 6;
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            this.colTimestamp.Width = 125;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtDeviceIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtLogFileName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblLogPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSlaveID);
            this.groupBox1.Location = new System.Drawing.Point(16, 50);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(533, 74);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры устройства";
            // 
            // txtLogFileName
            // 
            this.txtLogFileName.Location = new System.Drawing.Point(304, 20);
            this.txtLogFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtLogFileName.Name = "txtLogFileName";
            this.txtLogFileName.ReadOnly = true;
            this.txtLogFileName.Size = new System.Drawing.Size(201, 22);
            this.txtLogFileName.TabIndex = 20;
            // 
            // lblLogPath
            // 
            this.lblLogPath.AutoSize = true;
            this.lblLogPath.Location = new System.Drawing.Point(301, 0);
            this.lblLogPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogPath.Name = "lblLogPath";
            this.lblLogPath.Size = new System.Drawing.Size(106, 16);
            this.lblLogPath.TabIndex = 19;
            this.lblLogPath.Text = "Лог не активен";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtInterval);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtRegisters);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtPollCount);
            this.groupBox2.Location = new System.Drawing.Point(16, 132);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(533, 74);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры опроса";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(124, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 16);
            this.label6.TabIndex = 26;
            this.label6.Text = "Число опросов:";
            // 
            // txtPollCount
            // 
            this.txtPollCount.Location = new System.Drawing.Point(127, 39);
            this.txtPollCount.Margin = new System.Windows.Forms.Padding(4);
            this.txtPollCount.Name = "txtPollCount";
            this.txtPollCount.Size = new System.Drawing.Size(65, 22);
            this.txtPollCount.TabIndex = 25;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rtbLog);
            this.groupBox3.Location = new System.Drawing.Point(16, 213);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(533, 223);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Лог";
            // 
            // btnLogSettings
            // 
            this.btnLogSettings.Location = new System.Drawing.Point(340, 15);
            this.btnLogSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogSettings.Name = "btnLogSettings";
            this.btnLogSettings.Size = new System.Drawing.Size(200, 28);
            this.btnLogSettings.TabIndex = 18;
            this.btnLogSettings.Text = "Указать путь нового лога";
            this.btnLogSettings.UseVisualStyleBackColor = true;
            this.btnLogSettings.Click += new System.EventHandler(this.btnLogSettings_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "log";
            this.saveFileDialog.Filter = "Log files (*.log)|*.log|All files (*.*)|*.*";
            this.saveFileDialog.Title = "Выберите файл для сохранения лога";
            // 
            // FunctionCode
            // 
            this.FunctionCode.AutoSize = true;
            this.FunctionCode.Location = new System.Drawing.Point(24, 444);
            this.FunctionCode.Name = "FunctionCode";
            this.FunctionCode.Size = new System.Drawing.Size(116, 16);
            this.FunctionCode.TabIndex = 21;
            this.FunctionCode.Text = "Команда чтения:";
            this.FunctionCode.Click += new System.EventHandler(this.label6_Click);
            // 
            // cmbFunctionCode
            // 
            this.cmbFunctionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFunctionCode.FormattingEnabled = true;
            this.cmbFunctionCode.Items.AddRange(new object[] {
            "0x03 - Holding Registers",
            "0x04 - Input Registers",
            "0x01 - Coils",
            "0x02 - Discrete Inputs"});
            this.cmbFunctionCode.Location = new System.Drawing.Point(16, 465);
            this.cmbFunctionCode.Name = "cmbFunctionCode";
            this.cmbFunctionCode.Size = new System.Drawing.Size(178, 24);
            this.cmbFunctionCode.TabIndex = 22;
            // 
            // btnApplyFunction
            // 
            this.btnApplyFunction.Location = new System.Drawing.Point(200, 465);
            this.btnApplyFunction.Name = "btnApplyFunction";
            this.btnApplyFunction.Size = new System.Drawing.Size(106, 26);
            this.btnApplyFunction.TabIndex = 23;
            this.btnApplyFunction.Text = "Применить";
            this.btnApplyFunction.UseVisualStyleBackColor = true;
            this.btnApplyFunction.Click += new System.EventHandler(this.btnApplyFunction_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label7.Location = new System.Drawing.Point(325, 444);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(196, 64);
            this.label7.TabIndex = 24;
            this.label7.Text = "0x01 - дискретные выходы DO\r\n0x02 - дискретные входы DI\r\n0x03 - аналоговые выходы" +
    " AO\r\n0x04 - аналоговые входы AI";
            // 
            // lblDataFormat
            // 
            this.lblDataFormat.AutoSize = true;
            this.lblDataFormat.Location = new System.Drawing.Point(12, 507);
            this.lblDataFormat.Name = "lblDataFormat";
            this.lblDataFormat.Size = new System.Drawing.Size(111, 16);
            this.lblDataFormat.TabIndex = 25;
            this.lblDataFormat.Text = "Формат данных:";
            // 
            // cmbDataFormat
            // 
            this.cmbDataFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataFormat.FormattingEnabled = true;
            this.cmbDataFormat.Items.AddRange(new object[] {
            "DEC | HEX | BIN",
            "16-bit integer",
            "32-bit integer",
            "Float",
            "IP-адрес",
            "UTC время"});
            this.cmbDataFormat.Location = new System.Drawing.Point(129, 504);
            this.cmbDataFormat.Name = "cmbDataFormat";
            this.cmbDataFormat.Size = new System.Drawing.Size(177, 24);
            this.cmbDataFormat.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label8.Location = new System.Drawing.Point(325, 512);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(172, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = "⚠Порядок байтов - CDAB";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label9.Location = new System.Drawing.Point(288, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(223, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "Каждый старт требует новый лог";
            // 
            // PR103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(563, 537);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbDataFormat);
            this.Controls.Add(this.lblDataFormat);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnApplyFunction);
            this.Controls.Add(this.cmbFunctionCode);
            this.Controls.Add(this.FunctionCode);
            this.Controls.Add(this.btnLogSettings);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PR103";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ОВЕН ПР103 Modbus TCP Reader";
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtDeviceIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSlaveID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRegisters;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBinary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLogSettings;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label lblLogPath;
        private System.Windows.Forms.TextBox txtLogFileName;
        private System.Windows.Forms.Label FunctionCode;
        private System.Windows.Forms.ComboBox cmbFunctionCode;
        private System.Windows.Forms.Button btnApplyFunction;
        private System.Windows.Forms.TextBox txtPollCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDataFormat;
        private System.Windows.Forms.ComboBox cmbDataFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}