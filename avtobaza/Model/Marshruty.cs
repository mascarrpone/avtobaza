//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace avtobaza.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Marshruty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Marshruty()
        {
            this.OstanovkiMarshrutov = new HashSet<OstanovkiMarshrutov>();
            this.Raspisanie = new HashSet<Raspisanie>();
            this.Reysy = new HashSet<Reysy>();
        }
    
        public int IDMarshruta { get; set; }
        public int IDSotrudnika { get; set; }
        public string Nazvanie { get; set; }
        public string PunktOtpravleniya { get; set; }
        public string PunktPribytiya { get; set; }
        public Nullable<double> Protyazhennost { get; set; }
        public Nullable<System.TimeSpan> VremyaVPuti { get; set; }
    
        public virtual Sotrudniki Sotrudniki { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OstanovkiMarshrutov> OstanovkiMarshrutov { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Raspisanie> Raspisanie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reysy> Reysy { get; set; }
    }
}
