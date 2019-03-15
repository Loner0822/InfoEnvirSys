using System;
using System.Windows.Forms;

namespace EnvirInfoSys
{
	public class Map_Person
	{
		public string id
		{
			get;
			set;
		}

		public double lat
		{
			get;
			set;
		}

		public double lng
		{
			get;
			set;
		}

		public string name
		{
			get;
			set;
		}

		public DateTime time
		{
			get;
			set;
		}

		public string senderid
		{
			get;
			set;
		}

		public Timer timeclock
		{
			get;
			set;
		}

        public bool vis
        {
            get;
            set;
        }

        public string per_unit
        {
            get;
            set;
        }
	}
}
