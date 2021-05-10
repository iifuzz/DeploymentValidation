using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentValidation.Classes.ValidationScript.Messages
{
    class Message
    {
        public enum MessageTypes {Normal, Error, Warning, Success}

        public Message(string text, MessageTypes type)
        {
            Text = text;
            Type = type;
        }

        public string Text { get; private set; }
        public MessageTypes Type { get; private set; }
    }
}
