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

namespace NVG510Fixer
{

    [Activity (Label = "Choose an action for the NVG510")]			
    public class ControllerActivity : TrackedActivity
    {
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
        string Address="";
        string Password="";
        public void Execute(string action)
        {
            var second = new Intent(this, typeof(ExecutionActivity));
            second.PutExtra("address", Address);
            second.PutExtra("password", Password);
            second.PutExtra("action", action);
            StartActivity(second);
        }
        public void WarnFirst(string action, string warning, string learnmore)
        {
            var second = new Intent(this, typeof(WarningActivity));
            second.PutExtra("address", Address);
            second.PutExtra("password", Password);
            second.PutExtra("action", action);
            second.PutExtra("warning", warning);
            second.PutExtra("moreHelp", learnmore);
            StartActivity(second);
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Controller);
            Address = Intent.GetStringExtra("address");
            Password = Intent.GetStringExtra("password");
            new AlertDialog.Builder(this).
                SetTitle("Warning").
                SetMessage("Pushing any button here might reboot your modem, causing temporary internet loss!").
                Show();
            FindViewById<Button>(Resource.Id.enableTelnet).Click +=  (sender, e) => 
            {
                Execute("enableTelnet");
            };
            FindViewById<Button>(Resource.Id.disableTelnet).Click +=  (sender, e) =>
            {      
                Execute("disableTelnet");
            };
            FindViewById<Button>(Resource.Id.fixRedirect).Click +=  (sender, e) =>
            {    
                Execute("fixRedirect");
            };
            FindViewById<Button>(Resource.Id.enableUpnp).Click +=  (sender, e) =>
            {        
                Execute("enableUpnp");
            };
            FindViewById<Button>(Resource.Id.disableUpnp).Click +=  (sender, e) =>
            {        
                Execute("disableUpnp");
            };
            FindViewById<Button>(Resource.Id.reboot).Click +=  (sender, e) =>
            {        
                Execute("reboot");
            };      
            FindViewById<Button>(Resource.Id.factoryReset).Click += (sender, e) =>
            {
                string warning=@"This will reset your modem to FACTORY DEFAULTS!
All manual configuration changes you have made to the modem will be erased! ";
                WarnFirst("factoryReset", warning, "http://earlz.net/view/2013/08/03/2006/nvg510-fixer-an-android-application#factoryreset");
            };
            FindViewById<Button>(Resource.Id.enableBridge).Click += (sender, e) => 
            {
                string warning=@"If you use U-Verse Phones/VOIP on your NVG510 DO NOT USE THIS! YOU WILL LOSE PHONE SERVICE
This should only be used if you have an existing router that you want to 'bridge' the modem to (on Port 1).
If you do not have a router behind it, you will have to disable bridge mode and/or FACTORY RESET!";
                WarnFirst("enableBridge", warning, "http://earlz.net/view/2013/08/03/2006/nvg510-fixer-an-android-application#bridgemode");
            };
            FindViewById<Button>(Resource.Id.disableBridge).Click += (sender, e) => 
            {
                Execute("disableBridge");
            };
            FindViewById<Button>(Resource.Id.disableDhcp).Click += (sender, e) => 
            {
                Execute("disableDhcp");
            };
            FindViewById<Button>(Resource.Id.enableDhcp).Click += (sender, e) => 
            {
                Execute("enableDhcp");
            };
            
            FindViewById<Button>(Resource.Id.dumpconfig).Click += (sender, e) =>
            {
                Execute("dumpInfo");
            };
            


            // Create your application here
        }
    }
}

