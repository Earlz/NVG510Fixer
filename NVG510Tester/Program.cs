using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Earlz.NVG510Controller;
using MinimalisticTelnet;


namespace NVG510Tester
{
    class Program
    {
        static void Main(string[] args)
        {
           // var exploit = new WebExploiter("192.168.1.254", "8894501933");
          //  exploit.EnableBackdoor(true);
            
            
            var fixer=new ProblemFixer("192.168.1.254", 28, "8894501933");
            fixer.DisableDhcp();

            //fixer.FactoryReset();
            return;
            //create a new telnet connection to hostname "gobelijn" on port "23"
            TelnetConnection tc = new TelnetConnection("192.168.1.254", 23);

            //login with user "root",password "rootpassword", using a timeout of 100ms, 
            //and show server output
            string s = tc.Login("admin", "0387371392", 1000);
            Console.Write(s);

            // server output should end with "$" or ">", otherwise the connection failed
            string prompt = s.TrimEnd();
            prompt = s.Substring(prompt.Length - 1, 1);
            if (prompt != "$" && prompt != ">")
                throw new Exception("Connection failed");

            prompt = "";

            // while connected
            while (tc.IsConnected)
            {
                // display server output
                Console.Write(tc.Read());

                // send client input to server
                prompt = Console.ReadLine();
                tc.WriteLine(prompt);

                // display server output
                Console.Write(tc.Read());
            }

            Console.WriteLine("***DISCONNECTED");
            Console.ReadLine();
        }
    }
}
