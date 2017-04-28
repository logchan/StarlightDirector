﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using StarlightDirector.Commanding;
using StarlightDirector.Core.Interop;
using StarlightDirector.UI.Controls;

namespace StarlightDirector.App.UI.Forms {
    partial class FMain {

        private void UnregisterEventHandlers() {
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
            Load -= FMain_Load;
            sysMaximizeRestore.Click -= SysMaximizeRestore_Click;
            sysMinimize.Click -= SysMinimize_Click;
            picIcon.MouseDown -= PicIcon_MouseDown;
            tsbScoreDifficultySelection.Click -= TsbScoreDifficultySelection_Click;
            visualizer.ContextMenuRequested -= Visualizer_ContextMenuRequested;
            tsbEditMode.Click -= TsbEditMode_Click;
            tsbScoreNoteStartPosition.Click -= TsbScoreNoteStartPosition_Click;
        }

        private void RegisterEventHandlers() {
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            Load += FMain_Load;
            sysMaximizeRestore.Click += SysMaximizeRestore_Click;
            sysMinimize.Click += SysMinimize_Click;
            picIcon.MouseDown += PicIcon_MouseDown;
            tsbScoreDifficultySelection.Click += TsbScoreDifficultySelection_Click;
            visualizer.ContextMenuRequested += Visualizer_ContextMenuRequested;
            tsbEditMode.Click += TsbEditMode_Click;
            tsbScoreNoteStartPosition.Click += TsbScoreNoteStartPosition_Click;
        }

        private void TsbScoreNoteStartPosition_Click(object sender, EventArgs e) {
            var items = new ToolStripItem[mnuScoreNoteStartPosition.DropDownItems.Count];
            mnuScoreNoteStartPosition.DropDownItems.CopyTo(items, 0);
            tsbScoreNoteStartPosition.DropDownItems.AddRange(items);
            tsbScoreNoteStartPosition.DropDownClosed += DropDownClosed;
            tsbScoreNoteStartPosition.ShowDropDown();

            void DropDownClosed(object s, EventArgs ev)
            {
                mnuScoreNoteStartPosition.DropDownItems.AddRange(items);
                tsbScoreNoteStartPosition.DropDownClosed -= DropDownClosed;
            }
        }

        private void TsbEditMode_Click(object sender, EventArgs e) {
            var items = new ToolStripItem[mnuEditMode.DropDownItems.Count];
            mnuEditMode.DropDownItems.CopyTo(items, 0);
            tsbEditMode.DropDownItems.AddRange(items);
            tsbEditMode.DropDownClosed += DropDownClosed;
            tsbEditMode.ShowDropDown();

            void DropDownClosed(object s, EventArgs ev)
            {
                mnuEditMode.DropDownItems.AddRange(items);
                tsbEditMode.DropDownClosed -= DropDownClosed;
            }
        }

        private void Visualizer_ContextMenuRequested(object sender, ContextMenuRequestedEventArgs e) {
            var hasSelectedNotes = visualizer.Editor.HasSelectedNotes;
            var hasSelectedBars = visualizer.Editor.HasSelectedBars;
            if ((e.MenuType == VisualizerContextMenu.Note || e.MenuType == VisualizerContextMenu.Bar) && (!hasSelectedNotes && !hasSelectedBars)) {
                return;
            }
            switch (e.MenuType) {
                case VisualizerContextMenu.Note:
                    ctxSep1.Visible = hasSelectedNotes;
                    ctxScoreNoteDelete.Visible = hasSelectedNotes;
                    ctxSep2.Visible = false;
                    ctxScoreMeasureDelete.Visible = false;
                    ctxSep3.Visible = false;
                    ctxScoreNoteInsertSpecial.Visible = false;
                    ctxSep4.Visible = false;
                    ctxScoreNoteModifySpecial.Visible = false;
                    ctxScoreNoteDeleteSpecial.Visible = false;
                    ctxScoreNoteInsertSpecial.DeleteParameter();
                    ctxScoreNoteModifySpecial.DeleteParameter();
                    ctxScoreNoteDeleteSpecial.DeleteParameter();
                    break;
                case VisualizerContextMenu.Bar:
                    ctxSep1.Visible = false;
                    ctxScoreNoteDelete.Visible = false;
                    ctxSep2.Visible = hasSelectedBars;
                    ctxScoreMeasureDelete.Visible = hasSelectedBars;
                    ctxSep3.Visible = false;
                    ctxScoreNoteInsertSpecial.Visible = false;
                    ctxSep4.Visible = false;
                    ctxScoreNoteModifySpecial.Visible = false;
                    ctxScoreNoteDeleteSpecial.Visible = false;
                    ctxScoreNoteInsertSpecial.DeleteParameter();
                    ctxScoreNoteModifySpecial.DeleteParameter();
                    ctxScoreNoteDeleteSpecial.DeleteParameter();
                    break;
                case VisualizerContextMenu.SpecialNoteAdd:
                    ctxSep1.Visible = false;
                    ctxScoreNoteDelete.Visible = false;
                    ctxSep2.Visible = false;
                    ctxScoreMeasureDelete.Visible = false;
                    ctxSep3.Visible = true;
                    ctxScoreNoteInsertSpecial.Visible = true;
                    ctxSep4.Visible = false;
                    ctxScoreNoteModifySpecial.Visible = false;
                    ctxScoreNoteDeleteSpecial.Visible = false;
                    ctxScoreNoteInsertSpecial.SetParameter(e.HitTestResult);
                    ctxScoreNoteModifySpecial.DeleteParameter();
                    ctxScoreNoteDeleteSpecial.DeleteParameter();
                    break;
                case VisualizerContextMenu.SpecialNoteModify:
                    ctxSep1.Visible = false;
                    ctxScoreNoteDelete.Visible = false;
                    ctxSep2.Visible = false;
                    ctxScoreMeasureDelete.Visible = false;
                    ctxSep3.Visible = false;
                    ctxScoreNoteInsertSpecial.Visible = false;
                    ctxSep4.Visible = true;
                    ctxScoreNoteModifySpecial.Visible = true;
                    ctxScoreNoteDeleteSpecial.Visible = true;
                    ctxScoreNoteInsertSpecial.DeleteParameter();
                    ctxScoreNoteModifySpecial.SetParameter(e.HitTestResult);
                    ctxScoreNoteDeleteSpecial.SetParameter(e.HitTestResult);
                    break;
                default:
                    break;
            }

            ctxMain.Show(visualizer, e.Location);
        }

