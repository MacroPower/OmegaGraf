using System.Runtime.InteropServices;

namespace OmegaGraf.Compose
{
    public static class Unix
    {
        [DllImport("libc", SetLastError = true)]
        public static extern int chmod(string pathname, int mode);

        public const int OwnerR = 0x100;
        public const int OwnerW = 0x80;
        public const int OwnerX = 0x40;
        public const int GroupR = 0x20;
        public const int GroupW = 0x10;
        public const int GroupX = 0x8;
        public const int OtherR = 0x4;
        public const int OtherW = 0x2;
        public const int OtherX = 0x1;

        public const int P0777 =
              OwnerR | OwnerW | OwnerX
            | GroupR | GroupW | GroupX
            | OtherR | OtherW | OtherX;

        public const int P0755 =
              OwnerR | OwnerW | OwnerX
            | GroupR | GroupX
            | OtherR | OtherX;

        public const int P0666 =
              OwnerR | OwnerW
            | GroupR | GroupW
            | OtherR | OtherW;

        public const int P0644 =
              OwnerR | OwnerW
            | GroupR
            | OtherR;
    }
}
