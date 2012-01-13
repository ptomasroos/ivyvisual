using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("override")]
    public class Override
    {
        [XmlAttribute("matcher")]
        public string Matcher { get; set; }
        [XmlAttribute("rev")]
        public string Revision { get; set; }
        [XmlAttribute("branch")]
        public string Branch { get; set; }
        [XmlAttribute("org")]
        public string Organisation { get; set; }
        [XmlAttribute("module")]
        public string Module { get; set; }
    }
}
