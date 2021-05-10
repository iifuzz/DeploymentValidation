using System;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;
using Microsoft.Win32;

namespace DeploymentValidation.Classes.ValidationScript.Operations.Validators
{
    class RegistryValidator : IValidation
    {
        private readonly RegistryKey _registryKey;
        private readonly string _subKey;
        private readonly string _key;
        private readonly string _expectedValue;

        public RegistryValidator(RegistryKey registryKey, string subKey, string key, string expectedValue)
        {
            _registryKey = registryKey;
            _subKey = subKey;
            _key = key;
            _expectedValue = expectedValue;
        }

        public static RegistryValidator FromXmlNode(XmlNode registryXmlNode)
        {
            if (registryXmlNode.Attributes == null) 
                return null;

            try
            {
                string registryKeyString = registryXmlNode.Attributes["registryKey"].Value;
                string subKey = registryXmlNode.Attributes["subkey"].Value;
                string key = registryXmlNode.Attributes["key"].Value;
                string expectedValue = registryXmlNode.Attributes["expectedValue"].Value;

                return new RegistryValidator(GetRegistryKey(registryKeyString), subKey, key, expectedValue);

            } catch (Exception e)
            {
                MessageManager.Display(new Message("File Validator Exception: " + e.Message, Message.MessageTypes.Error));
                return null;
            }
        }

        private static RegistryKey GetRegistryKey(string registryKeyString)
        {
            switch (registryKeyString.ToLower())
            {
                case "localmachine":
                    return Registry.LocalMachine;
                case "classesroot":
                    return Registry.ClassesRoot;
                case "currentuser":
                    return Registry.CurrentUser;
            }

            return Registry.CurrentUser;
        }

        public bool IsValid()
        {
            RegistryKey sk1 = _registryKey.OpenSubKey(_subKey, false);
            if (sk1 == null)
                return false;

            string value = sk1.GetValue(_key).ToString();
            return value == _expectedValue;
        }

    }
}
