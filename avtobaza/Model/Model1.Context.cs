﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class avtobazaEntities1 : DbContext
    {
        public avtobazaEntities1()
            : base("name=avtobazaEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bilety> Bilety { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Dolzhnosti> Dolzhnosti { get; set; }
        public virtual DbSet<Marshruty> Marshruty { get; set; }
        public virtual DbSet<Oplata> Oplata { get; set; }
        public virtual DbSet<Ostanovki> Ostanovki { get; set; }
        public virtual DbSet<OstanovkiMarshrutov> OstanovkiMarshrutov { get; set; }
        public virtual DbSet<Raspisanie> Raspisanie { get; set; }
        public virtual DbSet<Reysy> Reysy { get; set; }
        public virtual DbSet<Sotrudniki> Sotrudniki { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Transport> Transport { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Voditeli> Voditeli { get; set; }
    }
}
