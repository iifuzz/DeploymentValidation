using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;

namespace DeploymentValidation.Classes.ValidationScript.Operations.Validators
{
    class FileValidator : IValidation
    {
        private readonly string _filePath;
        private readonly bool _validateChecksum;
        private readonly string _expectedMd5Checksum;


        private FileValidator(string filePath, bool validateChecksum, string expectedMd5Checksum)
        {
            _filePath = Environment.ExpandEnvironmentVariables(filePath);
            _expectedMd5Checksum = expectedMd5Checksum;
            _validateChecksum = validateChecksum;
        }

        public string FilePath => _filePath;

        public static FileValidator FromXmlNode(XmlNode xmlFileNode)
        {
            if (xmlFileNode.Attributes == null) 
                return null;

            try
            {
                string filePath = xmlFileNode.Attributes["path"].Value;
                bool validateChecksum = bool.Parse(xmlFileNode.Attributes["checksumVerification"].Value);
                string expectedChecksum = "";
                if(validateChecksum)
                    expectedChecksum = xmlFileNode.Attributes["expectedChecksum"].Value;

                return new FileValidator(filePath, validateChecksum, expectedChecksum);

            } catch (Exception e)
            {
                MessageManager.Display(new Message("File Validator Exception: " + e.Message, Message.MessageTypes.Error));
                return null;
            }
            
        }

        /// <summary>
        /// Checks if the file exists on the system.
        /// </summary>
        /// <returns>False if the file does not exist on the system</returns>
        public bool Exists()
        {
            return File.Exists(FilePath);
        }

        /// <summary>
        /// Compares expected md5 checksum against the checksum of the existing file
        /// </summary>
        /// <returns>False if the expected md5 checksum does not match the calculated md5 checksum</returns>
        public bool IsValid()
        {
            if (!Exists())
                return false;

            return !_validateChecksum || _expectedMd5Checksum.Trim() == GenerateMd5Checksum().Trim();
        }

        /// <summary>
        /// Calculates and returns the MD5 checksum of the file 
        /// </summary>
        /// <returns>String representing a MD5 checksum</returns>
        public string GenerateMd5Checksum()
        {
            string localmd5 = "";

            using (MD5 md5Generator = MD5.Create())
            {
                // If local checksum doesnt exist, calculate it
                using (BufferedStream stream = new BufferedStream(File.OpenRead(FilePath), 1200000))
                {
                    localmd5 = BitConverter.ToString(md5Generator.ComputeHash(stream)).Replace("-", "").ToLower();
                    stream.Close();
                }
            }
            
            return localmd5;
        }

    }
}
