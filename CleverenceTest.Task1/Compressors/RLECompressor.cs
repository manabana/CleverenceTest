using System;
using System.Collections.Generic;
using System.Text;

namespace CleverenceTest.Task1.Compressors
{
    internal class RLECompressor : ICompressor
    {
        public string CompressString(String stringForCompress)
        {
            Int32 characterSeriesCount = 0;
            StringBuilder outputText = new StringBuilder();

            for (Int32 i = 0; i < stringForCompress.Length; i++)
            {
                Char character = stringForCompress[i];

                Char? nextCharacter = (i + 1) < stringForCompress.Length 
                    ? stringForCompress[i + 1] 
                    : null;

                Char? prevCharacter = (i - 1) >= 0 
                    ? stringForCompress[i - 1] 
                    : null;

                if (character == prevCharacter || prevCharacter is null)
                {
                    characterSeriesCount++;
                }
                
                if (character != nextCharacter)
                {
                    outputText.Append($"{character}{(characterSeriesCount == 1 ? null : characterSeriesCount)}");
                    characterSeriesCount = 1;
                }
                
            }

            return outputText.ToString();
        }
    }
}
