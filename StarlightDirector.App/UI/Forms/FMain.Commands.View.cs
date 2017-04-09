﻿using StarlightDirector.Commanding;

namespace StarlightDirector.App.UI.Forms {
    partial class FMain {

        private void CmdViewZoomIn_Executed(object sender, ExecutedEventArgs e) {
            visualizer.Editor.ZoomIn();
        }

        private void CmdViewZoomOut_Executed(object sender, ExecutedEventArgs e) {
            visualizer.Editor.ZoomOut();
        }

        private readonly Command CmdViewZoomIn = CommandManager.CreateCommand();
        private readonly Command CmdViewZoomOut = CommandManager.CreateCommand();

    }
}
