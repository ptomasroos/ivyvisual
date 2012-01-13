using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("include")]
    public class Include
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("matcher")]
        public string Matcher { get; set; }
        [XmlAttribute("ext")]
        public string Extension { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("conf")]
        public string Configuration { get; set; }

        [XmlElement("conf")]
        public List<Configuration> ConfigurationList { get; set; }
    }
}
