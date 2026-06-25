using System;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using global::AoiKisaragi.Powers;

namespace AoiKisaragi.Powers
{
    public class MoonlitFortressCompanionPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override string? CustomPackedIconPath => null;
        public override string? CustomBigIconPath => null;
        public override string? CustomBigBetaIconPath => null;

        private readonly bool _upgraded;

        // Explicit parameterless constructor required by Activator.CreateInstance / reflection
        public MoonlitFortressCompanionPower() : this(false) { }

        // Existing gameplay constructor; keeps optional parameter for convenience
        public MoonlitFortressCompanionPower(bool upgraded = false)
        {
            _upgraded = upgraded;
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != base.Owner?.Player || base.Owner?.Player?.PlayerCombatState == null) return;
            try
            {
                Flash();
                await CreatureCmd.GainBlock(base.Owner, (_upgraded ? 8m : 5m), ValueProp.Unpowered, null);
                await PowerCmd.Apply<LunarStancePower>(base.Owner, 1m, base.Owner, (CardModel?)null);
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[MoonlitFortressCompanionPower] Error in startOfTurn: " + ex);
            }
        }
    }
}
