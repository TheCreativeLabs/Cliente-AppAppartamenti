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
    public class ScrollToCollectionView : Behavior<CollectionView>
    {
        public CollectionView AssociatedObject
        {
            get;
            private set;
        }

        protected override void OnAttachedTo(CollectionView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
            var items = AssociatedObject.ItemsSource as IList;
            AssociatedObject.ScrollTo(items[items.Count-1]);
        }

        protected override void OnDetachingFrom(CollectionView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        }
    }

}