﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Login2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class hotelEntities1 : DbContext
    {
        public hotelEntities1()
            : base("name=hotelEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<booking_details> booking_details { get; set; }
        public virtual DbSet<booking> bookings { get; set; }
        public virtual DbSet<customer_type> customer_type { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<room_status> room_status { get; set; }
        public virtual DbSet<room_type> room_type { get; set; }
        public virtual DbSet<room> rooms { get; set; }
        public virtual DbSet<service_details> service_details { get; set; }
        public virtual DbSet<service> services { get; set; }
        public virtual DbSet<staff> staffs { get; set; }
    }
}
