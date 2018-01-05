﻿namespace ClashRoyale.Messages.Server.Attack
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;

    public class OpponentRejoinsMatchNotificationMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20802;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpponentRejoinsMatchNotificationMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public OpponentRejoinsMatchNotificationMessage(Device Device) : base(Device)
        {
            // Opponent_Rejoins_Match_Notification_Message.
        }
    }
}