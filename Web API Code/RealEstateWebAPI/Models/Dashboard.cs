namespace RealEstateWebAPI.Models
{
	public class Dashboard
	{
		public List<RealEstateDashboardSummaryCountsModel> Counts { get; set; }
		public List<PropertyDashboardModel> RecentProperties { get; set; }
		public List<AgentDashboardModel> RecentAgents { get; set; }
	}
	public class RealEstateDashboardSummaryCountsModel
	{
		public string Metric { get; set; }
		public int Value { get; set; }
	}

	public class PropertyDashboardModel
	{
		public int PropertyID { get; set; }
		public string PropertyTitle { get; set; }
		public string TransactionType { get; set; }
		public decimal PropertyPrice { get; set; }
		public DateTime CreatedAt { get; set; }
	}

	public class AgentDashboardModel
	{
		public int? AgentID { get; set; }
		public string AgentName { get; set; }
		public string Email { get; set; }
		public int PropertiesListed { get; set; }
		public DateTime CreatedAt { get; set; }
	}
	public class RealEstateSummarySeller
	{
		public List<CountMetricSeller> Counts { get; set; }
		public List<RecentPropertySeller> RecentProperties { get; set; }
		public List<RecentAppointmentSeller> RecentAppointments { get; set; }
	}

	public class CountMetricSeller
	{
		public string Metric { get; set; }
		public int Value { get; set; }
	}

	public class RecentPropertySeller
	{
		public int PropertyID { get; set; }
		public string PropertyTitle { get; set; }
		public string TransactionType { get; set; }
		public decimal PropertyPrice { get; set; }
		public DateTime CreatedAt { get; set; }
		public int AgentID { get; set; }
	}

	public class RecentAppointmentSeller
	{
		public int AppointmentID { get; set; }
		public int BookerUserID { get; set; }
		public int AppointmentUserID { get; set; }
		public int PropertyID { get; set; }
		public DateTime AppointmentStartDate { get; set; }
		public DateTime AppointmentEndDate { get; set; }
		public string Status { get; set; }
		public string Notes { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
