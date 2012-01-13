using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("ivy-module")]
    public class IvyModule
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("info")]
        public Info Info { get; set; }
        [XmlElement("configurations")]
        public Configurations Configurations { get; set; }
        [XmlElement("publications")]
        public Publications Publications { get; set; }
        [XmlElement("dependencies")]
        public Dependencies Dependencies { get; set; }
        [XmlElement("conflicts")]
        public Conflics Conflicts { get; set; }
    }
}
