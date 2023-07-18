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

using skeleton_renderer_wpf.data;

namespace skeleton_renderer_wpf.module
{
    class Client
    {
        private TcpClient client;
        private NetworkStream ns;
        List<DerivedEvent> list_dEvent;

        public bool Connected
        {
            get
            {
                if (client == null) return false;
                return client.Connected;
            }
        }

        public Client(DerivedEvent event_main,DerivedEvent event_grid,DerivedEvent event_skeleton)
        {
            list_dEvent = new List<DerivedEvent>();
            list_dEvent.Add(event_main);
            list_dEvent.Add(event_grid);
            list_dEvent.Add(event_skeleton);
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
                    string msg = "";
                    if (DeserializeJson(recvData)) //Deserialize 성공
                    {
                        msg += (char)2;
                        msg += (char)6;
                        msg += (char)3;
                    }
                    else //Deserialize 실패
                    {
                        msg += (char)2;
                        msg += (char)21;
                        msg += (char)3;
                    }
                    int byteCount = Encoding.ASCII.GetByteCount(msg);
                    byte[] sendData = new byte[byteCount];
                    sendData = Encoding.ASCII.GetBytes(msg);
                    if (!client.Connected)
                        if (!ns.DataAvailable) break;
                    ns.Write(sendData, 0, sendData.Length);
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

        private bool DeserializeJson(string data)
        {
            try
            {
                data = data.Replace(((char)2).ToString(), "");
                data = data.Replace(((char)3).ToString(), "");
                string[] bodys = data.Split('/');
                List<DerivedData> list_dData = new List<DerivedData>();
                foreach (string body in bodys)
                {
                    if (body == "") continue;
                    DerivedData derivedData = new DerivedData();
                    JObject derivedJson = JsonConvert.DeserializeObject<dynamic>(body);
                    derivedData.part = derivedJson.Value<string>("part");
                    derivedData.time = derivedJson.Value<string>("time");
                    derivedData.roll = derivedJson.Value<float>("roll");
                    derivedData.pitch = derivedJson.Value<float>("pitch");
                    derivedData.yaw = derivedJson.Value<float>("pitch");
                    JArray vJson = derivedJson.Value<JArray>("velocity");
                    float[] velocity = vJson.ToObject<float[]>();
                    derivedData.vX = velocity[0];
                    derivedData.vY = velocity[1];
                    derivedData.vZ = velocity[2];
                    list_dData.Add(derivedData);
                }

                //부위 별로 정렬
                list_dData = list_dData.OrderBy(x => x.part).ToList();

                //이벤트 발생
                foreach(DerivedEvent dEvent in list_dEvent)
                {
                    dEvent.raise(list_dData);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            


        }



        public void Close()
        {
            if (ns != null) ns.Close();
            if (client != null) client.Close();
        }
    }
}
