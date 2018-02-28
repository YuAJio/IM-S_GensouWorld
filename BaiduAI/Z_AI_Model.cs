using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.BaiduAI
{
    #region 身份证模型实体类
    public class AIM_IDCard
    {
        /// <summary>
        /// 唯一的log id，用于问题定位
        /// </summary>
        public int log_id { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string error_msg { get; set; }
        /// <summary>
        /// 图像方向，当detect_direction=true时存在。
        /// - -1:未定义，
        /// - 0:正向，
        /// - 1: 逆时针90度，
        /// - 2:逆时针180度，
        /// - 3:逆时针270度
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// normal-识别正常
        /// reversed_side-身份证正反面颠倒
        /// non_idcard-上传的图片中不包含身份证
        /// blurred-身份证模糊
        /// other_type_card-其他类型证照
        /// over_exposure-身份证关键字段反光或过曝     
        /// unknown-未知状态
        /// </summary>
        public string Image_status { get; set; }
        /// <summary>
        /// 	输入参数 detect_risk = true 时，则返回该字段识别身份证类型: normal-正常身份证；copy-复印件；temporary-临时身份证；screen-翻拍；unknow-其他未知情况
        /// </summar>
        public string Idcard_type { get; set; }
        /// <summary>
        /// 如果参数 detect_risk = true 时，则返回此字段。如果检测身份证被编辑过，该字段指定编辑软件名称，如:Adobe Photoshop CC 2014 (Macintosh),如果没有被编辑过则返回值无此参数
        /// </summary>
        public string Edit_tool { get; set; }
        /// <summary>
        /// 定位和识别结果数组
        /// </summary>
        public Words_result Data { get; set; }
        /// <summary>
        /// 识别结果数，表示words_result的元素个数
        /// </summary>
        public int Words_result_num { get; set; }

        public class Words_result
        {
            /// <summary>
            /// 住址
            /// </summary>
            public Address 住址 { get; set; }
            /// <summary>
            /// 公民身份号码
            /// </summary>
            public IDNumber 公民身份号码 { get; set; }
            /// <summary>
            /// 出生
            /// </summary>
            public Birthday 出生 { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public Name 姓名 { get; set; }
            /// <summary>
            /// 性别            /// </summary>
            public Gender 性别 { get; set; }
            /// <summary>
            /// 民族
            /// </summary>
            public Nation 民族 { get; set; }
        }
        public class Address
        {
            /// <summary>
            /// 位置数组（坐标0点为左上角）
            /// </summary>
            public Location Location { get; set; }
            /// <summary>
            /// 识别结果字符串
            /// </summary>
            public string Words { get; set; }
        }


        public class IDNumber
        {
            /// <summary>
            /// 位置数组（坐标0点为左上角）
            /// </summary>
            public Location Location { get; set; }
            /// <summary>
            /// 识别结果字符串
            /// </summary>
            public string Words { get; set; }
        }

        public class Birthday
        {
            /// <summary>
            /// 位置数组（坐标0点为左上角）
            /// </summary>
            public Location Location { get; set; }
            /// <summary>
            /// 识别结果字符串
            /// </summary>
            public string Words { get; set; }
        }

        public class Name
        {
            /// <summary>
            /// 位置数组（坐标0点为左上角）
            /// </summary>
            public Location Location { get; set; }
            /// <summary>
            /// 识别结果字符串
            /// </summary>
            public string Words { get; set; }
        }

        public class Gender
        {
            /// <summary>
            /// 位置数组（坐标0点为左上角）
            /// </summary>
            public Location Location { get; set; }
            /// <summary>
            /// 识别结果字符串
            /// </summary>
            public string Words { get; set; }
        }
        public class Nation
        {
            /// <summary>
            /// 位置数组（坐标0点为左上角）
            /// </summary>
            public Location Location { get; set; }
            /// <summary>
            /// 识别结果字符串
            /// </summary>
            public string Words { get; set; }
        }

        public class Location
        {
            /// <summary>
            /// 表示定位位置的长方形左上顶点的水平坐标
            /// </summary>
            public int Left { get; set; }
            /// <summary>
            /// 表示定位位置的长方形左上顶点的垂直坐标
            /// </summary>
            public int Top { get; set; }
            /// <summary>
            /// 表示定位位置的长方形的宽度
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// 表示定位位置的长方形的高度
            /// </summary>
            public int Height { get; set; }
        }
    }
    #endregion

}