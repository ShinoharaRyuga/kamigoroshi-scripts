
/// <summary>�A�C�e�����ʂŃp�����[�^�[���ς�鏈�� </summary>
public interface IItemChangeParam
{
    /// <summary>���������̃A�C�e�����ʏ���</summary>
    /// <param name="targetParam">�Ώۂ̃p�����[�^�[</param>
    /// <param name="value">�����l</param>
    void ItemParamChangeActive(ParameterType targetParam, float value);

    /// <summary>�����A�C�e�����ʏ��� </summary>
    /// <param name="targetParam">�Ώۂ̃p�����[�^�[</param>
    /// <param name="duration">��������</param>  
    /// <param name="value">�����l</param>
    void ItemParamChangeHold(ParameterType targetParam, float duration, float value);
}
