using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("configurations")]
    public class Configurations
    {
        [XmlAttribute("confmappingoverride")]
        public bool ConfigurationMappingOverride { get; set; }
        [XmlAttribute("defaultconfmapping")]
        public string DefaultConfigurationMapping { get; set; }

        [XmlElement("conf")]
        public List<Configuration> ConfigurationList { get; set; }
    }
}
