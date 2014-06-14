using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApiNet_SyncUtil
{
    class Program
    {
        static string Username = "";
        static string Password = "";
        static string Mode = "full";
        static string Context = "background";
        static bool Debug = false;
        static string[] Numbers;
        static WhatsAppApi.WhatsApp Instance;

        static void Main(string[] args)
        {
            bool res = processArgs(args);

            if(!res)
            {
                printHelp();
                return;
            }

            Instance = new WhatsAppApi.WhatsApp(Username, Password, "WhatsApp", Debug, true);
            if (Debug)
            {
                WhatsAppApi.Helper.DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
            }
            Instance.OnError += Instance_OnError;
            Instance.OnGetSyncResult += Instance_OnGetSyncResult;
            Instance.Connect();
            Instance.Login();
            if (Instance.ConnectionStatus == WhatsAppApi.ApiBase.CONNECTION_STATUS.LOGGEDIN)
            {
                //logged in
                Instance.SendSync(Numbers, Mode, Context);

                while (Instance.pollMessage(false)) ;
            }
            else
            {
                Console.WriteLine("Login failed: {0}", Instance.ConnectionStatus);
            }
        }

        public static void Instance_OnGetSyncResult(int index, string sid, Dictionary<string, string> existingUsers, string[] failedNumbers)
        {
            foreach (string key in existingUsers.Keys)
            {
                Console.WriteLine(key.Trim(new char[] { '+' }));
            }
            Environment.Exit(0);
        }

        public static void Instance_OnError(string id, string from, int code, string text)
        {
            if (id.StartsWith("sendsync"))
                Console.WriteLine("Sync error: {0}({1})", text, code);
            Environment.Exit(1);
        }

        static void Instance_OnPrintDebug(object value)
        {
            Console.WriteLine(value);
        }

        protected static void printHelp()
        {
            System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string[] data = new string[] {
                "",
                String.Format("{0} {1} by shirioko", info.ProductName, info.ProductVersion),
                "",
                "https://github.com/shirioko/WhatsApiNet-SyncUtil",
                "",
                "",
                "Usage:",
                "\twasyncutil.exe username=PHONENUMBER password=PASSWORD (mode=MODE) (debug=true) (context=CONTEXT) number1 number2 ...",
                "",
                "Example: wasyncutil.exe username=31612345678 password=asdasdasASDASDasd== 3161234567 31576454543 316453634",
                "",
                "Options:",
                "\tmode: sync mode (full or delta)",
                "\tcontext: sync context (registration, background or interactive)",
                "\tdebug: enables debug output (true)"
            };
            foreach(string foo in data)
            {
                Console.WriteLine(foo);
            }
        }

        protected static bool processArgs(string[] args)
        {
            List<string> numbers = new List<string>();
            foreach (string foo in args)
            {
                if (foo.Contains('='))
                {
                    string[] data = foo.Split(new char[] { '=' }, 2);
                    switch (data[0])
                    {
                        case "username":
                            Username = data[1];
                            break;
                        case "password":
                            Password = data[1];
                            break;
                        case "mode":
                            Mode = data[1];
                            break;
                        case "context":
                            Context = data[1];
                            break;
                        case "debug":
                            if (data[1] == "true")
                                Debug = true;
                            break;
                    }
                }
                else
                {
                    numbers.Add(foo);
                }
            }

            Numbers = numbers.ToArray();

            //validate
            bool valid = true;
            if (string.IsNullOrEmpty(Username))
            {
                Console.WriteLine("ERROR: Missing arg username");
                valid = false;
            }
            if (String.IsNullOrEmpty(Password))
            {
                Console.WriteLine("ERROR: Missing arg password");
                valid = false;
            }
            if (Numbers == null || Numbers.Length == 0)
            {
                Console.WriteLine("ERROR: No numbers to sync");
                valid = false;
            }

            return valid;
        }
    }
}
