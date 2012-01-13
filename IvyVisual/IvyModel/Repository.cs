using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("repository")]
    public class Repository
    {
        [XmlAttribute("artifacts")]
        public bool Artifacts { get; set; }
        [XmlAttribute("url")]
        public string Url { get; set; }
        [XmlAttribute("pattern")]
        public string Pattern { get; set; }
        [XmlAttribute("ivys")]
        public bool Ivys { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
