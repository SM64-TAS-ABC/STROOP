using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace STROOP.Controls
{
    public class VisualLinkLineText : VisualLineText
    {

        public delegate void ClickHandler(VisualLinkLineText initiator);

        private ClickHandler _clicked;

        public string Text { get; private set; }

        /// <summary>
        /// Gets/Sets whether the user needs to press Control to click the link.
        /// The default value is true.
        /// </summary>
        public bool RequireControlModifierForClick { get; set; }

        /// <summary>
        /// Creates a visual line text element with the specified length.
        /// It uses the <see cref="ITextRunConstructionContext.VisualLine"/> and its
        /// <see cref="VisualLineElement.RelativeTextOffset"/> to find the actual text string.
        /// </summary>
        public VisualLinkLineText(string link, VisualLine parentVisualLine, int length, ClickHandler clickHandler)
            : base(parentVisualLine, length)
        {
            RequireControlModifierForClick = false;
            Text = link;
            _clicked = clickHandler;
        }


        public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
        {
            TextRunProperties.SetForegroundBrush(Brushes.DarkOrange);
            TextRunProperties.SetTextDecorations(TextDecorations.Underline);
            return base.CreateTextRun(startVisualColumn, context);
        }

        bool LinkIsClickable()
        {
            if (string.IsNullOrEmpty(Text))
                return false;

            return true;
        }


        protected override void OnQueryCursor(QueryCursorEventArgs e)
        {
            if (LinkIsClickable())
            {
                e.Handled = true;
                e.Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !e.Handled && LinkIsClickable())
            {
                if (_clicked != null)
                {
                    _clicked.Invoke(this);
                    e.Handled = true;
                }
            }
        }

        protected override VisualLineText CreateInstance(int length)
        {

            var a = new VisualLinkLineText(Text, ParentVisualLine, length, _clicked)
            {
                RequireControlModifierForClick = RequireControlModifierForClick,
            };
            return a;
        }
    }
}
