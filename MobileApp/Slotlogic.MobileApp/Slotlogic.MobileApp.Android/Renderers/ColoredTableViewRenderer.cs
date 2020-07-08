using System;
using Android.Widget;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Slotlogic.MobileApp.Droid.Renderers;
using Android.Views;
using Slotlogic.MobileApp.Models.View;

[assembly: ExportRenderer(typeof(ColoredTableView), typeof(ColoredTableViewRenderer))]
namespace Slotlogic.MobileApp.Droid.Renderers
{
    public class ColoredTableViewRenderer : TableViewRenderer
    {
        public ColoredTableViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override TableViewModelRenderer GetModelRenderer(Android.Widget.ListView listView, TableView view)
        {
            return new CustomHeaderTableViewModelRenderer(Context, listView, view);
        }

        private class CustomHeaderTableViewModelRenderer : TableViewModelRenderer
        {
            private readonly ColoredTableView _coloredTableView;

            public CustomHeaderTableViewModelRenderer(Context context, Android.Widget.ListView listView, TableView view) : base(context, listView, view)
            {
                _coloredTableView = view as ColoredTableView;
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
            {
                var view = base.GetView(position, convertView, parent);

                var element = GetCellForPosition(position);

                // section header will be a TextCell
                if (element.GetType() == typeof(TextCell))
                {
                    try
                    {
                        // Get the textView of the actual layout
                        var textView = ((((view as LinearLayout).GetChildAt(0) as LinearLayout).GetChildAt(1) as LinearLayout).GetChildAt(0) as TextView);

                        // get the divider below the header
                        var divider = (view as LinearLayout).GetChildAt(1);

                        // Set the color
                        textView.SetTextColor(_coloredTableView.GroupHeaderColor.ToAndroid());
                        textView.SetPadding(20,5,0,0);
                        textView.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
                        textView.TextAlignment = Android.Views.TextAlignment.TextStart;
                        textView.Gravity = GravityFlags.Start;
                        divider.SetBackgroundColor(_coloredTableView.GroupHeaderColor.ToAndroid());
                    }
                    catch (Exception) { }
                }

                return view;
            }
        }
    }
}
