using System;
using System.IO;
using System.Linq;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;

namespace DeploymentValidation.Classes.ValidationScript.Operations.Validators
{
    class FolderSizeValidator : IValidation
    {
        private readonly long _expectedFolderSize;

        private long? _folderSize = null;

        private FolderSizeValidator(string directory, long expectedFolderSize)
        {
            Folder = directory;
            _expectedFolderSize = expectedFolderSize;
        }

        public string Folder { get; }

        public long FolderSize
        {
            get
            {
                if(_folderSize == null)
                    _folderSize = Directory.GetFiles(Folder,"*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));

                return (long) _folderSize;
            }
        }

        public static FolderSizeValidator FromXmlNode(XmlNode xmlFileNode)
        {
            if (xmlFileNode.Attributes == null) 
                return null;

            try
            {
                string filePath = xmlFileNode.Attributes["path"].Value;
                string expectedFileSize = xmlFileNode.Attributes["expectedFileSize"].Value;

                return new FolderSizeValidator(filePath, Convert.ToInt64(expectedFileSize));

            } catch (Exception e)
            {
                MessageManager.Display(new Message("Folder Size Validator Exception: " + e.Message, Message.MessageTypes.Error));
                return null;
            }
            
        }

        public bool Exists()
        {
            return Directory.Exists(Folder);

        }
        public bool IsValid()
        {
            if(!Exists())
                return false;

            
            return _expectedFolderSize == FolderSize;
        }
    }
}
