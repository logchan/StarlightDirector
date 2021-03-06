﻿using System;
using System.Collections.Generic;
using System.IO;

namespace StarlightDirector.Beatmap {
    public sealed class Project {

        public Project() {
            var scores = _scores = new Dictionary<Difficulty, Score>();
            for (var i = Difficulty.Debut; i <= Difficulty.MasterPlus; ++i) {
                scores.Add(i, new Score(this, i));
            }
            Settings = ProjectSettings.CreateDefault();
        }

        public static Project CreateWithVersion(int version) {
            var project = new Project();
            project.Version = version;
            return project;
        }

        public Score GetScore(Difficulty difficulty) {
            return GetScore(difficulty, false);
        }

        public Score GetScore(Difficulty difficulty, bool throwIfNotFound) {
            if (throwIfNotFound) {
                return _scores[difficulty];
            } else {
                return _scores.ContainsKey(difficulty) ? _scores[difficulty] : null;
            }
        }

        public ProjectSettings Settings { get; }

        public string MusicFileName { get; set; } = string.Empty;

        public bool HasMusic => !string.IsNullOrEmpty(MusicFileName);

        public string SaveFileName { get; set; } = string.Empty;

        public int Version { get; internal set; }

        public HashSet<Guid> UsedNoteIDs { get; } = new HashSet<Guid>();

        public bool IsModified { get; set; }

        public bool WasSaved => !string.IsNullOrEmpty(SaveFileName) && File.Exists(SaveFileName);

        internal void SetScore(Difficulty difficulty, Score score) {
            if (difficulty == Difficulty.Invalid) {
                throw new ArgumentOutOfRangeException(nameof(difficulty), "Difficulty is invalid.");
            }
            _scores[difficulty] = score;
        }

        private readonly Dictionary<Difficulty, Score> _scores;

    }
}
