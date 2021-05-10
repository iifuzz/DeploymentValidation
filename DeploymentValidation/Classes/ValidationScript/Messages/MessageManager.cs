using Colorify;
using Colorify.UI;

namespace DeploymentValidation.Classes.ValidationScript.Messages
{
    static class MessageManager
    {
        private static Format Console {get; } = new Format(Theme.Dark);

        public static void Initialize()
        {
            Console.ResetColor();
            Console.Clear();
        }

        public static void Display(Message message)
        {
            if (message.Text.Trim().Length == 0)
                return;

            switch (message.Type)
            {
                case Message.MessageTypes.Normal:
                    Console.WriteLine(message.Text);
                    break;
                case Message.MessageTypes.Error:
                    Console.WriteLine(message.Text, Colors.bgDanger);
                    break;
                case Message.MessageTypes.Warning:
                    Console.WriteLine(message.Text, Colors.bgWarning);
                    break;
                case Message.MessageTypes.Success:
                    Console.WriteLine(message.Text, Colors.bgSuccess);
                    break;
            }
        }
    }
}
