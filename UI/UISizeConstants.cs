using System;

namespace DTwoMFTimerHelper.UI
{
    /// <summary>
    /// UI尺寸常量类，集中管理所有UI组件的宽度和高度
    /// 便于维护和统一修改
    /// </summary>
    public static class UISizeConstants
    {
        #region TimerControl 尺寸常量
        /// <summary>
        /// TimerControl 默认宽度
        /// </summary>
        public const int TimerControlWidth = 667;
        
        /// <summary>
        /// TimerControl 隐藏掉落记录时的高度
        /// </summary>
        public const int TimerControlHeightWithoutLoot = 500;
        
        /// <summary>
        /// TimerControl 显示掉落记录时的高度
        /// </summary>
        public const int TimerControlHeightWithLoot = 750;
        #endregion

        #region MainForm 尺寸常量
        /// <summary>
        /// MainForm 默认宽度
        /// </summary>
        public const int MainFormWidth = 894;
        
        /// <summary>
        /// TabControl 宽度
        /// </summary>
        public const int TabControlWidth = 894;
        
        /// <summary>
        /// TabControl 高度
        /// </summary>
        public const int TabControlHeight = 813;
        #endregion

        #region LootRecordsControl 尺寸常量
        /// <summary>
        /// LootRecordsControl 宽度
        /// </summary>
        public const int LootRecordsControlWidth = 421;
        
        /// <summary>
        /// LootRecordsControl 高度
        /// </summary>
        public const int LootRecordsControlHeight = 219;
        #endregion

        #region ToggleLootButton 尺寸常量
        /// <summary>
        /// 切换掉落按钮宽度
        /// </summary>
        public const int ToggleLootButtonWidth = 131;
        
        /// <summary>
        /// 切换掉落按钮高度
        /// </summary>
        public const int ToggleLootButtonHeight = 40;
        #endregion
    }
}