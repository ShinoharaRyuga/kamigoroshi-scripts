using System;

/// <summary>ダメージをやり取りするインターフェイス </summary>
public interface IDamage
{
    /// <summary>ダメージを受ける </summary>
    /// <param name="damageValue">ダメージ量</param>
    void AddDamage(float damageValue);
}