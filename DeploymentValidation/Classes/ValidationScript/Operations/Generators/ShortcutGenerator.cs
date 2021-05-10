using System;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Messages;

namespace DeploymentValidation.Classes.ValidationScript.Operations.Generators
{
    class ShortcutGenerator : IGenerator
    {
        private readonly string _shortcutName;
        private readonly string _shortcutDescription;
        private readonly string _applicationFilePath;

        private ShortcutGenerator(string shortcutName, string shortcutDescription, string applicationFilePath)
        {
            _shortcutName = shortcutName;
            _applicationFilePath = applicationFilePath;
            _shortcutDescription = shortcutDescription;
        }

        
        public static ShortcutGenerator FromXmlNode(XmlNode xmlFileNode)
        {
            if (xmlFileNode.Attributes == null) 
                return null;

            try
            {
                string shortcutName = xmlFileNode.Attributes["name"].Value;
                string description = xmlFileNode.Attributes["description"].Value;
                string targetApplication = xmlFileNode.Attributes["application"].Value;

                return new ShortcutGenerator(shortcutName, description, targetApplication);

            } catch (Exception e)
            {
                MessageManager.Display(new Message("File Validator Exception: " + e.Message, Message.MessageTypes.Error));
                return null;
            }
            
        }

        public bool Create()
        {
            string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut sc = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(desktopDirectory + "\\" + _shortcutName + ".lnk");
                sc.Description = _shortcutDescription;
                //shortcut.IconLocation = @"C:\..."; 
                sc.TargetPath = _applicationFilePath;
                // save shortcut to target
                sc.Save();

            } catch (Exception)
            {
                return false;
            }

            return true;
        }


    }
}
