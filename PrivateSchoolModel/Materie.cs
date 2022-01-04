namespace PrivateSchoolModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Materie")]
    public partial class Materie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materie()
        {
            Scoalas = new HashSet<Scoala>();
        }

        [Key]
        public int MaterieId { get; set; }

        [StringLength(50)]
        public string mdenumire { get; set; }

        [StringLength(50)]
        public string mclasa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scoala> Scoalas { get; set; }
    }
}
