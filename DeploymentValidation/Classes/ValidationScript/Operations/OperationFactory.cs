using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Operations.Validators;

namespace DeploymentValidation.Classes.ValidationScript.Operations
{
    static class OperationFactory
    {
        public static IOperation FromXmlNode(XmlNode xmlNode)
        {
            string operation = xmlNode.Name.ToLower();
            
            switch (operation)
            {
                case "validate":
                    return GetValidationOperation(xmlNode);
                    
                case "create":
                    return GetCreationOperation(xmlNode);
            }

            return null;
        }

        private static IOperation GetValidationOperation(XmlNode xmlNode)
        {
            string type = xmlNode.Attributes?["type"].Value.ToLower();

            switch (type)
            {
                case "file":
                    return FileOperation.FromXmlNode(xmlNode);
                case "registry":
                    return RegistryOperation.FromXmlNode(xmlNode);
                case "foldersize":
                    return FolderSizeOperation.FromXmlNode(xmlNode);
                case "shortcut":
                    break;
            }

            return null;
        }

        private static IOperation GetCreationOperation(XmlNode xmlNode)
        {
            
            return CreationOperation.FromXmlNode(xmlNode);
        }
    }
}
