using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("license")]
    public class License
    {
        [XmlAttribute("url")]
        public string Url { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
