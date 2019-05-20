using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ViewModels
{
	public class ReportViewModel
	{
		[DataType(DataType.Date)]
		public DateTime FromDate { get; set; }
		[DataType(DataType.Date)]
		public DateTime TillDate { get; set; }
	}
}
