using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    [XmlRoot("info")]
    public class Info
    {
        [XmlAttribute("default")]
        public bool Default { get; set; }
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }
        [XmlAttribute("branch")]
        public string Branch { get; set; }
        [XmlAttribute("resolver")]
        public string Resolver { get; set; }
        [XmlAttribute("status")]
        public string Status { get; set; }
        [XmlAttribute("revision")]
        public string Revision { get; set; }
        [XmlAttribute("module")]
        public string Module { get; set; }
        [XmlAttribute("publication")]
        public string Publication { get; set; }
        [XmlAttribute("organisation")]
        public string Organisation { get; set; }

        [XmlElement("license")]
        public List<License> License { get; set; }
        [XmlElement("ivyauthor")]
        public List<IvyAuthor> IvyAuthor { get; set; }
        [XmlElement("repository")]
        public List<Repository> Repository { get; set; }
        [XmlElement("description")]
        public Description Description { get; set; }
    }
}
