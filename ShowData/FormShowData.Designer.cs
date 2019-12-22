namespace ShowData
{
    partial class FormShowData
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.dataGridViewSensorData = new System.Windows.Forms.DataGridView();
            this.chartTemp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartHum = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labelBattery = new System.Windows.Forms.Label();
            this.labelHum = new System.Windows.Forms.Label();
            this.textBoxBattery = new System.Windows.Forms.TextBox();
            this.textBoxHum = new System.Windows.Forms.TextBox();
            this.labelTemp = new System.Windows.Forms.Label();
            this.textBoxTemp = new System.Windows.Forms.TextBox();
            this.Piso = new System.Windows.Forms.Label();
            this.comboBoxPiso = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBrokerIP = new System.Windows.Forms.TextBox();
            this.btnConnectToBroker = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSensorData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartHum)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewSensorData
            // 
            this.dataGridViewSensorData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewSensorData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSensorData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSensorData.Location = new System.Drawing.Point(309, 9);
            this.dataGridViewSensorData.Name = "dataGridViewSensorData";
            this.dataGridViewSensorData.ReadOnly = true;
            this.dataGridViewSensorData.Size = new System.Drawing.Size(524, 578);
            this.dataGridViewSensorData.TabIndex = 2;
            // 
            // chartTemp
            // 
            this.chartTemp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea5.Name = "ChartArea1";
            this.chartTemp.ChartAreas.Add(chartArea5);
            this.chartTemp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            legend5.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend5.Name = "Legend1";
            this.chartTemp.Legends.Add(legend5);
            this.chartTemp.Location = new System.Drawing.Point(635, 42);
            this.chartTemp.Name = "chartTemp";
            this.chartTemp.Size = new System.Drawing.Size(572, 512);
            this.chartTemp.TabIndex = 3;
            this.chartTemp.Text = "chart1";
            // 
            // chartHum
            // 
            this.chartHum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            chartArea6.Name = "ChartArea1";
            this.chartHum.ChartAreas.Add(chartArea6);
            this.chartHum.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            legend6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend6.Name = "Legend1";
            this.chartHum.Legends.Add(legend6);
            this.chartHum.Location = new System.Drawing.Point(6, 42);
            this.chartHum.Name = "chartHum";
            this.chartHum.Size = new System.Drawing.Size(572, 512);
            this.chartHum.TabIndex = 4;
            this.chartHum.Text = "chart1";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1225, 586);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox4);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.chartTemp);
            this.tabPage1.Controls.Add(this.chartHum);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1217, 560);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Gráficos";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.labelBattery);
            this.tabPage2.Controls.Add(this.labelHum);
            this.tabPage2.Controls.Add(this.textBoxBattery);
            this.tabPage2.Controls.Add(this.textBoxHum);
            this.tabPage2.Controls.Add(this.labelTemp);
            this.tabPage2.Controls.Add(this.textBoxTemp);
            this.tabPage2.Controls.Add(this.Piso);
            this.tabPage2.Controls.Add(this.comboBoxPiso);
            this.tabPage2.Controls.Add(this.dataGridViewSensorData);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1217, 560);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tabela";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // labelBattery
            // 
            this.labelBattery.AutoSize = true;
            this.labelBattery.Location = new System.Drawing.Point(35, 184);
            this.labelBattery.Name = "labelBattery";
            this.labelBattery.Size = new System.Drawing.Size(40, 13);
            this.labelBattery.TabIndex = 12;
            this.labelBattery.Text = "Battery";
            // 
            // labelHum
            // 
            this.labelHum.AutoSize = true;
            this.labelHum.Location = new System.Drawing.Point(35, 132);
            this.labelHum.Name = "labelHum";
            this.labelHum.Size = new System.Drawing.Size(55, 13);
            this.labelHum.TabIndex = 11;
            this.labelHum.Text = "Humidade";
            // 
            // textBoxBattery
            // 
            this.textBoxBattery.Location = new System.Drawing.Point(102, 181);
            this.textBoxBattery.Name = "textBoxBattery";
            this.textBoxBattery.Size = new System.Drawing.Size(151, 20);
            this.textBoxBattery.TabIndex = 10;
            this.textBoxBattery.TextChanged += new System.EventHandler(this.textBoxBattery_TextChanged);
            // 
            // textBoxHum
            // 
            this.textBoxHum.Location = new System.Drawing.Point(102, 129);
            this.textBoxHum.Name = "textBoxHum";
            this.textBoxHum.Size = new System.Drawing.Size(151, 20);
            this.textBoxHum.TabIndex = 9;
            this.textBoxHum.TextChanged += new System.EventHandler(this.textBoxHum_TextChanged);
            // 
            // labelTemp
            // 
            this.labelTemp.AutoSize = true;
            this.labelTemp.Location = new System.Drawing.Point(29, 73);
            this.labelTemp.Name = "labelTemp";
            this.labelTemp.Size = new System.Drawing.Size(67, 13);
            this.labelTemp.TabIndex = 8;
            this.labelTemp.Text = "Temperatura";
            // 
            // textBoxTemp
            // 
            this.textBoxTemp.Location = new System.Drawing.Point(102, 70);
            this.textBoxTemp.Name = "textBoxTemp";
            this.textBoxTemp.Size = new System.Drawing.Size(151, 20);
            this.textBoxTemp.TabIndex = 7;
            this.textBoxTemp.TextChanged += new System.EventHandler(this.textBoxTemp_TextChanged);
            // 
            // Piso
            // 
            this.Piso.AutoSize = true;
            this.Piso.Location = new System.Drawing.Point(42, 17);
            this.Piso.Name = "Piso";
            this.Piso.Size = new System.Drawing.Size(27, 13);
            this.Piso.TabIndex = 6;
            this.Piso.Text = "Piso";
            // 
            // comboBoxPiso
            // 
            this.comboBoxPiso.FormattingEnabled = true;
            this.comboBoxPiso.Location = new System.Drawing.Point(102, 14);
            this.comboBoxPiso.Name = "comboBoxPiso";
            this.comboBoxPiso.Size = new System.Drawing.Size(151, 21);
            this.comboBoxPiso.TabIndex = 5;
            this.comboBoxPiso.SelectedIndexChanged += new System.EventHandler(this.comboBoxPiso_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Broker IP";
            // 
            // textBoxBrokerIP
            // 
            this.textBoxBrokerIP.Location = new System.Drawing.Point(225, 6);
            this.textBoxBrokerIP.Name = "textBoxBrokerIP";
            this.textBoxBrokerIP.Size = new System.Drawing.Size(100, 20);
            this.textBoxBrokerIP.TabIndex = 6;
            this.textBoxBrokerIP.Text = "127.0.0.1";
            // 
            // btnConnectToBroker
            // 
            this.btnConnectToBroker.Location = new System.Drawing.Point(331, 4);
            this.btnConnectToBroker.Name = "btnConnectToBroker";
            this.btnConnectToBroker.Size = new System.Drawing.Size(116, 23);
            this.btnConnectToBroker.TabIndex = 7;
            this.btnConnectToBroker.Text = "Connect to Broker";
            this.btnConnectToBroker.UseVisualStyleBackColor = true;
            this.btnConnectToBroker.Click += new System.EventHandler(this.btnConnectToBroker_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(453, 4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 8;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(548, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(460, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Broker Comes Connected By Default, The TextBox Associated should be used in case " +
    "of failure!";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(25, 42);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(51, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "piso1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(82, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(54, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "piso 2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(673, 42);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(54, 17);
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "piso 1";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(760, 42);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(54, 17);
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Text = "piso 2";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // FormShowData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 634);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnectToBroker);
            this.Controls.Add(this.textBoxBrokerIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormShowData";
            this.Text = "Show Data";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSensorData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartHum)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewSensorData;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTemp;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHum;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBrokerIP;
        private System.Windows.Forms.Button btnConnectToBroker;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxPiso;
        private System.Windows.Forms.Label Piso;
        private System.Windows.Forms.TextBox textBoxTemp;
        private System.Windows.Forms.Label labelTemp;
        private System.Windows.Forms.Label labelBattery;
        private System.Windows.Forms.Label labelHum;
        private System.Windows.Forms.TextBox textBoxBattery;
        private System.Windows.Forms.TextBox textBoxHum;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox4;
    }
}

