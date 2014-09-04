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
    [Activity (Label = "Warning")]			
    public class WarningActivity : TrackedActivity
    {
        string Password;
        string Address;
        string MoreHelp;
        string WarningText;
        string Action;
        void LoadExtras()
        {
            Password = Intent.GetStringExtra("password");
            Address = Intent.GetStringExtra("address");
            MoreHelp = Intent.GetStringExtra("moreHelp");
            WarningText = Intent.GetStringExtra("warning");
            Action = Intent.GetStringExtra("action");
        }
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
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.WarningConfirmation);
            LoadExtras();
            FindViewById<TextView>(Resource.Id.warningText).Text = WarningText;
            FindViewById<Button>(Resource.Id.moreInfo).Click += (s, e) => 
            {
                var uri = Android.Net.Uri.Parse(MoreHelp);
                var intent = new Intent (Intent.ActionView, uri); 
                StartActivity (intent);  
            };
            FindViewById<Button>(Resource.Id.continueAnyway).Click += (sender, e) =>
            {
                var second = new Intent(this, typeof(ExecutionActivity));
                second.PutExtra("address", Address);
                second.PutExtra("password", Password);
                second.PutExtra("action", Action);
                StartActivity(second);
            };

            // Create your application here
        }
    }
}