        private void TsbScoreDifficultySelection_Click(object sender, EventArgs e) {
            var items = new ToolStripItem[mnuScoreDifficulty.DropDownItems.Count];
            mnuScoreDifficulty.DropDownItems.CopyTo(items, 0);
            tsbScoreDifficultySelection.DropDownItems.AddRange(items);
            tsbScoreDifficultySelection.DropDownClosed += DropDownClosed;
            tsbScoreDifficultySelection.ShowDropDown();

            void DropDownClosed(object s, EventArgs ev)
            {
                mnuScoreDifficulty.DropDownItems.AddRange(items);
                tsbScoreDifficultySelection.DropDownClosed -= DropDownClosed;
            }
        }

        private void PicIcon_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (e.Clicks == 1) {
                    var pt = picIcon.Location;
                    pt.Y = picIcon.Bottom;
                    pt = PointToScreen(pt);
                    var dispRect = NativeStructures.RECT.FromRectangle(ClientRectangle);
                    var hMenu = NativeMethods.GetSystemMenu(Handle, false);
                    NativeMethods.TrackPopupMenu(hMenu, NativeConstants.TPM_LEFTBUTTON, pt.X, pt.Y, 0, Handle, ref dispRect);
                } else {
                    Close();
                }
            } else if (e.Button == MouseButtons.Right) {
                var mouseScreen = MousePosition;
                var dispRect = NativeStructures.RECT.FromRectangle(ClientRectangle);
                var hMenu = NativeMethods.GetSystemMenu(Handle, false);
                NativeMethods.TrackPopupMenu(hMenu, NativeConstants.TPM_LEFTBUTTON, mouseScreen.X, mouseScreen.Y, 0, Handle, ref dispRect);
            }
        }

        private void SysMinimize_Click(object sender, EventArgs eventArgs) {
            WindowState = FormWindowState.Minimized;
        }

        private void SysMaximizeRestore_Click(object sender, EventArgs eventArgs) {
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private void FMain_Load(object sender, EventArgs eventArgs) {
            MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            using (var smallIcon = new Icon(Icon, new Size(16, 16))) {
                picIcon.InitialImage = picIcon.Image = smallIcon.ToBitmap();
                picIcon.ErrorImage = ToolStripRenderer.CreateDisabledImage(picIcon.InitialImage);
            }

            ApplyColorScheme(ColorScheme.Default);
            CursorFixup(this);

            mnuEditSelectClearAll.ShortcutKeyDisplayString = "Esc";
            mnuScoreNoteStartPositionAt0.ShortcutKeyDisplayString = "0";
            mnuScoreNoteStartPositionAt1.ShortcutKeyDisplayString = "1";
            mnuScoreNoteStartPositionAt2.ShortcutKeyDisplayString = "2";
            mnuScoreNoteStartPositionAt3.ShortcutKeyDisplayString = "3";
            mnuScoreNoteStartPositionAt4.ShortcutKeyDisplayString = "4";
            mnuScoreNoteStartPositionAt5.ShortcutKeyDisplayString = "5";

            CmdProjectNew.Execute(null, null);
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs eventArgs) {
            MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
        }

    }
}
