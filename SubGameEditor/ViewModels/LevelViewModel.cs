using Newtonsoft.Json;
using SubGameEditor.Models;
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
        public List<LevelData> Levels { get; set; }
        public LevelData SelectedLevel
        {
            get { return selectedLevel; }
            set { selectedLevel = value; SomethingIsChanged("SelectedLevel"); }
        }
        private LevelData selectedLevel;

        public event PropertyChangedEventHandler PropertyChanged;

        public LevelViewModel()
        {
            string path = @"C:\Repos\GitHub\ToasterGod\SubGame\SubGame\bin\Windows\x86\Debug\Levels.json";
            Levels = JsonConvert.DeserializeObject<List<LevelData>>(File.ReadAllText(path));
        }
        public void SomethingIsChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
