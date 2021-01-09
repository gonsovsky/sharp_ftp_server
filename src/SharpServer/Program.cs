using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using log4net;

namespace SharpServer
{
    class Program
    {
        static void Logging()
        {
            if (File.Exists("log.txt"))
                File.Delete("log.txt");
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("SharpServer.log4net"));

            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            _log = LogManager.GetLogger(typeof(Program));
        }

        protected static ILog _log;

        static void Main(string[] args)
        {
            Logging();
            FtpUserStore.Validate("rick", "test");

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            using (SharpServer.Ftp.FtpServer s = new SharpServer.Ftp.FtpServer(new[] { new IPEndPoint(IPAddress.Any, 1021) }))
            {
                s.Start();

                Console.WriteLine("Press any key to stop...");
                Console.Read();
            }

            return;

            using (Server<SharpServer.Email.ImapClientConnection> imapServer = new Server<SharpServer.Email.ImapClientConnection>(143))
            using (Server<SharpServer.Email.SmtpClientConnection> smtpServer = new Server<SharpServer.Email.SmtpClientConnection>(25))
            {
                smtpServer.Start();
                imapServer.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }

            return;

            using (Server<SharpServer.Email.Pop3ClientConnection> pop3server = new Server<SharpServer.Email.Pop3ClientConnection>(110))
            using (Server<SharpServer.Email.SmtpClientConnection> smtpServer = new Server<SharpServer.Email.SmtpClientConnection>(25))
            {
                pop3server.Start();
                smtpServer.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }


            return;

            using (Server<SharpServer.Email.SmtpClientConnection> Server = new Server<SharpServer.Email.SmtpClientConnection>(25))
            {
                Server.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);
            }

            return;

        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Fatal((Exception)e.ExceptionObject);
        }
    }
}
