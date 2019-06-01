using ConfigModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SubGameEditor.ViewModels
{
    public class LevelViewModel : INotifyPropertyChanged
    {
        private readonly string myPath = @"Levels.json";

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

        public LevelViewModel() => Levels = JsonConvert.DeserializeObject<List<LevelData>>(File.ReadAllText(myPath));

        ~LevelViewModel()
        {
            File.WriteAllText(myPath, JsonConvert.SerializeObject(Levels));
        }

        public void SomethingIsChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
