﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ItemChanger.Items
{
    public class GrubItem : AbstractItem
    {
        public override Container GetPreferredContainer() => Container.GrubJar;
        public override bool GiveEarly(Container container)
        {
            if (container == Container.GrubJar) return true;
            else return false;
        }
        public override void GiveImmediate(GiveInfo info)
        {
            PlayerData.instance.IncrementInt(nameof(PlayerData.grubsCollected));
            if (info.Container == Container.GrubJar) return;

            int clipIndex = new System.Random().Next(2);
            AudioSource.PlayClipAtPoint(ObjectCache.GrubCries[clipIndex],
                new Vector3(
                    Camera.main.transform.position.x - 2,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z + 2
                ));
            AudioSource.PlayClipAtPoint(ObjectCache.GrubCries[clipIndex],
                new Vector3(
                    Camera.main.transform.position.x + 2,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z + 2
                ));
        }
    }
}
