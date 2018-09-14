using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace MyCircle.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow _window;
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 600, 600);
            _window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            _window.MinSize = new CoreGraphics.CGSize(400,400);
            _window.Title = "My Circle";
            _window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow
        {
            get { return _window; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            LoadApplication(new MyCircle.App());
            base.DidFinishLaunching(notification);
        }
    }
}
