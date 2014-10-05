﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Enki.Common {
	/// <summary>
	/// A collection of WebMethodInfo objects
	/// </summary>
	public class WebMethodInfoCollection : KeyedCollection<string, WebMethodInfo> {
		/// <summary>
		/// Constructor
		/// </summary>
		public WebMethodInfoCollection() : base() { }

		protected override string GetKeyForItem(WebMethodInfo webMethodInfo) {
			return webMethodInfo.Name;
		}
	}

}
