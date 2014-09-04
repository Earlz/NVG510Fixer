using MinimalisticTelnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Earlz.NVG510Controller
{
    public interface ILogger
    {
        void Log(string msg);
    }
    public class ProblemFixer
    {
        public class NullLogger : ILogger
        {
            public void Log(string msg)
            {
            }
        }
        string Password;
        string Address;
        int Port;
        ILogger Logger;
        public ProblemFixer(string address, int port, string password, ILogger logger=null)
        {
            Address=address;
            Port=port;
            Password=password;
            Logger = logger ?? new NullLogger();
        }
        void NshLogin(TelnetConnection telnet)
        {
            Logger.Log("Logging in as admin..");
            string output=telnet.Login("admin", Password, 2000);
            if (output.Contains("Login incorrect"))
            {
                Logger.Log("Got incorrect login message! Ensure that you typed the correct access code!");
                throw new ApplicationException("Can not reach nsh shell");
            }
            output = telnet.Read();
            telnet.WriteLine(""); //because apparently it doesn't always print the prompt otherwise
            output = telnet.Read(3000);
            Assert(output.Contains("Axis"), "nsh shell does not appear to be working... or something");
        }
        void SaveChanges(TelnetConnection telnet)
        {
            telnet.WriteLine("validate");
            string output=telnet.Read();
            Assert(output.Contains("succeeded"), "Validation did not appear to be successful");
            telnet.WriteLine("apply");
            telnet.WriteLine("save");
            output = telnet.Read();
            //Assert(output.Contains("Saving the database"), "Settings did not appear to be properly saved to persistent database");
            Logger.Log("Changes Saved");
        }
        
        public void Assert(bool cond, string message)
        {
            if(!cond)
            {
                Logger.Log("Assertion Failure: " + message);
                Logger.Log("Attempting to continue despite possible error");
            }
        }
        public void FixRedirect()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending set mgmt.lan-redirect.enable off");
                telnet.WriteLine("set mgmt.lan-redirect.enable off");
                SaveChanges(telnet);
            }
        }

        public void DisableDhcp()
        {
            //conn[1].dhcps-enable
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending set conn[1].dhcps-enable off");
                telnet.WriteLine("set conn[1].dhcps-enable off");
                SaveChanges(telnet);
            }
        }

        public void EnableDhcp()
        {
            //conn[1].dhcps-enable
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending set conn[1].dhcps-enable on");
                telnet.WriteLine("set conn[1].dhcps-enable on");
                SaveChanges(telnet);
            }
        }
        public void FactoryReset()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending defaults");
                telnet.WriteLine("defaults");
                output = telnet.Read(1000);
                Assert(output.Contains("the factory defaults"), "Factory Reset request did not appear to succeed");
                Logger.Log("Factory Reset done. The device may take a minute to reset completely and come back online");
            }
        }

        public string DumpInfo()
        {
            StringBuilder output = new StringBuilder(10 * 1024);
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                output.AppendLine(telnet.Read());
                Logger.Log("Sending dump command");
                telnet.WriteLine("dump");
                Thread.Sleep(500);
                output.AppendLine(telnet.Read(1000));
                Logger.Log("Sending mfg command");
                telnet.WriteLine("mfg show");
                Thread.Sleep(500);
                output.AppendLine(telnet.Read(1000));
              //  Assert(output.Contains("the factory defaults"), "Factory Reset request did not appear to succeed");
                Logger.Log("Done receiving configuration data");
            }
            return output.ToString();
        }

        public void Reboot()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending reboot");
                telnet.WriteLine("reboot");
                telnet.WriteLine("");
                try
                {
                    telnet.Read(); //must read in order for command to actually be executed
                }
                catch
                {
                    //just in case modem decides to reboot early
                }
            }
        }
        public void EnableBridgeMode()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending set link[1].port-vlan.ports \"lan-2 lan-3 lan-4 ssid-1 ssid-2 ssid-3 ssid-4\"");
                telnet.WriteLine("set link[1].port-vlan.ports \"lan-2 lan-3 lan-4 ssid-1 ssid-2 ssid-3 ssid-4\"");
                Logger.Log("Sending set link[2].port-vlan.ports \"lan-1\"");
                telnet.WriteLine("set link[2].port-vlan.ports \"lan-1\"");
                SaveChanges(telnet);
            }
        }
        public void DisableBridgeMode()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                Logger.Log("Sending set link[1].port-vlan.ports \"lan-1 lan-2 lan-3 lan-4 ssid-1 ssid-2 ssid-3 ssid-4\"");
                telnet.WriteLine("set link[1].port-vlan.ports \"lan-1 lan-2 lan-3 lan-4 ssid-1 ssid-2 ssid-3 ssid-4\"");
                Logger.Log("Sending set link[2].port-vlan.ports \"vc-1\"");
                telnet.WriteLine("set link[2].port-vlan.ports \"vc-1\"");
                SaveChanges(telnet);
            }
        }
        public void EnableUpnp()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                telnet.WriteLine("set mgmt.upnp.enable on");
                SaveChanges(telnet);
            }
        }
        public void DisableUpnp()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                telnet.WriteLine("set mgmt.upnp.enable off");
                SaveChanges(telnet);
            }
        }
        public void UninstallBackdoor()
        {
            using (var telnet = new TelnetConnection(Address, Port))
            {
                NshLogin(telnet);
                string output = telnet.Read();
                telnet.WriteLine("!");
                telnet.WriteLine("");
                telnet.WriteLine("pfs -r /var/etc/inetd.d/telnet28");
                telnet.WriteLine("sleep 5");
                telnet.WriteLine("pfs -s");
               // telnet.WriteLine("pfs -l");
                telnet.WriteLine("exit");
                Logger.Log(telnet.Read()); //must read in order for command to actually be executed
            }

        }
    }
}
