using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;

namespace databin
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            //alterar o path depois ;))
            string inputFilename = @"data.bin";
            string ipAddress = "127.0.0.1";
            int sensor_id;
            float temperature;
            float humidity;
            int batery;
            long timestamp;
            DateTime data;
            byte[] bin;
            int pos = 0;

            if(args.Length > 0)
            {
                if(args.Length%2 != 0)
                {
                    Console.WriteLine("Numero incorreto de parametros");
                    Console.ReadKey();
                    return;
                }
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--path")
                    {
                        inputFilename = args[i + 1];
                    }
                    if(args[i] == "--ip")
                    {
                        ipAddress = args[i + 1];
                    }
                }
            }


            MqttClient mqttClient = new MqttClient(IPAddress.Parse(ipAddress));
            MqttClient mClient = mqttClient;
            string[] topicos = { "sensors" };

            mClient.Connect(Guid.NewGuid().ToString());
            if (!mClient.IsConnected)
            {
                Console.WriteLine("Error connecting to message broker...");
                return;
            }

            while (true)
            {
                using(FileStream fileStream = new FileStream(inputFilename, FileMode.Open, FileAccess.Read))
                {
                    using(BinaryReader reader = new BinaryReader(fileStream))
                    {
                        reader.BaseStream.Position += pos;
                        if (reader.PeekChar() > 0)
                        {
                            bin = reader.ReadBytes(24);
                            //lê o byte 0-4 
                            sensor_id = bin[0];
                            //le do byte 4-8
                            temperature = BitConverter.ToSingle(bin, 4);
                            //le do byte 8-12
                            humidity = BitConverter.ToSingle(bin, 8);
                            //le do byte 12-16
                            batery = bin[12];
                            //le do byte 16-24
                            timestamp = BitConverter.ToInt32(bin, 16);

                            data = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc); ;
                            data = data.AddSeconds(timestamp).ToLocalTime();

                            Console.WriteLine($"SensorID : {sensor_id}\nTemperature: {temperature}\nHumidity: {humidity}\nBatery: {batery}\nTimestamp:{data}{Environment.NewLine} ");

                            Sensor sensor = new Sensor
                            {
                                SensorID = sensor_id,
                                Temperature = temperature,
                                Humidity = humidity,
                                Battery = batery,
                                Timestamp = data
                            };

                            string json = JsonConvert.SerializeObject(sensor);
                            
                            mClient.Publish("sensors", Encoding.UTF8.GetBytes(json));

                            pos += 24;
                        }
                    }
                }
                Thread.Sleep(500);
            }

            if (mClient.IsConnected)
            {
                mClient.Unsubscribe(topicos);
                mClient.Disconnect();
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


}
