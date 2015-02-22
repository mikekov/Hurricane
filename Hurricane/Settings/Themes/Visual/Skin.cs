using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurricane.Settings.Themes.Visual
{
    public class Skin : ISkin
    {
        public Skin(string name, string translated, Type skin)
        {
            Name = name;
            TranslatedName = translated;
            skin_ = skin;
        }

        public string Name
        {
            get;
            private set;
        }

        public string TranslatedName
        {
            get;
            private set;
        }

        public AppMainWindow.WindowSkins.IWindowSkin CreateSkin()
        {
            return (AppMainWindow.WindowSkins.IWindowSkin)Activator.CreateInstance(skin_);
        }

        Type skin_;
    }
}
