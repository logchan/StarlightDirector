﻿using System;
using System.Data;
using System.Data.SQLite;
using StarlightDirector.Core;

namespace StarlightDirector.Beatmap.IO {
    partial class SldprojV4Writer {

        private static class SQLiteHelper {

            public static void SetValue(SQLiteTransaction transaction, string tableName, string key, string value, bool creatingNewDatabase, ref SQLiteCommand command) {
                SetValue(transaction.Connection, tableName, key, value, creatingNewDatabase, ref command);
            }

            public static void SetValue(SQLiteConnection connection, string tableName, string key, string value, bool creatingNewDatabase, ref SQLiteCommand command) {
                if (creatingNewDatabase) {
                    InsertValue(connection, tableName, key, value, ref command);
                } else {
                    UpdateValue(connection, tableName, key, value, ref command);
                }
            }

            public static void InsertValue(SQLiteTransaction transaction, string tableName, string key, string value, ref SQLiteCommand command) {
                InsertValue(transaction.Connection, tableName, key, value, ref command);
            }

            public static void InsertValue(SQLiteConnection connection, string tableName, string key, string value, ref SQLiteCommand command) {
                if (command == null) {
                    command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO {tableName} (key, value) VALUES (@key, @value);";
                    command.Parameters.Add("key", DbType.AnsiString).Value = key;
                    command.Parameters.Add("value", DbType.AnsiString).Value = value;
                } else {
                    command.CommandText = $"INSERT INTO {tableName} (key, value) VALUES (@key, @value);";
                    command.Parameters["key"].Value = key;
                    command.Parameters["value"].Value = value;
                }
                command.ExecuteNonQuery();
            }

            public static void UpdateValue(SQLiteTransaction transaction, string tableName, string key, string value, ref SQLiteCommand command) {
                UpdateValue(transaction.Connection, tableName, key, value, ref command);
            }

            public static void UpdateValue(SQLiteConnection connection, string tableName, string key, string value, ref SQLiteCommand command) {
                if (command == null) {
                    command = connection.CreateCommand();
                    command.CommandText = $"UPDATE {tableName} SET value = @value WHERE key = @key;";
                    command.Parameters.Add("key", DbType.AnsiString).Value = key;
                    command.Parameters.Add("value", DbType.AnsiString).Value = value;
                } else {
                    command.CommandText = $"UPDATE {tableName} SET value = @value WHERE key = @key;";
                    command.Parameters["key"].Value = key;
                    command.Parameters["value"].Value = value;
                }
                command.ExecuteNonQuery();
            }

            public static void CreateKeyValueTable(SQLiteTransaction transaction, string tableName) {
                CreateKeyValueTable(transaction.Connection, tableName);
            }

            public static void CreateKeyValueTable(SQLiteConnection connection, string tableName) {
                using (var command = connection.CreateCommand()) {
                    // Have to use LONGTEXT (2^31-1) rather than TEXT (32768).
                    command.CommandText = $"CREATE TABLE {tableName} (key LONGTEXT PRIMARY KEY NOT NULL, value LONGTEXT NOT NULL);";
                    command.ExecuteNonQuery();
                }
            }

            public static void CreateBarParamsTable(SQLiteTransaction transaction) {
                CreateBarParamsTable(transaction.Connection);
            }

            public static void CreateBarParamsTable(SQLiteConnection connection) {
                using (var command = connection.CreateCommand()) {
                    command.CommandText = $@"CREATE TABLE {Names.Table_BarParams} (
{Names.Column_Difficulty} INTEGER NOT NULL, {Names.Column_BarIndex} INTEGER NOT NULL, {Names.Column_GridPerSignature} INTEGER, {Names.Column_Signature} INTEGER,
PRIMARY KEY ({Names.Column_Difficulty}, {Names.Column_BarIndex}));";
                    command.ExecuteNonQuery();
                }
            }

            public static void CreateSpecialNotesTable(SQLiteTransaction transaction) {
                CreateSpecialNotesTable(transaction.Connection);
            }

            public static void CreateSpecialNotesTable(SQLiteConnection connection) {
                using (var command = connection.CreateCommand()) {
                    command.CommandText = $@"CREATE TABLE {Names.Table_SpecialNotes} (
{Names.Column_ID} BLOB NOT NULL PRIMARY KEY, {Names.Column_Difficulty} INTEGER NOT NULL, {Names.Column_BarIndex} INTEGER NOT NULL, {Names.Column_IndexInGrid} INTEGER NOT NULL,
{Names.Column_NoteType} INTEGER NOT NULL, {Names.Column_ParamValues} TEXT NOT NULL,
FOREIGN KEY ({Names.Column_ID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID}));";
                    command.ExecuteNonQuery();
                }
            }

