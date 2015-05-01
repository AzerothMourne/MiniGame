using UnityEngine;
using System.Collections;
using System.Net;
using NATUPNPLib;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Text;

public class tcpAddress : MonoBehaviour
{
    private static string GetExternalIP()
    {
        string tempip = "";
        try
        {
            WebRequest wr = WebRequest.Create("http://www.ip138.com/ip2city.asp");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("[") + 1;
            int end = all.IndexOf("]", start);
            tempip = all.Substring(start, end - start);
            sr.Close();
            s.Close();
        }
        catch
        {
        }
        return tempip;
    }
    void getIPAndPort()
    {
        //获取Host Name
        string serverName = Dns.GetHostName();
        Debug.Log("Server名称：" + serverName);
        //从当前Host Name解析IP地址，筛选IPv4地址是本机的内网IP地址。
        IPAddress internalIP = IPAddress.Parse(MGFoundtion.getInternIP());
        Debug.Log("Server内网IP：" + internalIP);

        UPnPNAT upnp = new UPnPNAT();
        IStaticPortMappingCollection staticPortMappingCollection = upnp.StaticPortMappingCollection;
        if (staticPortMappingCollection == null)
        {
            Debug.Log("没有检测到路由器，或者路由器不支持UPnP功能。");
            return;
        }

        int internalPort = MGGlobalDataCenter.defaultCenter().UPNPPort;//Server的端口号
        int externalPort = MGGlobalDataCenter.defaultCenter().UPNPPort;//对应映射的外部端口号

        staticPortMappingCollection.Add(
            externalPort,//外网端口
            "TCP",//协议类型
            internalPort,//内网端口
            internalIP.ToString(),//内网IP地址
            true,//是否开启
            "UPNP_Test"//描述
            );

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        EndPoint serverEndPoint = new IPEndPoint(internalIP, internalPort);
        serverSocket.Bind(serverEndPoint);
        serverSocket.Listen(1);
        Debug.Log("开启服务端监听......\n");

        Thread serverThread = new Thread(() =>
        {
            Socket socket = null;
            while (true)
            {
                if (socket != null || (socket = serverSocket.Accept()) != null)
                {
                    byte[] byteRec = new byte[1024 * 1024];//设置消息接收缓存区
                    int msgLen = socket.Receive(byteRec);
                    string msgRec = Encoding.UTF8.GetString(byteRec, 0, msgLen);
                    Debug.Log("来自Client的消息：" + msgRec);
                }
            }
        });
        serverThread.IsBackground = true;
        serverThread.Start();

        Thread clientThread = new Thread(() =>
        {
            ClientConnectToServer(IPAddress.Parse(GetExternalIP()), externalPort);
        });
        clientThread.Start();

    }
    static void ClientConnectToServer(IPAddress remoteIP, int remotePort)
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(remoteIP, remotePort));
        IPEndPoint remoteEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
        for (int i = 0; i < 3; i++)
        {
            string msgFromClient = "hello";
            byte[] bytes = Encoding.UTF8.GetBytes(msgFromClient);
            //客户端向服务端发送消息
            clientSocket.Send(bytes);
            Thread.Sleep(1000);
        }
    }
}