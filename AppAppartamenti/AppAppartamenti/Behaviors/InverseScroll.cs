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
    public class InverseScroll : Behavior<CollectionView>
    {
     public bool loadComplete;

        public static readonly BindableProperty ScrollCommandProperty = BindableProperty.Create("ScrollItemCommand", typeof(ICommand), typeof(InverseScroll), null);
        public ICommand ScrollItemCommand
        {
            get
            {
                return (ICommand)GetValue(ScrollCommandProperty);
            }
            set
            {
                SetValue(ScrollCommandProperty, value);
            }
        }

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
            //bindable.ItemAppearing += InfiniteListView_ItemAppearing;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        protected override void OnDetachingFrom(CollectionView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        }
    }

}