using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("manager")]
    public class Manager
    {
        [XmlAttribute("matcher")]
        public string Matcher { get; set; }
        [XmlAttribute("rev")]
        public string Revision { get; set; }
        [XmlAttribute("org")]
        public string Organisation { get; set; }
        [XmlAttribute("module")]
        public string Module { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
