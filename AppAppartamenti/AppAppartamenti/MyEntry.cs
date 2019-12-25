using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;

namespace AppAppartamenti
{
 
    public class DatePickerCtrl : DatePicker
    {
        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(DatePickerCtrl), defaultValue: default(string));
        public string Placeholder
        {
            get;
            set;
        }
    }

    public class MyEntry : Entry
    {
    }


    public class CustomEntry : AdvancedEntry
    {

        public CustomEntry()
        {
            this.BackgroundColor = Color.White;
            this.PlaceholderColor = Color.FromRgb(152,152,152);
            this.BorderColor = Color.FromRgb(220, 220, 220);
            this.TextColor = Color.Black;
            this.CornerRadius = 5;
        }   
    }

    public class SearchEntry : Entry
    {
    }

    public class ShadowFrame : Frame
    {
    }

    public class WebViewUserAgent : WebView
    {
    }

    public class ExtendedEditorControl : Editor
    {
        public static BindableProperty PlaceholderProperty
          = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ExtendedEditorControl));

        public static BindableProperty PlaceholderColorProperty
           = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ExtendedEditorControl), Color.LightGray);

        public static BindableProperty HasRoundedCornerProperty
        = BindableProperty.Create(nameof(HasRoundedCorner), typeof(bool), typeof(ExtendedEditorControl), false);

        public static BindableProperty IsExpandableProperty
        = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(ExtendedEditorControl), false);

        public bool IsExpandable
        {
            get { return (bool)GetValue(IsExpandableProperty); }
            set { SetValue(IsExpandableProperty, value); }
        }
        public bool HasRoundedCorner
        {
            get { return (bool)GetValue(HasRoundedCornerProperty); }
            set { SetValue(HasRoundedCornerProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public ExtendedEditorControl()
        {
            TextChanged += OnTextChanged;
        }

        ~ExtendedEditorControl()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable)
                InvalidateMeasure();


        }

    }

    public class ExtendedListView : ListView
    {
        public ExtendedListView() : this(ListViewCachingStrategy.RecycleElement)
        {
        }

        public ExtendedListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            this.ItemSelected += OnItemSelected;
            this.ItemTapped += OnItemTapped;
            this.ItemAppearing += OnItemAppearing;
            this.ItemDisappearing += OnItemDisappering;
        }

        public static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ExtendedListView), default(ICommand));

        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }

        public static readonly BindableProperty ItemAppearingCommandProperty =
            BindableProperty.Create(nameof(ItemAppearingCommand), typeof(ICommand), typeof(ExtendedListView), default(ICommand));

        public ICommand ItemAppearingCommand
        {
            get { return (ICommand)GetValue(ItemAppearingCommandProperty); }
            set { SetValue(ItemAppearingCommandProperty, value); }
        }

        public static readonly BindableProperty ItemDisappearingCommandProperty =
         BindableProperty.Create(nameof(ItemDisappearingCommand), typeof(ICommand), typeof(ExtendedListView), default(ICommand));


        public ICommand ItemDisappearingCommand
        {
            get { return (ICommand)GetValue(ItemDisappearingCommandProperty); }
            set { SetValue(ItemDisappearingCommandProperty, value); }
        }


        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = (ExtendedListView)sender;
            if (e == null) return;
            listView.SelectedItem = null;

        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (TappedCommand != null)
            {
                TappedCommand?.Execute(e.Item);
            }
            SelectedItem = null;
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemAppearingCommand != null)
            {
                ItemAppearingCommand?.Execute(e.Item);
            }
        }


        private void OnItemDisappering(object sender, ItemVisibilityEventArgs e)
        {
            ItemDisappearingCommand?.Execute(e.Item);
        }


        public void ScrollToFirst()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (ItemsSource != null && ItemsSource.Cast<object>().Count() > 0)
                    {
                        var msg = ItemsSource.Cast<object>().FirstOrDefault();
                        if (msg != null)
                        {
                            ScrollTo(msg, ScrollToPosition.Start, false);
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            });
        }

        public void ScrollToLast()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (ItemsSource != null && ItemsSource.Cast<object>().Count() > 0)
                    {
                        var msg = ItemsSource.Cast<object>().LastOrDefault();
                        if (msg != null)
                        {
                            ScrollTo(msg, ScrollToPosition.End, false);
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            });
        }
    }
}
