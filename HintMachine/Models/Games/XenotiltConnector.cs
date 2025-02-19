﻿using HintMachine.Helpers;
using HintMachine.Models.GenericConnectors;

namespace HintMachine.Models.Games
{
    [AvailableGameConnector]
    public class XenotiltConnector : IGameConnector
    {
        private readonly HintQuestCumulative _scoreQuest = new HintQuestCumulative
        {
            Name = "Score",
            GoalValue = 200000000,
        };

        private ProcessRamWatcher _ram = null;

        public XenotiltConnector()
        {
            Name = "Xenotilt";
            Description = "In this three parts pinball table, you will have to complete missions to earn a lot of points.";
            Platform = "PC";
            SupportedVersions.Add(".282");
            CoverFilename = "xenotilt.png";
            Author = "CalDrac";

            Quests.Add(_scoreQuest);
        }

        protected override bool Connect()
        {
            _ram = new ProcessRamWatcher("Xenotilt", "mono-2.0-bdwgc.dll");
            return _ram.TryConnect();
        }

        public override void Disconnect()
        {
            _ram = null;
        }

        protected override bool Poll()
        {
            long scoreAddress = _ram.ResolvePointerPath64(_ram.BaseAddress + 0x7270B8, new int[] { 0x30, 0x7e0, 0x7C0 });
            _scoreQuest.UpdateValue(_ram.ReadInt64(scoreAddress));

            return true;
        }
    }
}