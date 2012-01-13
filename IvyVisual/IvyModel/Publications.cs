using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("publications")]
    public class Publications
    {
        [XmlAttribute("defaultconf")]
        public string DefaultConfiguration { get; set; }

        [XmlElement("artifact")]
        public List<Artifact> ArtifactList { get; set; }
    }
}
