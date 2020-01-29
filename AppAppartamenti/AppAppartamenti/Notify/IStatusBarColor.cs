using System;
namespace AppAppartamenti.Notify
{
    public interface IStatusBarStyleManager
    {
        void SetColoredStatusBar(string hexColor);
        void SetWhiteStatusBar();
    }
}
