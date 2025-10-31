using UnityEngine;
using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace BeatThePiano
{
    public class Nota
    {
        private float _spawnTime;
        public float SpawnTime
        {
            get { return _spawnTime; }
            set { _spawnTime = value; }
        }
        
        private float _duration;
        public float Duration
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

        public Nota(float spawnTime, float duration, string note)
        {
            _spawnTime = spawnTime;
            _duration = duration;
            _note = note;
        }
        
        public override string ToString()
        {
            return $"Nota: {Note}, Spawn: {SpawnTime}, Duration: {Duration}";
        }
    }

    public static class MidiConverter
    {
        //Implementata con aiuto di ChatGPT
        public static List<Nota> ConvertMidiToNota(string midiPath)
        {
            var midiFile = MidiFile.Read(midiPath);
            var tempoMap = midiFile.GetTempoMap();
            var notas = new List<Nota>();

            foreach (var note in midiFile.GetNotes())
            {
                var start = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
                var end = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time + note.Length, tempoMap);

                float startSeconds = (float)start.TotalMicroseconds / 1_000_000f;
                float endSeconds = (float)end.TotalMicroseconds / 1_000_000f;
                float duration = endSeconds - startSeconds;

                notas.Add(new Nota(startSeconds, duration, note.NoteName.ToString()));
            }

            return notas;
        }
    }
}

