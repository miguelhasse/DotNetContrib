using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newhotel.PointOfService.Model
{
    public class TicketTransfer
    {
        public Guid ID { get; set; }

        /// <summary>
        /// column="IPOS_ORIG"
        /// Punto de venta de donde se transfiere
        /// </summary>
        //[PersistentColumn("IPOS_ORIG")]
        public Guid StandOrig { get; set; }

        /// <summary>
        /// column="CAJA_ORIG"
        /// Caja de donde se transfiere
        /// </summary>
        //[PersistentColumn("CAJA_ORIG")]
        public Guid CashierOrig { get; set; }

        /// <summary>
        /// column="IPOS_DEST"
        /// Punto de venta a donde se transfiere
        /// </summary>
        //[PersistentColumn("IPOS_DEST")]
        public Guid StandDest { get; set; }

        /// <summary>
        /// column="CAJA_DEST"
        /// Caja a donde se transfiere
        /// </summary>
        //[PersistentColumn("CAJA_DEST")]
        public Guid? CashierDest { get; set; }

        /// <summary>
        /// column="VEND_TRAN"
        /// Ticket de donde se transfiere
        /// </summary>
        //[PersistentColumn("VEND_TRAN")]
        public Guid TicketOrig { get; set; }

        /// <summary>
        /// column="UTIL_TRAN"
        /// Usuario que hace la transferencia
        /// </summary>
        //[PersistentColumn("UTIL_TRAN")]
        public Guid UtilTran { get; set; }

        /// <summary>
        /// column="TRAN_DATE"
        /// Fecha en que se hace la transferencia
        /// </summary>
        //[PersistentColumn("TRAN_DATE")]
        public DateTime TranDate { get; set; }

        /// <summary>
        /// column="TRAN_TIME"
        /// Hora en que se hace la transferencia
        /// </summary>
        //[PersistentColumn("TRAN_TIME")]
        public DateTime TranTime { get; set; }

        /// <summary>
        /// column="VEND_ACEP"
        /// Ticket a donde se transfiere
        /// </summary>
        //[PersistentColumn("VEND_ACEP")]
        public Guid? TicketDest { get; set; }

        /// <summary>
        /// column="UTIL_ACEP"
        /// Usuario que acepta la transferencia
        /// </summary>
        //[PersistentColumn("UTIL_ACEP")]
        public Guid? UtilAcep { get; set; }

        /// <summary>
        /// column="ACEP_DATE"
        /// Fecha en que se acepta la transferencia
        /// </summary>
        //[PersistentColumn("ACEP_DATE")]
        public DateTime? AcepDate { get; set; }

        /// <summary>
        /// column="ACEP_TIME"
        /// Hora en que se acepta la transferencia
        /// </summary>
        //[PersistentColumn("ACEP_TIME")]
        public DateTime? AcepTime { get; set; }
    }
}
