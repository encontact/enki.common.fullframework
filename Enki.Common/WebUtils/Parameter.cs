using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enki.Common {
	/// <summary>
	/// represents a parameter (input or output) of a web method.
	/// </summary>
	public struct Parameter {
		/// <summary>
		/// Name
		/// </summary>
		public string Name;
		/// <summary>
		/// Type
		/// </summary>
		public string Type;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="name">
		/// <param name="type">
		public Parameter(string name, string type) {
			this.Name = name;
			this.Type = type;
		}

	}
}
