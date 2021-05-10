using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;
using DeploymentValidation.Classes.ValidationScript.Operations.Validators;

namespace DeploymentValidation.Classes.ValidationScript.Operations
{
    class FolderSizeOperation : IOperation
    {
        private readonly List<FolderSizeValidator> _folderSizeValidators;
        private readonly OperationMessages _operationMessages;

        private FolderSizeOperation(List<FolderSizeValidator> folderSizeValidators, OperationMessages operationMessages)
        {
            _folderSizeValidators = folderSizeValidators;
            _operationMessages = operationMessages;
        }

        
        public static FolderSizeOperation FromXmlNode(XmlNode xmlNode)
        {
            var fileNodes = xmlNode.SelectNodes("./Directories/Directory");
            if (fileNodes == null)
                return null;

            var messageNodes = xmlNode.SelectNodes("./Messages/Message");
            var operationMessages = OperationMessages.FromXmlNode(messageNodes);

            var folderSizeValidators = fileNodes.OfType<XmlNode>().Select(FolderSizeValidator.FromXmlNode).Where(validator => validator != null);
            
            return new FolderSizeOperation(folderSizeValidators.ToList(), operationMessages);
        }

        public void Process()
        {
            MessageManager.Display(_operationMessages.GetMessage("initialize"));

            List<FolderSizeValidator> notExistingFiles = new List<FolderSizeValidator>();
            List<FolderSizeValidator> checksumErrorFiles = new List<FolderSizeValidator>();


            foreach (FolderSizeValidator fileValidator in _folderSizeValidators)
            {
                if (!fileValidator.Exists())
                    notExistingFiles.Add(fileValidator);
                else if (!fileValidator.IsValid()) 
                    checksumErrorFiles.Add(fileValidator);
            }

            if(notExistingFiles.Count == 0 && checksumErrorFiles.Count == 0)
                MessageManager.Display(_operationMessages.GetMessage("success"));
            else
            {
                if (notExistingFiles.Count > 0)
                {

                    MessageManager.Display(_operationMessages.GetMessage("doesNotExist"));
                    foreach (FolderSizeValidator notExistingFile in notExistingFiles) 
                        MessageManager.Display(new Message("\t" + notExistingFile.Folder, Message.MessageTypes.Normal));

                }
                
                if (checksumErrorFiles.Count > 0)
                {

                    MessageManager.Display(_operationMessages.GetMessage("fileSizeFailed"));
                    foreach (FolderSizeValidator checksumErrorFile in checksumErrorFiles) 
                        MessageManager.Display(new Message("\t" + checksumErrorFile.Folder + " (" + checksumErrorFile.FolderSize + " bytes)", Message.MessageTypes.Normal));

                }
            }

        }
    }
}
