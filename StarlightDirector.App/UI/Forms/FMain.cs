﻿using System.Drawing;
using System.Windows.Forms;
using StarlightDirector.Beatmap;
using StarlightDirector.Core;
using StarlightDirector.UI.Controls;

namespace StarlightDirector.App.UI.Forms {
    public sealed partial class FMain : Form {

        public FMain() {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            InitializeComponent();
            RegisterEventHandlers();
            RegisterCommands();
        }

        ~FMain() {
            UnregisterCommands();
            UnregisterEventHandlers();
        }

        public string StatusText {
            get { return _statusText; }
            set {
                if (value == null) {
                    value = string.Empty;
                }
                var b = value != _statusText;
                if (b) {
                    _statusText = value;
                    Invalidate();
                }
            }
        }

        public void UpdateUIIndications() {
            UpdateUIIndications(_editingFileName, _cachedTitleDifficulty);
        }

        public void UpdateUIIndications(Difficulty currentDifficulty) {
            UpdateUIIndications(_editingFileName, currentDifficulty);
        }

        public void UpdateUIIndications(string editingFileName) {
            UpdateUIIndications(editingFileName, _cachedTitleDifficulty);
        }

        public void UpdateUIIndications(string editingFileName, Difficulty currentDifficulty) {
            if (editingFileName == null) {
                editingFileName = string.Empty;
            }
            var applicationTitle = ApplicationHelper.GetTitle();
            var difficultyDescription = DescribedEnumConverter.GetEnumDescription(currentDifficulty);
            var text = string.IsNullOrEmpty(editingFileName) ? applicationTitle : $"{editingFileName} [{difficultyDescription}] - {applicationTitle}";
            var project = visualizer.Editor.Project;
            if (project.IsModified) {
                text = "* " + text;
            }
            Text = text;
            tsbScoreDifficultySelection.Text = difficultyDescription;
            _editingFileName = editingFileName;
            _cachedTitleDifficulty = currentDifficulty;
        }

        public void ApplyColorScheme(ColorScheme scheme) {
            BackColor = scheme.Workspace;

            sysMinimize.BackColor = scheme.SysMinNormalBackground;
            sysMinimize.IconColor = scheme.SysMinNormalIcon;
            sysMinimize.HoveringBackColor = scheme.SysMinHoveringBackground;
            sysMinimize.HoveringIconColor = scheme.SysMinHoveringIcon;
            sysMinimize.PressedBackColor = scheme.SysMinPressedBackground;
            sysMinimize.PressedIconColor = scheme.SysMinPressedIcon;

            sysMaximizeRestore.BackColor = scheme.SysMaxNormalBackground;
            sysMaximizeRestore.IconColor = scheme.SysMaxNormalIcon;
            sysMaximizeRestore.HoveringBackColor = scheme.SysMaxHoveringBackground;
            sysMaximizeRestore.HoveringIconColor = scheme.SysMaxHoveringIcon;
            sysMaximizeRestore.PressedBackColor = scheme.SysMaxPressedBackground;
            sysMaximizeRestore.PressedIconColor = scheme.SysMaxPressedIcon;

            sysClose.BackColor = scheme.SysCloseNormalBackground;
            sysClose.IconColor = scheme.SysCloseNormalIcon;
            sysClose.HoveringBackColor = scheme.SysCloseHoveringBackground;
            sysClose.HoveringIconColor = scheme.SysCloseHoveringIcon;
            sysClose.PressedBackColor = scheme.SysClosePressedBackground;
            sysClose.PressedIconColor = scheme.SysClosePressedIcon;

            mainMenuStrip.BackColor = scheme.MenuBarBackground;
            mainMenuStrip.ForeColor = scheme.ControlText;
            lblCaption.BackColor = scheme.CaptionBackground;
            picIcon.BackColor = scheme.CaptionBackground;
            panel2.BackColor = scheme.ToolbarBackground;
            visualizer.BackColor = scheme.Workspace;

            var toolStripRenderer = new StarlightToolStripRenderer(scheme);
            mainMenuStrip.Renderer = toolStripRenderer;
            ctxMain.Renderer = toolStripRenderer;
            tlbNote.Renderer = toolStripRenderer;
            tlbStandard.Renderer = toolStripRenderer;
            tlbPostprocessing.Renderer = toolStripRenderer;
            tlbMeasure.Renderer = toolStripRenderer;
            tlbEditAndView.Renderer = toolStripRenderer;
        }

        private static readonly Size FrameBorderSize = SystemInformation.FrameBorderSize;
        private static readonly int CaptionMargin = 65;
        private static readonly int ToolbarMargin = 250;
        private static readonly string DefaultDocumentName = "Untitled";

        private string _editingFileName = string.Empty;
        private Difficulty _cachedTitleDifficulty = Difficulty.Debut;

        private static readonly string TapAudioFilePath = "Resources/SFX/Director/se_live_tap_perfect.wav";
        private static readonly string FlickAudioFilePath = "Resources/SFX/Director/se_live_flic_perfect.wav";
        private static readonly string SlideAudioFilePath = "Resources/SFX/Director/se_live_slide_node.wav";

        private readonly object _sfxSyncObject = new object();
        private double _sfxBufferTime;

        private string _statusText = string.Empty;

    }
}
