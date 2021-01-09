using System;
using System.Net;
using System.Threading;

namespace demo
{
    class Program
    {
        private static bool LINUX = true;
        static void Main(string[] args)
        {
            for (int i = 0; i <= 19; i++)
            {
                FtpClientX f;
                if (LINUX)
                {
                    if (i % 2 == 0)
                        f = new FtpClientX("185.68.21.218", 1021, "rick", "test");
                    else
                        f = new FtpClientX("185.68.21.218", 21, "ftp-user", "Abc12345");
                }
                else
                {
                    if (i % 2 == 0)
                        f = new FtpClientX("45.11.27.54", 1021, "rick", "test");
                    else
                        f = new FtpClientX("45.11.27.54", 2021, "iis", "Test123");
                }

                f.Serve();
            }
            new Thread(Stat).Start();
            Console.ReadLine();
        }

        static void Stat()
        {
            while (true)
            {
                Thread.Sleep(3000);
                foreach (var z in FtpClientX.Succ)
                    Console.WriteLine($@"{z.Key.PadRight(12,' ')}: ok/err: {z.Value.ToString().PadRight(3, ' ')}/{FtpClientX.Fail[z.Key].ToString().PadRight(3,' ')}, bytes: {FtpClientX.Bytes[z.Key].ToString().PadRight(8,' ')}");
                Console.WriteLine();
            }
        }
    }
}
