using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System.Windows.Input;
using System.Collections;

namespace AppAppartamenti.Behaviors
{
    public class InverseScroll : Behavior<ListView>
    {
        public bool loadComplete;

        public static readonly BindableProperty LoadItemsCommandProperty = BindableProperty.Create("LoadItemsCommand", typeof(ICommand), typeof(InverseScroll), null);

        public ICommand LoadItemsCommand
        {
            get
            {
                return (ICommand)GetValue(LoadItemsCommandProperty);
            }
            set
            {
                SetValue(LoadItemsCommandProperty, value);
            }
        }

        public ListView AssociatedObject
        {
            get;
            private set;
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            bindable.ItemAppearing += InfiniteListView_ItemAppearing;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
            loadComplete = false;
        }
        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.ItemAppearing -= InfiniteListView_ItemAppearing;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (!loadComplete) { 
            var items = AssociatedObject.ItemsSource as IList;
            AssociatedObject.ScrollTo(items[items.Count - 1], ScrollToPosition.End, false);

            if (e.ItemIndex == items.Count - 1) { 
                loadComplete = true;
            }
            }
        }
    }

}