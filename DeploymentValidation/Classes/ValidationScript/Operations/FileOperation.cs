using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;
using DeploymentValidation.Classes.ValidationScript.Operations.Validators;

namespace DeploymentValidation.Classes.ValidationScript.Operations
{
    class FileOperation : IOperation
    {
        private readonly List<FileValidator> _fileValidators;
        private readonly OperationMessages _operationMessages;

        private FileOperation(List<FileValidator> fileValidators, OperationMessages operationMessages)
        {
            _fileValidators = fileValidators;
            _operationMessages = operationMessages;
        }

        public static FileOperation FromXmlNode(XmlNode xmlNode)
        {
            var fileNodes = xmlNode.SelectNodes("./Files/File");
            if (fileNodes == null)
                return null;

            var messageNodes = xmlNode.SelectNodes("./Messages/Message");
            var operationMessages = OperationMessages.FromXmlNode(messageNodes);

            var fileValidators = fileNodes.OfType<XmlNode>().Select(FileValidator.FromXmlNode).Where(validator => validator != null);
            
            return new FileOperation(fileValidators.ToList(), operationMessages);
        }

        public void Process()
        {

            MessageManager.Display(_operationMessages.GetMessage("initialize"));

            List<FileValidator> notExistingFiles = new List<FileValidator>();
            List<FileValidator> checksumErrorFiles = new List<FileValidator>();


            foreach (FileValidator fileValidator in _fileValidators)
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
                    foreach (FileValidator notExistingFile in notExistingFiles) 
                        MessageManager.Display(new Message("\t" + Path.GetFileName(notExistingFile.FilePath), Message.MessageTypes.Normal));

                }
                
                if (checksumErrorFiles.Count > 0)
                {

                    MessageManager.Display(_operationMessages.GetMessage("checksumFailed"));
                    foreach (FileValidator checksumErrorFile in checksumErrorFiles) 
                        MessageManager.Display(new Message("\t" + Path.GetFileName(checksumErrorFile.FilePath) + " (" + checksumErrorFile.GenerateMd5Checksum() + ")", Message.MessageTypes.Normal));

                }
            }

        }
    }
}
