﻿using System;
using System.Windows.Forms;
using StarlightDirector.Beatmap;
using StarlightDirector.Beatmap.Extensions;

namespace StarlightDirector.App.UI.Forms {
    public partial class FBeatmapStats : Form {

        public static void ShowDialog(IWin32Window parent, Project project) {
            using (var f = new FBeatmapStats()) {
                f._project = project;
                f.ShowDialog(parent);
            }
        }

        ~FBeatmapStats() {
            UnregisterEventHandlers();
        }

        private FBeatmapStats() {
            InitializeComponent();
            RegisterEventHandlers();
        }

        private void UnregisterEventHandlers() {
            Load -= FBeatmapInfo_Load;
        }

        private void RegisterEventHandlers() {
            Load += FBeatmapInfo_Load;
        }

        private void FBeatmapInfo_Load(object sender, EventArgs e) {
            var listView = lv;
            var project = _project;

            listView.View = View.Details;
            listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView.ShowItemToolTips = false;
            listView.ShowGroups = true;
            listView.FullRowSelect = true;
            listView.MultiSelect = false;

            listView.Items.Clear();
            listView.Groups.Clear();
            listView.Columns.Clear();

            listView.Columns.Add("Name");
            listView.Columns.Add("Value");

            var groupGeneral = listView.Groups.Add("General", "General");
            var it = listView.Items.Add("Project file name");
            it.SubItems.Add(project.SaveFileName ?? string.Empty);
            it.Group = groupGeneral;
            it = listView.Items.Add("Music file name");
            it.SubItems.Add(project.MusicFileName ?? string.Empty);
            it.Group = groupGeneral;

            var difficulties = new[] { Difficulty.Debut, Difficulty.Regular, Difficulty.Pro, Difficulty.Master, Difficulty.MasterPlus };
            foreach (var difficulty in difficulties) {
                var score = project.GetScore(difficulty);
                var numberOfNotes = score.GetAllNotes().Count;
                var numberOfBars = score.Bars.Count;
                var duration = score.CalculateDuration();
                var difficultyDescription = DescribedEnumConverter.GetEnumDescription(difficulty);
                var text = $"Difficulty: {difficultyDescription}";
                var group = listView.Groups.Add(text, text);
                var it1 = listView.Items.Add("Measures");
                it1.SubItems.Add(numberOfBars.ToString());
                var it2 = listView.Items.Add("Notes");
                it2.SubItems.Add(numberOfNotes.ToString());
                var it3 = listView.Items.Add("Duration");
                it3.SubItems.Add(duration.ToString("g"));
                it1.Group = group;
                it2.Group = group;
                it3.Group = group;
            }

            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private Project _project;

    }
}
