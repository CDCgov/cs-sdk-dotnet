using System;
using Newtonsoft.Json;

namespace CS.Mmg
{
	/// <summary>
	/// Information used when publishing a guide
	/// </summary>
	public sealed class PublishInfo
	{
		/// <summary>
		/// The internal version of the published guide to link to the storage
		/// </summary>
		public int InternalVersion { get; set; }

		/// <summary>
		/// Publish version
		/// </summary>
		public string PublishVersion { get; set; }

		/// <summary>
		/// Publish date
		/// </summary>
		public DateTime PublishDate { get; set; }
	}
}