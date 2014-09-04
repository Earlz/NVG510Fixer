using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Earlz.NVG510Controller;
using System.Threading.Tasks;
using Android.Content.PM;
using System.Net.Sockets;
using Android.Text.Method;

namespace NVG510Fixer
{
    //screw it. Keep it portait orientation otherwise the task restarts and all sorts of hell breaks loose
    [Activity (Label = "Executing Action On Modem", ScreenOrientation=ScreenOrientation.Portrait)]			
    public class ExecutionActivity : TrackedActivity
    {

        string Password;
        string Action;
        string Address;
        void LoadExtras()
        {
            Password = Intent.GetStringExtra("password");
            Address = Intent.GetStringExtra("address");
            Action = Intent.GetStringExtra("action");
        }

        void EnableTelnet(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit=new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");
            }
        }
        void DisableTelnet(string address, string password)
        {               
            lock(ConnectionLocker)
            {
                try
                {
                    var fixer = new ProblemFixer(address, 28, password, Logger);
                    fixer.UninstallBackdoor();
                    Logger.Log("Backdoor will be uninstalled next time your modem is rebooted!");
                    Logger.Log("To enable the backdoor again, you must first reboot your modem");
                    Logger.Log("Note: All existing configuration will be persisted until a factory reset is done");
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");
            }

        }
        void FixRedirect(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit = new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer = new ProblemFixer(address, 28, password, Logger);
                    fixer.FixRedirect();
                    Logger.Log("Fixed redirect problem");
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch (Exception ex)
                {
                    Logger.Log("Error: " + ex.Message);
                }
                Logger.Log("Done!");
            }
        }
        void EnableUpnp(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit = new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer = new ProblemFixer(address, 28, password, Logger);
                    fixer.EnableUpnp();
                    Logger.Log("Enabled UPnP");
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch (Exception ex)
                {
                    Logger.Log("Error: " + ex.Message);
                }
                Logger.Log("Done!");
            }
        }
        void Reboot(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit=new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer=new ProblemFixer(address, 28, password, Logger);
                    fixer.Reboot();
                    Logger.Log("Rebooted");
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");
            }
        }

        void EnableBridge(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit=new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer=new ProblemFixer(address, 28, password, Logger);
                    fixer.EnableBridgeMode();
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");
            }
        }

        void DisableBridge(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit=new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer=new ProblemFixer(address, 28, password, Logger);
                    fixer.DisableBridgeMode();
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");
            }
        }

        void Execute(string address, string password, Action<ProblemFixer> action)
        {
            lock (ConnectionLocker)
            {
                try
                {
                    var exploit = new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer = new ProblemFixer(address, 28, password, Logger);
                    action(fixer);
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch (Exception ex)
                {
                    Logger.Log("Error: " + ex.Message);
                }
                Logger.Log("Done!");
            }
        }

        void DumpInfo(string address, string password)
        {
            string output = "";
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit=new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer=new ProblemFixer(address, 28, password, Logger);
                    output=fixer.DumpInfo();

                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");           
                    RunOnUiThread(() => 
                {
                    ConfigText.Text=output;
                });;
            }        
        }

        void FactoryReset(string address, string password)
        {
            lock(ConnectionLocker)
            {
                try
                {
                    var exploit=new WebExploiter(address, password, Logger);
                    exploit.EnableBackdoorIfNeeded();
                    var fixer=new ProblemFixer(address, 28, password, Logger);
                    fixer.FactoryReset();
                }
                catch (SocketException e)
                {
                    Logger.Log("Error: " + e.Message);
                    Logger.Log("It appears that the port 28 backdoor is not installed or not working properly!");
                }
                catch(Exception ex)
                {
                    Logger.Log("Error: "+ex.Message);
                }
                Logger.Log("Done!");
            }
        }


        public object ConnectionLocker=new object();


        public class TextLogger : ILogger
        {
            Action<string> Action;
            public TextLogger(Action<string> action)
            {
                Action=action;
            }
            public void Log(string msg)
            {
                Action(msg);
            }
        }
        ILogger Logger;
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add("About/Help");
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var uri = Android.Net.Uri.Parse ("http://earlz.net/view/2013/08/03/2006/nvg510-fixer-an-android-application");
            var intent = new Intent (Intent.ActionView, uri); 
            StartActivity (intent);  
            return true;
        }
        protected override void OnResume()
        {
            base.OnResume();


        }
        protected override void OnStart()
        {
            base.OnStart();

        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutString("output", FindViewById<TextView>(Resource.Id.terminalOutput).Text);

        }
        EditText ConfigText;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            SetContentView(Resource.Layout.ExecutionLog);
            LoadExtras();
            if (Action == "dumpInfo")
            {
                var layout=FindViewById<LinearLayout>(Resource.Id.executionLayout);
                var edit = new EditText(this);
                edit.SetMinHeight(100);

               // edit.LayoutParameters = (Android.Views.ViewGroup.LayoutParams) Android.Views.ViewGroup.LayoutParams.MatchParent; // WindowManagerLayoutParams.MatchParent; // | WindowManagerLayoutParams.FillParent;
                layout.AddView(edit);
                ConfigText = edit;
            }
            if (bundle != null)
            {
                var second = new Intent(this, typeof(MainActivity));
                StartActivity(second);
                //FindViewById<TextView>(Resource.Id.terminalOutput).Text = bundle.GetString("output");
                //FindViewById<TextView>(Resource.Id.terminalOutput).Text = foobar;
            }
            if (bundle==null)
            {

                FindViewById<TextView>(Resource.Id.terminalOutput).MovementMethod = new ScrollingMovementMethod();
                Logger = new TextLogger((s) =>
                                    {
                RunOnUiThread(() =>
                              {
                    var textview=FindViewById<TextView>(Resource.Id.terminalOutput); 
                    textview.Append(s + "\n");
                    if(bundle!=null)
                    {
                        //bundle.PutString("output", textview.Text);
                    }
                    var scrollview=FindViewById<ScrollView>(Resource.Id.scrollView1);
                    //scrollview.FullScroll(FocusSearchDirection.Down);

                });
                    });
                FindViewById<TextView>(Resource.Id.terminalOutput).Text="";
                Task.Run(() => {
                    Logger.Log("This may take a few seconds.. Be patient");
                    switch (Action)
                    {
                        case "reboot":
                            Reboot(Address, Password);
                            break;
                        case "enableBridge":
                            EnableBridge(Address, Password);
                            break;
                        case "disableBridge":
                            DisableBridge(Address, Password);
                            break;
                        case "fixRedirect":
                            FixRedirect(Address, Password);
                            break;
                        case "enableTelnet":
                            EnableTelnet(Address, Password);
                            break;
                        case "disableTelnet":
                            DisableTelnet(Address, Password);
                            break;
                        case "factoryReset":
                            FactoryReset(Address, Password);
                            break;
                        case "enableUpnp":
                            EnableUpnp(Address, Password);
                            break;
                        case "disableUpnp":
                            Execute(Address, Password, x => x.DisableUpnp());
                            break;
                        case "disableDhcp":
                            Execute(Address, Password, x => x.DisableDhcp());
                            break;
                        case "enableDhcp":
                            Execute(Address, Password, x => x.EnableDhcp());
                            break;
                        case "dumpInfo":
                            DumpInfo(Address, Password);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                });
            }
            // Create your application here
        }
    }
}

