using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DataStorageApp
{

    class Program
    {
        static void Main(string[] args)
        {
            //-------------------------------mqtt-----------------------------

            string ipAddress = "127.0.0.1";

            if (args.Length > 0)
            {
                if (args.Length % 2 != 0)
                {
                    Console.WriteLine("Numero incorreto de parametros");
                    Console.ReadKey();
                    return;
                }
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--ip")
                    {
                        ipAddress = args[i + 1];
                    }
                }
            }

            MqttClient mClient = new MqttClient(IPAddress.Parse(ipAddress));
            string[] mStrTopicsInfo = { "sensors" ,"alerts"};
            mClient.Connect(Guid.NewGuid().ToString());

            if (!mClient.IsConnected)
            {
                Console.WriteLine("Error connecting to message broker...", "Error");
                return;
            }

            //Subscribe to topics
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            mClient.Subscribe(mStrTopicsInfo, qosLevels);

            //New Msg Arrived
            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }

        private static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //----------------------------------bd-----------------------------
            string connectionString = Properties.Settings.Default.ConnectionString;

            SqlConnection sqlConnection = null;
            if (e.Topic == "sensors")
            {
                string json = (Encoding.UTF8.GetString(e.Message));
                Sensor sensor = JsonConvert.DeserializeObject<Sensor>(json);

                try
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();

                    //inserir para a tabela Sensors se nao existir
                    SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM Sensors WHERE isPersonal=@isPersonal AND " +
                        "UserID IS NULL AND Floor = @floor)" +
                        "INSERT INTO Sensors (isPersonal,Floor) VALUES (@isPersonal,@sensorID)", sqlConnection);
                    cmd.Parameters.AddWithValue("@sensorID", sensor.SensorID);
                    cmd.Parameters.AddWithValue("@isPersonal", 0);
                    cmd.Parameters.AddWithValue("@floor", sensor.SensorID);

                    //SqlDataReader select = cmd.ExecuteReader();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine($"New Value added to Sensors :{sensor.SensorID.ToString()} - 0");
                    }
                    else
                    {
                        Console.WriteLine("Duplicated Sensor Resource");
                    }

                    //inserir para tabela sensorsData se nao existir
                    cmd = new SqlCommand("IF NOT EXISTS (SELECT * " +
                        "FROM SensorsData " +
                        "WHERE SensorId = @sensorID AND Battery = @battery " +
                        "AND Temperature = @temperature AND Humidity = @humidity " +
                        "AND Timestamp = @timestamp AND isValid = @isValid)" +
                        "INSERT INTO SensorsData(SensorId,Battery,Temperature,Humidity,Timestamp,isValid) VALUES (@sensorID ,@battery ,@temperature, @humidity, @timestamp, @isValid)", sqlConnection);
                    //cmd = new SqlCommand("INSERT INTO SensorsData(SensorId,Battery,Temperature,Humidity,Timestamp,isValid) VALUES (@sensorID ,@battery ,@temperature, @humidity, @timestamp, @isValid)", sqlConnection);
                    cmd.Parameters.AddWithValue("@sensorID", sensor.SensorID);
                    cmd.Parameters.AddWithValue("@battery", sensor.Battery);
                    cmd.Parameters.AddWithValue("@temperature", sensor.Temperature);
                    cmd.Parameters.AddWithValue("@humidity", sensor.Humidity);
                    cmd.Parameters.AddWithValue("@timestamp", sensor.Timestamp);
                    cmd.Parameters.AddWithValue("@isValid", 1);

                    result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine($"New Value added to SensorsData: {sensor.SensorID.ToString()}-{sensor.Battery.ToString()}-{sensor.Temperature.ToString()}-{sensor.Humidity.ToString()}-{sensor.Timestamp.ToString()}");
                    }
                    else
                    {
                        Console.WriteLine("Duplicated SensorData Resource");
                    }

                    sqlConnection.Close();

                }
                catch (Exception exeption)
                {
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                    Console.WriteLine("Error Ocurred : " + exeption.Message);
                }
            }
            else if(e.Topic == "alerts")
            {
                string json = (Encoding.UTF8.GetString(e.Message));
                Alert alert = JsonConvert.DeserializeObject<Alert>(json);

                try
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();

                    foreach(Sensor s in alert.Sensor){

                        //inserir para a tabela Alerts se nao existir
                        SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT * " +
                            "FROM Alerts " +
                            "WHERE type = @type AND " +
                            "value = @value AND " +
                            "operator = @operator AND " +
                            "between_value = @between_value AND " +
                            "message = @message AND " +
                            "enabled = @enabled AND " +
                            "sensorID = @sensorID AND " +
                            "timestamp = @timestamp) " +
                            "INSERT INTO Alerts VALUES (@type, @value,@operator,@between_value,@message,@enabled,@sensorID,@timestamp)", sqlConnection);
                        cmd.Parameters.AddWithValue("@type", alert.Type);
                        cmd.Parameters.AddWithValue("@value", alert.Value);
                        cmd.Parameters.AddWithValue("@operator", alert.Operator);
                        cmd.Parameters.AddWithValue("@between_value", alert.Between_value);
                        cmd.Parameters.AddWithValue("@message", alert.Message);
                        if (alert.Enabled.ToString() == "enabled")
                        {
                            cmd.Parameters.AddWithValue("@enabled", true);

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@enabled", false);
                        }
                        cmd.Parameters.AddWithValue("@sensorID", s.SensorID);
                        cmd.Parameters.AddWithValue("@timestamp",s.Timestamp);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            Console.WriteLine($"New Value added to Alerts :{alert.Type.ToString()} - {alert.Value.ToString()}- {alert.Operator.ToString()}-{alert.Between_value.ToString()}-{alert.Message.ToString()}-{alert.Enabled.ToString()}-{s.SensorID}-{s.Timestamp}");
                        }
                        else
                        {
                            Console.WriteLine("Duplicated Alert Resource");
                        }

                    }
                        sqlConnection.Close();

                }
                catch (Exception exeption)
                {
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                    Console.WriteLine("Error Ocurred : " + exeption.Message);
                }
            }
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

    public class Alert
    {
        public String Type { get; set; }
        public int Value { get; set; }
        public String Operator { get; set; }
        public int Between_value { get; set; }
        public String Message { get; set; }
        public String Enabled { get; set; }
        public List<Sensor> Sensor { get; set; }


        //for new data just add new prop

    }
}
