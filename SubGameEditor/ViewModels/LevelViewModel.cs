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
        public List<LevelData> AccessLevels { get; set; }

        public LevelData SelectedLevel
        {
            get { return selectedLevel; }
            set { selectedLevel = value; SomethingIsChanged("SelectedLevel"); }
        }
        private LevelData selectedLevel;

        public event PropertyChangedEventHandler PropertyChanged;

        public LevelViewModel()
        {
            AccessLevels = JsonConvert.DeserializeObject<List<LevelData>>(File.ReadAllText(myPath));
        }

        ~LevelViewModel()
        {
            File.WriteAllText(myPath, JsonConvert.SerializeObject(AccessLevels));
        }

        public void SomethingIsChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
