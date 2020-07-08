using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Slotlogic.MobileApp.Droid.Renderers;
using Slotlogic.MobileApp.Models.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntryView), typeof(CustomEntryViewRenderer))]
namespace Slotlogic.MobileApp.Droid.Renderers
{
    class CustomEntryViewRenderer : EntryRenderer
    {
        public CustomEntryViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                Control.SetRawInputType(InputTypes.ClassText | InputTypes.TextFlagCapCharacters);
            }
        }
    }
}