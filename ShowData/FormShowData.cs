using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ShowData
{

    public partial class FormShowData : Form
    {
        private string json = "";
        private Series seriesTemp = null;
        private Series seriesHum = null;
        private Series seriesTempPiso2 = null;
        private Series seriesHumPiso2 = null;
        private MqttClient mClient = null;
        private string[] mStrTopicsInfo = { "sensors" };
        private BindingList<Sensor> sensorsList = new BindingList<Sensor>();

        public FormShowData()
        {
            InitializeComponent();

            //**************************************** CHART**************************************
            //Titulo
            Title title1 = new Title();
            title1.Font = new Font("Arial", 15, FontStyle.Bold);
            title1.Text = "Temperatura por Timestamp";
            this.chartTemp.Titles.Add(title1);

            Title title2 = new Title();
            title2.Font = new Font("Arial", 15, FontStyle.Bold);
            title2.Text = "Humidade por Timestamp";
            this.chartHum.Titles.Add(title2);
            //Titulo

            //Fields charted por piso
            seriesHum = chartHum.Series.Add("Humidade Piso 1");
            seriesHum.Color = Color.Blue;
            seriesHum.ChartType = SeriesChartType.Spline;
            seriesHum.MarkerStyle = MarkerStyle.Circle;

            seriesTemp = chartTemp.Series.Add("Temperatura Piso 1");
            seriesTemp.Color = Color.Red;
            seriesTemp.ChartType = SeriesChartType.Spline;
            seriesTemp.MarkerStyle = MarkerStyle.Circle;

            seriesHumPiso2 = chartHum.Series.Add("Humidade Piso 2");
            seriesHumPiso2.Color = Color.Green;
            seriesHumPiso2.ChartType = SeriesChartType.Spline;
            seriesHumPiso2.MarkerStyle = MarkerStyle.Circle;


            seriesTempPiso2 = chartTemp.Series.Add("Temperatura Piso 2");
            seriesTempPiso2.Color = Color.Orange;
            seriesTempPiso2.ChartType = SeriesChartType.Spline;
            seriesTempPiso2.MarkerStyle = MarkerStyle.Circle;

            //Fields charted por piso

            //Max e Min values
            chartTemp.ChartAreas[0].AxisY.Maximum = 50;
            chartTemp.ChartAreas[0].AxisY.Minimum = -20;
            chartHum.ChartAreas[0].AxisY.Maximum = 100;
            chartHum.ChartAreas[0].AxisY.Minimum = 0;
            //Max e Min values

            //Zoom e scrool
            chartTemp.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartTemp.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chartTemp.ChartAreas[0].CursorX.AutoScroll = true;
            chartTemp.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            //chartTempHum.ChartAreas[0].AxisX.IsMarginVisible = false;
            chartTemp.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartTemp.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartTemp.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chartTemp.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartTemp.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            
            chartHum.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chartHum.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chartHum.ChartAreas[0].CursorX.AutoScroll = true;
            chartHum.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            //chartTempHum.ChartAreas[0].AxisX.IsMarginVisible = false;
            chartHum.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chartHum.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartHum.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chartHum.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartHum.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            //dropdown
            comboBoxPiso.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPiso.Items.Insert(0, "All");
            comboBoxPiso.Items.Insert(1, "piso 1");
            comboBoxPiso.Items.Insert(2, "piso 2");
            comboBoxPiso.SelectedIndex = 0;

            //Zoom e scrool
            //**************************************** CHART**************************************

            try
            {
                mClient = new MqttClient(IPAddress.Parse(textBoxBrokerIP.Text));

                mClient.Connect(Guid.NewGuid().ToString());

                if (!mClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Subscribe to topics
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }; //QoS – depends on the topics number
                mClient.Subscribe(mStrTopicsInfo, qosLevels);

                //New Msg Arrived
                mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //**************************************** TABELA **************************************
                json = (Encoding.UTF8.GetString(e.Message));
                dataGridViewSensorData.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //deserializa o json recebido para a classe sensor criada abaixo
                Sensor sensor = JsonConvert.DeserializeObject<Sensor>(json);

                sensorsList.Add(sensor);

                dataGridViewSensorData.DataSource = sensorsList;

                //scroll to end
                dataGridViewSensorData.FirstDisplayedScrollingRowIndex = dataGridViewSensorData.RowCount - 1;


                //**************************************** CHART **************************************
                if (sensor.SensorID == 1)
                {

                    seriesTemp.Points.AddXY(sensor.Timestamp.ToString(), sensor.Temperature); 
                    seriesHum.Points.AddXY(sensor.Timestamp.ToString(), sensor.Humidity);
                }
                else if (sensor.SensorID == 2)
                {
                    seriesTempPiso2.Points.AddXY(sensor.Timestamp.ToString(), sensor.Temperature);
                    seriesHumPiso2.Points.AddXY(sensor.Timestamp.ToString(), sensor.Humidity);
                }
                //****************************************CHART * *************************************
            });
        }

        private void btnConnectToBroker_Click(object sender, EventArgs e)
        {
            try
            {
                mClient = new MqttClient(IPAddress.Parse(textBoxBrokerIP.Text));
                
                mClient.Connect(Guid.NewGuid().ToString());

                if (!mClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Successfully connected to broker at " + textBoxBrokerIP.Text, "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //Subscribe to topics
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }; //QoS – depends on the topics number
                mClient.Subscribe(mStrTopicsInfo, qosLevels);

                //New Msg Arrived
                mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                
            }catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                mClient.Unsubscribe(mStrTopicsInfo);
                mClient.Disconnect();
                MessageBox.Show("Disconnected From Broker And Unsubscribed from all topis", "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch(Exception exception)
            {
                MessageBox.Show("Can't disconnect if not connected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
       
        private void comboBoxPiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxPiso.SelectedIndex)
            {
                case 0:
                    dataGridViewSensorData.DataSource = sensorsList;
                    dataGridViewSensorData.Update();
                    break;
                case 1:
                    BindingList<Sensor> filtered = new BindingList<Sensor>(sensorsList.Where(obj => obj.SensorID.ToString().Contains("1")).ToList());
                    dataGridViewSensorData.DataSource = filtered;
                    dataGridViewSensorData.Update();
                    break;
                case 2:
                    BindingList<Sensor> filtered2 = new BindingList<Sensor>(sensorsList.Where(obj => obj.SensorID.ToString().Contains("2")).ToList());
                    dataGridViewSensorData.DataSource = filtered2;
                    dataGridViewSensorData.Update();
                    break;
            }
            //filterData();

        }


        private void textBoxTemp_TextChanged(object sender, EventArgs e)
        {
            filterData();
        }

        private void textBoxHum_TextChanged(object sender, EventArgs e)
        {
            filterData();
        }

        private void textBoxBattery_TextChanged(object sender, EventArgs e)
        {
            filterData();
        }

        private void filterData()
        {
            //indice 0 == todos os "pisos"
            //else filter 
            if(comboBoxPiso.SelectedIndex == 0)
            {
                BindingList<Sensor> filtered2 = new BindingList<Sensor>(sensorsList.Where(obj => obj.Temperature.ToString().Contains(textBoxTemp.Text)).ToList()
                   .Where(obj => obj.Humidity.ToString().Contains(textBoxHum.Text)).ToList()
                   .Where(obj => obj.Battery.ToString().Contains(textBoxBattery.Text)).ToList());
                dataGridViewSensorData.DataSource = filtered2;
                dataGridViewSensorData.Update();
            }
            else
            {
                BindingList<Sensor> filtered2 = new BindingList<Sensor>(sensorsList.Where(obj => obj.Temperature.ToString().Contains(textBoxTemp.Text)).ToList()
                   .Where(obj => obj.Humidity.ToString().Contains(textBoxHum.Text)).ToList()
                   .Where(obj => obj.Battery.ToString().Contains(textBoxBattery.Text)).ToList()
                   .Where(obj => obj.SensorID.ToString().Contains(comboBoxPiso.SelectedIndex.ToString())).ToList());
                dataGridViewSensorData.DataSource = filtered2;
                dataGridViewSensorData.Update();
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Series sz = chartHum.Series["Humidade Piso 1"];
            sz.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Series sz = chartHum.Series["Humidade Piso 2"];
            sz.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Series sz = chartTemp.Series["Temperatura Piso 1"];
            sz.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Series sz = chartTemp.Series["Temperatura Piso 2"];
            sz.Enabled = checkBox4.Checked;
        }
    }

    public class Sensor
    {
        public int SensorID { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public int Battery { get; set; }
        public DateTime Timestamp { get; set; }

        //for new data just add new prop

    }
}
