using System;
using System.Collections.Generic;
using System.Text;

namespace Task1.Compressors
{
    internal interface ICompressor
    {
        String CompressString(String stringForCompress);
    }
}
