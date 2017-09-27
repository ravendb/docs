using System;

namespace Raven.Documentation.Samples
{
	#region camera
	public class Camera
	{
		public DateTime DateOfListing { get; set; }

		public string Model { get; set; }

		public decimal Cost { get; set; }

		public int Zoom { get; set; }

		public double Megapixels { get; set; }

		public bool ImageStabilizer { get; set; }

		public string Manufacturer { get; set; }
	}
	#endregion
}