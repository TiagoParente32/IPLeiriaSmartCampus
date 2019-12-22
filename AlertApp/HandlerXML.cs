using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AlertApp
{
    class HandlerXML
    {
        public HandlerXML(string xmlFile)
        {
            XmlFilePath = xmlFile;
        }
        public string XmlFilePath { get; set; }
        public Boolean AddAlertNormal(String id,String t,String val,String op,String desc)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNodeList nodes = doc.SelectNodes($"alerts/alert/id");
            foreach (XmlNode item in nodes)
            {
                if (item.InnerText == id)
                {
                    return false;

                }
            }


            XmlNode alerts = doc.SelectSingleNode("/alerts");

            
           
            XmlElement alert = doc.CreateElement("alert");

            XmlElement type = doc.CreateElement("type");
            type.InnerText = t;
            XmlElement iden = doc.CreateElement("id");
            iden.InnerText = id;
            XmlElement value = doc.CreateElement("value");
            value.InnerText = val;
            XmlElement @operator = doc.CreateElement("operator");
            @operator.InnerText = op;

            XmlElement enable = doc.CreateElement("enable");
            enable.InnerText = "enabled";

            XmlElement description = doc.CreateElement("desc");
            description.InnerText = desc;

            alert.AppendChild(iden);
            alert.AppendChild(type);
            alert.AppendChild(value);
            alert.AppendChild(@operator);
            alert.AppendChild(enable);
            alert.AppendChild(description);
            alerts.AppendChild(alert);

            doc.Save(XmlFilePath);
            return true;
        }
        public Boolean AddAlertBetween(String id,String t, String val, String op, String bet, string desc)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNodeList nodes = doc.SelectNodes($"alerts/alert/id");
            foreach (XmlNode item in nodes)
            {
                 if (item.InnerText == id)
                {
                    return false;

                }
            }
            

            XmlNode alerts = doc.SelectSingleNode("/alerts");

            XmlElement alert = doc.CreateElement("alert");
            XmlElement type = doc.CreateElement("type");
            type.InnerText = t;
            XmlElement iden = doc.CreateElement("id");
            iden.InnerText = id;
            XmlElement value = doc.CreateElement("value");
            value.InnerText = val;
            XmlElement @operator = doc.CreateElement("operator");
            @operator.InnerText = op;
            XmlElement description = doc.CreateElement("desc");
            description.InnerText = desc;

            alert.AppendChild(iden);
            alert.AppendChild(type);
            alert.AppendChild(value);
            alert.AppendChild(@operator);

            XmlElement enable = doc.CreateElement("enable");
            enable.InnerText = "enabled";

            

            XmlElement betweenValue = doc.CreateElement("betweenValue");
            betweenValue.InnerText = bet;
            alert.AppendChild(betweenValue);

            alert.AppendChild(enable);
            alert.AppendChild(description);
            alerts.AppendChild(alert);
            

            doc.Save(XmlFilePath);
            return true;

        }
        internal object GetAllType(string v)
        {
            List<string> titles = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNodeList nodes = doc.SelectNodes($"alerts/alert[type='{v}']/id");
            foreach (XmlNode item in nodes)
            { 
                titles.Add(item.InnerText);
            }
            return titles;
        }
        internal string getValue(string v)
        {
            String value;
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNode node = doc.SelectSingleNode($"alerts/alert[id='{v}']/value");
            value = node.InnerText;
            return value;
        }
        internal object GetAllDesc(string v)
        {
            List<string> titles = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNodeList nodes = doc.SelectNodes($"alerts/alert[type='{v}']/desc");
            foreach (XmlNode item in nodes)
            {
                titles.Add(item.InnerText);
            }
            return titles;
        }
        internal string getOperator(string v)
        {
            String value;
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNode node = doc.SelectSingleNode($"alerts/alert[id='{v}']/operator");
            value = node.InnerText;
            return value;
        }
        internal string getDesc(string v)
        {
            String value;
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNode node = doc.SelectSingleNode($"alerts/alert[id='{v}']/desc");
            value = node.InnerText;
            return value;
        }
        internal string getEnable(string v)
        {
            String value;
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNode node = doc.SelectSingleNode($"alerts/alert[id='{v}']/enable");
            value = node.InnerText;
            return value;
        }
        internal void updateSensor(string id, string value, string op, string betValue, bool enabled, string text)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNode node = doc.SelectSingleNode($"alerts/alert[id='{id}']/enable");
            XmlNode node1 = doc.SelectSingleNode($"alerts/alert[id='{id}']/value");
            XmlNode node2 = doc.SelectSingleNode($"alerts/alert[id='{id}']/betvalue");
            XmlNode node3 = doc.SelectSingleNode($"alerts/alert[id='{id}']/desc");

            node1.InnerText = value;
            node3.InnerText = text;
            if (op == "between")
            {
                node2.InnerText = betValue;
            }
            if (enabled == true)
            {
                node.InnerText = "Enabled";
            }else
            {
                node.InnerText = "Disabled";
            }
            doc.Save(XmlFilePath);
            // } 
        }
        internal string getBetween(string v)
        {
            String value;
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlFilePath);

            XmlNode node = doc.SelectSingleNode($"alerts/alert[id='{v}']/betweenValue");
            value = node.InnerText;
            return value;
        }
    }
}
