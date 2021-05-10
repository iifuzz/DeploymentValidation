using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;
using DeploymentValidation.Classes.ValidationScript.Operations.Validators;

namespace DeploymentValidation.Classes.ValidationScript.Operations
{
    class RegistryOperation : IOperation
    {
        private readonly List<RegistryValidator> _registryValidators;
        private readonly OperationMessages _operationMessages;

        public RegistryOperation(List<RegistryValidator> registryValidators, OperationMessages operationMessages)
        {
            _registryValidators = registryValidators;
            _operationMessages = operationMessages;
        }

        
        public static RegistryOperation FromXmlNode(XmlNode xmlNode)
        {
            var fileNodes = xmlNode.SelectNodes("./Entries/Entry");
            if (fileNodes == null)
                return null;

            var messageNodes = xmlNode.SelectNodes("./Messages/Message");
            var operationMessages = OperationMessages.FromXmlNode(messageNodes);

            var fileValidators = fileNodes.OfType<XmlNode>().Select(RegistryValidator.FromXmlNode).Where(validator => validator != null);
            
            return new RegistryOperation(fileValidators.ToList(), operationMessages);
        }


        public void Process()
        {
            MessageManager.Display(_operationMessages.GetMessage("initialize"));

            List<RegistryValidator> registryValidators = new List<RegistryValidator>();

            foreach (RegistryValidator fileValidator in _registryValidators)
            {
                if (!fileValidator.IsValid()) 
                    registryValidators.Add(fileValidator);
            }

            if (registryValidators.Count == 0) 
                MessageManager.Display(_operationMessages.GetMessage("success"));
            else
            {
                MessageManager.Display(_operationMessages.GetMessage("doesNotMatch"));
            }

        }
    }
}
