﻿using HutongGames.PlayMaker.Actions;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;

namespace ItemChanger.Locations.SpecialLocations
{
    public class TukDefendersCrestLocation : AutoLocation
    {
        public int requiredCharmID = 10;

        protected override void OnLoad()
        {
            Events.AddFsmEdit(sceneName, new("Alive Tuk", "Steel Soul"), OverrideAliveTuk);
            Events.AddFsmEdit(sceneName, new("Dead Tuk", "Steel Soul"), OverrideDeadTuk);
            Events.AddFsmEdit(sceneName, new("Tuk NPC", "Conversation Control"), OverrideTukConvo);
            Events.AddLanguageEdit(new("Minor NPC", "TUK_EGGMAX"), AddDefendersCrestReminder);
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new("Alive Tuk", "Steel Soul"), OverrideAliveTuk);
            Events.RemoveFsmEdit(sceneName, new("Dead Tuk", "Steel Soul"), OverrideDeadTuk);
            Events.RemoveFsmEdit(sceneName, new("Tuk NPC", "Conversation Control"), OverrideTukConvo);
            Events.RemoveLanguageEdit(new("Minor NPC", "TUK_EGGMAX"), AddDefendersCrestReminder);
        }

        private void OverrideAliveTuk(PlayMakerFSM fsm)
        {
            fsm.GetState("Check").ClearTransitions();
        }

        private void OverrideDeadTuk(PlayMakerFSM fsm)
        {
            fsm.GetState("Check").ClearActions();
            fsm.GetState("Check").Transitions[0].FsmEvent = FsmEvent.Finished;
        }

        private void OverrideTukConvo(PlayMakerFSM fsm)
        {
            FsmState convoChoice = fsm.GetState("Convo Choice");
            FsmState dung = fsm.GetState("Dung");
            FsmState eggMax = fsm.GetState("Egg Max");
            FsmState give = fsm.GetState("Give");

            FsmEvent dungEvent = FsmEvent.GetFsmEvent("DUNG");

            convoChoice.Transitions = new FsmTransition[]
            {
                new(){ FsmEvent = dungEvent, ToFsmState = dung, ToState = dung.Name },
                new(){ FsmEvent = FsmEvent.Finished, ToFsmState = eggMax, ToState = eggMax.Name },
            };

            convoChoice.Actions = new FsmStateAction[]
            {
                new SetBoolValue{ boolVariable = fsm.FsmVariables.FindFsmBool("Give Egg"), boolValue = false },
                new DelegateBoolTest(ShouldGiveItem, FsmEvent.GetFsmEvent("DUNG"), null),
            };

            give.Actions = new FsmStateAction[]
            {
                new Lambda(() => FlingShiny(fsm.FsmVariables.FindFsmGameObject("Egg Spawn").Value.transform.position)),
            };
        }

        private void FlingShiny(Vector3 init)
        {
            Container c = Container.GetContainer(Container.Shiny);
            GameObject shiny = c.GetNewContainer(Placement, Placement.Items, flingType);
            Util.ShinyUtility.FlingShinyLeft(shiny.LocateFSM("Shiny Control"));
            c.ApplyTargetContext(shiny, init.x, init.y, 0);
        }

        private bool ShouldGiveItem()
        {
            return PlayerData.instance.GetBool("equippedCharm_" + requiredCharmID) && !Placement.AllObtained();
        }

        private void AddDefendersCrestReminder(ref string value)
        {
            if (!PlayerData.instance.GetBool("equippedCharm_" + requiredCharmID) && !Placement.AllObtained())
            {
                string charmName = Util.CharmNameUtil.GetCharmName(requiredCharmID);
                value += $"<page>It would be different if you were wearing {charmName}...";
            }
        }
    }
}
