using ConfigModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get { return selectedLevel; }
            set { selectedLevel = value; SomethingIsChanged("SelectedLevel"); }
        }
        private LevelData selectedLevel;

        public event PropertyChangedEventHandler PropertyChanged;

        public LevelViewModel()
        {
            Levels = JsonConvert.DeserializeObject<List<LevelData>>(File.ReadAllText(myPath));
        }

        ~LevelViewModel()
        {
            File.WriteAllText(myPath, JsonConvert.SerializeObject(Levels));
        }

        public void SomethingIsChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
