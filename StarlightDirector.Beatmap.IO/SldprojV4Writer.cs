﻿using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using StarlightDirector.Core;

namespace StarlightDirector.Beatmap.IO {
    public sealed partial class SldprojV4Writer : ProjectWriter {

        public override void WriteProject(Project project, string fileName) {
            var fileInfo = new FileInfo(fileName);
            var csb = new SQLiteConnectionStringBuilder {
                DataSource = fileName
            };
            project.Version = ProjectVersion.Current;
            SQLiteConnection.CreateFile(fileInfo.FullName);
            using (var db = new SQLiteConnection(csb.ToString())) {
                db.Open();
                WriteProject(project, db);
                db.Close();
            }
            project.SaveFileName = fileInfo.FullName;
            project.IsModified = false;
        }

        private static void WriteProject(Project project, SQLiteConnection db) {
            SQLiteCommand setValue = null, insertNote = null, insertNoteID = null, insertBarParams = null, insertSpecialNote = null;
            using (var transaction = db.BeginTransaction()) {
                // Table structure
                SQLiteHelper.CreateKeyValueTable(transaction, Names.Table_Main);
                SQLiteHelper.CreateScoresTable(transaction);
                SQLiteHelper.CreateKeyValueTable(transaction, Names.Table_ScoreSettings);
                SQLiteHelper.CreateKeyValueTable(transaction, Names.Table_Metadata);
                SQLiteHelper.CreateBarParamsTable(transaction);
                SQLiteHelper.CreateSpecialNotesTable(transaction);

                // Main
                SQLiteHelper.InsertValue(transaction, Names.Table_Main, Names.Field_MusicFileName, project.MusicFileName ?? string.Empty, ref setValue);
                SQLiteHelper.InsertValue(transaction, Names.Table_Main, Names.Field_Version, project.Version.ToString(), ref setValue);

                // Notes
                SQLiteHelper.InsertNoteID(transaction, StarlightID.Invalid, ref insertNoteID);
                foreach (var difficulty in Difficulties) {
                    var score = project.GetScore(difficulty);
                    foreach (var note in score.GetAllNotes()) {
                        if (note.Helper.IsGaming) {
                            SQLiteHelper.InsertNoteID(transaction, note.Basic.ID, ref insertNoteID);
                        }
                    }
                }
                foreach (var difficulty in Difficulties) {
                    var score = project.GetScore(difficulty);
                    foreach (var note in score.GetAllNotes()) {
                        if (note.Helper.IsGaming) {
                            SQLiteHelper.InsertNote(transaction, note, ref insertNote);
                        }
                    }
                }

                // Score settings
                var settings = project.Settings;
                SQLiteHelper.InsertValue(transaction, Names.Table_ScoreSettings, Names.Field_GlobalBpm, settings.BeatPerMinute.ToString(CultureInfo.InvariantCulture), ref setValue);
                SQLiteHelper.InsertValue(transaction, Names.Table_ScoreSettings, Names.Field_StartTimeOffset, settings.StartTimeOffset.ToString(CultureInfo.InvariantCulture), ref setValue);
                SQLiteHelper.InsertValue(transaction, Names.Table_ScoreSettings, Names.Field_GlobalGridPerSignature, settings.GridPerSignature.ToString(), ref setValue);
                SQLiteHelper.InsertValue(transaction, Names.Table_ScoreSettings, Names.Field_GlobalSignature, settings.Signature.ToString(), ref setValue);

                // Bar params && Special notes
                foreach (var difficulty in Difficulties) {
                    var score = project.GetScore(difficulty);
                    foreach (var bar in score.Bars) {
                        if (bar.Params != null) {
                            SQLiteHelper.InsertBarParams(transaction, bar, ref insertBarParams);
                        }
                    }
                    foreach (var note in score.GetAllNotes().Where(note => !note.Helper.IsGaming)) {
                        SQLiteHelper.InsertNoteID(transaction, note.Basic.ID, ref insertNoteID);
                        SQLiteHelper.InsertSpecialNote(transaction, note, ref insertSpecialNote);
                    }
                }

                // Metadata (none for now)

                // Commit!
                transaction.Commit();
            }

            // Cleanup
            insertNoteID?.Dispose();
            insertNote?.Dispose();
            setValue?.Dispose();
        }

        private static readonly Difficulty[] Difficulties = { Difficulty.Debut, Difficulty.Regular, Difficulty.Pro, Difficulty.Master, Difficulty.MasterPlus };

    }

}
