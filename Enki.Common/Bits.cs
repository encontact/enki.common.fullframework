﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enki.Common {
	public static class Bits {
		public static byte[] Not(byte[] bytes) {
			return bytes.Select(b => (byte)~b).ToArray();
		}

		public static byte[] And(byte[] A, byte[] B) {
			return A.Zip(B, (a, b) => (byte)(a & b)).ToArray();
		}

		public static byte[] Or(byte[] A, byte[] B) {
			return A.Zip(B, (a, b) => (byte)(a | b)).ToArray();
		}

		public static bool GE(byte[] A, byte[] B) {
			return A.Zip(B, (a, b) => a == b ? 0 : a < b ? 1 : -1)
				.SkipWhile(c => c == 0)
				.FirstOrDefault() >= 0;
		}

		public static bool LE(byte[] A, byte[] B) {
			return A.Zip(B, (a, b) => a == b ? 0 : a < b ? 1 : -1)
				.SkipWhile(c => c == 0)
				.FirstOrDefault() <= 0;
		}

		public static byte[] GetBitMask(int sizeOfBuff, int bitLen) {
			var maskBytes = new byte[sizeOfBuff];
			var bytesLen = bitLen / 8;
			var bitsLen = bitLen % 8;
			for (int i = 0; i < bytesLen; i++) {
				maskBytes[i] = 0xff;
			}
			if (bitsLen > 0) maskBytes[bytesLen] = (byte)~Enumerable.Range(1, 8 - bitsLen).Select(n => 1 << n - 1).Aggregate((a, b) => a | b);
			return maskBytes;
		}
	}
}
