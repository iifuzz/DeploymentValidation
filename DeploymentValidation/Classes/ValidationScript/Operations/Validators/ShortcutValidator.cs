using System;
using System.IO;

namespace DeploymentValidation.Classes.ValidationScript.Operations.Validators
{
    class ShortcutValidator : IValidation, IOperation
    {
        private readonly string _shortcutName;

        public ShortcutValidator(string shortcutName)
        {
            _shortcutName = shortcutName;
        }

        public bool IsValid()
        {
            string desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return File.Exists(desktopDirectory + _shortcutName + ".lnk");
        }

        public void Process()
        {
            if (!IsValid())
            {
                // Process Messages
            }
        }
    }
}
