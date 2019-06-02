using ConfigModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SubGameEditor.ViewModels
{
    public class LevelViewModel : INotifyPropertyChanged
    {
        private readonly string fileName = @"Levels.json";
        private readonly string savePath = AppDomain.CurrentDomain.BaseDirectory;

        //Beware of changing this name since it's a binding property in LevelView.xaml
        public List<LevelData> Levels { get; set; }

        //Beware of changing this name since it's a binding property in both LevelView.xaml and DataView.xaml
        public LevelData SelectedLevel
        {
            get => selectedLevel;
            set { selectedLevel = value; SomethingIsChanged("SelectedLevel"); }
        }
        private LevelData selectedLevel;

        public event PropertyChangedEventHandler PropertyChanged;

        public LevelViewModel()
        {
            Levels = JsonConvert.DeserializeObject<List<LevelData>>(File.ReadAllText(Path.Combine(savePath, fileName)));
        }

        ~LevelViewModel()
        {
            File.WriteAllText(Path.Combine(savePath, fileName), JsonConvert.SerializeObject(Levels));
        }

        public void SomethingIsChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
