using Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Gui.Views
{
    public class MediaPlayer
    {


        public static bool GetEnablePlayMedia(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnablePlayMediaProperty);
        }

        public static void SetEnablePlayMedia(DependencyObject obj, bool value)
        {
            obj.SetValue(EnablePlayMediaProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnablePlayMedia.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnablePlayMediaProperty =
            DependencyProperty.RegisterAttached("EnablePlayMedia", typeof(bool), typeof(MediaPlayer), new PropertyMetadata(false, OnEnablePlayMedia));

        private static void OnEnablePlayMedia(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is UserControl win)
            {
                win.Loaded += (s, e) =>
                {
                    if (win.DataContext is IMediaPlayer vm)
                    {
                        vm.Play += () =>
                        {
                            GetPlay(win);
                        };
                    }
                };
            }
        }

        public static bool GetPlay(DependencyObject obj)
        {
            return (bool)obj.GetValue(PlayProperty);
        }

        public static void SetPlay(DependencyObject obj, bool value)
        {
            obj.SetValue(PlayProperty, value);
        }

        // Using a DependencyProperty as the backing store for Play.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayProperty =
            DependencyProperty.RegisterAttached("Play", typeof(bool), typeof(MediaPlayer), new PropertyMetadata(false, OnPlayMedia));

        private static void OnPlayMedia(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is MediaElement mediaElement)
            {
                mediaElement.Play();
            }
        }
    }
}
