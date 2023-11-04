namespace InputControl
{
    /// <summary>
    /// インゲームのどの処理を実行するか
    /// <para>各項目のsummaryに対応ボタンが記載されています</para>
    /// </summary>
    public enum ActionType
    {
        /// <summary>PS4:R1 <para>PC:左クリック</para> </summary>
        LightAttack = 0,
        /// <summary>PS4:R2 <para>PC:右クリック</para> </summary>
        HeavyAttack = 1,
        /// <summary>PS4:未定 <para>PC:未定</para> </summary>
        SpecialAttack01 = 2,
        /// <summary>PS4:未定 <para>PC:未定</para> </summary>
        SpecialAttack02 = 3,
        /// <summary>PS4:未定 <para>PC:未定</para> </summary>
        SpecialAttack03 = 4,
        /// <summary>PS4:×ボタン <para>PC:左Shift</para> </summary>
        DodgeAct = 5,
        /// <summary>PS4:○ボタン <para>PC:左クリック</para> </summary>
        Submit = 6,
        /// <summary>PS4:□ボタン <para>PC:Fキー</para> </summary>
        Item = 7,
        /// <summary>PS4:OPTIONSボタン <para>PC:Tキー</para> </summary>
        Pause = 8,
        /// <summary>PS4:未定 <para>PC:未定</para> </summary>
        LockOn = 9,
        /// <summary>PS4:左スティック <para>PC:WASD</para> </summary>
        Move = 10,
        /// <summary>PS4:右スティック <para>PC:マウス</para> </summary>
        Camera = 11,
        /// <summary>PS4:入力なし <para>PC:入力なし</para> </summary>
        None = 12,
    }

    /// <summary>UIのどの処理を実行するか</summary>
    public enum UIActionType
    {
        Navigate = 0,
        Submit = 1,
        Cancel = 2,
        Point = 3,
        Click = 4,
        ScrollWheel = 5,
        MiddleClick = 6,
        RightClick = 7,
        TrackedDevicePosition = 8,
        TrackedDeviceOrientaion = 9,
        Pause = 10,
    }

    /// <summary>ActionMapの種類 </summary>
    public enum ActionMaps
    {
        InGame = 0,
        UI = 1,
        NoReceive = 2,
        Events = 3,
    }
}

