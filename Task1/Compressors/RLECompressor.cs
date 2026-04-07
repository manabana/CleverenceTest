using System;
using System.Collections.Generic;
using System.Text;

namespace Task1.Compressors
{
    internal class RLECompressor : ICompressor
    {
        public string CompressString(String stringForCompress)
        {
            Char? lastCharacter = null;
            Int32 characterSeriesCount = 0;
            StringBuilder outputText = new StringBuilder();

            foreach (Char character in stringForCompress)
            {
                if (character == lastCharacter)
                {
                    characterSeriesCount++;
                }
                else
                {
                    outputText.Append($"{character}{characterSeriesCount}");
                    characterSeriesCount = 0;
                }
            }

            return outputText.ToString();
        }
    }
}
