using Sabine.Shared;

namespace Sabine.Zone.Compatibility
{
	/// <summary>
	/// Methods related to compatibility conversions for character class ids.
	/// </summary>
	public static class CharacterClasses
	{
		private const int MonsterRangeStart = 1001;

		/// <summary>
		/// Converts the given class id to one appropriate for the given
		/// version.
		/// </summary>
		/// <param name="version"></param>
		/// <param name="classId"></param>
		/// <returns></returns>
		public static int GetDisplayClassId(int version, int classId)
		{
			if (version <= Versions.Alpha)
			{
				return GetAlphaId(classId);
			}
			else if (version < Versions.Beta2)
			{
				return GetBeta1Id(classId);
			}

			// Just return the class id for all versions that (presumably)
			// use the modern numeration scheme.
			return classId;
		}

		private static int GetAlphaId(int classId)
		{
			if (classId < MonsterRangeStart)
			{
				switch (classId)
				{
					case 45: return 32; // Warp
					case 46: return 43;
					case 48: return 56;
					case 49: return 59;
					case 50: return 47;
					case 53: return 52;
					case 54: return 54;
					case 55: return 53;
					case 56: return 37;
					case 57: return 40;
					case 58: return 62;
					case 60: return 51;
					case 61: return 49;
					case 62: return 41;
					case 65: return 36;
					case 69: return 42;
					case 71: return 39;
					case 72: return 38;
					case 73: return 46;
					case 74: return 45;
					case 79: return 48;
					case 80: return 50;
					case 83: return 55;
					case 94: return 57;
					case 96: return 60;
					case 97: return 44;
					case 99: return 58;
					case 101: return 61;
					case 102: return 34;
					case 103: return 35;
					case 105: return 33;

					default: return 55;
				}
			}
			else
			{
				// The alpha client supports Poring~Doppelganger, use Poring
				// as fallback
				if (classId < 1001 || classId > 1046)
					return 66;

				// Poring Offset = 1002 - 66
				return classId - 936;
			}
		}

		private static int GetBeta1Id(int classId)
		{
			if (classId < MonsterRangeStart)
			{
				switch (classId)
				{
					case 45: return 32; // Warp
					case 46: return 33;
					case 47: return 34;
					case 48: return 35;
					case 49: return 36;
					case 50: return 37;
					case 51: return 38;
					case 52: return 39;
					case 53: return 40;
					case 54: return 41;
					case 55: return 42;
					case 56: return 43;
					case 57: return 44;
					case 58: return 45;
					case 59: return 46;
					case 60: return 47;
					case 61: return 48;
					case 62: return 49;
					case 63: return 50;
					case 64: return 51;
					case 65: return 52;
					case 66: return 53;
					case 67: return 54;
					case 68: return 55;
					case 69: return 56;
					case 70: return 57;
					case 71: return 58;
					case 72: return 59;
					case 73: return 60;
					case 74: return 61;
					case 75: return 62;
					case 76: return 63;
					case 77: return 64;
					case 78: return 65;
					case 79: return 66;
					case 80: return 67;
					case 81: return 68;
					case 82: return 69;
					case 83: return 70;
					case 84: return 71;
					case 85: return 72;
					case 86: return 73;
					case 87: return 74;
					case 88: return 75;
					case 89: return 76;
					case 90: return 77;
					case 91: return 78;
					case 92: return 79;
					case 93: return 80;
					case 94: return 81;
					case 95: return 82;
					case 96: return 83;
					case 97: return 84;
					case 98: return 85;
					case 99: return 86;
					case 100: return 87;
					case 101: return 88;
					case 105: return 92;
					case 106: return 93;
					case 107: return 94;
					case 108: return 95;
					case 109: return 96;
					case 110: return 97;

					default: return 70;
				}
			}
			else
			{
				// The beta 1 client supports Poring~OrcHero, use Poring
				// as fallback
				if (classId < 1001 || classId > 1087)
					return 104;

				// Poring Offset = 1002 - 104
				return classId - 898;
			}
		}
	}
}
