using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Earlz.NVG510Controller;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Views.Animations;
using System.Threading;

namespace NVG510Fixer
{
    [Activity (Label = "NVG510 Fixer", MainLauncher = true)]
    public class MainActivity : TrackedActivity
    {
        string ModemPassword
        {
            get
            {
                return FindViewById<EditText>(Resource.Id.modemPassword).Text;
            }
        }
        string ModemAddress
        {
            get
            {
                return FindViewById<EditText>(Resource.Id.modemAddress).Text;
            }
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
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            FindViewById<EditText>(Resource.Id.modemPassword).RequestFocus();

            FindViewById<Button>(Resource.Id.next).Click += (s,e) => {
                if(ModemPassword=="")
                {
                    var diag=new AlertDialog.Builder(this);
                    diag.SetTitle("Device Access Code Required!");
                    diag.SetMessage("A password or Device Access Code is required! It is usually a 10-digit number printed on your modem");
                    diag.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                    diag.SetNeutralButton("Ok", new EventHandler<DialogClickEventArgs>((s2, e2) => {} ));
                    diag.Show();
                    return;
                }
                var controller = new Intent(this, typeof(ControllerActivity));
                controller.PutExtra("address", ModemAddress);
                controller.PutExtra("password", ModemPassword);
                StartActivity(controller);
            };
            // Get our button from the layout resource,
            // and attach an event to it
            //must get address and password in this context because it can change later, but other threads can't access it


        }
    }
}


