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
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
        }
    }
}
