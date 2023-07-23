using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

using skeleton_renderer_wpf.data;

namespace skeleton_renderer_wpf.module
{
    public class Client
    {
        private TcpClient client;
        private NetworkStream ns;
        public List<DerivedData> currentData;

        public bool Connected
        {
            get
            {
                if (client == null) return false;
                return client.Connected;
            }
        }

        public Client()
        {
            currentData = new List<DerivedData>();
        }

        public void Connect(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), port);
                client.Connect(ipEnd);
                ns = client.GetStream();
                while (client.Connected)
                {
                    if (ns == null) break;
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    
                    int bytesRead = ns.Read(buffer, 0, client.ReceiveBufferSize);
                    string recvData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DeserializeJson(recvData);
                }
            }

            catch(SocketException socketException)
            {
                throw socketException;
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        private void DeserializeJson(string data)
        {
            data = data.Replace(((char)2).ToString(), "");
            data = data.Replace(((char)3).ToString(), "");

            DerivedData derivedData = new DerivedData();
            JObject derivedJson = JsonConvert.DeserializeObject<dynamic>(data);
            derivedData.part = derivedJson.Value<string>("part");
            derivedData.time = derivedJson.Value<string>("time");
            derivedData.roll = derivedJson.Value<float>("roll");
            derivedData.pitch = derivedJson.Value<float>("pitch");
            derivedData.yaw = derivedJson.Value<float>("yaw");
            JArray accJson = derivedJson.Value<JArray>("acc");
            float[] acc = accJson.ToObject<float[]>();
            JArray gyroJson = derivedJson.Value<JArray>("gyro");
            float[] gyro = gyroJson.ToObject<float[]>();
            JArray vJson = derivedJson.Value<JArray>("velocity");
            float[] velocity = vJson.ToObject<float[]>();
            JArray dJson = derivedJson.Value<JArray>("displacement");
            float[] displacement = dJson.ToObject<float[]>();
            derivedData.accX = acc[0];
            derivedData.accY = acc[1];
            derivedData.accZ = acc[2];
            derivedData.gyroX = gyro[0];
            derivedData.gyroY = gyro[1];
            derivedData.gyroZ = gyro[2];
            derivedData.vX = velocity[0];
            derivedData.vY = velocity[1];
            derivedData.vZ = velocity[2];
            derivedData.dX = displacement[0];
            derivedData.dY = displacement[1];
            derivedData.dZ = displacement[2];

            bool partHasExist = false;
            for (int i = 0; i < currentData.Count; i++)
            {
                if (currentData[i].part == derivedData.part)
                {
                    currentData[i] = derivedData;
                    partHasExist = true;
                    break;
                }
            }

            if (!partHasExist)
            {
                currentData.Add(derivedData);
            }

        }



        public void Close()
        {
            if (ns != null) ns.Close();
            if (client != null) client.Close();
        }
    }
}