            public static void CreateScoresTable(SQLiteTransaction transaction) {
                CreateScoresTable(transaction.Connection);
            }

            public static void CreateScoresTable(SQLiteConnection connection) {
                using (var command = connection.CreateCommand()) {
                    command.CommandText = $"CREATE TABLE {Names.Table_NoteIDs} ({Names.Column_ID} BLOB NOT NULL PRIMARY KEY);";
                    command.ExecuteNonQuery();
                    command.CommandText = $@"CREATE TABLE {Names.Table_Notes} (
{Names.Column_ID} BLOB PRIMARY KEY NOT NULL, {Names.Column_Difficulty} INTEGER NOT NULL, {Names.Column_BarIndex} INTEGER NOT NULL, {Names.Column_IndexInGrid} INTEGER NOT NULL,
{Names.Column_NoteType} INTEGER NOT NULL, {Names.Column_StartPosition} INTEGER NOT NULL, {Names.Column_FinishPosition} INTEGER NOT NULL, {Names.Column_FlickType} INTEGER NOT NULL,
{Names.Column_PrevFlickNoteID} BLOB NOT NULL, {Names.Column_NextFlickNoteID} BLOB NOT NULL, {Names.Column_PrevSlideNoteID} BLOB NOT NULL, {Names.Column_NextSlideNoteID} BLOB NOT NULL, {Names.Column_HoldTargetID} BLOB NOT NULL,
FOREIGN KEY ({Names.Column_ID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID}), FOREIGN KEY ({Names.Column_PrevFlickNoteID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID}),
FOREIGN KEY ({Names.Column_NextFlickNoteID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID}), FOREIGN KEY ({Names.Column_PrevSlideNoteID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID}), FOREIGN KEY ({Names.Column_NextSlideNoteID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID})
FOREIGN KEY ({Names.Column_HoldTargetID}) REFERENCES {Names.Table_NoteIDs}({Names.Column_ID}));";
                    command.ExecuteNonQuery();
                }
            }

            public static void InsertNoteID(SQLiteTransaction transaction, Guid id, ref SQLiteCommand command) {
                InsertNoteID(transaction.Connection, id, ref command);
            }

            public static void InsertNoteID(SQLiteConnection connection, Guid id, ref SQLiteCommand command) {
                if (command == null) {
                    command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO {Names.Table_NoteIDs} ({Names.Column_ID}) VALUES (@id);";
                    command.Parameters.Add("id", DbType.Guid);
                }
                command.Parameters["id"].Value = id;
                command.ExecuteNonQuery();
            }

            public static void InsertNote(SQLiteTransaction transaction, Note note, ref SQLiteCommand command) {
                InsertNote(transaction.Connection, note, ref command);
            }

            public static void InsertNote(SQLiteConnection connection, Note note, ref SQLiteCommand command) {
                if (command == null) {
                    command = connection.CreateCommand();
                    command.CommandText = $@"INSERT INTO {Names.Table_Notes} (
{Names.Column_ID}, {Names.Column_Difficulty}, {Names.Column_BarIndex}, {Names.Column_IndexInGrid}, {Names.Column_NoteType}, {Names.Column_StartPosition}, {Names.Column_FinishPosition},
{Names.Column_FlickType}, {Names.Column_PrevFlickNoteID}, {Names.Column_NextFlickNoteID}, {Names.Column_PrevSlideNoteID}, {Names.Column_NextSlideNoteID}, {Names.Column_HoldTargetID}
) VALUES (@id, @difficulty, @bar, @grid, @note_type, @start, @finish, @flick, @prev_flick, @next_flick, @prev_slide, @next_slide, @hold);";
                    command.Parameters.Add("id", DbType.Guid);
                    command.Parameters.Add("difficulty", DbType.Int32);
                    command.Parameters.Add("bar", DbType.Int32);
                    command.Parameters.Add("grid", DbType.Int32);
                    command.Parameters.Add("note_type", DbType.Int32);
                    command.Parameters.Add("start", DbType.Int32);
                    command.Parameters.Add("finish", DbType.Int32);
                    command.Parameters.Add("flick", DbType.Int32);
                    command.Parameters.Add("prev_flick", DbType.Guid);
                    command.Parameters.Add("next_flick", DbType.Guid);
                    command.Parameters.Add("prev_slide", DbType.Guid);
                    command.Parameters.Add("next_slide", DbType.Guid);
                    command.Parameters.Add("hold", DbType.Guid);
                }
                command.Parameters["id"].Value = note.Basic.ID;
                command.Parameters["difficulty"].Value = note.Basic.Bar.Basic.Score.Difficulty;
                command.Parameters["bar"].Value = note.Basic.Bar.Basic.Index;
                command.Parameters["grid"].Value = note.Basic.IndexInGrid;
                command.Parameters["note_type"].Value = (int)note.Basic.Type;
                command.Parameters["start"].Value = (int)note.Basic.StartPosition;
                command.Parameters["finish"].Value = (int)note.Basic.FinishPosition;
                command.Parameters["flick"].Value = (int)note.Basic.FlickType;
                command.Parameters["prev_flick"].Value = note.Editor.PrevFlick?.Basic.ID ?? StarlightID.Invalid;
                command.Parameters["next_flick"].Value = note.Editor.NextFlick?.Basic.ID ?? StarlightID.Invalid;
                command.Parameters["prev_slide"].Value = note.Editor.PrevSlide?.Basic.ID ?? StarlightID.Invalid;
                command.Parameters["next_slide"].Value = note.Editor.NextSlide?.Basic.ID ?? StarlightID.Invalid;
                command.Parameters["hold"].Value = note.Editor.HoldPair?.Basic.ID ?? StarlightID.Invalid;
                command.ExecuteNonQuery();
            }

            public static void InsertBarParams(SQLiteTransaction transaction, Bar bar, ref SQLiteCommand command) {
                InsertBarParams(transaction.Connection, bar, ref command);
            }

            public static void InsertBarParams(SQLiteConnection connection, Bar bar, ref SQLiteCommand command) {
                if (bar.Params == null) {
                    return;
                }
                if (command == null) {
                    command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO {Names.Table_BarParams} ({Names.Column_Difficulty}, {Names.Column_BarIndex}, {Names.Column_GridPerSignature}, {Names.Column_Signature}) VALUES (@difficulty, @index, @grid, @signature);";
                    command.Parameters.Add("difficulty", DbType.Int32);
                    command.Parameters.Add("index", DbType.Int32);
                    command.Parameters.Add("grid", DbType.Int32);
                    command.Parameters.Add("signature", DbType.Int32);
                }
                command.Parameters["difficulty"].Value = (int)bar.Basic.Score.Difficulty;
                command.Parameters["index"].Value = bar.Basic.Index;
                command.Parameters["grid"].Value = bar.Params.UserDefinedGridPerSignature;
                command.Parameters["signature"].Value = bar.Params.UserDefinedSignature;
                command.ExecuteNonQuery();
            }

            public static void InsertSpecialNote(SQLiteTransaction transaction, Note note, ref SQLiteCommand command) {
                InsertSpecialNote(transaction.Connection, note, ref command);
            }

            public static void InsertSpecialNote(SQLiteConnection connection, Note note, ref SQLiteCommand command) {
                if (command == null) {
                    command = connection.CreateCommand();
                    command.CommandText = $"INSERT INTO {Names.Table_SpecialNotes} ({Names.Column_ID}, {Names.Column_Difficulty}, {Names.Column_BarIndex}, {Names.Column_IndexInGrid}, {Names.Column_NoteType}, {Names.Column_ParamValues}) VALUES (@id, @diff, @bar, @grid, @type, @pv);";
                    command.Parameters.Add("id", DbType.Guid);
                    command.Parameters.Add("diff", DbType.Int32);
                    command.Parameters.Add("bar", DbType.Int32);
                    command.Parameters.Add("grid", DbType.Int32);
                    command.Parameters.Add("type", DbType.Int32);
                    // Parameter values
                    command.Parameters.Add("pv", DbType.AnsiString);
                }
                command.Parameters["id"].Value = note.Basic.ID;
                command.Parameters["diff"].Value = (int)note.Basic.Bar.Basic.Score.Difficulty;
                command.Parameters["bar"].Value = note.Basic.Bar.Basic.Index;
                command.Parameters["grid"].Value = note.Basic.IndexInGrid;
                command.Parameters["type"].Value = (int)note.Basic.Type;
                command.Parameters["pv"].Value = note.Params.GetDataString();
                command.ExecuteNonQuery();
            }

        }

    }
}
