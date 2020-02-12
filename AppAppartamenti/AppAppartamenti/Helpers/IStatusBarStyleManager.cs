using System;
namespace AppAppartamenti.Helpers
{
    public interface IStatusBarStyleManager
    {
        void SetColoredStatusBar(string hexColor);
        void SetWhiteStatusBar();
    }
}
