using UnityEngine;

namespace BeatThePiano
{
    public class Nota
    {
        private int _name;
        public int Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        
        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public Nota(int name, int duration, string note)
        {
            _name = name;
            _duration = duration;
            _note = note;
        }
    }
}

