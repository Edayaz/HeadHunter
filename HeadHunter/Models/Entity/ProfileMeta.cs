//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HeadHunter.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProfileMeta
    {
        public int ProfileMetaId { get; set; }
        public Nullable<int> ProfileId { get; set; }
        public Nullable<int> ProfilScanId { get; set; }
        public Nullable<int> SourceDataTypeId { get; set; }
        public string ProfileMetaValue { get; set; }
        public Nullable<bool> ProfilMetaFlag { get; set; }
    
        public virtual Profil Profil { get; set; }
        public virtual ProfilScan ProfilScan { get; set; }
        public virtual SourceDataType SourceDataType { get; set; }
    }
}
