using AgaHackTools.Main.Default;

namespace AgaHackTools.Main.Interfaces
{
    public interface IPattern
    {
        ScanResult Find(byte[] myPattern, string mask, int offsetToAdd, bool isOffsetMode, bool reBase);
        ScanResult Find(byte[] pattern, int offsetToAdd, bool isOffsetMode, bool reBase);
        ScanResult Find(string patternText, int offsetToAdd, bool isOffsetMode, bool reBase);
        ScanResult Find(Pattern pattern);
    }
}
