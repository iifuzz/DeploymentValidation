using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;
using DeploymentValidation.Classes.ValidationScript.Operations;

namespace DeploymentValidation.Classes.ValidationScript
{
    class Script
    {
        private readonly string _filePath;

        private Script(string filePath)
        {
            _filePath = filePath;
        }

        public List<IOperation> GetOperations()
        {
            XmlDocument root = new XmlDocument
            {
                PreserveWhitespace = true
            };

            try
            {
                root.Load(_filePath);
            } catch (Exception e)
            {
                MessageManager.Display(new Message("Could not load validation script.", Message.MessageTypes.Warning));
                return new List<IOperation>();
            }

            var operationXmlNodeList = root.SelectNodes("/ValidationScript/*");
            if (operationXmlNodeList == null)
                return new List<IOperation>();

            return (from XmlNode operationXmlNode in operationXmlNodeList select OperationFactory.FromXmlNode(operationXmlNode) into operation where operation != null select operation).ToList();
        }

        public static Script FromFile(string filePath)
        {
            return new Script(filePath);
        }
    }
}
