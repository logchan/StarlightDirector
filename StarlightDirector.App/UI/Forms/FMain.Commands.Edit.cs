﻿using System.Linq;
using StarlightDirector.Commanding;
using StarlightDirector.UI.Controls.Editing;

namespace StarlightDirector.App.UI.Forms {
    partial class FMain {

        private void CmdEditUndo_Executed(object sender, ExecutedEventArgs e) {
            // TODO
        }

        private void CmdEditUndo_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            // TODO
            e.CanExecute = false;
        }

        private void CmdEditRedo_Executed(object sender, ExecutedEventArgs e) {
            // TODO
        }

        private void CmdEditRedo_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            // TODO
            e.CanExecute = false;
        }

        private void CmdEditCut_Executed(object sender, ExecutedEventArgs e) {
            // TODO
        }

        private void CmdEditCut_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            // TODO
            e.CanExecute = false;
        }

        private void CmdEditCopy_Executed(object sender, ExecutedEventArgs e) {
            // TODO
        }

        private void CmdEditCopy_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            // TODO
            e.CanExecute = false;
        }

        private void CmdEditPaste_Executed(object sender, ExecutedEventArgs e) {
            // TODO
        }

        private void CmdEditPaste_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            // TODO
            e.CanExecute = false;
        }

        private void CmdEditModeSet_Executed(object sender, ExecutedEventArgs e) {
            var modeMenuItems = new[] { mnuEditModeSelect, mnuEditModeTap, mnuEditModeHoldFlick, mnuEditModeSlide };
            var mode = (ScoreEditMode)e.Parameter;
            var pressedItem = modeMenuItems.First(m => (ScoreEditMode)m.GetParameter() == mode);
            foreach (var item in modeMenuItems) {
                item.Checked = item == pressedItem;
            }
            visualizer.Editor.EditMode = mode;
            tsbEditMode.Image = pressedItem.Image;
        }

        private void CmdEditModeSelect_Executed(object sender, ExecutedEventArgs e) {
            CmdEditModeSet.Execute(sender, e.Parameter);
        }

        private void CmdEditModeTap_Executed(object sender, ExecutedEventArgs e) {
            CmdEditModeSet.Execute(sender, e.Parameter);
        }

        private void CmdEditModeHoldFlick_Executed(object sender, ExecutedEventArgs e) {
            CmdEditModeSet.Execute(sender, e.Parameter);
        }

        private void CmdEditModeSlide_Executed(object sender, ExecutedEventArgs e) {
            CmdEditModeSet.Execute(sender, e.Parameter);
        }

        private void CmdEditSelectAllMeasures_Executed(object sender, ExecutedEventArgs e) {
            visualizer.Editor.SelectAllBars();
            visualizer.Editor.Invalidate();
        }

        private void CmdEditSelectAllNotes_Executed(object sender, ExecutedEventArgs e) {
            visualizer.Editor.SelectAllNotes();
            visualizer.Editor.Invalidate();
        }

        private void CmdEditSelectClearAll_Executed(object sender, ExecutedEventArgs e) {
            visualizer.Editor.ClearSelectedBars();
            visualizer.Editor.ClearSelectedNotes();
            visualizer.Editor.Invalidate();
        }

        private void CmdEditGoToMeasure_Executed(object sender, ExecutedEventArgs e) {
            var (r, t) = FGoTo.RequestInput(this, 0, visualizer.Editor);
        }

        private void CmdEditGoToMeasure_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            e.CanExecute = false;
        }

        private void CmdEditGoToTime_Executed(object sender, ExecutedEventArgs e) {
            var (r, t) = FGoTo.RequestInput(this, 1, visualizer.Editor);
        }

        private void CmdEditGoToTime_QueryCanExecute(object sender, QueryCanExecuteEventArgs e) {
            e.CanExecute = false;
        }

        private readonly Command CmdEditUndo = CommandManager.CreateCommand("Ctrl+Z");
        private readonly Command CmdEditRedo = CommandManager.CreateCommand("Ctrl+Y");
        private readonly Command CmdEditCut = CommandManager.CreateCommand("Ctrl+X");
        private readonly Command CmdEditCopy = CommandManager.CreateCommand("Ctrl+C");
        private readonly Command CmdEditPaste = CommandManager.CreateCommand("Ctrl+V");
        private readonly Command CmdEditModeSet = CommandManager.CreateCommand();
        private readonly Command CmdEditModeSelect = CommandManager.CreateCommand();
        private readonly Command CmdEditModeTap = CommandManager.CreateCommand();
        private readonly Command CmdEditModeHoldFlick = CommandManager.CreateCommand();
        private readonly Command CmdEditModeSlide = CommandManager.CreateCommand();
        private readonly Command CmdEditSelectAllMeasures = CommandManager.CreateCommand("Ctrl+Shift+A");
        private readonly Command CmdEditSelectAllNotes = CommandManager.CreateCommand("Ctrl+A");
        private readonly Command CmdEditSelectClearAll = CommandManager.CreateCommand();
        private readonly Command CmdEditGoToMeasure = CommandManager.CreateCommand("Ctrl+G");
        private readonly Command CmdEditGoToTime = CommandManager.CreateCommand("Ctrl+Shift+G");

    }
}
