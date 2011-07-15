using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    static class SFPacketType
    {
        public const int FileTransfer = 0;
        public const int SearchForFile = 1;
        public const int FileList = 2;
        public const int FileDownloadRequest = 3;
    }
}
