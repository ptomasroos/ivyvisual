using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("conf")]
    public class Configuration
    {
        [XmlAttribute("extends")]
        public string Extends { get; set; }
        [XmlAttribute("transitive")]
        public bool Transitive { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("visibility")]
        public string Visibility { get; set; }
        [XmlAttribute("deprecated")]
        public string Deprecated { get; set; }
    }
}
