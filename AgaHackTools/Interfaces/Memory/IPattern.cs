using AgaHackTools.Main.Default;

namespace AgaHackTools.Main.Interfaces
{
    public interface IPattern
    {
        ScanResult Find(byte[] myPattern, string mask);
        ScanResult Find(byte[] pattern);
        ScanResult Find(string patternText);
        ScanResult Find(Pattern pattern);
    }
}
