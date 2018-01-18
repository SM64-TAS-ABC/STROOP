using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public class XmlBuilder
    {
        private string _rootElement;
        private string _value;
        private readonly Dictionary<string, string> _elementDictionary;

        public XmlBuilder()
        {
            _elementDictionary = new Dictionary<string, string>();
        }

        public void SetRootElement(string rootElement)
        {
            _rootElement = rootElement;
        }

        public void SetValue(string value)
        {
            _value = value;
        }

        public void SetElement(string elementName, string elementValue)
        {
            _elementDictionary.Add(elementName, elementValue);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<");
            stringBuilder.Append(_rootElement);
            foreach (KeyValuePair<string, string> elementPair in _elementDictionary) {
                stringBuilder.Append(" ");
                stringBuilder.Append(elementPair.Key);
                stringBuilder.Append("=");
                stringBuilder.Append("\"");
                stringBuilder.Append(elementPair.Value);
                stringBuilder.Append("\"");
            }
            stringBuilder.Append(">");
            stringBuilder.Append(_value);
            stringBuilder.Append("<");
            stringBuilder.Append("/");
            stringBuilder.Append(_rootElement);
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }
    }
}
