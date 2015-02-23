using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Hurricane.AppMainWindow.WindowSkins;
using Hurricane.Designer.Data;
using Hurricane.Settings.Themes.AudioVisualisation;
using Hurricane.Settings.Themes.AudioVisualisation.DefaultAudioVisualisation;
using Hurricane.Settings.Themes.Visual;
using Hurricane.Settings.Themes.Visual.BaseThemes;
using Hurricane.Settings.Themes.Visual.ColorThemes;
using MahApps.Metro;
using Application = System.Windows.Application;

namespace Hurricane.Settings.Themes
{
    public class ApplicationThemeManager
    {
        #region "Singleton & Constructor"

        private static ApplicationThemeManager _instance;
        public static ApplicationThemeManager Instance
        {
            get { return _instance ?? (_instance = new ApplicationThemeManager()); }
        }


        private ApplicationThemeManager()
        {
            _loadedResources = new Dictionary<string, ResourceDictionary>();
            Refresh();
        }

        #endregion

        private ObservableCollection<ColorThemeBase> _colorThemes;
        public ObservableCollection<ColorThemeBase> ColorThemes
        {
            get
            {
                return _colorThemes;
            }
        }

        private ObservableCollection<BaseThemeBase> _baseThemes;
        public ObservableCollection<BaseThemeBase> BaseThemes
        {
            get
            {
                return _baseThemes;
            }
        }

        private ObservableCollection<ThemePack> _themePacks;
        public ObservableCollection<ThemePack> ThemePacks
        {
            get
            {
                return _themePacks;
            }
        }

        private ObservableCollection<IAudioVisualisationContainer> _audioVisualisations;
        public ObservableCollection<IAudioVisualisationContainer> AudioVisualisations
        {
            get
            {
                return _audioVisualisations;
            }
        }

        private ObservableCollection<ISkin> _skins;
        public ObservableCollection<ISkin> Skins
        {
            get
            {
                return _skins;
            }
        }

        public static ISkin DefaultSkin
        {
            get { return Instance.Skins.First(s => s.Name == "Default skin"); }
        }

        public void Refresh()
        {
            _colorThemes = new ObservableCollection<ColorThemeBase>();
            _baseThemes = new ObservableCollection<BaseThemeBase>();
            _themePacks = new ObservableCollection<ThemePack>();
            _audioVisualisations = new ObservableCollection<IAudioVisualisationContainer>();

            foreach (var t in ThemeManager.Accents.Select(a => new AccentColorTheme { Name = a.Name }).OrderBy(x => x.TranslatedName))
            {
                _colorThemes.Add(t);
            }

            foreach (var t in ThemeManager.AppThemes.Select(a => new BaseTheme { Name = a.Name }).OrderBy(x => x.TranslatedName))
            {
                _baseThemes.Add(t);
            }

            _audioVisualisations.Add(DefaultAudioVisualisation.GetDefault());

            var colorThemesFolder = new DirectoryInfo(HurricaneSettings.Instance.ColorThemesDirectory);
            if (colorThemesFolder.Exists)
            {
                foreach (var file in colorThemesFolder.GetFiles("*.xaml"))
                {
                    CustomColorTheme theme;

                    if (CustomColorTheme.FromFile(file.FullName, out theme))
                        _colorThemes.Add(theme);
                }
            }

            var baseThemesFolder = new DirectoryInfo(HurricaneSettings.Instance.BaseThemesDirectory);
            if (baseThemesFolder.Exists)
            {
                foreach (var file in baseThemesFolder.GetFiles("*.xaml"))
                {
                    CustomBaseTheme theme;
                    if (CustomBaseTheme.FromFile(file.FullName, out theme))
                        _baseThemes.Add(theme);
                }
            }

            var themePacksFolder = new DirectoryInfo(HurricaneSettings.Instance.ThemePacksDirectory);
            if (themePacksFolder.Exists)
            {
                foreach (var file in themePacksFolder.GetFiles("*.htp"))
                {
                    ThemePack pack;
                    if (ThemePack.FromFile(file.FullName, out pack))
                    {
                        _themePacks.Add(pack);
                    }
                }
            }

            var audioVisualisations = new DirectoryInfo(HurricaneSettings.Instance.AudioVisualisationsDirectory);
            if (audioVisualisations.Exists)
            {
                foreach (var file in audioVisualisations.GetFiles("*.dll"))
                {
                    _audioVisualisations.Add(new CustomAudioVisualisation { FileName = file.Name });
                }
            }

            var skin_types = typeof(ApplicationThemeManager).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IWindowSkin)));
            var skins = new List<Visual.ISkin>();
            foreach (var skin in skin_types)
            {
                var attribs = skin.GetCustomAttributes(typeof(Hurricane.AppMainWindow.WindowSkins.WindowSkinAttribute), false);
                if (attribs.Length > 0)
                {
                    var attribute = (Hurricane.AppMainWindow.WindowSkins.WindowSkinAttribute)(attribs.First());
#if DEBUG
#endif
                    skins.Add(new Hurricane.Settings.Themes.Visual.Skin(attribute.Name, attribute.Name, skin));
                }
            }

            skins.Sort((s1, s2) => s1.TranslatedName.CompareTo(s2.TranslatedName));
            _skins = new ObservableCollection<Visual.ISkin>(skins);
        }

        public ThemePack GetThemePack(string name)
        {
            return null;
        }

        public void Apply(ApplicationDesign design)
        {
            try
            {
                design.ColorTheme.ApplyTheme();
            }
            catch (Exception)
            {
                design.ColorTheme = ColorThemes.First(x => x.Name == "Blue");
                design.ColorTheme.ApplyTheme();
            }

            try
            {
                design.BaseTheme.ApplyTheme();
            }
            catch (Exception)
            {
                design.BaseTheme = BaseThemes.First();
                design.BaseTheme.ApplyTheme();
            }

            if (design.AudioVisualisation != null)
                design.AudioVisualisation.AudioVisualisationPlugin.Refresh();
        }

        private readonly Dictionary<string, ResourceDictionary> _loadedResources;
        public void LoadResource(string key, ResourceDictionary resource)
        {
            Application.Current.Resources.MergedDictionaries.Add(resource);

            if (_loadedResources.ContainsKey(key))
            {
                Application.Current.Resources.MergedDictionaries.Remove(_loadedResources[key]);
                _loadedResources.Remove(key);
            }

            _loadedResources.Add(key, resource);
        }
    }
}