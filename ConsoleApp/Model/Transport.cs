//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transport()
        {
            this.Reysy = new HashSet<Reysy>();
        }
    
        public int IDTransporta { get; set; }
        public int IDSotrudnika { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string GodVypusla { get; set; }
        public int Vmestimost { get; set; }
        public string Sostoyanie { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reysy> Reysy { get; set; }
        public virtual Sotrudniki Sotrudniki { get; set; }
        public virtual Voditeli Voditeli { get; set; }
    }
}
