using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Main.Default
{
    /// <summary>
    ///     A class that represents basic pattrn scanning properties.
    /// </summary>
    public class Pattern
    {
        /// <summary>
        ///     A description of the pattern being scanned.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The Dword format text of the pattern.
        ///     <example>A2 5B ?? ?? ?? A2</example>
        /// </summary>
        public string TextPattern { get; set; }

        /// <summary>
        ///     The value to add to the offset result when the pattern is first found.
        /// </summary>
        public int OffsetToAdd { get; set; }

        /// <summary>
        ///     If the result should be from the address or offset.
        /// </summary>
        public bool IsOffsetMode { get; set; }

        /// <summary>
        ///     If the address should be rebased to a process modules address or not.
        /// </summary>
        public bool RebaseAddress { get; set; }

        /// <summary>
        ///     Tyhe type of pointer the scan results in.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        ///     Creates a new instance of <see cref="Pattern" />.
        /// </summary>
        /// <param name="description">A description of the pattern being created.</param>
        /// <param name="pattern">The patterns Dword formatted text pattern.</param>
        /// <param name="offseToAdd">The offset to add to the result found before returning the value.</param>
        /// <param name="isOffsetMode">If we should return the address or offset in the result if this is a xml patternscan.</param>
        /// <param name="rebase">If the address should be rebased to a process module.</param>
        /// <param name="comment">Any comments about the pattern. Useful for xml files.</param>
        public Pattern(string description, string pattern, int offseToAdd, bool isOffsetMode, bool rebase,
            string comment)
        {
            Description = description;
            TextPattern = pattern;
            OffsetToAdd = offseToAdd;
            IsOffsetMode = isOffsetMode;
            RebaseAddress = rebase;
            Comments = comment;
        }

        /// <summary>
        ///     Gets the mask from a string based byte pattern to scan for.
        /// </summary>
        /// <param name="pattern">The string pattern to search for. ?? is mask and space between each byte and mask.</param>
        /// <returns>The mask from the pattern.</returns>
        public string GetMaskFromDwordPattern()
        {
            return GetMaskFromDwordPattern(TextPattern);
        }

        /// <summary>
        ///     Gets the byte[] pattern from string format patterns.
        /// </summary>
        /// <param name="pattern">The string pattern to search for. ?? is mask and space between each byte and mask.</param>
        /// <returns>An array of bytes.</returns>
        public  byte[] GetBytesFromDwordPattern()
        {
            return GetBytesFromDwordPattern(TextPattern);
        }

        /// <summary>
        ///     Gets the mask from a string based byte pattern to scan for.
        /// </summary>
        /// <param name="pattern">The string pattern to search for. ?? is mask and space between each byte and mask.</param>
        /// <returns>The mask from the pattern.</returns>
        public static string GetMaskFromDwordPattern(string pattern)
        {
            var mask = pattern
                .Split(' ')
                .Select(s => s.Contains('?') ? "?" : "x");

            return string.Concat(mask);
        }

        /// <summary>
        ///     Gets the byte[] pattern from string format patterns.
        /// </summary>
        /// <param name="pattern">The string pattern to search for. ?? is mask and space between each byte and mask.</param>
        /// <returns>An array of bytes.</returns>
        public static byte[] GetBytesFromDwordPattern(string pattern)
        {
            return pattern
                .Split(' ')
                .Select(s => s.Contains('?') ? (byte)0 : byte.Parse(s, NumberStyles.HexNumber))
                .ToArray();
        }

        /// <summary>
        /// Creates a mask from a given pattern, using the given chars
        /// </summary>
        /// <param name="pattern">The pattern this functions designs a mask for</param>
        /// <param name="wildcardByte">Byte that is interpreted as a wildcard</param>
        /// <param name="wildcardChar">Char that is used as wildcard</param>
        /// <param name="matchChar">Char that is no wildcard</param>
        /// <returns></returns>
        public static string MaskFromPattern(byte[] pattern, byte wildcardByte =0, char wildcardChar = '?', char matchChar = 'x')
        {
            char[] chr = new char[pattern.Length];
            for (int i = 0; i < chr.Length; i++)
                chr[i] = pattern[i] == wildcardByte ? wildcardChar : matchChar;
            return new string(chr);
        }
    }
}
