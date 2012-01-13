using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("artifact")]
    public class Artifact
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("url")]
        public string Url { get; set; }
        [XmlAttribute("ext")]
        public string Extension { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("conf")]
        public string Configuration { get; set; }

        [XmlElement("conf")]
        public List<Configuration> ConfigurationList { get; set; }

        public string Filename
        {
            get
            {
                return Name + "." + Type;
            }
        }
    }
}
