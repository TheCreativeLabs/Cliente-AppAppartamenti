using System;
using System.Collections.Generic;
using CoreGraphics;
using AppAppartamenti;
using AppAppartamenti.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace AppAppartamenti.iOS
{

    public class CustomMKAnnotationView : MKAnnotationView
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id)
        {
        }
    }

    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        List<CustomPin> customPins;
        MKMapView map;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = null;
                nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                map = nativeMap;
                customPins = formsMap.CustomPins;
                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
                nativeMap.DidAddAnnotationViews += OnDidAddAnnotationViews;
            }
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation || annotation == null)
                return null;

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Name);
            //if (annotationView == null || customPin.IsSelected)
            //{
                annotationView = new CustomMKAnnotationView(annotation, customPin.Name);

                if (customPin.IsSelected)
                {
                    annotationView.Image = UIImage.FromFile("pinnero.png");
                }
                else
                {
                    annotationView.Image = UIImage.FromFile("pinblu.png");
                }

                annotationView.CalloutOffset = new CGPoint(0, 0);
                annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("pin.png"));
                annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                ((CustomMKAnnotationView)annotationView).Name = customPin.Name;
                ((CustomMKAnnotationView)annotationView).Url = customPin.Url;
            //}
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            if (!string.IsNullOrWhiteSpace(customView.Url))
            {
                UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            }
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            customPinView = new UIView();

            if (customView.Name.Equals("Xamarin"))
            {
                customPinView.Frame = new CGRect(0, 0, 200, 84);
                var image = new UIImageView(new CGRect(0, 0, 200, 84));
                image.Image = UIImage.FromFile("pin.png");
                customPinView.AddSubview(image);
                customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
                e.View.AddSubview(customPinView);
            }
        }

        void OnDidAddAnnotationViews(object sender, MKMapViewAnnotationEventArgs e)
        {
            map.GetViewForAnnotation = null;
             map.GetViewForAnnotation = GetViewForAnnotation;
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                if(customPinView != null)
                {
                    customPinView.RemoveFromSuperview();
                    customPinView.Dispose();
                    customPinView = null;
                }
            }
        }

        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            if(annotation != null)
            {
                var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
                foreach (var pin in customPins)
                {
                    if (pin.Position == position)
                    {
                        return pin;
                    }
                }
            }
            return null;
        }
    }
}