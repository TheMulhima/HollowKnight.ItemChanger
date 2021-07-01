﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItemChanger.Items
{
    public class CharmItem : AbstractItem
    {
        public int charmNum;

        public string gotBool => $"gotCharm_{charmNum}";

        public override void GiveImmediate(GiveInfo info)
        {
            PlayerData.instance.SetBool(nameof(PlayerData.hasCharm), true);
            PlayerData.instance.SetBool(gotBool, true);
            PlayerData.instance.IncrementInt(nameof(PlayerData.charmsOwned));
        }

        public override bool Redundant()
        {
            return PlayerData.instance.GetBool(gotBool);
        }
    }
}
