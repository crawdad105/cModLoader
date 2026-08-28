using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cModLoader
{
    public class cModLauncher
    {
        public static string Version = "v0.1";
        public static void Open() {
            Window.Window window = new Window.Window($"cModLauncher ({Version})", 820, 493);
            window.BackgroundColour = Microsoft.Xna.Framework.Color.Gray;
            window.Resizeable = true;
            window.Icon = Accessibility.MessageBoxIcon.cModLoader;

            string tipText = "";

            var btnW = 180;
            var btnH = 25;
            var btnY = 10;
            var btnX = 10;

            var btn1 = new Window.Button("Install", btnX, btnY, btnW, btnH);
            btn1.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btn1.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btn1.OnHover += () => { tipText = "Installs cModLoader at the default terraria directory."; };
            btn1.OnPress += () => { cModLoaderInstaller.Install(); };
            btnY += btnH + 10;

            var btn2 = new Window.Button("Uninstall", btnX, btnY, btnW, btnH);
            btn2.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btn2.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btn2.OnHover += () => { tipText = "Uninstalls cModLoader at the default terraria directory."; };
            btn2.OnPress += () => { cModLoaderInstaller.Uninstall(); };
            btnY += btnH + 10;

            var btn3 = new Window.Button("Verify Files", btnX, btnY, btnW, btnH);
            btn3.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btn3.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btn3.OnHover += () => { tipText = "Unimplemented"; };
            btnY += btnH + 10;

            btnY = 10;
            btnX += btnW + 10;

            var btn4 = new Window.Button("Virtual Install", btnX, btnY, btnW, btnH);
            btn4.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btn4.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btn4.OnHover += () => { tipText = "Installs cModLoader at the specified location which you select."; };
            btn4.OnPress += () => {
                string str = Accessibility.DirectorySearch("Select a Terraria directory.", out bool pass, Path.GetDefaultPath());
                if (pass) {
                    if (!File.Exists(str)) return;
                    cModLoaderInstaller.Install(str);
                }
            };
            btnY += btnH + 10;

            var btn5 = new Window.Button("Virtual Uninstall", btnX, btnY, btnW, btnH);
            btn5.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btn5.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btn5.OnHover += () => { tipText = "Uninstalls cModLoader at the specified location which you select."; };
            btn5.OnPress += () => {
                string str = Accessibility.DirectorySearch("Select a Terraria directory.", out bool pass, Path.GetDefaultPath());
                if (pass) {
                    if (!File.Exists(str)) return;
                    cModLoaderInstaller.Uninstall(installPath: str);
                }
            };
            btnY += btnH + 10;

            btnY = 10;
            btnX += btnW + 10;

            var btnStartTerraria1 = new Window.Button("Launch Terraria", btnX, btnY, btnW, btnH);
            btnStartTerraria1.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btnStartTerraria1.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btnStartTerraria1.OnHover += () => { tipText = "Launches Terraria normally from the exe."; };
            btnStartTerraria1.OnPress += () => {
                cModLoaderInstaller.LaunchNewTerraria();
            };
            btnY += btnH + 10;

            btnH = 35;

            var btnStartTerraria2 = new Window.Button("Launch Terraria", btnX, btnY, btnW, btnH);
            btnStartTerraria2.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btnStartTerraria2.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btnStartTerraria2.OnHover += () => { tipText = "Launches Terraria normally from steam, this can sometimes be better."; };
            btnStartTerraria2.OnPress += () => {
                Process.Start("explorer.exe", "steam://launch/105600");
            };
            var btnStartTerraria2_SubLabel = new Window.Label("(From Steam)", btnX + btnW / 2, btnY + btnH - 2) { AlignX = 0.5f, AlignY = 1f };
            btnY += btnH + 10;

            var btnStartTerraria3 = new Window.Button("Launch Terraria", btnX, btnY, btnW, btnH);
            btnStartTerraria3.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            btnStartTerraria3.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            btnStartTerraria3.OnHover += () => { tipText = "Launches Terraria at the specified location which you select."; };
            btnStartTerraria3.OnPress += () => {
                string path = Accessibility.DirectorySearch("Select a Terraria directory.", out bool pass, Path.GetDefaultPath());
                if (!pass) return;
                if (!File.Exists(path)) return;

                // TODO: check if the exe is actually terraria

                // Unfortunately a cModLoader file is required in the terraria directory
                var cMod = Path.GetCurrentPath();
                var subPath = path.Substring(0, path.LastIndexOf("\\"));
                var cModPath2 = subPath + "\\cModLoader.exe";
                if (cMod != cModPath2) {
                    while (true) {
                        try { File.Copy(cMod, cModPath2, true); break; }
                        catch (Exception) {
                            var num = Accessibility.ShowMessageBox("Could not move cModLoader to terraria folder.", "File Error", new string[] { "Cancel", "Retry", "Continue" }, Accessibility.MessageBoxIcon.Error);
                            if (num == 0) return;
                            else if (num == 1) continue;
                            else break;
                        }
                    }
                }
                var psi = new ProcessStartInfo {
                    FileName = cModPath2,
                    WorkingDirectory = subPath,
                    Arguments = "virtual \"" + path + "\"",
                    UseShellExecute = true
                };
                Process.Start(psi);
            };
            var btnStartTerraria3_SubLabel = new Window.Label("(Virtually)", btnX + btnW / 2, btnY + btnH - 2) { AlignX = 0.5f, AlignY = 1f };
            btnY += btnH + 10;

            btnH = 25;

            btnX += btnW + 10;
            btnY = 10;

            var DebugButton1 = new Window.Button("Test Window Blocking", btnX, btnY, btnW, btnH) { Enabled = false };
            DebugButton1.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            DebugButton1.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            DebugButton1.OnPress += () => {
                Accessibility.ShowMessageBox(new string[] { "Test Window Blocking" }, "Test", new string[] { "OK" }, Accessibility.MessageBoxIcon.Information, true);
            };
            btnY += btnH + 10;
            
            var DebugButton2 = new Window.Button("Test Window non-blocking", btnX, btnY, btnW, btnH) { Enabled = false };
            DebugButton2.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            DebugButton2.ForgroundColour = Microsoft.Xna.Framework.Color.Black;
            DebugButton2.OnPress += () => {
                Accessibility.ShowMessageBox(new string[] { "Test Window Blocking" }, "Test", new string[] { "OK" }, Accessibility.MessageBoxIcon.Information, false);
            };
            btnY += btnH + 10;

            var check1 = new Window.CheckBox("Debug Text", window.StartWidth + 5, 5, 12, 12);
            check1.BackgroundColour = Microsoft.Xna.Framework.Color.LightGray;
            check1.ForgroundColour = Microsoft.Xna.Framework.Color.Black;

            window.AddChildren(
                btn1, btn2, btn3, btn4, btn5, btnStartTerraria1, btnStartTerraria2, btnStartTerraria3, btnStartTerraria2_SubLabel, btnStartTerraria3_SubLabel,
                check1, DebugButton1, DebugButton2 
                );

            window.OnDraw += (isSDL) => {
                tipText = "";
                for (int i = 0; i < window.Children.Count; i++) {
                    var c = window.Children[i];
                    if (c.IsType<Window.Button>(out var btn)) {
                        if (btn.IsHovered) {
                            btn.DoHover();
                        }
                    }
                }

                window.Get_WindowSize(out int w, out int h);
                if (tipText != "") {
                    window.Draw_Text(10, h - 10, 1f, tipText, 0f, 1f);
                } else {
                    window.Draw_Text(10, h - 10, 1f, "cModLoader: " + BuildData.RAW_STRING, 0f, 1f);
                }
                window.Draw_DefaultIcon(Accessibility.MessageBoxIcon.cModLoader, w - 32 - 10, h - 32 - 10);

                DebugButton1.Enabled = check1.Checked;
                DebugButton2.Enabled = check1.Checked;
                if (check1.Checked) {
                    var str = string.Join("\n", Output.CustomWriter.ConsoleOutput.GetOutput(h / 12));
                    window.Draw_SetDrawColour(190, 115, 255); // some arbitrary colour, purple-ish pink
                    window.Draw_Text(10, 10, 1f, str);
                }

                Window.Window.UpdateWindows();

            };

            Output.Print("Opening cModLauncher");
            window.Open(true);
        
        }

    }
}
