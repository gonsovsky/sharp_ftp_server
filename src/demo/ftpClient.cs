using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using FluentFTP;

namespace demo
{
    public class FtpClientX
    {
        public string Server;
        public int Port;
        public string Login;
        public string Pass;

        public FtpClientX(string server, int port, string login, string pass)
        {
            Server = server;
            Port = port;
            Login = login;
            Pass = pass;
        }

        static FtpClientX()
        {
            Check = System.IO.File.ReadAllText("1.txt");
            return;
            var chunk = "1234567890";
            var sb = new StringBuilder();
            for (int i = 1; i <= 1000; i++)
            {
                sb.Append(chunk);
            }

            Check = sb.ToString();
        }

        public string Dn(string file)
        {
            using (var conn = new FtpClient(Server, Port, Login, Pass))
            {
                conn.Connect();

                var ms = new MemoryStream();
                conn.Download(ms, file);

                conn.Disconnect();

                var x = Encoding.ASCII.GetString(ms.ToArray());

                return x;
            }
        }

        public List<string> List()
        {
            using (var conn = new FtpClient(Server, Port, Login, Pass))
            {
                conn.DownloadDataType = FtpDataType.ASCII;
                conn.Encoding = Encoding.ASCII;
                conn.Connect();
                conn.Encoding = Encoding.ASCII;
                var res = conn.GetListing("", FtpListOption.NameList)
                    .Select(x => x.Name).ToList();
                conn.Disconnect();
                return res;
            }
        }

        public Thread Tr;

        public void Serve()
        {
            Succ[key] = 0;
            Fail[key] = 0;
            Bytes[key] = 0;
            Tr = new Thread(ServeThread);
            Tr.Start();
        }

        public string key =>  Login + " " + Port.ToString();

        public void ServeThread()
        {
            
            while (true)
            {
                try
                {
                    var list = List();
                    foreach (var q in list)
                    {
                        var cn = Dn(q);

                        if (cn.CompareTo(Check) == 0)
                        {
                            Succ[key] = Succ[key] + 1;
                        Bytes[key] = Bytes[key] + cn.Length;
                        }
                        else
                        {
                            Console.WriteLine("error content: " + key + ": " + cn);
                            Fail[key] = Fail[key] + 1;
                        }
                    }
    
                }
                catch (Exception e)
                {
                    Fail[key] = Fail[key] + 1;
                    Console.WriteLine(key + ": " + e.Message);
                    Thread.Sleep(500);
                }
            
            }
        }
        public static ConcurrentDictionary<string, int> Fail = new ConcurrentDictionary<string, int>();
        public static ConcurrentDictionary<string, int> Succ = new ConcurrentDictionary<string, int>();
        public static ConcurrentDictionary<string, int> Bytes = new ConcurrentDictionary<string, int>();
        public static string Check;
    }
}
