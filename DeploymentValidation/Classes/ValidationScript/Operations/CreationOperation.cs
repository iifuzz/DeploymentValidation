using System.Collections.Generic;
using System.Xml;
using DeploymentValidation.Classes.ValidationScript.Operations.Generators;

namespace DeploymentValidation.Classes.ValidationScript.Operations
{
    class CreationOperation : IOperation
    {
        private readonly List<IGenerator> _generators;

        public CreationOperation(List<IGenerator> generators)
        {
            _generators = generators;
        }

        public static CreationOperation FromXmlNode(XmlNode xmlNode)
        {
            var creationNodes = xmlNode.SelectNodes("./*");
            if (creationNodes == null)
                return null;

            var generators = new List<IGenerator>();

            foreach (XmlNode creationNode in creationNodes)
            {
                if (creationNode.Name.ToLower() == "shortcut")
                {
                    ShortcutGenerator generator = ShortcutGenerator.FromXmlNode(creationNode);
                    if(generator != null)
                        generators.Add(generator);
                }
            }

            return new CreationOperation(generators);
        }

        public void Process()
        {
            foreach (IGenerator generator in _generators)
            {
                generator.Create();
            }

        }
    }
}
