using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("dependencies")]
    public class Dependencies
    {
        [XmlAttribute("defaultconf")]
        public string DefaultConfiguration { get; set; }
        [XmlAttribute("confmappingoverride")]
        public bool ConfigurationMappingOverride { get; set; }
        [XmlAttribute("defaultconfmapping")]
        public string DefaultConfigurationMapping { get; set; }

        [XmlElement("dependency")]
        public List<Dependency> DependencyList { get; set; }
        [XmlElement("exclude")]
        public List<Exclude> ExcludeList { get; set; }
        [XmlElement("override")]
        public List<Override> OverrideList { get; set; }
        [XmlElement("conflict")]
        public List<Conflict> ConflictList { get; set; }
    }
}
