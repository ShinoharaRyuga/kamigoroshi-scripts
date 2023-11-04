
/// <summary>アイテム効果でパラメーターが変わる処理 </summary>
public interface IItemChangeParam
{
    /// <summary>即時発動のアイテム効果処理</summary>
    /// <param name="targetParam">対象のパラメーター</param>
    /// <param name="value">増減値</param>
    void ItemParamChangeActive(ParameterType targetParam, float value);

    /// <summary>持続アイテム効果処理 </summary>
    /// <param name="targetParam">対象のパラメーター</param>
    /// <param name="duration">持続時間</param>  
    /// <param name="value">増減値</param>
    void ItemParamChangeHold(ParameterType targetParam, float duration, float value);
}
