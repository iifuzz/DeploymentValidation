using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DeploymentValidation.Classes.ValidationScript.Messages
{
    class OperationMessages
    {
        private readonly Dictionary<string, Message> _messages = new Dictionary<string, Message>();

        public static OperationMessages FromXmlNode(XmlNodeList messageNodes)
        {
            OperationMessages operationMessages = new OperationMessages();

            operationMessages.PopulateMessages(messageNodes.OfType<XmlNode>());

            return operationMessages;
        }

        public Message GetMessage(string key)
        {
            return !_messages.ContainsKey(key) ? new Message(key + " key not found.", Message.MessageTypes.Warning) : _messages[key];
        }

        private void PopulateMessages(IEnumerable<XmlNode> messageNodes)
        {
            foreach (XmlNode messageNode in messageNodes)
            {
                if (messageNode.Attributes == null)
                    continue;

                string key = messageNode.Attributes["key"].Value;
                string messageText = messageNode.Attributes["text"].Value;
                string messageType = messageNode.Attributes["type"].Value;
                if (!Enum.TryParse(messageType, true, out Message.MessageTypes type))
                    continue;

                var message = new Message(messageText, type);
                _messages[key] = message;
            }
        }
    }
}
