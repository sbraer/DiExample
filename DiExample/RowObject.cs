using System;

namespace DiExample
{
	public class RowObject
	{
		public RowObject(string name, DateTime date)
		{
			Name = name;
			Date = date;
		}

		public string Name { get; set; }
		public DateTime Date { get; set; }
	}
}
