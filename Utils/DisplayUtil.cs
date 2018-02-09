
namespace IdoMaster_GensouWorld.Utils
{
    /// <summary>
    /// 像素转换类
    /// </summary>
    public static class DisplayUtil
    {  /// <summary>
       ///  将px值转换为dip或dp值，保证尺寸大小不变
       /// </summary>
       /// <param name="pxValue">Px value.</param>
       /// <param name="scale">Scale.</param>
        public static int px2dip(float pxValue, float scale)
        {
            return (int)(pxValue / scale + 0.5f);
        }
        /// <summary>
        ///将dip或dp值转换为px值，保证尺寸大小不变
        /// </summary>
        /// <param name="dipValue">Dip value.</param>
        /// <param name="scale">Scale.(DisplayMetrics类中属性density)</param>
        public static int dip2px(float dipValue, float scale)
        {
            return (int)(dipValue * scale + 0.5f);
        }
        /// <summary>
        /// 将px值转换为sp值，保证文字大小不变
        /// </summary>
        /// <param name="pxValue">Px value.</param>
        /// <param name="fontScale">Font scale.</param>
        public static int px2sp(float pxValue, float fontScale)
        {
            return (int)(pxValue / fontScale + 0.5f);
        }
        /// <summary>
        ///  将sp值转换为px值，保证文字大小不变
        /// </summary>
        /// <param name="spValue">Sp value.</param>
        /// <param name="fontScale">Font scale.</param>
        public static int sp2px(float spValue, float fontScale)
        {
            return (int)(spValue * fontScale + 0.5f);
        }
    }
}