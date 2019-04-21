using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Structs.Configurations;
using System.Drawing;

namespace STROOP.Managers
{
    public class SoundManager
    {
        public SoundManager(TabPage tabPage)
        {
            SplitContainer splitContainerSound = tabPage.Controls["splitContainerSound"] as SplitContainer;

            SplitContainer splitContainerSoundMusic = splitContainerSound.Panel1.Controls["splitContainerSoundMusic"] as SplitContainer;
            ListBox listBoxSoundMusic = splitContainerSoundMusic.Panel1.Controls["listBoxSoundMusic"] as ListBox;
            TextBox textBoxSoundMusic = splitContainerSoundMusic.Panel2.Controls["textBoxSoundMusic"] as TextBox;
            Button buttonSoundPlayMusic = splitContainerSoundMusic.Panel2.Controls["buttonSoundPlayMusic"] as Button;

            SplitContainer splitContainerSoundSoundEffect = splitContainerSound.Panel2.Controls["splitContainerSoundSoundEffect"] as SplitContainer;
            ListBox listBoxSoundSoundEffect = splitContainerSoundSoundEffect.Panel1.Controls["listBoxSoundSoundEffect"] as ListBox;
            TextBox textBoxSoundSoundEffect = splitContainerSoundSoundEffect.Panel2.Controls["textBoxSoundSoundEffect"] as TextBox;
            Button buttonSoundPlaySoundEffect = splitContainerSoundSoundEffect.Panel2.Controls["buttonSoundPlaySoundEffect"] as Button;

            TableConfig.MusicData.GetMusicEntryList().ForEach(musicEntry => listBoxSoundMusic.Items.Add(musicEntry));
            listBoxSoundMusic.Click += (sender, e) =>
            {
                MusicEntry musicEntry = listBoxSoundMusic.SelectedItem as MusicEntry;
                textBoxSoundMusic.Text = musicEntry.Index.ToString();
            };
            buttonSoundPlayMusic.Click += (sender, e) =>
            {
                int? musicIndexNullable = ParsingUtilities.ParseIntNullable(textBoxSoundMusic.Text);
                if (musicIndexNullable == null) return;
                int musicIndex = musicIndexNullable.Value;
                if (musicIndex < 0 || musicIndex > 34) return;
                uint setMusic = RomVersionConfig.Switch(0x80320544, 0x8031F690);
                InGameFunctionCall.WriteInGameFunctionCall(setMusic, 0, (uint)musicIndex, 0);
            };
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
        }
    }
}
