﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItemChanger.Tags
{
    public class ItemChainTag : Tag
    {
        public string predecessor;
        public string successor;

        public override void Load(object parent)
        {
            AbstractItem item = (AbstractItem)parent;
            item.ModifyItem += ModifyItem;
        }

        public override void Unload(object parent)
        {
            AbstractItem item = (AbstractItem)parent;
            item.ModifyItem -= ModifyItem;
        }


        protected virtual AbstractItem GetItem(string name)
        {
            return Finder.GetItem(name);
        }

        public void ModifyItem(GiveEventArgs args)
        {
            if (args.Item.Redundant())
            {
                while (args.Item?.GetTag<ItemChainTag>()?.successor is string succ && !string.IsNullOrEmpty(succ))
                {
                    args.Item = GetItem(succ);
                    if (!args.Item.Redundant())
                    {
                        return;
                    }
                }

                args.Item = null;
                return;
            }
            else
            {
                while (args.Item?.GetTag<ItemChainTag>()?.predecessor is string pred && !string.IsNullOrEmpty(pred))
                {
                    AbstractItem item = GetItem(pred);
                    if (item.Redundant())
                    {
                        return;
                    }
                    else args.Item = item;
                }
                return;
            }
        }
    }
}
