using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.TransactionModels
{
    public class TransferModel
    {
        [Required]
        public int SourceAccountID { get; set; }

        [Required]
        public int DestinationAccountID { get; set; }

        [Range(1,double.MaxValue)]
        public decimal Amount { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
