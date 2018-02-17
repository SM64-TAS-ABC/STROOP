using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Rendering;

namespace STROOP.Controls
{
    class VisualRegexLinkGenerator : VisualLineElementGenerator
    { 
        public event EventHandler<string> LinkClick;

        private Regex _regex;

        public VisualRegexLinkGenerator(Regex regex)
        {
            _regex = regex;
        }

        Match FindMatch(int startOffset)
        {
            int endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
            string relevantText = CurrentContext.Document.GetText(startOffset, endOffset - startOffset);
            return _regex.Match(relevantText);
        }

        public override VisualLineElement ConstructElement(int offset)
        {
            Match match = FindMatch(offset);
            if (!match.Success || match.Index != 0)
                return null;

            return new VisualLinkLineText(match.Value, CurrentContext.VisualLine, match.Length,
                (customlink) => LinkClick?.Invoke(this, customlink.Text));
        }

        public override int GetFirstInterestedOffset(int startOffset)
        {
            Match match = FindMatch(startOffset);
            return match.Success ? startOffset + match.Index : -1;
        }
    }
}
