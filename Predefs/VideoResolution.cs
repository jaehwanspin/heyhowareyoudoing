
namespace AppSystemSimulator.Predefs
{
    enum VideoResolution : System.Byte
    {
        GCIF = 0,
        CIF,
        VGA,
    }

    class VideoResolutionUtil
    {
        public static System.Tuple<System.UInt32, System.UInt32> GetResolution(VideoResolution code)
        {
            System.Tuple<System.UInt32, System.UInt32> result = null;
            
            switch (code)
            {
                case VideoResolution.GCIF:
                    result = new System.Tuple<System.UInt32, System.UInt32>(176, 144);
                    break;
                case VideoResolution.CIF:
                    result = new System.Tuple<System.UInt32, System.UInt32>(352, 288);
                    break;
                case VideoResolution.VGA:
                    result = new System.Tuple<System.UInt32, System.UInt32>(640, 480);
                    break;
            }

            return result;
        }
    }
}
