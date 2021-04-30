using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.Map
{
    public class MapObjectCustomBackground : MapObjectBackground
    {
        private readonly Dictionary<string, object> _dictionary;
        private object _backgroundChoice;

        public MapObjectCustomBackground()
            : base()
        {
            _dictionary = new Dictionary<string, object>();
            _backgroundChoice = "Recommended";
        }

        public override Image GetInternalImage()
        {
            return MapUtilities.GetBackgroundImage(_backgroundChoice);
        }

        public override string GetName()
        {
            return "Custom Background";
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
                List<object> backgroundImageChoices = new List<object>() { "Recommended" };
                backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));
                backgroundImageChoices.ForEach(backgroundImage => _dictionary[backgroundImage.ToString()] = backgroundImage);

                ToolStripMenuItem itemSelectMap = new ToolStripMenuItem("Select Background");
                itemSelectMap.Click += (sender, e) =>
                {
                    SelectionForm form = new SelectionForm();
                    form.Initialize(
                        "Select a Background",
                        "Set Background",
                        backgroundImageChoices,
                        backgroundChoice =>
                        {
                            MapObjectSettings settings = new MapObjectSettings(
                                changeBackground: true, newBackground: backgroundChoice.ToString());
                            GetParentMapTracker().ApplySettings(settings);
                        });
                    form.Show();
                };
                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSelectMap);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeBackground)
            {
                _backgroundChoice = _dictionary[settings.NewBackground];
            }
        }
    }
}
