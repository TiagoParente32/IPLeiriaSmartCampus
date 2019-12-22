using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AlertApp
{

    public partial class FormAlert : Form
    {
        string XmlPath  = @"sample.xml";
        public string ipAddress = "127.0.0.1";
        private List<Sensor> allValues = new List<Sensor>();
        private List<string> valuesAlert = new List<string>();
        private List<Alert> allValuesAlert = new List<Alert>();
        private string json = "";
        private MqttClient mClient = null;
        private string[] mStrTopicsInfo = { "sensors"};
        


        public FormAlert()
        {
            InitializeComponent();

            try
            {
                //ir buscar ultimo id 
                XmlDocument doc = new XmlDocument();
                doc.Load(XmlPath);
                XmlNode node = doc.SelectSingleNode($"alerts/alert[not(../alert/id > id)]/id");
                numericUpDown1.Value = int.Parse(node.InnerText) + 1;


                comboBoxTipos.SelectedIndex = 1;
                comboBox1.SelectedIndex = 1;
                comboBox2.SelectedIndex = 1;
                textBoxTemperature.Text = "0";
                richTextBox2.Text = "Insert a Description";

                mClient = new MqttClient(IPAddress.Parse(ipAddress));
                mClient.Connect(Guid.NewGuid().ToString());

                if (!mClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Subscribe to topics
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }; //QoS – depends on the topics number
                mClient.Subscribe(mStrTopicsInfo, qosLevels);

                mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                json = (Encoding.UTF8.GetString(e.Message));

                Sensor sensor = JsonConvert.DeserializeObject<Sensor>(json);

                allValues.Add(sensor);


                //verificar se este sensor expulta algum alert ja criado

                string[] topicos = { "alerts" };

                if (!mClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(XmlPath);
                XmlNodeList nodes = doc.SelectNodes($"alerts/alert");

                Alert alert = new Alert();
                alert.Sensor = new List<Sensor>();

                foreach (XmlNode node  in nodes)
                {
                    if(node["enable"].InnerText == "enabled")
                    {
                        //isto nao devia estar aqui tho
                        //verificar se algum faz alerta
                        switch (node["operator"].InnerText)
                        {
                            case ">":
                                if (sensor.Temperature > float.Parse(node["value"].InnerText) && node["type"].InnerText == "Temperature")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    
                                    break;

                                }
                                if (sensor.Humidity > float.Parse(node["value"].InnerText) && node["type"].InnerText == "Humidity")
                                {

                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }

                                    break;

                                }
                                if (sensor.Battery > int.Parse(node["value"].InnerText) && node["type"].InnerText == "Battery")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;
                                }
                                break;
                            case "between":
                                if (node["type"].InnerText == "Temperature" && ((sensor.Temperature >= float.Parse(node["value"].InnerText) && 
                                sensor.Temperature <= float.Parse(node["betweenValue"].InnerText)) || (sensor.Temperature <= float.Parse(node["value"].InnerText) &&
                                sensor.Temperature >= float.Parse(node["betweenValue"].InnerText))))
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;
                                }
                                if (node["type"].InnerText == "Humidity" && ((sensor.Temperature >= float.Parse(node["value"].InnerText) &&
                                sensor.Temperature <= float.Parse(node["betweenValue"].InnerText)) || (sensor.Temperature <= float.Parse(node["value"].InnerText) &&
                                sensor.Temperature >= float.Parse(node["betweenValue"].InnerText))))
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                if (node["type"].InnerText == "Battery" && ((sensor.Temperature >= float.Parse(node["value"].InnerText) &&
                                sensor.Temperature <= float.Parse(node["betweenValue"].InnerText)) || (sensor.Temperature <= float.Parse(node["value"].InnerText) &&
                                sensor.Temperature >= float.Parse(node["betweenValue"].InnerText))))
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                break;
                            case "<":                             
                                if (sensor.Temperature < float.Parse(node["value"].InnerText) && node["type"].InnerText == "Temperature")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                if (sensor.Humidity < float.Parse(node["value"].InnerText) && node["type"].InnerText == "Humidity")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                if (sensor.Battery < int.Parse(node["value"].InnerText) && node["type"].InnerText == "Battery")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                break;
                            case "=":
                                if (sensor.Temperature == float.Parse(node["value"].InnerText) && node["type"].InnerText == "Temperature")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                if (sensor.Humidity == float.Parse(node["value"].InnerText) && node["type"].InnerText == "Humidity")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                if (sensor.Battery == int.Parse(node["value"].InnerText) && node["type"].InnerText == "Battery")
                                {
                                    if (!alert.Sensor.Contains(sensor))
                                    {
                                        alert.Sensor.Add(sensor);
                                    }
                                    break;

                                }
                                break;
                        }
                    }
                    if (alert.Sensor.Count > 0)
                    {
                        alert.AlertID = int.Parse(node["id"].InnerText);
                        alert.Enabled = node["enable"].InnerText;
                        alert.Operator = node["operator"].InnerText;
                        if (alert.Operator.Equals("between"))
                        {
                            alert.Message = "Alarm for alert " + node["id"].InnerText + " with " + node["type"].InnerText + " " + node["operator"].InnerText
                            + " " + node["value"].InnerText + " and " + node["betweenValue"].InnerText;
                        }
                        else
                        {
                            alert.Message = "Alarm for alert " + node["id"].InnerText + " with " + node["type"].InnerText + " " + node["operator"].InnerText
                            + " " + node["value"].InnerText;
                        }
                        alert.Value = int.Parse(node["value"].InnerText);
                        alert.Type = node["type"].InnerText;

                        string jsonToSend = JsonConvert.SerializeObject(alert);
                        mClient.Publish("alerts", Encoding.UTF8.GetBytes(jsonToSend));
                    }
                }
                
                //por cada atributo na classe sensor

                allValues.Add(sensor);

            });
        }

        private void Alert_Click(object sender, EventArgs e)
        {
            HandlerXML xmlHelper = new HandlerXML(XmlPath);
            Alert alert;

            List<Sensor> sensorsID = new List<Sensor>();
            if (int.Parse(textBoxTemperature.Text) > 0 && int.Parse(textBoxTemperature.Text) < 100)
            {

                Boolean check = false;
                if (!comboBox1.SelectedItem.ToString().Equals("between"))
                {
                    check = xmlHelper.AddAlertNormal(numericUpDown1.Value.ToString(), comboBoxTipos.SelectedItem.ToString(), textBoxTemperature.Text, comboBox1.SelectedItem.ToString(), richTextBox2.Text);
                }
                else
                {
                    if (int.Parse(textBox1.Text) < 0 && int.Parse(textBox1.Text) > 100)
                    {
                        MessageBox.Show("Invalid Fields");
                        return;
                    }
                    check = xmlHelper.AddAlertBetween(numericUpDown1.Value.ToString(), comboBoxTipos.SelectedItem.ToString(), textBoxTemperature.Text, comboBox1.SelectedItem.ToString(), textBox1.Text, richTextBox2.Text);
                }
                if (check == false)
                {
                    MessageBox.Show("Invalid ID or Invalid values");
                    return;
                }

            }
            else
            {
                MessageBox.Show("Invalid Fields");

                return;
            }

            MqttClient mqttClient = new MqttClient(IPAddress.Parse(ipAddress));
            MqttClient mClient = mqttClient;
            string[] topicos = { "alerts" };

            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker...");
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPath);

            foreach (Sensor sensor in allValues)
            {

                if (comboBoxTipos.SelectedItem.ToString().Equals("Temperature"))
                {
                    if (comboBox1.SelectedItem.ToString().Equals(">"))
                    {
                        if ( sensor.Temperature > float.Parse(textBoxTemperature.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("="))
                    {
                        if (int.Parse(textBoxTemperature.Text) == sensor.Temperature)
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("<"))
                    {
                        if (sensor.Temperature  < int.Parse(textBoxTemperature.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("betweeen"))
                    {
                        if (int.Parse(textBoxTemperature.Text) < sensor.Temperature && int.Parse(textBox2.Text) > sensor.Temperature)
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                }

                if (comboBoxTipos.SelectedItem.ToString().Equals("Humidity"))
                {
                    if (comboBox1.SelectedItem.ToString().Equals(">"))
                    {
                        if (sensor.Humidity > int.Parse(textBoxTemperature.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("<"))
                    {
                        if (sensor.Humidity < int.Parse(textBoxTemperature.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("="))
                    {
                        if (int.Parse(textBoxTemperature.Text) == sensor.Humidity)
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("betweeen"))
                    {
                        if (int.Parse(textBoxTemperature.Text) < sensor.Humidity && int.Parse(textBoxTemperature.Text) > sensor.Humidity)
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                }

                if (comboBoxTipos.SelectedItem.ToString().Equals("Battery"))
                {
                    if (comboBox1.SelectedItem.ToString().Equals(">"))
                    {
                        if (sensor.Battery > int.Parse(textBoxTemperature.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("<"))
                    {
                        if (sensor.Battery < int.Parse(textBoxTemperature.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }

                    if (comboBox1.SelectedItem.ToString().Equals("="))
                    {
                        if (int.Parse(textBoxTemperature.Text) == sensor.Battery)
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }
                    if (comboBox1.SelectedItem.ToString().Equals("betweeen"))
                    {
                        if (sensor.Battery > int.Parse(textBoxTemperature.Text) && sensor.Battery < int.Parse(textBox2.Text))
                        {
                            if (!sensorsID.Contains(sensor))
                            {
                                sensorsID.Add(sensor);
                            }
                        }
                    }

                }
            }
            if (sensorsID.Count > 0)
            {
                if (!comboBox1.SelectedItem.ToString().Equals("between"))
                {
                    alert = new Alert
                    {
                        AlertID = int.Parse(numericUpDown1.Value.ToString()),
                        Type = comboBoxTipos.SelectedItem.ToString(),
                        Value = int.Parse(textBoxTemperature.Text),
                        Operator = comboBox1.SelectedItem.ToString(),
                        Between_Value = -1,
                        Enabled = "enabled",
                        Message = richTextBox2.Text,
                        Sensor = sensorsID
                    };
                }
                else
                {
                    alert = new Alert
                    {
                        AlertID = int.Parse(numericUpDown1.Value.ToString()),
                        Type = comboBoxTipos.SelectedItem.ToString(),
                        Value = int.Parse(textBoxTemperature.Text),
                        Operator = comboBox1.SelectedItem.ToString(),
                        Between_Value = int.Parse(textBox1.Text),
                        Enabled = "enabled",
                        Message = richTextBox2.Text,
                        Sensor = sensorsID
                    };
                }

                string json = JsonConvert.SerializeObject(alert);

                mClient.Publish("alerts", Encoding.UTF8.GetBytes(json));

            }
            else
            {
                MessageBox.Show("No sensors found");
            }




            MessageBox.Show("Alert Creation Sucessful!!!");
                numericUpDown1.Value = 0;
                textBoxTemperature.Text = "";
                textBox1.Text = "";
                richTextBox2.Text = "";

                // de la adicionar a bd 
            
        }
             
            private void FormAlert_Load(object sender, EventArgs e)
            {
                //   CreateXML();

                //  MessageBox.Show(xmlHelper.GetAll().ToString());
            }


            private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
            {
                HandlerXML xmlHelper = new HandlerXML(XmlPath);
                listBox2.DataSource = xmlHelper.GetAllType(comboBox2.SelectedItem.ToString());
                listBox1.DataSource = xmlHelper.GetAllDesc(comboBox2.SelectedItem.ToString());
            }

            private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
            {
                HandlerXML xmlHelper = new HandlerXML(XmlPath);
                textBoxValue2.Text = xmlHelper.getValue(listBox2.SelectedItem.ToString());
                label10.Text = xmlHelper.getOperator(listBox2.SelectedItem.ToString());
                if (label10.Text == "between")
                {
                    textBox2.Text = xmlHelper.getBetween(listBox2.SelectedItem.ToString());
                    textBox2.Enabled = true;
                }
                if (xmlHelper.getEnable(listBox2.SelectedItem.ToString()) == "enabled")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                richTextBox1.Text = xmlHelper.getDesc(listBox2.SelectedItem.ToString());

            }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            HandlerXML xmlHelper = new HandlerXML(XmlPath);
            //Alert alert;
            //List<Sensor> sensorsID = new List<Sensor>();

            if (int.Parse(textBoxValue2.Text) < 0 && int.Parse(textBoxValue2.Text) > 100)
            {
                MessageBox.Show("Invalid Value...");
                return;
            }
            else
            {
                if (label10.ToString().Equals("between") && (int.Parse(textBox2.Text) > int.Parse(textBoxValue2.Text) || int.Parse(textBoxValue2.Text) > 100))
                {
                    MessageBox.Show("Invalid Value...");
                    return;
                }
                xmlHelper.updateSensor(listBox2.SelectedItem.ToString(), textBoxValue2.Text, label10.ToString(), textBox2.Text, checkBox1.Checked, richTextBox1.Text);
            }
            
            /*
            MqttClient mqttClient = new MqttClient(IPAddress.Parse(ipAddress));
            MqttClient mClient = mqttClient;
            string[] topicos = { "alertsUpdate" };

            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker...");
                return;
            }
            
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPath);

            foreach (Sensor sensor in allValues)
            {

                if (comboBox2.SelectedItem.ToString().Equals("Temperature"))
                {
                    if (label10.ToString().Equals(">"))
                    {
                        if (sensor.Temperature > int.Parse(textBoxValue2.Text))
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("="))
                    {
                        if (int.Parse(textBoxValue2.Text) == sensor.Temperature)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("<"))
                    {
                        if (sensor.Temperature < int.Parse(textBoxValue2.Text))
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("betweeen"))
                    {
                        if (int.Parse(textBoxValue2.Text) < sensor.Temperature && int.Parse(textBox2.Text) > sensor.Temperature)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                }

                if (comboBox2.SelectedItem.ToString().Equals("Humidity"))
                {
                    if (label10.ToString().Equals(">"))
                    {
                        if (sensor.Humidity > int.Parse(textBoxValue2.Text))
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("<"))
                    {
                        if (int.Parse(textBoxValue2.Text) > sensor.Humidity)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("="))
                    {
                        if (int.Parse(textBoxValue2.Text) == sensor.Humidity)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("betweeen"))
                    {
                        if (int.Parse(textBoxValue2.Text) < sensor.Humidity && int.Parse(textBox2.Text) > sensor.Humidity)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                }

                if (comboBox2.SelectedItem.ToString().Equals("Battery"))
                {
                    if (label10.ToString().Equals(">"))
                    {
                        if (int.Parse(textBoxValue2.Text) < sensor.Battery)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("<"))
                    {
                        if (int.Parse(textBoxValue2.Text) > sensor.Battery)
                        {
                            sensorsID.Add(sensor);
                        }
                    }

                    if (label10.ToString().Equals("="))
                    {
                        if (int.Parse(textBoxValue2.Text) == sensor.Battery)
                        {
                            sensorsID.Add(sensor);
                        }
                    }
                    if (label10.ToString().Equals("betweeen"))
                    {
                        if (int.Parse(textBoxValue2.Text) < sensor.Battery && int.Parse(textBox2.Text) > sensor.Battery)
                        {
                            sensorsID.Add(sensor);
                        }
                    }

                }
            }
            if (sensorsID.Count > 0)
            {
                if (!label10.ToString().Equals("between"))
                {
                    if (checkBox1.Checked)
                    {
                        alert = new Alert
                        {
                            AlertID = int.Parse(listBox2.SelectedItem.ToString()),
                            Type = comboBox2.SelectedItem.ToString(),
                            Value = int.Parse(textBoxValue2.Text),
                            Operator = label10.ToString(),
                            Between_Value = -1,
                            Enabled = "enabled",
                            Message = richTextBox2.Text,
                            Sensor = sensorsID
                        };
                    }
                    else
                    {
                        alert = new Alert
                        {
                            AlertID = int.Parse(listBox2.SelectedItem.ToString()),
                            Type = comboBox2.SelectedItem.ToString(),
                            Value = int.Parse(textBoxValue2.Text),
                            Operator = label10.ToString(),
                            Between_Value = -1,
                            Enabled = "disabled",
                            Message = richTextBox1.Text,
                            Sensor = sensorsID
                        };
                    }

                }
                else
                {
                    if (checkBox1.Checked)
                    {
                        alert = new Alert
                        {
                            AlertID = int.Parse(listBox2.SelectedItem.ToString()),
                            Type = comboBox2.SelectedItem.ToString(),
                            Value = int.Parse(textBoxValue2.Text),
                            Operator = label10.ToString(),
                            Between_Value = int.Parse(textBox2.Text),
                            Enabled = "enabled",
                            Message = richTextBox2.Text,
                            Sensor = sensorsID
                        };
                    }
                    else
                    {
                        alert = new Alert
                        {
                            AlertID = int.Parse(listBox2.SelectedItem.ToString()),
                            Type = comboBox2.SelectedItem.ToString(),
                            Value = int.Parse(textBoxValue2.Text),
                            Operator = label10.ToString(),
                            Between_Value = int.Parse(textBox2.Text),
                            Enabled = "disabled",
                            Message = richTextBox2.Text,
                            Sensor = sensorsID
                        };
                    }
                }

                string json = JsonConvert.SerializeObject(alert);

                mClient.Publish("alerts", Encoding.UTF8.GetBytes(json));


            }
*/
        }



        [JsonObject(ItemRequired = Required.Always)]
        public class Alert
        {
            public int AlertID { get; set; }
            public String Type { get; set; }
            public int Value { get; set; }
            public String Operator { get; set; }
            [JsonIgnore]
            [JsonProperty(Required = Required.Default)]
            public int Between_Value { get; set;  }
            public String Enabled { get; set; }
            public String Message { get; set; }
            public List<Sensor> Sensor { get; set; }

        }



        public class Sensor
        {
            public int SensorID { get; set; }
            public float Temperature { get; set; }
            public float Humidity { get; set; }
            public int Battery { get; set; }
            public DateTime Timestamp { get; set; }
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

            }
            catch (Exception exception)
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
            }
            catch (Exception exception)
            {
                MessageBox.Show("Can't disconnect if not connected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
