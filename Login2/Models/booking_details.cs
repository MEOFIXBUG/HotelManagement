//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class booking_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public booking_details()
        {
            this.service_details = new HashSet<service_details>();
        }
    
        public int ID { get; set; }
        public int Booking_id { get; set; }
        public int Room_id { get; set; }
        public int NumberOfPeople { get; set; }
        public double Amount { get; set; }
        public Nullable<System.DateTime> DateOfRentStart { get; set; }
        public Nullable<System.DateTime> DateOfRentEnd { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
        public bool hasForeigner { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual booking booking { get; set; }
        public virtual room room { get; set; }
        public virtual booking_status booking_status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<service_details> service_details { get; set; }
    }
}
