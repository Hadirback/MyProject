using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Slotlogic.MobileApp.Droid.Renderers;
using Slotlogic.MobileApp.Models.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPickerView), typeof(CustomPickerViewRenderer))]
namespace Slotlogic.MobileApp.Droid.Renderers
{
    class CustomPickerViewRenderer : PickerRenderer
    {
        CustomPickerView element;
        public CustomPickerViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            element = (CustomPickerView)this.Element;

            if (Control != null && Element != null)
            {

                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.Transparent);
                Control.SetBackground(gd);


                if (!string.IsNullOrEmpty(element.Icon))
                {
                    Control.Background = AddPickerStyles(element.Icon);
                    Control.SetHintTextColor(Android.Graphics.Color.White);  
                }
            }
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Transparent;
            border.SetPadding(10, 10, 10, 10);
            border.Paint.SetStyle(Paint.Style.Stroke);
            Drawable[] layers = {
                            border,
                            GetDrawable(imagePath)
                        };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            return layerDrawable;
        }


        private BitmapDrawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));
            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }
    }
}