using AppKit;

namespace MyCircle.Mac
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            SQLitePCL.Batteries_V2.Init();

            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
    }
}
