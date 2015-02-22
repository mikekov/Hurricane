using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurricane.Settings.Themes.Visual
{
    public interface ISkin
    {
        string Name { get; }
        string TranslatedName { get; }
        Hurricane.AppMainWindow.WindowSkins.IWindowSkin CreateSkin();
    }
}
