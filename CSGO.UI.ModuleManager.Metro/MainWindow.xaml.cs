using System;
using System.IO;
using System.Linq;
using AgaHackTools.Example;
using AgaHackTools.Example.Shared;
using AgaHackTools.Main.Interfaces;
using Json;
using MahApps.Metro.Controls;
using Newtonsoft.Json;

namespace CSGO.UI.ModuleManager.Metro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigurationPath = AppDomain.CurrentDomain.BaseDirectory;
            textBox1.Text = AppDomain.CurrentDomain.BaseDirectory;
            textBox.Text = JsonConvert.SerializeObject(CSGOData.Config);
            SelectedCfg = "Settings.cfg";
            if (!ConfigurationPath.EndsWith("\\"))
                ConfigurationPath += "\\";
            if (!File.Exists(ConfigurationPath + SelectedCfg))
            {
                statustextBlock.Text = "Not found " + ConfigurationPath + SelectedCfg;
                return;
            }
            var text = File.ReadAllText(ConfigurationPath + SelectedCfg);
        }

        private void startAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
            var settingsjson =textBox.Text;
            var settings = JsonConvert.DeserializeObject<Options>(settingsjson);
            CSGOData.Config = settings;
                statustextBlock.Text = "Configuration is in Game!";
            }
            catch (Exception)
            {
                statustextBlock.Text = "Failed to load current config!";
            }
        }

        private void stopAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
            var settingsjson = textBox.Text;
            var settings = JsonConvert.DeserializeObject<Options>(settingsjson);
            ConfigurationManagerStatic.RewriteConfiguration(settings, ConfigurationPath + SelectedCfg);
            statustextBlock.Text = "Saved " + ConfigurationPath+ SelectedCfg;

            }
            catch (Exception)
            {
                statustextBlock.Text = "Failed to save " + ConfigurationPath + SelectedCfg;
            }
        }

        private void refreshAvailableModules_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!Directory.Exists(ConfigurationPath))
            {
                statustextBlock.Text = "Not found " + ConfigurationPath;
                return;
            }
            statustextBlock.Text = "Found " + ConfigurationPath;
            listBoxAvailable.Items.Clear();
            foreach (var cfg in Directory.EnumerateFiles(ConfigurationPath, "*.cfg").Except(new[] { "Modules.cfg" }))
            {
                var name = Path.GetFileName(cfg);
                listBoxAvailable.Items.Add(name);
            }
        }

        private void startSingle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void stopSingle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //selectedmodule = (IModule<object>)boxControlModules.SelectionBoxItem;
            //nameShow.Content = selectedmodule.Name;
            //statusShow.Content = selectedmodule.IsRunning;
            //fpsShow.Content = selectedmodule.Ticks;
            //try
            //{
            //    var settings = JsonConvert.SerializeObject(selectedmodule.Settings);
            //    settingsBox.Text = settings;
            //}
            //catch (Exception)
            //{
                
            //}
        }

        private void buttonChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //var settingsstr = settingsBox.Text;
            //object settings;
            //try
            //{
            //    settings = JsonConvert.DeserializeObject(settingsstr);
            //    selectedmodule.Settings = settings;
            //}
            //catch (Exception)
            //{
                
            //}
        }

        private void buttonLoad_Click(object sender, System.Windows.RoutedEventArgs e)
        {
//            object settings;
//            try
//            {
//                settings = JsonConvert.DeserializeObject(settingsstr);
//                selectedmodule.Settings = settings;
//            }
//            catch (Exception)
//            {
//
//            }
        }

        private void buttonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //csgoProcess.ConfigurationManager.
        }

        private void listBoxAvailable_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void loadModule_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selected = (string)listBoxAvailable.SelectedItem;
            if (selected == null)
            {
                statustextBlock.Text = "Not selected item " + ConfigurationPath + selected;
                return;
            }
            if (!ConfigurationPath.EndsWith("\\"))
                ConfigurationPath += "\\";
            if (!File.Exists(ConfigurationPath + selected))
            {
                statustextBlock.Text = "Not found " + ConfigurationPath + selected;
                return;
            }
            SelectedCfg = selected;
            var text =File.ReadAllText(ConfigurationPath + selected);
            textBox.Text = text;
            statustextBlock.Text = "Loaded " + ConfigurationPath + selected;
        }

        private void textBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var path = textBox1.Text;
            ConfigurationPath = path;
            if (!Directory.Exists(path))
            {
                return;
            }
            statustextBlock.Text = "Found " + ConfigurationPath;
            listBoxAvailable.Items.Clear();
            foreach (var cfg in Directory.EnumerateFiles(ConfigurationPath, "*.cfg").Except(new[] {"Modules.cfg"} ))
            {
                var name = Path.GetFileName(cfg);
                listBoxAvailable.Items.Add(name);
            }
        }

        public static string ConfigurationPath=@"";
        public static string SelectedCfg=@"";

        private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
