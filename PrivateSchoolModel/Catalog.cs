namespace PrivateSchoolModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Catalog")]
    public partial class Catalog
    {
        public int CatalogId { get; set; }

        public int? NotaId { get; set; }

        public int? ElevId { get; set; }

        public virtual Nota Nota { get; set; }

        public virtual Elev Elev { get; set; }
    }
}
