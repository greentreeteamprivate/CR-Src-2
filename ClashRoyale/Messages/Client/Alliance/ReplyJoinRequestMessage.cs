﻿namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Alliance.Entries;
    using ClashRoyale.Logic.Alliance.Stream;
    using ClashRoyale.Logic.Collections;

    public class ReplyJoinRequestMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14321;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        /// <summary>
        /// Gets the stream id.
        /// </summary>
        internal long StreamId
        {
            get
            {
                return (uint) this.HighId << 32 | (uint) this.LowId;
            }
        }

        private int HighId;
        private int LowId;

        private bool IsAccepted;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyJoinRequestMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ReplyJoinRequestMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // ReplyJoinRequestMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.HighId     = this.Stream.ReadInt();
            this.LowId      = this.Stream.ReadInt();
            this.IsAccepted = this.Stream.ReadBoolean();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override async void Process()
        {
            if (this.Device.GameMode.Player.IsInAlliance)
            {
                Clan Clan = await Clans.Get(this.Device.GameMode.Player.ClanHighId, this.Device.GameMode.Player.ClanLowId);

                if (Clan != null)
                {
                    AllianceMemberEntry Member;

                    if (Clan.Members.TryGetValue(this.Device.GameMode.Player.PlayerId, out Member))
                    {
                        if (Member.Role == 2 || Member.Role == 4)
                        {
                            JoinRequestAllianceStreamEntry Entry = (JoinRequestAllianceStreamEntry) Clan.Messages.GetEntry(this.StreamId);

                            if (Entry != null)
                            {
                                if (this.IsAccepted)
                                {
                                    Entry.AcceptRequest(this.Device.GameMode.Player.Name);

                                    // TODO : Inform the player about the AcceptRequest(Name).
                                }
                                else
                                {
                                    Entry.RefuseRequest(this.Device.GameMode.Player.Name);

                                    // TODO : Inform the player about the RefuseRequest(Name).
                                }

                                Clan.Messages.UpdateEntry(Entry);
                            }
                            else
                            {
                                Logging.Error(this.GetType(), "Player tried to answer the join request but the entry was null.");
                            }
                        }
                        else
                        {
                            Logging.Warning(this.GetType(), "Player tried to answer the join request but had insufficient permissions.");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Player tried to answer the join request but the AllianceMemberEntry was null.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Player tried to answer the join request but database returned a null value.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Player tried to answer the join request but is not in a clan.");
            }
        }
    }
}