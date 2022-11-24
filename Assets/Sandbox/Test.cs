﻿using TMPro;
using AVG.Runtime;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TMP_Text charName;
    public TMP_Text text;


    public void Next()
    {
        if (Engine.TryGetService<PlotPlayer>() is not PlotPlayer player) return;
        player.info(out var a, out var t);
        charName.SetText(a);
        text.SetText(t);
        player.UpdateThis();
    }
}