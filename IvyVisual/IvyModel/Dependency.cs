using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IvyVisual.IvyModel
{
    //<dependency branchConstraint="String" rev="String" branch="String" org="String" force="true" revConstraint="String" transitive="true" name="String" conf="String" changing="false">
    //  <conf mapped="String" name="String">
    //    <mapped name="String"/>
    //  </conf>
    //  <include type="String" matcher="String" ext="String" name="String" conf="String">
    //    <conf name="String"/>
    //  </include>
    //  <exclude type="String" matcher="String" org="String" ext="String" module="String" name="String" conf="String">
    //    <conf name="String"/>
    //  </exclude>
    //</dependency>
    [XmlRoot("dependency")]
    public class Dependency
    {
        [XmlAttribute("branchConstraint")]
        public string BranchConstraint { get; set; }
        [XmlAttribute("rev")]
        public string Revision { get; set; }
        [XmlAttribute("branch")]
        public string Branch { get; set; }
        [XmlAttribute("org")]
        public string Organisation { get; set; }
        [XmlAttribute("force")]
        public bool Force { get; set; }
        [XmlAttribute("revConstraint")]
        public string RevisionConstraint { get; set; }
        [XmlAttribute("transitive")]
        public bool Transitive { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("conf")]
        public string Configuration { get; set; }
        [XmlAttribute("changing")]
        public bool Changing { get; set; }

        [XmlElement("conf")]
        public List<Configuration> ConfigurationList { get; set; }
        [XmlElement("artifact")]
        public List<Artifact> ArtifactList { get; set; }
        [XmlElement("include")]
        public List<Include> IncludeList { get; set; }
        [XmlElement("exclude")]
        public List<Exclude> ExcludeList { get; set; }
    }
}
